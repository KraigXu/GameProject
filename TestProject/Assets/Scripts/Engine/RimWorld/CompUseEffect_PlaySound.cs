using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		
		// (get) Token: 0x060054DC RID: 21724 RVA: 0x001C4831 File Offset: 0x001C2A31
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
