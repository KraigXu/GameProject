using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A3B RID: 2619
	public struct CellWithRadius : IEquatable<CellWithRadius>
	{
		// Token: 0x06003DDF RID: 15839 RVA: 0x00146552 File Offset: 0x00144752
		public CellWithRadius(IntVec3 c, float r)
		{
			this.cell = c;
			this.radius = r;
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00146564 File Offset: 0x00144764
		public bool Equals(CellWithRadius other)
		{
			return this.cell.Equals(other.cell) && this.radius.Equals(other.radius);
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x001465A0 File Offset: 0x001447A0
		public override bool Equals(object obj)
		{
			if (obj is CellWithRadius)
			{
				CellWithRadius other = (CellWithRadius)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x001465C8 File Offset: 0x001447C8
		public override int GetHashCode()
		{
			return this.cell.GetHashCode() * 397 ^ this.radius.GetHashCode();
		}

		// Token: 0x0400241B RID: 9243
		public readonly IntVec3 cell;

		// Token: 0x0400241C RID: 9244
		public readonly float radius;
	}
}
