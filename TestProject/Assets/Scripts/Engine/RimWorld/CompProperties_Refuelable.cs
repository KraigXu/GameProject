using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Refuelable : CompProperties
	{
		
		// (get) Token: 0x06005272 RID: 21106 RVA: 0x001B8EDA File Offset: 0x001B70DA
		public string FuelLabel
		{
			get
			{
				if (this.fuelLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelLabel;
			}
		}

		
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x001B8EFA File Offset: 0x001B70FA
		public string FuelGizmoLabel
		{
			get
			{
				if (this.fuelGizmoLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelGizmoLabel;
			}
		}

		
		// (get) Token: 0x06005274 RID: 21108 RVA: 0x001B8F1C File Offset: 0x001B711C
		public Texture2D FuelIcon
		{
			get
			{
				if (this.fuelIcon == null)
				{
					if (!this.fuelIconPath.NullOrEmpty())
					{
						this.fuelIcon = ContentFinder<Texture2D>.Get(this.fuelIconPath, true);
					}
					else
					{
						ThingDef thingDef;
						if (this.fuelFilter.AnyAllowedDef != null)
						{
							thingDef = this.fuelFilter.AnyAllowedDef;
						}
						else
						{
							thingDef = ThingDefOf.Chemfuel;
						}
						this.fuelIcon = thingDef.uiIcon;
					}
				}
				return this.fuelIcon;
			}
		}

		
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x001B8F8B File Offset: 0x001B718B
		public float FuelMultiplierCurrentDifficulty
		{
			get
			{
				if (this.factorByDifficulty)
				{
					return this.fuelMultiplier / Find.Storyteller.difficulty.maintenanceCostFactor;
				}
				return this.fuelMultiplier;
			}
		}

		
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.destroyOnNoFuel && this.initialFuelPercent <= 0f)
			{
				yield return "Refuelable component has destroyOnNoFuel, but initialFuelPercent <= 0";
			}
			if ((!this.consumeFuelOnlyWhenUsed || this.fuelConsumptionPerTickInRain > 0f) && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("Refuelable component set to consume fuel per tick, but parent tickertype is {0} instead of {1}", parentDef.tickerType, TickerType.Normal);
			}
			yield break;
			yield break;
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.n__1(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (((ThingDef)req.Def).building.IsTurret)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "RearmCost".Translate(), GenLabel.ThingLabel(this.fuelFilter.AnyAllowedDef, null, (int)(this.fuelCapacity / this.FuelMultiplierCurrentDifficulty)).CapitalizeFirst(), "RearmCostExplanation".Translate(), 3171, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "ShotsBeforeRearm".Translate(), ((int)this.fuelCapacity).ToString(), "ShotsBeforeRearmExplanation".Translate(), 3171, null, null, false);
			}
			yield break;
			yield break;
		}

		
		public float fuelConsumptionRate = 1f;

		
		public float fuelCapacity = 2f;

		
		public float initialFuelPercent;

		
		public float autoRefuelPercent = 0.3f;

		
		public float fuelConsumptionPerTickInRain;

		
		public ThingFilter fuelFilter;

		
		public bool destroyOnNoFuel;

		
		public bool consumeFuelOnlyWhenUsed;

		
		public bool showFuelGizmo;

		
		public bool initialAllowAutoRefuel = true;

		
		public bool showAllowAutoRefuelToggle;

		
		public bool targetFuelLevelConfigurable;

		
		public float initialConfigurableTargetFuelLevel;

		
		public bool drawOutOfFuelOverlay = true;

		
		public float minimumFueledThreshold;

		
		public bool drawFuelGaugeInMap;

		
		public bool atomicFueling;

		
		private float fuelMultiplier = 1f;

		
		public bool factorByDifficulty;

		
		public string fuelLabel;

		
		public string fuelGizmoLabel;

		
		public string outOfFuelMessage;

		
		public string fuelIconPath;

		
		private Texture2D fuelIcon;
	}
}
