using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F2 RID: 1778
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002F1D RID: 12061 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanDoDuringGathering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x00108F8A File Offset: 0x0010718A
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x00108FAC File Offset: 0x001071AC
		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && !c.Standable(t.Map))
				{
					return false;
				}
			}
			return true;
		}
	}
}
