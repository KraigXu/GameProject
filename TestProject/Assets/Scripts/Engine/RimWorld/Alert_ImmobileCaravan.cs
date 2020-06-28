using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDA RID: 3546
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06005614 RID: 22036 RVA: 0x001C8A70 File Offset: 0x001C6C70
		private List<Caravan> ImmobileCaravans
		{
			get
			{
				this.immobileCaravansResult.Clear();
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (caravans[i].IsPlayerControlled && caravans[i].ImmobilizedByMass)
					{
						this.immobileCaravansResult.Add(caravans[i]);
					}
				}
				return this.immobileCaravansResult;
			}
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x001C8AD8 File Offset: 0x001C6CD8
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x001C8B15 File Offset: 0x001C6D15
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}

		// Token: 0x04002F13 RID: 12051
		private List<Caravan> immobileCaravansResult = new List<Caravan>();
	}
}
