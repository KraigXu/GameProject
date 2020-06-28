using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000817 RID: 2071
	public class ThoughtWorker_TeetotalerVsAddict : ThoughtWorker
	{
		// Token: 0x0600343B RID: 13371 RVA: 0x0011F20C File Offset: 0x0011D40C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			List<Hediff> hediffs = other.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.IsAddiction)
				{
					return true;
				}
			}
			return false;
		}
	}
}
