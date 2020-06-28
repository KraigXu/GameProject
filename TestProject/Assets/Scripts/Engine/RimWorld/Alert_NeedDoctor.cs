using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE2 RID: 3554
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x06005636 RID: 22070 RVA: 0x001C912D File Offset: 0x001C732D
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005637 RID: 22071 RVA: 0x001C915C File Offset: 0x001C735C
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

		// Token: 0x06005638 RID: 22072 RVA: 0x001C92A4 File Offset: 0x001C74A4
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "NeedDoctorDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x001C932C File Offset: 0x001C752C
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Patients);
		}

		// Token: 0x04002F1D RID: 12061
		private List<Pawn> patientsResult = new List<Pawn>();
	}
}
