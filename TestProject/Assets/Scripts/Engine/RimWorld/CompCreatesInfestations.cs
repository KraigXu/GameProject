using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompCreatesInfestations : ThingComp
	{
		
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x001B2338 File Offset: 0x001B0538
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedLastTick()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x001B2373 File Offset: 0x001B0573
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 420000;
			}
		}

		
		// (get) Token: 0x060050DB RID: 20699 RVA: 0x001B2390 File Offset: 0x001B0590
		public bool CantFireBecauseSomethingElseCreatedInfestationRecently
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this.parent && list[i].Position.InHorDistOf(this.parent.Position, 10f) && list[i].TryGetComp<CompCreatesInfestations>().CantFireBecauseCreatedInfestationRecently)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		
		private int lastCreatedInfestationTick = -999999;

		
		private const float MinRefireDays = 7f;

		
		private const float PreventInfestationsDist = 10f;
	}
}
