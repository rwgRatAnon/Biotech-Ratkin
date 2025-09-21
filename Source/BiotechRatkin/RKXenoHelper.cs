using RimWorld;
using Verse;

namespace NewRatkin.helper
{
	public class RKXenoHelper
	{
		public static readonly GeneDef BodySize_RK = DefDatabase<GeneDef>.GetNamed("BodySize_RK");

		public static readonly LifeStageDef lifeStageHumanlikeRatkinChild = DefDatabase<LifeStageDef>.GetNamed("lifeStageHumanlikeRatkinChild");

		public static readonly LifeStageDef lifeStageHumanlikeRatkinTeenager = DefDatabase<LifeStageDef>.GetNamed("lifeStageHumanlikeRatkinTeenager");

		public static readonly LifeStageDef lifeStageHumanlikeRatkinAdult = DefDatabase<LifeStageDef>.GetNamed("lifeStageHumanlikeRatkinAdult");

		public static bool IsRatkinSized(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.genes != null)
			{
				return pawn.genes.HasActiveGene(BodySize_RK);
			}
			return false;
		}

		public static bool IsRatkinBody(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.genes != null)
			{
				return pawn.genes.HasActiveGene(BodySize_RK);
			}
			return false;
		}
	}
}
