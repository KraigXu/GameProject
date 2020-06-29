using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_NeedDoctor : Alert
	{
		
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		
		private List<Pawn> Patients
		{
			get
			{
				this.patientsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						bool flag = false;
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							foreach (Pawn pawn2 in maps[i].mapPawns.FreeColonistsSpawned)
							{
								if ((pawn2.Downed && pawn2.needs.food.CurCategory < HungerCategory.Fed && pawn2.InBed()) || HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2))
								{
									this.patientsResult.Add(pawn2);
								}
							}
						}
					}
				}
				return this.patientsResult;
			}
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "NeedDoctorDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Patients);
		}

		
		private List<Pawn> patientsResult = new List<Pawn>();
	}
}
