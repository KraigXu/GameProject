using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x000332B3 File Offset: 0x000314B3
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000332C0 File Offset: 0x000314C0
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		
		// (get) Token: 0x06000950 RID: 2384
		public abstract string Label { get; }

		
		// (get) Token: 0x06000951 RID: 2385
		public abstract Color Color { get; }

		
		// (get) Token: 0x06000952 RID: 2386
		public abstract int ListPriority { get; }

		
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

		
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x00033395 File Offset: 0x00031595
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		
		public Area()
		{
		}

		
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		
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

		
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		
		public abstract string GetUniqueLoadID();

		
		public AreaManager areaManager;

		
		public int ID = -1;

		
		private BoolGrid innerGrid;

		
		private CellBoolDrawer drawer;

		
		private Texture2D colorTextureInt;
	}
}
