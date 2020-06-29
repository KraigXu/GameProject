using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		
		
		private CompProperties_UseEffectPlaySound Props
		{
			get
			{
				return (CompProperties_UseEffectPlaySound)this.props;
			}
		}

		
		public override void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap && this.Props.soundOnUsed != null)
			{
				this.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(usedBy, MaintenanceType.None));
			}
		}
	}
}
