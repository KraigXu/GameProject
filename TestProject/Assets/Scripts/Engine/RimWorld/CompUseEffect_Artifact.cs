using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D9D RID: 3485
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x001C3A25 File Offset: 0x001C1C25
		public CompProperties_UseEffectArtifact Props
		{
			get
			{
				return (CompProperties_UseEffectArtifact)this.props;
			}
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x001C3A34 File Offset: 0x001C1C34
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(this.parent.Position, usedBy.MapHeld, false));
			}
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
