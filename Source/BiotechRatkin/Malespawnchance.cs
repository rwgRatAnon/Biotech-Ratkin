using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NewRatkin
{
    [StaticConstructorOnStartup]
    public static class BioratMalespawn
    {
        static BioratMalespawn()
        {
            Harmony harmonyInstance = new Harmony("com.NewRatkin.rimworld.mod");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[1] { typeof(PawnGenerationRequest) })]
    internal static class GeneratePawnPatch
    {
        private static void Prefix(ref PawnGenerationRequest request)
        {
            string log = "";
            XenotypeDef forcedXenotype = request.ForcedXenotype;
            XenotypeSet xenos = request.KindDef?.xenotypeSet;
            if ((forcedXenotype == null || forcedXenotype == XenotypeDefOf.Baseliner) && xenos != null && xenos.Count > 0)
            {
                for (int i = 0; i < xenos.Count; i++)
                {
                    if (xenos[i].chance > 0 && xenos[i].xenotype.HasModExtension<GeneExtension>())
                    {
                        forcedXenotype = xenos[i].xenotype;
                        break;
                    }
                }
            }

            GeneExtension extension = forcedXenotype?.GetModExtension<GeneExtension>();
            if (extension != null)
            {
                BioratsHelpers.Warn("Starting Biorats.PreGeneratePawn");
                if (forcedXenotype != null) { log += $"Forced Xenotype: {forcedXenotype.label}   "; }
                log += $"Male spawn chance: {extension.maleSpawnChance}   ";
                if (request.FixedGender.HasValue)
                {
                    log += $"Pawn has fixed gender {request.FixedGender.Value}   ";
                }
                else
                {
                    if (!Rand.Chance(extension.MaleSpawnChance()))
                    {
                        log += "Setting female gender   ";
                        request.FixedGender = Gender.Female;
                    }
                    else
                    {
                        log += "Setting male gender   ";
                        request.FixedGender = Gender.Male;
                    }
                }
            }
            BioratsHelpers.Log(log);
        }

        private static void Postfix(PawnGenerationRequest request, ref Pawn __result)
        {
            if (__result == null) { BioratsHelpers.Warn("   Pawn is null - aborting"); return; }
            else if (!__result.RaceProps.Humanlike) { return; }

            string log = "";

            GeneExtension extension = __result?.genes?.Xenotype.GetModExtension<GeneExtension>();
            if (extension != null)
            {
                BioratsHelpers.Warn($"Starting Biorats.PostGeneratePawn for {__result.Label}");
                log += $"Pawn is {__result.genes.XenotypeLabel}   ";
                if (__result.gender == Gender.Male && !request.FixedGender.HasValue)
                {
                    if (!Rand.Chance(extension.MaleSpawnChance()) && __result.relations.ChildrenCount == 0)
                    {
                        log += $"Forcing female spawn";
                        __result.gender = Gender.Female;
                        __result.story.bodyType = PawnGenerator.GetBodyTypeFor(__result);
                        PawnBioAndNameGenerator.GeneratePawnName(__result, NameStyle.Full, request.FixedLastName, false, __result.genes.Xenotype);
                    }
                }
            }
            BioratsHelpers.Log(log);
        }
    }
}