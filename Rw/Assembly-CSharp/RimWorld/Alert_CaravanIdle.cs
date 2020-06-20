using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF2 RID: 3570
	public class Alert_CaravanIdle : Alert
	{
		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x0600567A RID: 22138 RVA: 0x001CAC64 File Offset: 0x001C8E64
		private List<Caravan> IdleCaravans
		{
			get
			{
				this.idleCaravansResult.Clear();
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					if (caravan.Spawned && caravan.IsPlayerControlled && !caravan.pather.MovingNow && !caravan.CantMove)
					{
						this.idleCaravansResult.Add(caravan);
					}
				}
				return this.idleCaravansResult;
			}
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x001CACF8 File Offset: 0x001C8EF8
		public override string GetLabel()
		{
			return "CaravanIdle".Translate();
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x001CAD0C File Offset: 0x001C8F0C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Caravan caravan in this.IdleCaravans)
			{
				stringBuilder.AppendLine("  - " + caravan.Label);
			}
			return "CaravanIdleDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x001CAD8C File Offset: 0x001C8F8C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.IdleCaravans);
		}

		// Token: 0x04002F32 RID: 12082
		private List<Caravan> idleCaravansResult = new List<Caravan>();
	}
}
