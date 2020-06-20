using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED6 RID: 3798
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06005D18 RID: 23832 RVA: 0x0020441F File Offset: 0x0020261F
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x00204429 File Offset: 0x00202629
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x0020443D File Offset: 0x0020263D
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x00204454 File Offset: 0x00202654
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}

		// Token: 0x040032BC RID: 12988
		private static List<TimeAssignmentDef> clipboard;
	}
}
