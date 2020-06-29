using System;
using UnityEngine;

namespace Verse
{
	
	public class ColorForStuff
	{
		
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001751B File Offset: 0x0001571B
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00017523 File Offset: 0x00015723
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		private ThingDef stuff;

		
		private Color color = Color.white;
	}
}
