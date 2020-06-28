using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000085 RID: 133
	public class ColorOption
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x00017C2C File Offset: 0x00015E2C
		public Color RandomizedColor()
		{
			if (this.only.a >= 0f)
			{
				return this.only;
			}
			return new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00017CC3 File Offset: 0x00015EC3
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00017CCC File Offset: 0x00015ECC
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00017CD5 File Offset: 0x00015ED5
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x04000216 RID: 534
		public float weight = 1f;

		// Token: 0x04000217 RID: 535
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000218 RID: 536
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000219 RID: 537
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}
