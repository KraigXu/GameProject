using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A7C RID: 2684
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003F5B RID: 16219 RVA: 0x00150AFD File Offset: 0x0014ECFD
		protected override float DesiredPowerOutput
		{
			get
			{
				if (this.cacheDirty)
				{
					this.RebuildCache();
				}
				if (!this.waterUsable)
				{
					return 0f;
				}
				if (this.waterDoubleUsed)
				{
					return base.DesiredPowerOutput * 0.3f;
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x00150B36 File Offset: 0x0014ED36
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			this.RebuildCache();
			this.ForceOthersToRebuildCache(this.parent.Map);
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00150B6B File Offset: 0x0014ED6B
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.ForceOthersToRebuildCache(map);
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x00150B7B File Offset: 0x0014ED7B
		private void ClearCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x00150B84 File Offset: 0x0014ED84
		private void RebuildCache()
		{
			this.waterUsable = true;
			foreach (IntVec3 c in this.WaterCells())
			{
				if (c.InBounds(this.parent.Map) && !this.parent.Map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					this.waterUsable = false;
					break;
				}
			}
			this.waterDoubleUsed = false;
			IEnumerable<Building> enumerable = this.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator);
			foreach (IntVec3 c2 in this.WaterUseCells())
			{
				if (c2.InBounds(this.parent.Map))
				{
					foreach (Building building in enumerable)
					{
						if (building != this.parent && building.GetComp<CompPowerPlantWater>().WaterUseRect().Contains(c2))
						{
							this.waterDoubleUsed = true;
							break;
						}
					}
				}
			}
			if (!this.waterUsable)
			{
				this.spinRate = 0f;
				return;
			}
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterCells())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinRate = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinRate *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
			if (this.waterDoubleUsed)
			{
				this.spinRate *= 0.5f;
			}
			this.cacheDirty = false;
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x00150DE4 File Offset: 0x0014EFE4
		private void ForceOthersToRebuildCache(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator))
			{
				building.GetComp<CompPowerPlantWater>().ClearCache();
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x00150E40 File Offset: 0x0014F040
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + 0.006666667f * this.spinRate + 6.28318548f) % 6.28318548f;
			}
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x00150E7A File Offset: 0x0014F07A
		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x00150E97 File Offset: 0x0014F097
		public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc + rot.FacingCell * 3;
			yield return loc + rot.FacingCell * 3 - perpOffset;
			yield return loc + rot.FacingCell * 3 - perpOffset * 2;
			yield return loc + rot.FacingCell * 3 + perpOffset;
			yield return loc + rot.FacingCell * 3 + perpOffset * 2;
			yield break;
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x00150EAE File Offset: 0x0014F0AE
		public CellRect WaterUseRect()
		{
			return CompPowerPlantWater.WaterUseRect(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00150ECC File Offset: 0x0014F0CC
		public static CellRect WaterUseRect(IntVec3 loc, Rot4 rot)
		{
			int width = rot.IsHorizontal ? 7 : 13;
			int height = rot.IsHorizontal ? 13 : 7;
			return CellRect.CenteredOn(loc + rot.FacingCell * 4, width, height);
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x00150F11 File Offset: 0x0014F111
		public IEnumerable<IntVec3> WaterUseCells()
		{
			return CompPowerPlantWater.WaterUseCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x00150F2E File Offset: 0x0014F12E
		public static IEnumerable<IntVec3> WaterUseCells(IntVec3 loc, Rot4 rot)
		{
			foreach (IntVec3 intVec in CompPowerPlantWater.WaterUseRect(loc, rot))
			{
				yield return intVec;
			}
			yield break;
			yield break;
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x00150F45 File Offset: 0x0014F145
		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x00150F62 File Offset: 0x0014F162
		public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc - rot.FacingCell;
			yield return loc - rot.FacingCell - perpOffset;
			yield return loc - rot.FacingCell + perpOffset;
			yield return loc;
			yield return loc - perpOffset;
			yield return loc + perpOffset;
			yield return loc + rot.FacingCell;
			yield return loc + rot.FacingCell - perpOffset;
			yield return loc + rot.FacingCell + perpOffset;
			yield break;
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x00150F7C File Offset: 0x0014F17C
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * 2.36f;
			for (int i = 0; i < 9; i++)
			{
				float num = this.spinPosition + 6.28318548f * (float)i / 9f;
				float x = Mathf.Abs(4f * Mathf.Sin(num));
				bool flag = num % 6.28318548f < 3.14159274f;
				Vector2 vector = new Vector2(x, 1f);
				Vector3 s = new Vector3(vector.x, 1f, vector.y);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a + Vector3.up * 0.0454545468f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x0015109C File Offset: 0x0014F29C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.waterUsable && this.waterDoubleUsed)
			{
				text += "\n" + "Watermill_WaterUsedTwice".Translate();
			}
			return text;
		}

		// Token: 0x040024D3 RID: 9427
		private float spinPosition;

		// Token: 0x040024D4 RID: 9428
		private bool cacheDirty = true;

		// Token: 0x040024D5 RID: 9429
		private bool waterUsable;

		// Token: 0x040024D6 RID: 9430
		private bool waterDoubleUsed;

		// Token: 0x040024D7 RID: 9431
		private float spinRate = 1f;

		// Token: 0x040024D8 RID: 9432
		private const float PowerFactorIfWaterDoubleUsed = 0.3f;

		// Token: 0x040024D9 RID: 9433
		private const float SpinRateFactor = 0.006666667f;

		// Token: 0x040024DA RID: 9434
		private const float BladeOffset = 2.36f;

		// Token: 0x040024DB RID: 9435
		private const int BladeCount = 9;

		// Token: 0x040024DC RID: 9436
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");
	}
}
