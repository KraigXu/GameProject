using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C99 RID: 3225
	[StaticConstructorOnStartup]
	public class Tornado : ThingWithComps
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06004DC2 RID: 19906 RVA: 0x001A2184 File Offset: 0x001A0384
		private float FadeInOutFactor
		{
			get
			{
				float a = Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.spawnTick) / 120f);
				float b = (this.leftFadeOutTicks < 0) ? 1f : Mathf.Min((float)this.leftFadeOutTicks / 120f, 1f);
				return Mathf.Min(a, b);
			}
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001A21DC File Offset: 0x001A03DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector2>(ref this.realPosition, "realPosition", default(Vector2), false);
			Scribe_Values.Look<float>(ref this.direction, "direction", 0f, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.leftFadeOutTicks, "leftFadeOutTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksLeftToDisappear, "ticksLeftToDisappear", 0, false);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001A2258 File Offset: 0x001A0458
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				Vector3 vector = base.Position.ToVector3Shifted();
				this.realPosition = new Vector2(vector.x, vector.z);
				this.direction = Rand.Range(0f, 360f);
				this.spawnTick = Find.TickManager.TicksGame;
				this.leftFadeOutTicks = -1;
				this.ticksLeftToDisappear = Tornado.DurationTicks.RandomInRange;
			}
			this.CreateSustainer();
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x001A22DC File Offset: 0x001A04DC
		public override void Tick()
		{
			if (base.Spawned)
			{
				if (this.sustainer == null)
				{
					Log.Error("Tornado sustainer is null.", false);
					this.CreateSustainer();
				}
				this.sustainer.Maintain();
				this.UpdateSustainerVolume();
				base.GetComp<CompWindSource>().wind = 5f * this.FadeInOutFactor;
				if (this.leftFadeOutTicks > 0)
				{
					this.leftFadeOutTicks--;
					if (this.leftFadeOutTicks == 0)
					{
						this.Destroy(DestroyMode.Vanish);
						return;
					}
				}
				else
				{
					if (Tornado.directionNoise == null)
					{
						Tornado.directionNoise = new Perlin(0.0020000000949949026, 2.0, 0.5, 4, 1948573612, QualityMode.Medium);
					}
					this.direction += (float)Tornado.directionNoise.GetValue((double)Find.TickManager.TicksAbs, (double)((float)(this.thingIDNumber % 500) * 1000f), 0.0) * 0.78f;
					this.realPosition = this.realPosition.Moved(this.direction, 0.0283333343f);
					IntVec3 intVec = new Vector3(this.realPosition.x, 0f, this.realPosition.y).ToIntVec3();
					if (intVec.InBounds(base.Map))
					{
						base.Position = intVec;
						if (this.IsHashIntervalTick(15))
						{
							this.DamageCloseThings();
						}
						if (Rand.MTBEventOccurs(15f, 1f, 1f))
						{
							this.DamageFarThings();
						}
						if (this.IsHashIntervalTick(20))
						{
							this.DestroyRoofs();
						}
						if (this.ticksLeftToDisappear > 0)
						{
							this.ticksLeftToDisappear--;
							if (this.ticksLeftToDisappear == 0)
							{
								this.leftFadeOutTicks = 120;
								Messages.Message("MessageTornadoDissipated".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
							}
						}
						if (this.IsHashIntervalTick(4) && !this.CellImmuneToDamage(base.Position))
						{
							float num = Rand.Range(0.6f, 1f);
							MoteMaker.ThrowTornadoDustPuff(new Vector3(this.realPosition.x, 0f, this.realPosition.y)
							{
								y = AltitudeLayer.MoteOverhead.AltitudeFor()
							} + Vector3Utility.RandomHorizontalOffset(1.5f), base.Map, Rand.Range(1.5f, 3f), new Color(num, num, num));
							return;
						}
					}
					else
					{
						this.leftFadeOutTicks = 120;
						Messages.Message("MessageTornadoLeftMap".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
					}
				}
			}
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x001A2590 File Offset: 0x001A0790
		public override void Draw()
		{
			Rand.PushState();
			Rand.Seed = this.thingIDNumber;
			for (int i = 0; i < 180; i++)
			{
				this.DrawTornadoPart(Tornado.PartsDistanceFromCenter.RandomInRange, Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f), Rand.Range(0.52f, 0.88f));
			}
			Rand.PopState();
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x001A2604 File Offset: 0x001A0804
		private void DrawTornadoPart(float distanceFromCenter, float initialAngle, float speedMultiplier, float colorMultiplier)
		{
			int ticksGame = Find.TickManager.TicksGame;
			float num = 1f / distanceFromCenter;
			float num2 = 25f * speedMultiplier * num;
			float num3 = (initialAngle + (float)ticksGame * num2) % 360f;
			Vector2 vector = this.realPosition.Moved(num3, this.AdjustedDistanceFromCenter(distanceFromCenter));
			vector.y += distanceFromCenter * 4f;
			vector.y += Tornado.ZOffsetBias;
			Vector3 a = new Vector3(vector.x, AltitudeLayer.Weather.AltitudeFor() + 0.0454545468f * Rand.Range(0f, 1f), vector.y);
			float num4 = distanceFromCenter * 3f;
			float num5 = 1f;
			if (num3 > 270f)
			{
				num5 = GenMath.LerpDouble(270f, 360f, 0f, 1f, num3);
			}
			else if (num3 > 180f)
			{
				num5 = GenMath.LerpDouble(180f, 270f, 1f, 0f, num3);
			}
			float num6 = Mathf.Min(distanceFromCenter / (Tornado.PartsDistanceFromCenter.max + 2f), 1f);
			float d = Mathf.InverseLerp(0.18f, 0.4f, num6);
			Vector3 a2 = new Vector3(Mathf.Sin((float)ticksGame / 1000f + (float)(this.thingIDNumber * 10)) * 2f, 0f, 0f);
			Vector3 pos = a + a2 * d;
			float a3 = Mathf.Max(1f - num6, 0f) * num5 * this.FadeInOutFactor;
			Color value = new Color(colorMultiplier, colorMultiplier, colorMultiplier, a3);
			Tornado.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, num3, 0f), new Vector3(num4, 1f, num4));
			Graphics.DrawMesh(MeshPool.plane10, matrix, Tornado.TornadoMaterial, 0, null, 0, Tornado.matPropertyBlock);
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x001A27E8 File Offset: 0x001A09E8
		private float AdjustedDistanceFromCenter(float distanceFromCenter)
		{
			float num = Mathf.Min(distanceFromCenter / 8f, 1f);
			num *= num;
			return distanceFromCenter * num;
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x001A280E File Offset: 0x001A0A0E
		private void UpdateSustainerVolume()
		{
			this.sustainer.info.volumeFactor = this.FadeInOutFactor;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x001A2826 File Offset: 0x001A0A26
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef tornado = SoundDefOf.Tornado;
				this.sustainer = tornado.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				this.UpdateSustainerVolume();
			});
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x001A283C File Offset: 0x001A0A3C
		private void DamageCloseThings()
		{
			int num = GenRadial.NumCellsInRadius(4.2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map) && !this.CellImmuneToDamage(intVec))
				{
					Pawn firstPawn = intVec.GetFirstPawn(base.Map);
					if (firstPawn == null || !firstPawn.Downed || !Rand.Bool)
					{
						float damageFactor = GenMath.LerpDouble(0f, 4.2f, 1f, 0.2f, intVec.DistanceTo(base.Position));
						this.DoDamage(intVec, damageFactor);
					}
				}
			}
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x001A28E4 File Offset: 0x001A0AE4
		private void DamageFarThings()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 10f, true)
			where x.InBounds(base.Map)
			select x).RandomElement<IntVec3>();
			if (this.CellImmuneToDamage(c))
			{
				return;
			}
			this.DoDamage(c, 0.5f);
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x001A2930 File Offset: 0x001A0B30
		private void DestroyRoofs()
		{
			this.removedRoofsTmp.Clear();
			foreach (IntVec3 intVec in from x in GenRadial.RadialCellsAround(base.Position, 4.2f, true)
			where x.InBounds(base.Map)
			select x)
			{
				if (!this.CellImmuneToDamage(intVec) && intVec.Roofed(base.Map))
				{
					RoofDef roof = intVec.GetRoof(base.Map);
					if (!roof.isThickRoof && !roof.isNatural)
					{
						RoofCollapserImmediate.DropRoofInCells(intVec, base.Map, null);
						this.removedRoofsTmp.Add(intVec);
					}
				}
			}
			if (this.removedRoofsTmp.Count > 0)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(this.removedRoofsTmp, base.Map, true, false);
			}
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x001A2A0C File Offset: 0x001A0C0C
		private bool CellImmuneToDamage(IntVec3 c)
		{
			if (c.Roofed(base.Map) && c.GetRoof(base.Map).isThickRoof)
			{
				return true;
			}
			Building edifice = c.GetEdifice(base.Map);
			return edifice != null && edifice.def.category == ThingCategory.Building && (edifice.def.building.isNaturalRock || (edifice.def == ThingDefOf.Wall && edifice.Faction == null));
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x001A2A84 File Offset: 0x001A0C84
		private void DoDamage(IntVec3 c, float damageFactor)
		{
			Tornado.tmpThings.Clear();
			Tornado.tmpThings.AddRange(c.GetThingList(base.Map));
			Vector3 vector = c.ToVector3Shifted();
			Vector2 b = new Vector2(vector.x, vector.z);
			float angle = -this.realPosition.AngleTo(b) + 180f;
			for (int i = 0; i < Tornado.tmpThings.Count; i++)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				switch (Tornado.tmpThings[i].def.category)
				{
				case ThingCategory.Pawn:
				{
					Pawn pawn = (Pawn)Tornado.tmpThings[i];
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Tornado, null);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
					if (pawn.RaceProps.baseHealthScale < 1f)
					{
						damageFactor *= pawn.RaceProps.baseHealthScale;
					}
					if (pawn.RaceProps.Animal)
					{
						damageFactor *= 0.75f;
					}
					if (pawn.Downed)
					{
						damageFactor *= 0.2f;
					}
					break;
				}
				case ThingCategory.Item:
					damageFactor *= 0.68f;
					break;
				case ThingCategory.Building:
					damageFactor *= 0.8f;
					break;
				case ThingCategory.Plant:
					damageFactor *= 1.7f;
					break;
				}
				int num = Mathf.Max(GenMath.RoundRandom(30f * damageFactor), 1);
				Tornado.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.TornadoScratch, (float)num, 0f, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			Tornado.tmpThings.Clear();
		}

		// Token: 0x04002B8B RID: 11147
		private Vector2 realPosition;

		// Token: 0x04002B8C RID: 11148
		private float direction;

		// Token: 0x04002B8D RID: 11149
		private int spawnTick;

		// Token: 0x04002B8E RID: 11150
		private int leftFadeOutTicks = -1;

		// Token: 0x04002B8F RID: 11151
		private int ticksLeftToDisappear = -1;

		// Token: 0x04002B90 RID: 11152
		private Sustainer sustainer;

		// Token: 0x04002B91 RID: 11153
		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04002B92 RID: 11154
		private static ModuleBase directionNoise;

		// Token: 0x04002B93 RID: 11155
		private const float Wind = 5f;

		// Token: 0x04002B94 RID: 11156
		private const int CloseDamageIntervalTicks = 15;

		// Token: 0x04002B95 RID: 11157
		private const int RoofDestructionIntervalTicks = 20;

		// Token: 0x04002B96 RID: 11158
		private const float FarDamageMTBTicks = 15f;

		// Token: 0x04002B97 RID: 11159
		private const float CloseDamageRadius = 4.2f;

		// Token: 0x04002B98 RID: 11160
		private const float FarDamageRadius = 10f;

		// Token: 0x04002B99 RID: 11161
		private const float BaseDamage = 30f;

		// Token: 0x04002B9A RID: 11162
		private const int SpawnMoteEveryTicks = 4;

		// Token: 0x04002B9B RID: 11163
		private static readonly IntRange DurationTicks = new IntRange(2700, 10080);

		// Token: 0x04002B9C RID: 11164
		private const float DownedPawnDamageFactor = 0.2f;

		// Token: 0x04002B9D RID: 11165
		private const float AnimalPawnDamageFactor = 0.75f;

		// Token: 0x04002B9E RID: 11166
		private const float BuildingDamageFactor = 0.8f;

		// Token: 0x04002B9F RID: 11167
		private const float PlantDamageFactor = 1.7f;

		// Token: 0x04002BA0 RID: 11168
		private const float ItemDamageFactor = 0.68f;

		// Token: 0x04002BA1 RID: 11169
		private const float CellsPerSecond = 1.7f;

		// Token: 0x04002BA2 RID: 11170
		private const float DirectionChangeSpeed = 0.78f;

		// Token: 0x04002BA3 RID: 11171
		private const float DirectionNoiseFrequency = 0.002f;

		// Token: 0x04002BA4 RID: 11172
		private const float TornadoAnimationSpeed = 25f;

		// Token: 0x04002BA5 RID: 11173
		private const float ThreeDimensionalEffectStrength = 4f;

		// Token: 0x04002BA6 RID: 11174
		private const int FadeInTicks = 120;

		// Token: 0x04002BA7 RID: 11175
		private const int FadeOutTicks = 120;

		// Token: 0x04002BA8 RID: 11176
		private const float MaxMidOffset = 2f;

		// Token: 0x04002BA9 RID: 11177
		private static readonly Material TornadoMaterial = MaterialPool.MatFrom("Things/Ethereal/Tornado", ShaderDatabase.Transparent, MapMaterialRenderQueues.Tornado);

		// Token: 0x04002BAA RID: 11178
		private static readonly FloatRange PartsDistanceFromCenter = new FloatRange(1f, 10f);

		// Token: 0x04002BAB RID: 11179
		private static readonly float ZOffsetBias = -4f * Tornado.PartsDistanceFromCenter.min;

		// Token: 0x04002BAC RID: 11180
		private List<IntVec3> removedRoofsTmp = new List<IntVec3>();

		// Token: 0x04002BAD RID: 11181
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
