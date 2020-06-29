using System;
using Verse;

namespace RimWorld
{
	
	public struct CellWithRadius : IEquatable<CellWithRadius>
	{
		
		public CellWithRadius(IntVec3 c, float r)
		{
			this.cell = c;
			this.radius = r;
		}

		
		public bool Equals(CellWithRadius other)
		{
			return this.cell.Equals(other.cell) && this.radius.Equals(other.radius);
		}

		
		public override bool Equals(object obj)
		{
			if (obj is CellWithRadius)
			{
				CellWithRadius other = (CellWithRadius)obj;
				return this.Equals(other);
			}
			return false;
		}

		
		public override int GetHashCode()
		{
			return this.cell.GetHashCode() * 397 ^ this.radius.GetHashCode();
		}

		
		public readonly IntVec3 cell;

		
		public readonly float radius;
	}
}
