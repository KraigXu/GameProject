using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A38 RID: 2616
	public sealed class FertilityGrid
	{
		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x001461CC File Offset: 0x001443CC
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

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00146240 File Offset: 0x00144440
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0014624F File Offset: 0x0014444F
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00146258 File Offset: 0x00144458
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			if (edifice != null && edifice.def.AffectsFertility)
			{
				return edifice.def.fertility;
			}
			return this.map.terrainGrid.TerrainAt(loc).fertility;
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x001462A4 File Offset: 0x001444A4
		public void FertilityGridUpdate()
		{
			if (Find.PlaySettings.showFertilityOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00017A00 File Offset: 0x00015C00
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x001462C8 File Offset: 0x001444C8
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.FertilityAt(intVec) > 0.69f;
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x00146318 File Offset: 0x00144518
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

		// Token: 0x04002412 RID: 9234
		private Map map;

		// Token: 0x04002413 RID: 9235
		private CellBoolDrawer drawerInt;

		// Token: 0x04002414 RID: 9236
		private static readonly Color MediumFertilityColor = new Color(0.59f, 0.98f, 0.59f, 1f);

		// Token: 0x04002415 RID: 9237
		private static readonly Color LowFertilityColor = Color.yellow;

		// Token: 0x04002416 RID: 9238
		private static readonly Color HighFertilityColor = Color.green;
	}
}
