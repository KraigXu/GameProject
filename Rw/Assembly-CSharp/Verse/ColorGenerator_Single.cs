using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000082 RID: 130
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x00017A0F File Offset: 0x00015C0F
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x04000213 RID: 531
		public Color color;
	}
}
