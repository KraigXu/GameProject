using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011F1 RID: 4593
	public class WorldLayer_Sun : WorldLayer
	{
		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06006A40 RID: 27200 RVA: 0x00250F13 File Offset: 0x0024F113
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06006A41 RID: 27201 RVA: 0x00250FA9 File Offset: 0x0024F1A9
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x00250FB5 File Offset: 0x0024F1B5
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.<>n__0())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Sun);
			WorldRendererUtility.PrintQuadTangentialToPlanet(Vector3.forward * 10f, 15f, 0f, subMesh, true, false, true);
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x04004237 RID: 16951
		private const float SunDrawSize = 15f;
	}
}
