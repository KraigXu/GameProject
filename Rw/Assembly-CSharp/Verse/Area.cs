using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200014C RID: 332
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x000332B3 File Offset: 0x000314B3
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000332C0 File Offset: 0x000314C0
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000950 RID: 2384
		public abstract string Label { get; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000951 RID: 2385
		public abstract Color Color { get; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000952 RID: 2386
		public abstract int ListPriority { get; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x000332CD File Offset: 0x000314CD
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.Color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x170001D3 RID: 467
		public bool this[int index]
		{
			get
			{
				return this.innerGrid[index];
			}
			set
			{
				this.Set(this.Map.cellIndices.IndexToCell(index), value);
			}
		}

		// Token: 0x170001D4 RID: 468
		public bool this[IntVec3 c]
		{
			get
			{
				return this.innerGrid[this.Map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00033344 File Offset: 0x00031544
		private CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new CellBoolDrawer(this, this.Map.Size.x, this.Map.Size.z, 3650, 0.33f);
				}
				return this.drawer;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x00033395 File Offset: 0x00031595
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000333A2 File Offset: 0x000315A2
		public Area()
		{
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000333B1 File Offset: 0x000315B1
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x000333E8 File Offset: 0x000315E8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000332F4 File Offset: 0x000314F4
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00017A00 File Offset: 0x00015C00
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00033414 File Offset: 0x00031614
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] == val)
			{
				return;
			}
			this.innerGrid[index] = val;
			this.MarkDirty(c);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00033458 File Offset: 0x00031658
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00033488 File Offset: 0x00031688
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00033496 File Offset: 0x00031696
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000334B0 File Offset: 0x000316B0
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x000334BD File Offset: 0x000316BD
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x06000968 RID: 2408
		public abstract string GetUniqueLoadID();

		// Token: 0x040007C3 RID: 1987
		public AreaManager areaManager;

		// Token: 0x040007C4 RID: 1988
		public int ID = -1;

		// Token: 0x040007C5 RID: 1989
		private BoolGrid innerGrid;

		// Token: 0x040007C6 RID: 1990
		private CellBoolDrawer drawer;

		// Token: 0x040007C7 RID: 1991
		private Texture2D colorTextureInt;
	}
}
