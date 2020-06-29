using System;
using System.Collections.Generic;

namespace Verse
{
	
	public struct EdgeSpan
	{
		
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000479DC File Offset: 0x00045BDC
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		
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

		
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		
		public IntVec3 root;

		
		public SpanDirection dir;

		
		public int length;
	}
}
