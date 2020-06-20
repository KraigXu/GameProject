using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF1 RID: 3569
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005676 RID: 22134 RVA: 0x001CAAB4 File Offset: 0x001C8CB4
		private List<Pawn> IdleColonists
		{
			get
			{
				this.idleColonistsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (pawn.mindState.IsIdle)
							{
								if (pawn.royalty != null)
								{
									RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
									if (mostSeniorTitle == null || !mostSeniorTitle.def.suppressIdleAlert)
									{
										this.idleColonistsResult.Add(pawn);
									}
								}
								else
								{
									this.idleColonistsResult.Add(pawn);
								}
							}
						}
					}
				}
				return this.idleColonistsResult;
			}
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x001CAB98 File Offset: 0x001C8D98
		public override string GetLabel()
		{
			return "ColonistsIdle".Translate(this.IdleColonists.Count.ToStringCached());
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x001CABC0 File Offset: 0x001C8DC0
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsIdleDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x001CAC48 File Offset: 0x001C8E48
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		// Token: 0x04002F30 RID: 12080
		public const int MinDaysPassed = 1;

		// Token: 0x04002F31 RID: 12081
		private List<Pawn> idleColonistsResult = new List<Pawn>();
	}
}
