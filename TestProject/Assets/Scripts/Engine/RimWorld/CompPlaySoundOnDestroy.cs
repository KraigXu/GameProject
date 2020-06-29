using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompPlaySoundOnDestroy : ThingComp
	{
		
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x001B7725 File Offset: 0x001B5925
		private CompProperties_PlaySoundOnDestroy Props
		{
			get
			{
				return (CompProperties_PlaySoundOnDestroy)this.props;
			}
		}

		
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				this.Props.sound.PlayOneShotOnCamera(previousMap);
			}
		}
	}
}
