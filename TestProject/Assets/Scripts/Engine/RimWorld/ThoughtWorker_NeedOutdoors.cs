using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000801 RID: 2049
	public class ThoughtWorker_NeedOutdoors : ThoughtWorker
	{
		// Token: 0x06003409 RID: 13321 RVA: 0x0011E810 File Offset: 0x0011CA10
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.outdoors == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.HostFaction != null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.outdoors.CurCategory)
			{
			case OutdoorsCategory.Entombed:
				return ThoughtState.ActiveAtStage(0);
			case OutdoorsCategory.Trapped:
				return ThoughtState.ActiveAtStage(1);
			case OutdoorsCategory.CabinFeverSevere:
				return ThoughtState.ActiveAtStage(2);
			case OutdoorsCategory.CabinFeverLight:
				return ThoughtState.ActiveAtStage(3);
			case OutdoorsCategory.NeedFreshAir:
				return ThoughtState.ActiveAtStage(4);
			case OutdoorsCategory.Free:
				return ThoughtState.Inactive;
			default:
				throw new InvalidOperationException("Unknown OutdoorsCategory");
			}
		}
	}
}
