using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_CaravanIdle : Alert
	{
		
		
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

		
		public override string GetLabel()
		{
			return "CaravanIdle".Translate();
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Caravan caravan in this.IdleCaravans)
			{
				stringBuilder.AppendLine("  - " + caravan.Label);
			}
			return "CaravanIdleDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.IdleCaravans);
		}

		
		private List<Caravan> idleCaravansResult = new List<Caravan>();
	}
}
