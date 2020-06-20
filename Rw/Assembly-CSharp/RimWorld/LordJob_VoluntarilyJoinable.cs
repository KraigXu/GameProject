using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000782 RID: 1922
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x0600324C RID: 12876 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
