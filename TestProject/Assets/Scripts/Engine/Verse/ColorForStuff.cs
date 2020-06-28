using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200007A RID: 122
	public class ColorForStuff
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001751B File Offset: 0x0001571B
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00017523 File Offset: 0x00015723
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x040001B0 RID: 432
		private ThingDef stuff;

		// Token: 0x040001B1 RID: 433
		private Color color = Color.white;
	}
}
