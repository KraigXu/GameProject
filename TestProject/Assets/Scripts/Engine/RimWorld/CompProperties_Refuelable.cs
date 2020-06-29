﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Refuelable : CompProperties
	{
		
		
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
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
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
