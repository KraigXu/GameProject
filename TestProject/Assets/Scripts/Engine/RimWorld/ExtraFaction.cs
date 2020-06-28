using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000978 RID: 2424
	public class ExtraFaction : IExposable
	{
		// Token: 0x06003960 RID: 14688 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public ExtraFaction()
		{
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x001314B7 File Offset: 0x0012F6B7
		public ExtraFaction(Faction faction, ExtraFactionType factionType)
		{
			this.faction = faction;
			this.factionType = factionType;
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x001314CD File Offset: 0x0012F6CD
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<ExtraFactionType>(ref this.factionType, "factionType", ExtraFactionType.HomeFaction, false);
		}

		// Token: 0x040021D2 RID: 8658
		public Faction faction;

		// Token: 0x040021D3 RID: 8659
		public ExtraFactionType factionType;
	}
}
