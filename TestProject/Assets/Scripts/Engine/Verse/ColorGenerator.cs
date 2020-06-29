using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class ColorGenerator
	{
		
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x000179E9 File Offset: 0x00015BE9
		public virtual Color ExemplaryColor
		{
			get
			{
				Rand.PushState(764543439);
				Color result = this.NewRandomizedColor();
				Rand.PopState();
				return result;
			}
		}

		
		public abstract Color NewRandomizedColor();
	}
}
