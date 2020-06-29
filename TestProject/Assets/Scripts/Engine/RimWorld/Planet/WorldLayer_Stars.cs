﻿using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldLayer_Stars : WorldLayer
	{
		
		
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		
		
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		
		
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		
		
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
			foreach (object obj in this.Regenerate())
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
