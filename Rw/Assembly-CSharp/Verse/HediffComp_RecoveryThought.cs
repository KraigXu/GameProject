using System;

namespace Verse
{
	// Token: 0x0200026A RID: 618
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0005EF09 File Offset: 0x0005D109
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0005EF18 File Offset: 0x0005D118
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
