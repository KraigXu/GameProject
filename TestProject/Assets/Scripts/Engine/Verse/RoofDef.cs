using System;

namespace Verse
{
	
	public class RoofDef : Def
	{
		
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		
		public bool isNatural;

		
		public bool isThickRoof;

		
		public ThingDef collapseLeavingThingDef;

		
		public ThingDef filthLeaving;

		
		public SoundDef soundPunchThrough;
	}
}
