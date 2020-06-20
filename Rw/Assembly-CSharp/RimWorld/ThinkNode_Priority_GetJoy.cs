using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C3 RID: 1987
	public class ThinkNode_Priority_GetJoy : ThinkNode_Priority
	{
		// Token: 0x06003377 RID: 13175 RVA: 0x0011D7CC File Offset: 0x0011B9CC
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				return 0f;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn))
			{
				return 0f;
			}
			float curLevel = pawn.needs.joy.CurLevel;
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
			if (!timeAssignmentDef.allowJoy)
			{
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.35f)
				{
					return 6f;
				}
				return 0f;
			}
			else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				if (curLevel < 0.95f)
				{
					return 7f;
				}
				return 0f;
			}
			else if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				if (curLevel < 0.95f)
				{
					return 2f;
				}
				return 0f;
			}
			else
			{
				if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
				{
					return 0f;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x04001BA5 RID: 7077
		private const int GameStartNoJoyTicks = 5000;
	}
}
