using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class WorldLayer_SingleTile : WorldLayer
	{
		
		// (get) Token: 0x06006A32 RID: 27186
		protected abstract int Tile { get; }

		
		// (get) Token: 0x06006A33 RID: 27187
		protected abstract Material Material { get; }

		
		// (get) Token: 0x06006A34 RID: 27188 RVA: 0x00250ECC File Offset: 0x0024F0CC
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnTile;
			}
		}

		
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.n__0())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			int tile = this.Tile;
			if (tile >= 0)
			{
				LayerSubMesh subMesh = base.GetSubMesh(this.Material);
				Find.WorldGrid.GetTileVertices(tile, this.verts);
				int count = subMesh.verts.Count;
				int i = 0;
				int count2 = this.verts.Count;
				while (i < count2)
				{
					subMesh.verts.Add(this.verts[i] + this.verts[i].normalized * 0.012f);
					subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, i) + Vector2.one) / 2f);
					if (i < count2 - 2)
					{
						subMesh.tris.Add(count + i + 2);
						subMesh.tris.Add(count + i + 1);
						subMesh.tris.Add(count);
					}
					i++;
				}
				base.FinalizeMesh(MeshParts.All);
			}
			this.lastDrawnTile = tile;
			yield break;
			yield break;
		}

		
		private int lastDrawnTile = -1;

		
		private List<Vector3> verts = new List<Vector3>();
	}
}
