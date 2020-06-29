using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_AwaitingMedicalOperation : Alert
	{
		
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x001CBB10 File Offset: 0x001C9D10
		private List<Pawn> AwaitingMedicalOperation
		{
			get
			{
				this.awaitingMedicalOperationResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					//if (Alert_AwaitingMedicalOperation.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(list[i]))
					//{
					//	this.awaitingMedicalOperationResult.Add(list[i]);
					//}
				}
				List<Pawn> allMaps_PrisonersOfColonySpawned = PawnsFinder.AllMaps_PrisonersOfColonySpawned;
				for (int j = 0; j < allMaps_PrisonersOfColonySpawned.Count; j++)
				{
					//if (Alert_AwaitingMedicalOperation.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(allMaps_PrisonersOfColonySpawned[j]))
					//{
					//	this.awaitingMedicalOperationResult.Add(allMaps_PrisonersOfColonySpawned[j]);
					//}
				}
				return this.awaitingMedicalOperationResult;
			}
		}

		
		public override string GetLabel()
		{
			return "PatientsAwaitingMedicalOperation".Translate(this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		
		private List<Pawn> awaitingMedicalOperationResult = new List<Pawn>();
	}
}
