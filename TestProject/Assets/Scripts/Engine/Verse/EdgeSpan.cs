using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001BF RID: 447
	public struct EdgeSpan
	{
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000479DC File Offset: 0x00045BDC
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x000479E7 File Offset: 0x00045BE7
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.length; i = num + 1)
				{
					if (this.dir == SpanDirection.North)
					{
						yield return new IntVec3(this.root.x, 0, this.root.z + i);
					}
					else if (this.dir == SpanDirection.East)
					{
						yield return new IntVec3(this.root.x + i, 0, this.root.z);
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000479FC File Offset: 0x00045BFC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(root=",
				this.root,
				", dir=",
				this.dir.ToString(),
				" + length=",
				this.length,
				")"
			});
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00047A64 File Offset: 0x00045C64
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00047A7C File Offset: 0x00045C7C
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x040009E4 RID: 2532
		public IntVec3 root;

		// Token: 0x040009E5 RID: 2533
		public SpanDirection dir;

		// Token: 0x040009E6 RID: 2534
		public int length;
	}
}
