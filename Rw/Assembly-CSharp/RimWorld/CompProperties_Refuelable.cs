using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D41 RID: 3393
	public class CompProperties_Refuelable : CompProperties
	{
		// Token: 0x17000E94 RID: 3732
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

		// Token: 0x17000E95 RID: 3733
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

		// Token: 0x17000E96 RID: 3734
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

		// Token: 0x17000E97 RID: 3735
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

		// Token: 0x06005276 RID: 21110 RVA: 0x001B8FB4 File Offset: 0x001B71B4
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x001B9011 File Offset: 0x001B7211
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x001B9025 File Offset: 0x001B7225
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
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

		// Token: 0x06005279 RID: 21113 RVA: 0x001B903C File Offset: 0x001B723C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__1(req))
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

		// Token: 0x04002D81 RID: 11649
		public float fuelConsumptionRate = 1f;

		// Token: 0x04002D82 RID: 11650
		public float fuelCapacity = 2f;

		// Token: 0x04002D83 RID: 11651
		public float initialFuelPercent;

		// Token: 0x04002D84 RID: 11652
		public float autoRefuelPercent = 0.3f;

		// Token: 0x04002D85 RID: 11653
		public float fuelConsumptionPerTickInRain;

		// Token: 0x04002D86 RID: 11654
		public ThingFilter fuelFilter;

		// Token: 0x04002D87 RID: 11655
		public bool destroyOnNoFuel;

		// Token: 0x04002D88 RID: 11656
		public bool consumeFuelOnlyWhenUsed;

		// Token: 0x04002D89 RID: 11657
		public bool showFuelGizmo;

		// Token: 0x04002D8A RID: 11658
		public bool initialAllowAutoRefuel = true;

		// Token: 0x04002D8B RID: 11659
		public bool showAllowAutoRefuelToggle;

		// Token: 0x04002D8C RID: 11660
		public bool targetFuelLevelConfigurable;

		// Token: 0x04002D8D RID: 11661
		public float initialConfigurableTargetFuelLevel;

		// Token: 0x04002D8E RID: 11662
		public bool drawOutOfFuelOverlay = true;

		// Token: 0x04002D8F RID: 11663
		public float minimumFueledThreshold;

		// Token: 0x04002D90 RID: 11664
		public bool drawFuelGaugeInMap;

		// Token: 0x04002D91 RID: 11665
		public bool atomicFueling;

		// Token: 0x04002D92 RID: 11666
		private float fuelMultiplier = 1f;

		// Token: 0x04002D93 RID: 11667
		public bool factorByDifficulty;

		// Token: 0x04002D94 RID: 11668
		public string fuelLabel;

		// Token: 0x04002D95 RID: 11669
		public string fuelGizmoLabel;

		// Token: 0x04002D96 RID: 11670
		public string outOfFuelMessage;

		// Token: 0x04002D97 RID: 11671
		public string fuelIconPath;

		// Token: 0x04002D98 RID: 11672
		private Texture2D fuelIcon;
	}
}
