using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldLayer_Stars : WorldLayer
	{
		
		// (get) Token: 0x06006A38 RID: 27192 RVA: 0x00250F13 File Offset: 0x0024F113
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		
		// (get) Token: 0x06006A39 RID: 27193 RVA: 0x00250F1A File Offset: 0x0024F11A
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		
		// (get) Token: 0x06006A3A RID: 27194 RVA: 0x00250F50 File Offset: 0x0024F150
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		
		// (get) Token: 0x06006A3B RID: 27195 RVA: 0x00250F5A File Offset: 0x0024F15A
		protected override Quaternion Rotation
		{
			get
			{
				if (this.UseStaticRotation)
				{
					return Quaternion.identity;
				}
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
			for (int i = 0; i < 1500; i++)
			{
				Vector3 unitVector = Rand.UnitVector3;
				Vector3 pos = unitVector * 10f;
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Stars);
				float num = WorldLayer_Stars.StarsDrawSize.RandomInRange;
				Vector3 rhs = this.UseStaticRotation ? GenCelestial.CurSunPositionInWorldSpace().normalized : Vector3.forward;
				float num2 = Vector3.Dot(unitVector, rhs);
				if (num2 > 0.8f)
				{
					num *= GenMath.LerpDouble(0.8f, 1f, 1f, 0.35f, num2);
				}
				WorldRendererUtility.PrintQuadTangentialToPlanet(pos, num, 0f, subMesh, true, true, true);
			}
			this.calculatedForStartingTile = ((Find.GameInitData != null) ? Find.GameInitData.startingTile : -1);
			this.calculatedForStaticRotation = this.UseStaticRotation;
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		
		private bool calculatedForStaticRotation;

		
		private int calculatedForStartingTile = -1;

		
		public const float DistanceToStars = 10f;

		
		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		
		private const int StarsCount = 1500;

		
		private const float DistToSunToReduceStarSize = 0.8f;
	}
}
