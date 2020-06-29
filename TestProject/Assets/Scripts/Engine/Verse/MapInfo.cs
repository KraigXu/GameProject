using System;
using RimWorld.Planet;

namespace Verse
{
	
	public sealed class MapInfo : IExposable
	{
		
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00042DE4 File Offset: 0x00040FE4
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00042DF1 File Offset: 0x00040FF1
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		
		private IntVec3 sizeInt;

		
		public MapParent parent;
	}
}
