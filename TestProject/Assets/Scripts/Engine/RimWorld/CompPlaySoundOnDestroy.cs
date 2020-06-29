using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompPlaySoundOnDestroy : ThingComp
	{
		
		
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
