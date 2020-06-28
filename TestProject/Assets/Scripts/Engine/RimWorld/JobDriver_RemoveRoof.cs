using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000636 RID: 1590
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002B8F RID: 11151 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000FB530 File Offset: 0x000F9730
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.NoRoof[base.Cell]);
			foreach (Toil toil in this.<>n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000FB540 File Offset: 0x000F9740
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true, false);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000FB59A File Offset: 0x000F979A
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		// Token: 0x040019A5 RID: 6565
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
