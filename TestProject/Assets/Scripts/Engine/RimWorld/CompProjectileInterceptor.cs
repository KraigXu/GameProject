using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompProjectileInterceptor : ThingComp
	{
		
		// (get) Token: 0x06005238 RID: 21048 RVA: 0x001B776B File Offset: 0x001B596B
		public CompProperties_ProjectileInterceptor Props
		{
			get
			{
				return (CompProperties_ProjectileInterceptor)this.props;
			}
		}

		
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x001B7778 File Offset: 0x001B5978
		public bool Active
		{
			get
			{
				return !this.OnCooldown && !this.stunner.Stunned && !this.shutDown && !this.Charging;
			}
		}

		
		// (get) Token: 0x0600523A RID: 21050 RVA: 0x001B77A2 File Offset: 0x001B59A2
		public bool OnCooldown
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastInterceptTicks + this.Props.cooldownTicks;
			}
		}

		
		// (get) Token: 0x0600523B RID: 21051 RVA: 0x001B77C2 File Offset: 0x001B59C2
		public bool Charging
		{
			get
			{
				return this.nextChargeTick >= 0 && Find.TickManager.TicksGame > this.nextChargeTick;
			}
		}

		
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x001B77E1 File Offset: 0x001B59E1
		public int ChargeCycleStartTick
		{
			get
			{
				if (this.nextChargeTick < 0)
				{
					return 0;
				}
				return this.nextChargeTick;
			}
		}

		
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x001B77F4 File Offset: 0x001B59F4
		public int ChargingTicksLeft
		{
			get
			{
				if (this.nextChargeTick < 0)
				{
					return 0;
				}
				return this.nextChargeTick + this.Props.chargeDurationTicks - Find.TickManager.TicksGame;
			}
		}

		
		// (get) Token: 0x0600523E RID: 21054 RVA: 0x001B781E File Offset: 0x001B5A1E
		public int CooldownTicksLeft
		{
			get
			{
				if (!this.OnCooldown)
				{
					return 0;
				}
				return this.Props.cooldownTicks - (Find.TickManager.TicksGame - this.lastInterceptTicks);
			}
		}

		
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x001B7847 File Offset: 0x001B5A47
		public bool ReactivatedThisTick
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastInterceptTicks == this.Props.cooldownTicks;
			}
		}

		
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.chargeIntervalTicks > 0)
			{
				this.nextChargeTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
			}
			this.stunner = new StunHandler(this.parent);
		}

		
		public bool CheckIntercept(Projectile projectile, Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shields are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657212, false);
				return false;
			}
			Vector3 vector = this.parent.Position.ToVector3Shifted();
			float num = this.Props.radius + projectile.def.projectile.SpeedTilesPerTick + 0.1f;
			if ((newExactPos.x - vector.x) * (newExactPos.x - vector.x) + (newExactPos.z - vector.z) * (newExactPos.z - vector.z) > num * num)
			{
				return false;
			}
			if (!this.Active)
			{
				return false;
			}
			bool flag;
			if (this.Props.interceptGroundProjectiles)
			{
				flag = !projectile.def.projectile.flyOverhead;
			}
			else
			{
				flag = (this.Props.interceptAirProjectiles && projectile.def.projectile.flyOverhead);
			}
			if (!flag)
			{
				return false;
			}
			if ((projectile.Launcher == null || !projectile.Launcher.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			if (!this.Props.interceptOutgoingProjectiles && (new Vector2(vector.x, vector.z) - new Vector2(lastExactPos.x, lastExactPos.z)).sqrMagnitude <= this.Props.radius * this.Props.radius)
			{
				return false;
			}
			if (!GenGeo.IntersectLineCircleOutline(new Vector2(vector.x, vector.z), this.Props.radius, new Vector2(lastExactPos.x, lastExactPos.z), new Vector2(newExactPos.x, newExactPos.z)))
			{
				return false;
			}
			this.lastInterceptAngle = lastExactPos.AngleToFlat(this.parent.TrueCenter());
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			if (projectile.def.projectile.damageDef == DamageDefOf.EMP)
			{
				this.BreakShield(new DamageInfo(projectile.def.projectile.damageDef, (float)projectile.def.projectile.damageDef.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			Effecter effecter = new Effecter(this.Props.interceptEffect ?? EffecterDefOf.Interceptor_BlockedProjectile);
			effecter.Trigger(new TargetInfo(newExactPos.ToIntVec3(), this.parent.Map, false), TargetInfo.Invalid);
			effecter.Cleanup();
			return true;
		}

		
		public override void CompTick()
		{
			if (this.ReactivatedThisTick && this.Props.reactivateEffect != null)
			{
				Effecter effecter = new Effecter(this.Props.reactivateEffect);
				effecter.Trigger(this.parent, TargetInfo.Invalid);
				effecter.Cleanup();
			}
			if (Find.TickManager.TicksGame >= this.nextChargeTick + this.Props.chargeDurationTicks)
			{
				this.nextChargeTick += this.Props.chargeIntervalTicks;
			}
			this.stunner.StunHandlerTick();
		}

		
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			this.shutDown = true;
		}

		
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 pos = this.parent.Position.ToVector3Shifted();
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			float currentAlpha = this.GetCurrentAlpha();
			if (currentAlpha > 0f)
			{
				Color value;
				if (this.Active || !Find.Selector.IsSelected(this.parent))
				{
					value = this.Props.color;
				}
				else
				{
					value = CompProjectileInterceptor.InactiveColor;
				}
				value.a *= currentAlpha;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(pos, Quaternion.identity, new Vector3(this.Props.radius * 2f * 1.16015625f, 1f, this.Props.radius * 2f * 1.16015625f));
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompProjectileInterceptor.ForceFieldMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
			float currentConeAlpha_RecentlyIntercepted = this.GetCurrentConeAlpha_RecentlyIntercepted();
			if (currentConeAlpha_RecentlyIntercepted > 0f)
			{
				Color color = this.Props.color;
				color.a *= currentConeAlpha_RecentlyIntercepted;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(pos, Quaternion.Euler(0f, this.lastInterceptAngle - 90f, 0f), new Vector3(this.Props.radius * 2f * 1.16015625f, 1f, this.Props.radius * 2f * 1.16015625f));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, CompProjectileInterceptor.ForceFieldConeMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
		}

		
		private float GetCurrentAlpha()
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.GetCurrentAlpha_Idle(), this.GetCurrentAlpha_Selected()), this.GetCurrentAlpha_RecentlyIntercepted()), this.GetCurrentAlpha_RecentlyActivated()), this.Props.minAlpha);
		}

		
		private float GetCurrentAlpha_Idle()
		{
			if (!this.Active)
			{
				return 0f;
			}
			if (this.parent.Faction == Faction.OfPlayer && !this.debugInterceptNonHostileProjectiles)
			{
				return 0f;
			}
			if (Find.Selector.IsSelected(this.parent))
			{
				return 0f;
			}
			return Mathf.Lerp(-1.7f, 0.11f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 96804938) % 100) + Time.realtimeSinceStartup * 0.7f) + 1f) / 2f);
		}

		
		private float GetCurrentAlpha_Selected()
		{
			if (!Find.Selector.IsSelected(this.parent) || this.stunner.Stunned || this.shutDown)
			{
				return 0f;
			}
			if (!this.Active)
			{
				return 0.41f;
			}
			return Mathf.Lerp(0.2f, 0.62f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 35990913) % 100) + Time.realtimeSinceStartup * 2f) + 1f) / 2f);
		}

		
		private float GetCurrentAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
		}

		
		private float GetCurrentAlpha_RecentlyActivated()
		{
			if (!this.Active)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - (this.lastInterceptTicks + this.Props.cooldownTicks);
			return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
		}

		
		private float GetCurrentConeAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				if (this.OnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Reset cooldown",
						action = delegate
						{
							this.lastInterceptTicks = Find.TickManager.TicksGame - this.Props.cooldownTicks;
						}
					};
				}
				yield return new Command_Toggle
				{
					defaultLabel = "Dev: Intercept non-hostile",
					isActive = (() => this.debugInterceptNonHostileProjectiles),
					toggleAction = delegate
					{
						this.debugInterceptNonHostileProjectiles = !this.debugInterceptNonHostileProjectiles;
					}
				};
			}
			yield break;
		}

		
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Props.interceptGroundProjectiles || this.Props.interceptAirProjectiles)
			{
				string value;
				if (this.Props.interceptGroundProjectiles)
				{
					value = "InterceptsProjectiles_GroundProjectiles".Translate();
				}
				else
				{
					value = "InterceptsProjectiles_AerialProjectiles".Translate();
				}
				if (this.Props.cooldownTicks > 0)
				{
					stringBuilder.Append("InterceptsProjectilesEvery".Translate(value, this.Props.cooldownTicks.ToStringTicksToPeriod(true, false, true, true)));
				}
				else
				{
					stringBuilder.Append("InterceptsProjectiles".Translate(value));
				}
			}
			if (this.OnCooldown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("CooldownTime".Translate() + ": " + this.CooldownTicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			if (this.stunner.Stunned)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("DisarmedTime".Translate() + ": " + this.stunner.StunTicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			if (this.shutDown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ShutDown".Translate());
			}
			else if (this.Props.chargeIntervalTicks > 0)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				if (this.Charging)
				{
					stringBuilder.Append("ChargingTime".Translate() + ": " + this.ChargingTicksLeft.ToStringTicksToPeriod(true, false, true, true));
				}
				else
				{
					stringBuilder.Append("ChargingNext".Translate((this.ChargeCycleStartTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true), this.Props.chargeDurationTicks.ToStringTicksToPeriod(true, false, true, true), this.Props.chargeIntervalTicks.ToStringTicksToPeriod(true, false, true, true)));
				}
			}
			return stringBuilder.ToString();
		}

		
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(dinfo, out absorbed);
			if (dinfo.Def == DamageDefOf.EMP)
			{
				this.BreakShield(dinfo);
			}
		}

		
		private void BreakShield(DamageInfo dinfo)
		{
			if (this.Active)
			{
				SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(this.parent));
				int num = Mathf.CeilToInt(this.Props.radius * 2f);
				//CompProjectileInterceptor.c__DisplayClass41_0 c__DisplayClass41_;
				//c__DisplayClass41_.fTheta = 6.28318548f / (float)num;
				//for (int i = 0; i < num; i++)
				//{
				//	MoteMaker.MakeConnectingLine(this.<BreakShield>g__PosAtIndex|41_0(i, ref c__DisplayClass41_), this.<BreakShield>g__PosAtIndex|41_0((i + 1) % num, ref c__DisplayClass41_), ThingDefOf.Mote_LineEMP, this.parent.Map, 1.5f);
				//}
			}
			dinfo.SetAmount((float)this.Props.disarmedByEmpForTicks / 30f);
			this.stunner.Notify_DamageApplied(dinfo, true);
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastInterceptTicks, "lastInterceptTicks", -999999, false);
			Scribe_Values.Look<bool>(ref this.shutDown, "shutDown", false, false);
			Scribe_Values.Look<int>(ref this.nextChargeTick, "nextChargeTick", -1, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.parent
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.Props.chargeIntervalTicks > 0 && this.nextChargeTick <= 0)
				{
					this.nextChargeTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
				}
				if (this.stunner == null)
				{
					this.stunner = new StunHandler(this.parent);
				}
			}
		}

		
		private int lastInterceptTicks = -999999;

		
		private int nextChargeTick = -1;

		
		private bool shutDown;

		
		private StunHandler stunner;

		
		private float lastInterceptAngle;

		
		private bool debugInterceptNonHostileProjectiles;

		
		private static readonly Material ForceFieldMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		
		private static readonly Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);

		
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		
		private const float TextureActualRingSizeFactor = 1.16015625f;

		
		private static readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f);
	}
}
