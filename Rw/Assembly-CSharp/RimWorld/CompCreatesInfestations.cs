using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFD RID: 3325
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x001B2338 File Offset: 0x001B0538
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedLastTick()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x001B2373 File Offset: 0x001B0573
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 420000;
			}
		}

		// Token: 0x17000E33 RID: 3635
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

		// Token: 0x060050DC RID: 20700 RVA: 0x001B2420 File Offset: 0x001B0620
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x001B2438 File Offset: 0x001B0638
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04002CE3 RID: 11491
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x04002CE4 RID: 11492
		private const float MinRefireDays = 7f;

		// Token: 0x04002CE5 RID: 11493
		private const float PreventInfestationsDist = 10f;
	}
}
