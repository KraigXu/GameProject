using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011F0 RID: 4592
	public class WorldLayer_Stars : WorldLayer
	{
		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x06006A38 RID: 27192 RVA: 0x00250F13 File Offset: 0x0024F113
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x06006A39 RID: 27193 RVA: 0x00250F1A File Offset: 0x0024F11A
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06006A3A RID: 27194 RVA: 0x00250F50 File Offset: 0x0024F150
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		// Token: 0x170011CF RID: 4559
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

		// Token: 0x06006A3C RID: 27196 RVA: 0x00250F74 File Offset: 0x0024F174
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.<>n__0())
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

		// Token: 0x04004231 RID: 16945
		private bool calculatedForStaticRotation;

		// Token: 0x04004232 RID: 16946
		private int calculatedForStartingTile = -1;

		// Token: 0x04004233 RID: 16947
		public const float DistanceToStars = 10f;

		// Token: 0x04004234 RID: 16948
		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		// Token: 0x04004235 RID: 16949
		private const int StarsCount = 1500;

		// Token: 0x04004236 RID: 16950
		private const float DistToSunToReduceStarSize = 0.8f;
	}
}
