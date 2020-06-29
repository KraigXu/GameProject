using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_ChangeNeed : HediffComp
	{
		
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0005D2DF File Offset: 0x0005B4DF
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		
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

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f;
			}
		}

		
		private Need needCached;
	}
}
