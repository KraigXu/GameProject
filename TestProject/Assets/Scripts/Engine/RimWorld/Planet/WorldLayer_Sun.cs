using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldLayer_Sun : WorldLayer
	{
		
		// (get) Token: 0x06006A40 RID: 27200 RVA: 0x00250F13 File Offset: 0x0024F113
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		
		// (get) Token: 0x06006A41 RID: 27201 RVA: 0x00250FA9 File Offset: 0x0024F1A9
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.n__0())
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

		
		private const float SunDrawSize = 15f;
	}
}
