using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D39 RID: 3385
	public class CompPlaySoundOnDestroy : ThingComp
	{
		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x001B7725 File Offset: 0x001B5925
		private CompProperties_PlaySoundOnDestroy Props
		{
			get
			{
				return (CompProperties_PlaySoundOnDestroy)this.props;
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x001B7732 File Offset: 0x001B5932
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				this.Props.sound.PlayOneShotOnCamera(previousMap);
			}
		}
	}
}
