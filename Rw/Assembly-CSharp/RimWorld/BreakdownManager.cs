using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF7 RID: 3319
	public class BreakdownManager : MapComponent
	{
		// Token: 0x060050AF RID: 20655 RVA: 0x001B1B6F File Offset: 0x001AFD6F
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x001B1B8E File Offset: 0x001AFD8E
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x001B1BB6 File Offset: 0x001AFDB6
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001B1BD8 File Offset: 0x001AFDD8
		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 1041 == 0)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CheckForBreakdown();
				}
			}
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001B1C1E File Offset: 0x001AFE1E
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x001B1C2D File Offset: 0x001AFE2D
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}

		// Token: 0x04002CD1 RID: 11473
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x04002CD2 RID: 11474
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x04002CD3 RID: 11475
		public const int CheckIntervalTicks = 1041;
	}
}
