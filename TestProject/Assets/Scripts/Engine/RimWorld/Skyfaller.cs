using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CBF RID: 3263
	[StaticConstructorOnStartup]
	public class Skyfaller : Thing, IThingHolder
	{
		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004F1A RID: 20250 RVA: 0x001AA244 File Offset: 0x001A8444
		public override Graphic Graphic
		{
			get
			{
				Thing thingForGraphic = this.GetThingForGraphic();
				if (thingForGraphic == this)
				{
					return base.Graphic;
				}
				return thingForGraphic.Graphic.ExtractInnerGraphicFor(thingForGraphic).GetShadowlessGraphic();
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06004F1B RID: 20251 RVA: 0x001AA274 File Offset: 0x001A8474
		public override Vector3 DrawPos
		{
			get
			{
				switch (this.def.skyfaller.movementType)
				{
				case SkyfallerMovementType.Accelerate:
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed);
				case SkyfallerMovementType.ConstantSpeed:
					return SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed);
				case SkyfallerMovementType.Decelerate:
					return SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed);
				default:
					Log.ErrorOnce("SkyfallerMovementType not handled: " + this.def.skyfaller.movementType, this.thingIDNumber ^ 1948576711, false);
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed);
				}
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06004F1C RID: 20252 RVA: 0x001AA350 File Offset: 0x001A8550
		private Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.def.skyfaller.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.def.skyfaller.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06004F1D RID: 20253 RVA: 0x001AA3A8 File Offset: 0x001A85A8
		private float TimeInAnimation
		{
			get
			{
				if (this.def.skyfaller.reversed)
				{
					return (float)this.ticksToImpact / 220f;
				}
				return 1f - (float)this.ticksToImpact / (float)this.ticksToImpactMax;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06004F1E RID: 20254 RVA: 0x001AA3E0 File Offset: 0x001A85E0
		private float CurrentSpeed
		{
			get
			{
				if (this.def.skyfaller.speedCurve == null)
				{
					return this.def.skyfaller.speed;
				}
				return this.def.skyfaller.speedCurve.Evaluate(this.TimeInAnimation) * this.def.skyfaller.speed;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06004F1F RID: 20255 RVA: 0x001AA43C File Offset: 0x001A863C
		private bool SpawnTimedMotes
		{
			get
			{
				return this.def.skyfaller.moteSpawnTime != float.MinValue && Mathf.Approximately(this.def.skyfaller.moteSpawnTime, this.TimeInAnimation);
			}
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x001AA472 File Offset: 0x001A8672
		public Skyfaller()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x001AA494 File Offset: 0x001A8694
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToImpactMax, "ticksToImpactMax", 220, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<float>(ref this.shrapnelDirection, "shrapnelDirection", 0f, false);
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x001AA515 File Offset: 0x001A8715
		public override void PostMake()
		{
			base.PostMake();
			if (this.def.skyfaller.MakesShrapnel)
			{
				this.shrapnelDirection = Rand.Range(0f, 360f);
			}
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x001AA544 File Offset: 0x001A8744
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToImpact = (this.ticksToImpactMax = this.def.skyfaller.ticksToImpactRange.RandomInRange);
				if (this.def.skyfaller.MakesShrapnel)
				{
					float num = GenMath.PositiveMod(this.shrapnelDirection, 360f);
					if (num < 270f && num >= 90f)
					{
						this.angle = Rand.Range(0f, 33f);
					}
					else
					{
						this.angle = Rand.Range(-33f, 0f);
					}
				}
				else if (this.def.skyfaller.angleCurve != null)
				{
					this.angle = this.def.skyfaller.angleCurve.Evaluate(0f);
				}
				else
				{
					this.angle = -33.7f;
				}
				if (this.def.rotatable && this.innerContainer.Any)
				{
					base.Rotation = this.innerContainer[0].Rotation;
				}
			}
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x001AA654 File Offset: 0x001A8854
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x001AA66C File Offset: 0x001A886C
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Thing thingForGraphic = this.GetThingForGraphic();
			float num = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				num = this.angle;
			}
			if (this.def.skyfaller.angleCurve != null)
			{
				this.angle = this.def.skyfaller.angleCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.rotationCurve != null)
			{
				num += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.xPositionCurve != null)
			{
				drawLoc.x += this.def.skyfaller.xPositionCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.zPositionCurve != null)
			{
				drawLoc.z += this.def.skyfaller.zPositionCurve.Evaluate(this.TimeInAnimation);
			}
			this.Graphic.Draw(drawLoc, flip ? thingForGraphic.Rotation.Opposite : thingForGraphic.Rotation, thingForGraphic, num);
			this.DrawDropSpotShadow();
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x001AA7A0 File Offset: 0x001A89A0
		public override void Tick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.SpawnTimedMotes)
			{
				CellRect cellRect = this.OccupiedRect();
				for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
				{
					MoteMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
				}
			}
			if (this.def.skyfaller.reversed)
			{
				this.ticksToImpact++;
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact > this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					this.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 220)
				{
					this.LeaveMap();
					return;
				}
				if (this.ticksToImpact > 220)
				{
					Log.Error("ticksToImpact > LeaveMapAfterTicks. Was there an exception? Destroying skyfaller.", false);
					this.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				this.ticksToImpact--;
				if (this.ticksToImpact == 15)
				{
					this.HitRoof();
				}
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact < this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					this.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 0)
				{
					this.Impact();
					return;
				}
				if (this.ticksToImpact < 0)
				{
					Log.Error("ticksToImpact < 0. Was there an exception? Destroying skyfaller.", false);
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x001AA970 File Offset: 0x001A8B70
		protected virtual void HitRoof()
		{
			if (!this.def.skyfaller.hitRoof)
			{
				return;
			}
			CellRect cr = this.OccupiedRect();
			if (cr.Cells.Any((IntVec3 x) => x.Roofed(this.Map)))
			{
				RoofDef roof = cr.Cells.First((IntVec3 x) => x.Roofed(this.Map)).GetRoof(base.Map);
				if (!roof.soundPunchThrough.NullOrUndefined())
				{
					roof.soundPunchThrough.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				RoofCollapserImmediate.DropRoofInCells(cr.ExpandedBy(1).ClipInsideMap(base.Map).Cells.Where(delegate(IntVec3 c)
				{
					if (!c.InBounds(this.Map))
					{
						return false;
					}
					if (cr.Contains(c))
					{
						return true;
					}
					if (c.GetFirstPawn(this.Map) != null)
					{
						return false;
					}
					Building edifice = c.GetEdifice(this.Map);
					return edifice == null || !edifice.def.holdsRoof;
				}), base.Map, null);
			}
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x001AAA60 File Offset: 0x001A8C60
		protected virtual void SpawnThings()
		{
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenPlace.TryPlaceThing(this.innerContainer[i], base.Position, base.Map, ThingPlaceMode.Near, delegate(Thing thing, int count)
				{
					PawnUtility.RecoverFromUnwalkablePositionOrKill(thing.Position, thing.Map);
					if (thing.def.Fillage == FillCategory.Full && this.def.skyfaller.CausesExplosion && this.def.skyfaller.explosionDamage.isExplosive && thing.Position.InHorDistOf(base.Position, this.def.skyfaller.explosionRadius))
					{
						base.Map.terrainGrid.Notify_TerrainDestroyed(thing.Position);
					}
				}, null, this.innerContainer[i].def.defaultPlacingRot);
			}
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x001AAAC8 File Offset: 0x001A8CC8
		protected virtual void Impact()
		{
			if (this.def.skyfaller.CausesExplosion)
			{
				GenExplosion.DoExplosion(base.Position, base.Map, this.def.skyfaller.explosionRadius, this.def.skyfaller.explosionDamage, null, GenMath.RoundRandom((float)this.def.skyfaller.explosionDamage.defaultDamage * this.def.skyfaller.explosionDamageFactor), -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, (!this.def.skyfaller.damageSpawnedThings) ? this.innerContainer.ToList<Thing>() : null);
			}
			this.SpawnThings();
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			CellRect cellRect = this.OccupiedRect();
			for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
			{
				MoteMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
			}
			if (this.def.skyfaller.MakesShrapnel)
			{
				SkyfallerShrapnelUtility.MakeShrapnel(base.Position, base.Map, this.shrapnelDirection, this.def.skyfaller.shrapnelDistanceFactor, this.def.skyfaller.metalShrapnelCountRange.RandomInRange, this.def.skyfaller.rubbleShrapnelCountRange.RandomInRange, true);
			}
			if (this.def.skyfaller.cameraShake > 0f && base.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(this.def.skyfaller.cameraShake);
			}
			if (this.def.skyfaller.impactSound != null)
			{
				this.def.skyfaller.impactSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x001AACCF File Offset: 0x001A8ECF
		protected virtual void LeaveMap()
		{
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x001AACD8 File Offset: 0x001A8ED8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001AACE0 File Offset: 0x001A8EE0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001AACEE File Offset: 0x001A8EEE
		private Thing GetThingForGraphic()
		{
			if (this.def.graphicData != null || !this.innerContainer.Any)
			{
				return this;
			}
			return this.innerContainer[0];
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x001AAD18 File Offset: 0x001A8F18
		private void DrawDropSpotShadow()
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			Skyfaller.DrawDropSpotShadow(base.DrawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x001AAD60 File Offset: 0x001A8F60
		public static void DrawDropSpotShadow(Vector3 center, Rot4 rot, Material material, Vector2 shadowSize, int ticksToImpact)
		{
			if (rot.IsHorizontal)
			{
				Gen.Swap<float>(ref shadowSize.x, ref shadowSize.y);
			}
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			Vector3 pos = center;
			pos.y = AltitudeLayer.Shadows.AltitudeFor();
			float num = 1f + (float)ticksToImpact / 100f;
			Vector3 s = new Vector3(num * shadowSize.x, 1f, num * shadowSize.y);
			Color white = Color.white;
			if (ticksToImpact > 150)
			{
				white.a = Mathf.InverseLerp(200f, 150f, (float)ticksToImpact);
			}
			Skyfaller.shadowPropertyBlock.SetColor(ShaderPropertyIDs.Color, white);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, rot.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10Back, matrix, material, 0, null, 0, Skyfaller.shadowPropertyBlock);
		}

		// Token: 0x04002C6C RID: 11372
		public ThingOwner innerContainer;

		// Token: 0x04002C6D RID: 11373
		public int ticksToImpact;

		// Token: 0x04002C6E RID: 11374
		public float angle;

		// Token: 0x04002C6F RID: 11375
		public float shrapnelDirection;

		// Token: 0x04002C70 RID: 11376
		private int ticksToImpactMax = 220;

		// Token: 0x04002C71 RID: 11377
		private Material cachedShadowMaterial;

		// Token: 0x04002C72 RID: 11378
		private bool anticipationSoundPlayed;

		// Token: 0x04002C73 RID: 11379
		private static MaterialPropertyBlock shadowPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04002C74 RID: 11380
		public const float DefaultAngle = -33.7f;

		// Token: 0x04002C75 RID: 11381
		private const int RoofHitPreDelay = 15;

		// Token: 0x04002C76 RID: 11382
		private const int LeaveMapAfterTicks = 220;
	}
}
