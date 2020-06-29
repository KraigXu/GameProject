using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompUseEffect_Artifact : CompUseEffect
	{
		
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x001C3A25 File Offset: 0x001C1C25
		public CompProperties_UseEffectArtifact Props
		{
			get
			{
				return (CompProperties_UseEffectArtifact)this.props;
			}
		}

		
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
