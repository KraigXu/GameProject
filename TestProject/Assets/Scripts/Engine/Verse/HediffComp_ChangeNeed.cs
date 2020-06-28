using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000246 RID: 582
	public class HediffComp_ChangeNeed : HediffComp
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0005D2DF File Offset: 0x0005B4DF
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0005D2EC File Offset: 0x0005B4EC
		private Need Need
		{
			get
			{
				if (this.needCached == null)
				{
					this.needCached = base.Pawn.needs.TryGetNeed(this.Props.needDef);
				}
				return this.needCached;
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0005D31D File Offset: 0x0005B51D
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f;
			}
		}

		// Token: 0x04000BE4 RID: 3044
		private Need needCached;
	}
}
