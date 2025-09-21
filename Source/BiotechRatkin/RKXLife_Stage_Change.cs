using HarmonyLib;
using NewRatkin.helper;
using RimWorld;
using Verse;

namespace NewRatkin.sized
{
	public class RKXLife_Stage_Change
	{
		[HarmonyPatch(typeof(Pawn_AgeTracker), "CurLifeStage", MethodType.Getter)]
		private static class Pawn_AgeTracker__CurLifeStage
		{
			private static void Postfix(ref Pawn ___pawn, ref LifeStageDef __result)
			{
				if (___pawn.RaceProps.Humanlike && ___pawn.ageTracker != null && RKXenoHelper.IsRatkinSized(___pawn))
				{
					if (___pawn.ageTracker.AgeBiologicalYears >= 18)
					{
						__result = RKXenoHelper.lifeStageHumanlikeRatkinAdult;
					}
					else if (___pawn.ageTracker.AgeBiologicalYears >= 13)
					{
						__result = RKXenoHelper.lifeStageHumanlikeRatkinTeenager;
					}
					else if (___pawn.ageTracker.AgeBiologicalYears >= 3)
					{
						__result = RKXenoHelper.lifeStageHumanlikeRatkinChild;
					}
				}
			}
		}
	}
}