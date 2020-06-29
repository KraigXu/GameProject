using System;

namespace Verse
{
	
	public class HediffComp_RecoveryThought : HediffComp
	{
		
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0005EF09 File Offset: 0x0005D109
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null);
			}
		}
	}
}
