using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD6 RID: 3542
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x06005602 RID: 22018 RVA: 0x001C8468 File Offset: 0x001C6668
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06005603 RID: 22019 RVA: 0x001C8490 File Offset: 0x001C6690
		private List<Pawn> HypothermiaDangerColonists
		{
			get
			{
				this.hypothermiaDangerColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
						{
							this.hypothermiaDangerColonistsResult.Add(pawn);
						}
					}
				}
				return this.hypothermiaDangerColonistsResult;
			}
		}

		// Token: 0x06005604 RID: 22020 RVA: 0x001C8534 File Offset: 0x001C6734
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HypothermiaDangerColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertHypothermiaDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x001C85BC File Offset: 0x001C67BC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}

		// Token: 0x04002F10 RID: 12048
		private List<Pawn> hypothermiaDangerColonistsResult = new List<Pawn>();
	}
}
