using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019E RID: 414
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06000BB8 RID: 3000 RVA: 0x0004286C File Offset: 0x00040A6C
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0004287D File Offset: 0x00040A7D
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00042888 File Offset: 0x00040A88
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
				}
			}
		}
	}
}
