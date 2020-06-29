using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_StarvationAnimals : Alert
	{
		
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		
		// (get) Token: 0x060056B3 RID: 22195 RVA: 0x001CBE90 File Offset: 0x001CA090
		private List<Pawn> StarvingAnimals
		{
			get
			{
				this.starvingAnimalsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep)
				{
					if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && pawn.needs.food != null && (pawn.needs.food.TicksStarving > 30000 || (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true) && pawn.needs.food.TicksStarving > 5000)))
					{
						this.starvingAnimalsResult.Add(pawn);
					}
				}
				return this.starvingAnimalsResult;
			}
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in from a in this.StarvingAnimals
			orderby a.def.label
			select a)
			{
				stringBuilder.Append("    " + pawn.LabelShortCap);
				if (pawn.Name.IsValid && !pawn.Name.Numerical)
				{
					stringBuilder.Append(" (" + pawn.def.label + ")");
				}
				stringBuilder.AppendLine();
			}
			return "StarvationAnimalsDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}

		
		private List<Pawn> starvingAnimalsResult = new List<Pawn>();
	}
}
