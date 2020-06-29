using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public sealed class FertilityGrid
	{
		
		
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3610, 0.33f);
				}
				return this.drawerInt;
			}
		}

		
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			if (edifice != null && edifice.def.AffectsFertility)
			{
				return edifice.def.fertility;
			}
			return this.map.terrainGrid.TerrainAt(loc).fertility;
		}

		
		public void FertilityGridUpdate()
		{
			if (Find.PlaySettings.showFertilityOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.FertilityAt(intVec) > 0.69f;
		}

		
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			float num = this.FertilityAt(CellIndicesUtility.IndexToCell(index, this.map.Size.x));
			if (num <= 0.95f)
			{
				return FertilityGrid.LowFertilityColor;
			}
			if (num <= 1.1f)
			{
				return FertilityGrid.MediumFertilityColor;
			}
			if (num >= 1.1f)
			{
				return FertilityGrid.HighFertilityColor;
			}
			return Color.white;
		}

		
		private Map map;

		
		private CellBoolDrawer drawerInt;

		
		private static readonly Color MediumFertilityColor = new Color(0.59f, 0.98f, 0.59f, 1f);

		
		private static readonly Color LowFertilityColor = Color.yellow;

		
		private static readonly Color HighFertilityColor = Color.green;
	}
}
