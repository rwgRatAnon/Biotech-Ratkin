using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace NewRatkin
{
    public class NewRatkin : Mod
    {
        internal static bool isDebug = false;

        BioratsModSettings settings;

        public NewRatkin(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BioratsModSettings>();
        }

        /*public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("EnableRatkinFA".Translate(), ref settings.useRatkinFacialAnimation, "EnableRatkinFacialAnimationToolTip".Translate());
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "BioratsSettings".Translate();
        }*/
    }

    public class BioratsModSettings : ModSettings
    {
        public bool useRatkinFacialAnimation = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref useRatkinFacialAnimation, "useRatkinFacialAnimation");
            base.ExposeData();
        }
    }

    public static class BioratsHelpers
    {
        internal static void Log(string message)
        {
            if (NewRatkin.isDebug && !message.NullOrEmpty())
            {
                Verse.Log.Message(message);
            }
        }

        internal static void Warn(string message)
        {
            if (NewRatkin.isDebug && !message.NullOrEmpty())
            {
                Verse.Log.Warning(message);
            }
        }

    }

    public class GeneExtension : DefModExtension
    {
        //Xenotype extension
        public int maleSpawnChance;
        public float MaleSpawnChance()
        {
            if (maleSpawnChance >= 0)
                return maleSpawnChance / 100f;
            else return 0f;
        }
    }
}
