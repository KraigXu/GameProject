using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000DA6 RID: 3494
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x060054DC RID: 21724 RVA: 0x001C4831 File Offset: 0x001C2A31
		private CompProperties_UseEffectPlaySound Props
		{
			get
			{
				return (CompProperties_UseEffectPlaySound)this.props;
			}
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x001C483E File Offset: 0x001C2A3E
		public override void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap && this.Props.soundOnUsed != null)
			{
				this.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(usedBy, MaintenanceType.None));
			}
		}
	}
}
