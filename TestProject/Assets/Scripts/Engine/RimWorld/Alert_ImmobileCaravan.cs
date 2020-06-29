﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		
		// (get) Token: 0x06005614 RID: 22036 RVA: 0x001C8A70 File Offset: 0x001C6C70
		private List<Caravan> ImmobileCaravans
		{
			get
			{
				this.immobileCaravansResult.Clear();
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (caravans[i].IsPlayerControlled && caravans[i].ImmobilizedByMass)
					{
						this.immobileCaravansResult.Add(caravans[i]);
					}
				}
				return this.immobileCaravansResult;
			}
		}

		
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}

		
		private List<Caravan> immobileCaravansResult = new List<Caravan>();
	}
}
