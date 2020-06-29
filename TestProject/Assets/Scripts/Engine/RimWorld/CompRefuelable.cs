﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompRefuelable : ThingComp
	{
		
		// (get) Token: 0x0600527C RID: 21116 RVA: 0x001B9053 File Offset: 0x001B7253
		// (set) Token: 0x0600527D RID: 21117 RVA: 0x001B908D File Offset: 0x001B728D
		public float TargetFuelLevel
		{
			get
			{
				if (this.configuredTargetFuelLevel >= 0f)
				{
					return this.configuredTargetFuelLevel;
				}
				if (this.Props.targetFuelLevelConfigurable)
				{
					return this.Props.initialConfigurableTargetFuelLevel;
				}
				return this.Props.fuelCapacity;
			}
			set
			{
				this.configuredTargetFuelLevel = Mathf.Clamp(value, 0f, this.Props.fuelCapacity);
			}
		}

		
		// (get) Token: 0x0600527E RID: 21118 RVA: 0x001B90AB File Offset: 0x001B72AB
		public CompProperties_Refuelable Props
		{
			get
			{
				return (CompProperties_Refuelable)this.props;
			}
		}

		
		// (get) Token: 0x0600527F RID: 21119 RVA: 0x001B90B8 File Offset: 0x001B72B8
		public float Fuel
		{
			get
			{
				return this.fuel;
			}
		}

		
		// (get) Token: 0x06005280 RID: 21120 RVA: 0x001B90C0 File Offset: 0x001B72C0
		public float FuelPercentOfTarget
		{
			get
			{
				return this.fuel / this.TargetFuelLevel;
			}
		}

		
		// (get) Token: 0x06005281 RID: 21121 RVA: 0x001B90CF File Offset: 0x001B72CF
		public float FuelPercentOfMax
		{
			get
			{
				return this.fuel / this.Props.fuelCapacity;
			}
		}

		
		// (get) Token: 0x06005282 RID: 21122 RVA: 0x001B90E3 File Offset: 0x001B72E3
		public bool IsFull
		{
			get
			{
				return this.TargetFuelLevel - this.fuel < 1f;
			}
		}

		
		// (get) Token: 0x06005283 RID: 21123 RVA: 0x001B90F9 File Offset: 0x001B72F9
		public bool HasFuel
		{
			get
			{
				return this.fuel > 0f && this.fuel >= this.Props.minimumFueledThreshold;
			}
		}

		
		// (get) Token: 0x06005284 RID: 21124 RVA: 0x001B9120 File Offset: 0x001B7320
		private float ConsumptionRatePerTick
		{
			get
			{
				return this.Props.fuelConsumptionRate / 60000f;
			}
		}

		
		// (get) Token: 0x06005285 RID: 21125 RVA: 0x001B9133 File Offset: 0x001B7333
		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0f && this.ShouldAutoRefuelNowIgnoringFuelPct;
			}
		}

		
		// (get) Token: 0x06005286 RID: 21126 RVA: 0x001B9168 File Offset: 0x001B7368
		public bool ShouldAutoRefuelNowIgnoringFuelPct
		{
			get
			{
				return !this.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Flick) == null && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			this.fuel = this.Props.fuelCapacity * this.Props.initialFuelPercent;
			this.flickComp = this.parent.GetComp<CompFlickable>();
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fuel, "fuel", 0f, false);
			Scribe_Values.Look<float>(ref this.configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f, false);
			Scribe_Values.Look<bool>(ref this.allowAutoRefuel, "allowAutoRefuel", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.Props.showAllowAutoRefuelToggle)
			{
				this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			}
		}

		
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.allowAutoRefuel)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenRefuel);
			}
			else if (!this.HasFuel && this.Props.drawOutOfFuelOverlay)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.OutOfFuel);
			}
			if (this.Props.drawFuelGaugeInMap)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.parent.DrawPos + Vector3.up * 0.1f;
				r.size = CompRefuelable.FuelBarSize;
				r.fillPercent = this.FuelPercentOfMax;
				r.filledMat = CompRefuelable.FuelBarFilledMat;
				r.unfilledMat = CompRefuelable.FuelBarUnfilledMat;
				r.margin = 0.15f;
				Rot4 rotation = this.parent.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				r.rotation = rotation;
				GenDraw.DrawFillableBar(r);
			}
		}

		
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (previousMap != null && this.Props.fuelFilter.AllowedDefCount == 1 && this.Props.initialFuelPercent == 0f)
			{
				ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
				int i = GenMath.RoundRandom(1f * this.fuel);
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
					i -= thing.stackCount;
					GenPlace.TryPlaceThing(thing, this.parent.Position, previousMap, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		
		public override string CompInspectStringExtra()
		{
			string text = string.Concat(new string[]
			{
				this.Props.FuelLabel,
				": ",
				this.fuel.ToStringDecimalIfSmall(),
				" / ",
				this.Props.fuelCapacity.ToStringDecimalIfSmall()
			});
			if (!this.Props.consumeFuelOnlyWhenUsed && this.HasFuel)
			{
				int numTicks = (int)(this.fuel / this.Props.fuelConsumptionRate * 60000f);
				text = text + " (" + numTicks.ToStringTicksToPeriod(true, false, true, true) + ")";
			}
			if (!this.HasFuel && !this.Props.outOfFuelMessage.NullOrEmpty())
			{
				text += string.Format("\n{0} ({1}x {2})", this.Props.outOfFuelMessage, this.GetFuelCountToFullyRefuel(), this.Props.fuelFilter.AnyAllowedDef.label);
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				text += "\n" + "ConfiguredTargetFuelLevel".Translate(this.TargetFuelLevel.ToStringDecimalIfSmall());
			}
			return text;
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Props.consumeFuelOnlyWhenUsed && (this.flickComp == null || this.flickComp.SwitchIsOn))
			{
				this.ConsumeFuel(this.ConsumptionRatePerTick);
			}
			if (this.Props.fuelConsumptionPerTickInRain > 0f && this.parent.Spawned && this.parent.Map.weatherManager.RainRate > 0.4f && !this.parent.Map.roofGrid.Roofed(this.parent.Position))
			{
				this.ConsumeFuel(this.Props.fuelConsumptionPerTickInRain);
			}
		}

		
		public void ConsumeFuel(float amount)
		{
			if (this.fuel <= 0f)
			{
				return;
			}
			this.fuel -= amount;
			if (this.fuel <= 0f)
			{
				this.fuel = 0f;
				if (this.Props.destroyOnNoFuel)
				{
					this.parent.Destroy(DestroyMode.Vanish);
				}
				this.parent.BroadcastCompSignal("RanOutOfFuel");
			}
		}

		
		public void Refuel(List<Thing> fuelThings)
		{
			if (this.Props.atomicFueling)
			{
				if (fuelThings.Sum((Thing t) => t.stackCount) < this.GetFuelCountToFullyRefuel())
				{
					Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442, false);
					return;
				}
			}
			int num = this.GetFuelCountToFullyRefuel();
			while (num > 0 && fuelThings.Count > 0)
			{
				Thing thing = fuelThings.Pop<Thing>();
				int num2 = Mathf.Min(num, thing.stackCount);
				this.Refuel((float)num2);
				thing.SplitOff(num2).Destroy(DestroyMode.Vanish);
				num -= num2;
			}
		}

		
		public void Refuel(float amount)
		{
			this.fuel += amount * this.Props.FuelMultiplierCurrentDifficulty;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.parent.BroadcastCompSignal("Refueled");
		}

		
		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		
		public int GetFuelCountToFullyRefuel()
		{
			if (this.Props.atomicFueling)
			{
				return Mathf.CeilToInt(this.Props.fuelCapacity / this.Props.FuelMultiplierCurrentDifficulty);
			}
			return Mathf.Max(Mathf.CeilToInt((this.TargetFuelLevel - this.fuel) / this.Props.FuelMultiplierCurrentDifficulty), 1);
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Props.targetFuelLevelConfigurable)
			{
				yield return new Command_SetTargetFuelLevel
				{
					refuelable = this,
					defaultLabel = "CommandSetTargetFuelLevel".Translate(),
					defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
					icon = CompRefuelable.SetTargetFuelLevelCommand
				};
			}
			if (this.Props.showFuelGizmo && Find.Selector.SingleSelectedThing == this.parent)
			{
				yield return new Gizmo_RefuelableFuelStatus
				{
					refuelable = this
				};
			}
			if (this.Props.showAllowAutoRefuelToggle)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleAllowAutoRefuel".Translate(),
					defaultDesc = "CommandToggleAllowAutoRefuelDesc".Translate(),
					hotKey = KeyBindingDefOf.Command_ItemForbid,
					icon = (this.allowAutoRefuel ? TexCommand.ForbidOff : TexCommand.ForbidOn),
					isActive = (() => this.allowAutoRefuel),
					toggleAction = delegate
					{
						this.allowAutoRefuel = !this.allowAutoRefuel;
					}
				};
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0",
					action = delegate
					{
						this.fuel = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0.1",
					action = delegate
					{
						this.fuel = 0.1f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to max",
					action = delegate
					{
						this.fuel = this.Props.fuelCapacity;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
			}
			yield break;
		}

		
		private float fuel;

		
		private float configuredTargetFuelLevel = -1f;

		
		public bool allowAutoRefuel = true;

		
		private CompFlickable flickComp;

		
		public const string RefueledSignal = "Refueled";

		
		public const string RanOutOfFuelSignal = "RanOutOfFuel";

		
		private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel", true);

		
		private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

		
		private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f), false);

		
		private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
