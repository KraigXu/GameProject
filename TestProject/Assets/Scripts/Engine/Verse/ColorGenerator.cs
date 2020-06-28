using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000080 RID: 128
	public abstract class ColorGenerator
	{
		// Token: 0x170000D0 RID: 208
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

		// Token: 0x060004AA RID: 1194
		public abstract Color NewRandomizedColor();
	}
}
