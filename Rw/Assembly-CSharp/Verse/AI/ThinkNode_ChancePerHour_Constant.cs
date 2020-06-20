using System;

namespace Verse.AI
{
	// Token: 0x02000587 RID: 1415
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x0600284F RID: 10319 RVA: 0x000EE867 File Offset: 0x000ECA67
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			thinkNode_ChancePerHour_Constant.mtbDays = this.mtbDays;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000EE88D File Offset: 0x000ECA8D
		protected override float MtbHours(Pawn Pawn)
		{
			if (this.mtbDays > 0f)
			{
				return this.mtbDays * 24f;
			}
			return this.mtbHours;
		}

		// Token: 0x0400183B RID: 6203
		private float mtbHours = -1f;

		// Token: 0x0400183C RID: 6204
		private float mtbDays = -1f;
	}
}
