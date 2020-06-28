using System;

namespace Verse
{
	// Token: 0x02000254 RID: 596
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0005DD81 File Offset: 0x0005BF81
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0005DD90 File Offset: 0x0005BF90
		public EffecterDef CurrentStateEffecter()
		{
			if (this.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || this.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				return this.Props.stateEffecter;
			}
			return null;
		}
	}
}
