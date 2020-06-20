using System;

namespace Verse
{
	// Token: 0x020000E0 RID: 224
	public class RoofDef : Def
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x04000544 RID: 1348
		public bool isNatural;

		// Token: 0x04000545 RID: 1349
		public bool isThickRoof;

		// Token: 0x04000546 RID: 1350
		public ThingDef collapseLeavingThingDef;

		// Token: 0x04000547 RID: 1351
		public ThingDef filthLeaving;

		// Token: 0x04000548 RID: 1352
		public SoundDef soundPunchThrough;
	}
}
