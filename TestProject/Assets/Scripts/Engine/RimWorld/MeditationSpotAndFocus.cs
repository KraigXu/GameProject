using System;
using Verse;

namespace RimWorld
{
	
	public struct MeditationSpotAndFocus
	{
		
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x0010B763 File Offset: 0x00109963
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
