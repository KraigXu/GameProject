using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000724 RID: 1828
	public abstract class WorkGiver_ConstructAffectFloor : WorkGiver_Scanner
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003017 RID: 12311
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x0010E7CF File Offset: 0x0010C9CF
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.DesDef))
			{
				yield return designation.target.Cell;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x0010E7E6 File Offset: 0x0010C9E6
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.DesDef);
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x0010E801 File Offset: 0x0010CA01
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return !c.IsForbidden(pawn) && pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced);
		}
	}
}
