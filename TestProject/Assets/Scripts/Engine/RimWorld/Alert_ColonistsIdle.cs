using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_ColonistsIdle : Alert
	{
		
		
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

		
		public override string GetLabel()
		{
			return "ColonistsIdle".Translate(this.IdleColonists.Count.ToStringCached());
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsIdleDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		
		public const int MinDaysPassed = 1;

		
		private List<Pawn> idleColonistsResult = new List<Pawn>();
	}
}
