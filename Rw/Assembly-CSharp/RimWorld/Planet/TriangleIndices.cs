using System;

namespace RimWorld.Planet
{
	// Token: 0x0200120D RID: 4621
	public struct TriangleIndices
	{
		// Token: 0x06006AE8 RID: 27368 RVA: 0x00254CF0 File Offset: 0x00252EF0
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x00254D08 File Offset: 0x00252F08
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x00254DB7 File Offset: 0x00252FB7
		public int GetNextOrderedVertex(int root)
		{
			if (this.v1 == root)
			{
				return this.v2;
			}
			if (this.v2 == root)
			{
				return this.v3;
			}
			return this.v1;
		}

		// Token: 0x040042CE RID: 17102
		public int v1;

		// Token: 0x040042CF RID: 17103
		public int v2;

		// Token: 0x040042D0 RID: 17104
		public int v3;
	}
}
