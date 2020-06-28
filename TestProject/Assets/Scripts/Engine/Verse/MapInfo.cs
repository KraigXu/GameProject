using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001A1 RID: 417
	public sealed class MapInfo : IExposable
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00042DE4 File Offset: 0x00040FE4
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00042DF1 File Offset: 0x00040FF1
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00042E16 File Offset: 0x00041016
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x00042E1E File Offset: 0x0004101E
		public IntVec3 Size
		{
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00042E28 File Offset: 0x00041028
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x0400095D RID: 2397
		private IntVec3 sizeInt;

		// Token: 0x0400095E RID: 2398
		public MapParent parent;
	}
}
