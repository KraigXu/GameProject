using System;

namespace Verse
{
	
	public class HediffComp_Effecter : HediffComp
	{
		
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0005DD81 File Offset: 0x0005BF81
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		
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
