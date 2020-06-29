using System;

namespace Verse
{
	
	public class RoofDef : Def
	{
		
		
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
