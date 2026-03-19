using System.Runtime.CompilerServices;
using HarmonyLib;
using NewRatkin.helper;
using RimWorld;
using Verse;

namespace NewRatkin.sized
{
    public class RKXLife_Stage_Change
    {
        [HarmonyPatch(typeof(Pawn_AgeTracker), nameof(Pawn_AgeTracker.CurLifeStage), MethodType.Getter)]
        private static class Pawn_AgeTracker__CurLifeStage
        {
            // Explicit constructor (C# 7.3 compatible)
            private static readonly ConditionalWeakTable<Pawn, BoolCache> ratkinCache =
                new ConditionalWeakTable<Pawn, BoolCache>();

            private class BoolCache
            {
                public bool isRatkinSized;
            }

            [HarmonyPostfix]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void Postfix(Pawn ___pawn, ref LifeStageDef __result)
            {
                // Fast exits
                if (___pawn == null)
                    return;

                RaceProperties race = ___pawn.RaceProps;
                if (race == null || !race.Humanlike)
                    return;

                Pawn_AgeTracker ageTracker = ___pawn.ageTracker;
                if (ageTracker == null)
                    return;

                // Cached expensive check
                if (!IsRatkinSizedCached(___pawn))
                    return;

                // Cache age locally
                float age = ageTracker.AgeBiologicalYears;

                // Minimal branching
                if (age >= 18f)
                {
                    __result = RKXenoHelper.lifeStageHumanlikeRatkinAdult;
                    return;
                }

                if (age >= 13f)
                {
                    __result = RKXenoHelper.lifeStageHumanlikeRatkinTeenager;
                    return;
                }

                if (age >= 3f)
                {
                    __result = RKXenoHelper.lifeStageHumanlikeRatkinChild;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsRatkinSizedCached(Pawn pawn)
            {
                BoolCache cache;
                if (!ratkinCache.TryGetValue(pawn, out cache))
                {
                    cache = new BoolCache();
                    cache.isRatkinSized = RKXenoHelper.IsRatkinSized(pawn);
                    ratkinCache.Add(pawn, cache);
                }
                return cache.isRatkinSized;
            }
        }
    }
}