using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000815 RID: 2069
	public class ThoughtWorker_ImprisonedByFaction : ThoughtWorker
	{
		// Token: 0x06003437 RID: 13367 RVA: 0x0011F16B File Offset: 0x0011D36B
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.IsPrisoner && p.guest.HostFaction == other.Faction;
		}
	}
}
