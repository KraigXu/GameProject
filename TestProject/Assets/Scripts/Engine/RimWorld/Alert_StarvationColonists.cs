using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_StarvationColonists : Alert
	{
		
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		// (get) Token: 0x06005662 RID: 22114 RVA: 0x001CA3D4 File Offset: 0x001C85D4
		private List<Pawn> StarvingColonists
		{
			get
			{
				this.starvingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (pawn.needs.food != null && pawn.needs.food.Starving)
					{
						this.starvingColonistsResult.Add(pawn);
					}
				}
				return this.starvingColonistsResult;
			}
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "StarvationDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}

		
		private List<Pawn> starvingColonistsResult = new List<Pawn>();
	}
}
