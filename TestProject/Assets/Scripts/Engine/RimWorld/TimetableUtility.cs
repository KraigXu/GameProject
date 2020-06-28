using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB4 RID: 2996
	public static class TimetableUtility
	{
		// Token: 0x060046C3 RID: 18115 RVA: 0x0017EF11 File Offset: 0x0017D111
		public static TimeAssignmentDef GetTimeAssignment(this Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return TimeAssignmentDefOf.Anything;
			}
			return pawn.timetable.CurrentAssignment;
		}
	}
}
