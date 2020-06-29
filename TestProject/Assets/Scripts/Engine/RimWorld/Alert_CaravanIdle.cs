﻿using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_CaravanIdle : Alert
	{
		
		// (get) Token: 0x0600567A RID: 22138 RVA: 0x001CAC64 File Offset: 0x001C8E64
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
