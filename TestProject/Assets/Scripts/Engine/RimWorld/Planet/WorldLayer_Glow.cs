using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E7 RID: 4583
	public class WorldLayer_Glow : WorldLayer
	{
		// Token: 0x06006A14 RID: 27156 RVA: 0x002503E9 File Offset: 0x0024E5E9
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.<>n__0())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			List<Vector3> collection;
			List<int> collection2;
			SphereGenerator.Generate(4, 108.1f, Vector3.forward, 360f, out collection, out collection2);
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.PlanetGlow);
			subMesh.verts.AddRange(collection);
			subMesh.tris.AddRange(collection2);
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x04004218 RID: 16920
		private const int SubdivisionsCount = 4;

		// Token: 0x04004219 RID: 16921
		public const float GlowRadius = 8f;
	}
}
