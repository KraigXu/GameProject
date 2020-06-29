using System;
using Verse;

namespace RimWorld
{
	
	public class StatPart_IsFlesh : StatPart
	{
		
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, delegate(Pawn x)
			{
				if (!x.RaceProps.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, delegate(ThingDef x)
			{
				if (!x.race.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, out bodySize);
		}
	}
}
