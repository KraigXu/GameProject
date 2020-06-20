using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFE RID: 3582
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x001CBB10 File Offset: 0x001C9D10
		private List<Pawn> AwaitingMedicalOperation
		{
			get
			{
				this.awaitingMedicalOperationResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (Alert_AwaitingMedicalOperation.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(list[i]))
					{
						this.awaitingMedicalOperationResult.Add(list[i]);
					}
				}
				List<Pawn> allMaps_PrisonersOfColonySpawned = PawnsFinder.AllMaps_PrisonersOfColonySpawned;
				for (int j = 0; j < allMaps_PrisonersOfColonySpawned.Count; j++)
				{
					if (Alert_AwaitingMedicalOperation.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(allMaps_PrisonersOfColonySpawned[j]))
					{
						this.awaitingMedicalOperationResult.Add(allMaps_PrisonersOfColonySpawned[j]);
					}
				}
				return this.awaitingMedicalOperationResult;
			}
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x001CBBA1 File Offset: 0x001C9DA1
		public override string GetLabel()
		{
			return "PatientsAwaitingMedicalOperation".Translate(this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x001CBBC8 File Offset: 0x001C9DC8
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x001CBC50 File Offset: 0x001C9E50
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		// Token: 0x04002F3B RID: 12091
		private List<Pawn> awaitingMedicalOperationResult = new List<Pawn>();
	}
}
