using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011BC RID: 4540
	public class WorldReachability
	{
		// Token: 0x060068FB RID: 26875 RVA: 0x0024ABDD File Offset: 0x00248DDD
		public WorldReachability()
		{
			this.fields = new int[Find.WorldGrid.TilesCount];
			this.nextFieldID = 1;
			this.InvalidateAllFields();
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x0024AC07 File Offset: 0x00248E07
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x060068FD RID: 26877 RVA: 0x0024AC0F File Offset: 0x00248E0F
		public bool CanReach(Caravan c, int tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x0024AC20 File Offset: 0x00248E20
		public bool CanReach(int startTile, int destTile)
		{
			if (startTile < 0 || startTile >= this.fields.Length || destTile < 0 || destTile >= this.fields.Length)
			{
				return false;
			}
			if (this.fields[startTile] == this.impassableFieldID || this.fields[destTile] == this.impassableFieldID)
			{
				return false;
			}
			if (this.IsValidField(this.fields[startTile]) || this.IsValidField(this.fields[destTile]))
			{
				return this.fields[startTile] == this.fields[destTile];
			}
			this.FloodFillAt(startTile);
			return this.fields[startTile] != this.impassableFieldID && this.fields[startTile] == this.fields[destTile];
		}

		// Token: 0x060068FF RID: 26879 RVA: 0x0024ACCD File Offset: 0x00248ECD
		private void InvalidateAllFields()
		{
			if (this.nextFieldID == 2147483646)
			{
				this.nextFieldID = 1;
			}
			this.minValidFieldID = this.nextFieldID;
			this.impassableFieldID = this.nextFieldID;
			this.nextFieldID++;
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x0024AD09 File Offset: 0x00248F09
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x0024AD18 File Offset: 0x00248F18
		private void FloodFillAt(int tile)
		{
			World world = Find.World;
			if (world.Impassable(tile))
			{
				this.fields[tile] = this.impassableFieldID;
				return;
			}
			Find.WorldFloodFiller.FloodFill(tile, (int x) => !world.Impassable(x), delegate(int x)
			{
				this.fields[x] = this.nextFieldID;
			}, int.MaxValue, null);
			this.nextFieldID++;
		}

		// Token: 0x04004154 RID: 16724
		private int[] fields;

		// Token: 0x04004155 RID: 16725
		private int nextFieldID;

		// Token: 0x04004156 RID: 16726
		private int impassableFieldID;

		// Token: 0x04004157 RID: 16727
		private int minValidFieldID;
	}
}
