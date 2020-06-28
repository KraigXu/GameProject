using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000197 RID: 407
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x000414B0 File Offset: 0x0003F6B0
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00041AE2 File Offset: 0x0003FCE2
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00041B00 File Offset: 0x0003FD00
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			this.scats.RemoveAll((SectionLayer_TerrainScatter.Scatterable scat) => !scat.IsOnValidTerrain);
			int num = 0;
			TerrainDef[] topGrid = base.Map.terrainGrid.topGrid;
			CellRect cellRect = this.section.CellRect;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (topGrid[cellIndices.CellToIndex(j, i)].scatterType != null)
					{
						num++;
					}
				}
			}
			num /= 40;
			int num2 = 0;
			while (this.scats.Count < num && num2 < 200)
			{
				num2++;
				IntVec3 randomCell = this.section.CellRect.RandomCell;
				string terrScatType = base.Map.terrainGrid.TerrainAt(randomCell).scatterType;
				ScatterableDef def2;
				if (terrScatType != null && !randomCell.Filled(base.Map) && (from def in DefDatabase<ScatterableDef>.AllDefs
				where def.scatterType == terrScatType
				select def).TryRandomElement(out def2))
				{
					Vector3 loc = new Vector3((float)randomCell.x + Rand.Value, (float)randomCell.y, (float)randomCell.z + Rand.Value);
					SectionLayer_TerrainScatter.Scatterable scatterable = new SectionLayer_TerrainScatter.Scatterable(def2, loc, base.Map);
					this.scats.Add(scatterable);
					scatterable.PrintOnto(this);
				}
			}
			for (int k = 0; k < this.scats.Count; k++)
			{
				this.scats[k].PrintOnto(this);
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x04000952 RID: 2386
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x020013C6 RID: 5062
		private class Scatterable
		{
			// Token: 0x0600778E RID: 30606 RVA: 0x00291564 File Offset: 0x0028F764
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x0600778F RID: 30607 RVA: 0x002915B8 File Offset: 0x0028F7B8
			public void PrintOnto(SectionLayer layer)
			{
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, this.def.mat, this.rotation, false, null, null, 0.01f, 0f);
			}

			// Token: 0x17001444 RID: 5188
			// (get) Token: 0x06007790 RID: 30608 RVA: 0x00291600 File Offset: 0x0028F800
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 c = this.loc.ToIntVec3();
					TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
					return this.def.scatterType == terrainDef.scatterType && !c.Filled(this.map);
				}
			}

			// Token: 0x04004B2E RID: 19246
			private Map map;

			// Token: 0x04004B2F RID: 19247
			public ScatterableDef def;

			// Token: 0x04004B30 RID: 19248
			public Vector3 loc;

			// Token: 0x04004B31 RID: 19249
			public float size;

			// Token: 0x04004B32 RID: 19250
			public float rotation;
		}
	}
}
