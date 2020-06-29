using System;
using Verse;

namespace RimWorld
{
	
	public struct MeditationSpotAndFocus
	{
		
		
		public bool IsValid
		{
			get
			{
				return this.spot.IsValid;
			}
		}

		
		public MeditationSpotAndFocus(LocalTargetInfo spot)
		{
			this.spot = spot;
			this.focus = LocalTargetInfo.Invalid;
		}

		
		public MeditationSpotAndFocus(LocalTargetInfo spot, LocalTargetInfo focus)
		{
			this.spot = spot;
			this.focus = focus;
		}

		
		public LocalTargetInfo spot;

		
		public LocalTargetInfo focus;
	}
}
