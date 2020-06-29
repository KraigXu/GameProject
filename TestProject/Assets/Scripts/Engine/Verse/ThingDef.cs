using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	
	public class ThingDef : BuildableDef
	{
		
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001E7CC File Offset: 0x0001C9CC
		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001E7DE File Offset: 0x0001C9DE
		public float VolumePerUnit
		{
			get
			{
				if (this.smallVolume)
				{
					return 0.1f;
				}
				return 1f;
			}
		}

		
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001E7F3 File Offset: 0x0001C9F3
		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001E7FB File Offset: 0x0001C9FB
		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001E806 File Offset: 0x0001CA06
		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001E819 File Offset: 0x0001CA19
		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0001E827 File Offset: 0x0001CA27
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x0001E835 File Offset: 0x0001CA35
		public float BaseMarketValue
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			set
			{
				this.SetStatBaseValue(StatDefOf.MarketValue, value);
			}
		}

		
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x0001E843 File Offset: 0x0001CA43
		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001E851 File Offset: 0x0001CA51
		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop;
			}
		}

		
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001E85C File Offset: 0x0001CA5C
		public bool EverTransmitsPower
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					CompProperties_Power compProperties_Power = this.comps[i] as CompProperties_Power;
					if (compProperties_Power != null && compProperties_Power.transmitsPower)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001E89F File Offset: 0x0001CA9F
		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001E8AA File Offset: 0x0001CAAA
		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001E8BC File Offset: 0x0001CABC
		public List<RecipeDef> AllRecipes
		{
			get
			{
				if (this.allRecipesCached == null)
				{
					this.allRecipesCached = new List<RecipeDef>();
					if (this.recipes != null)
					{
						for (int i = 0; i < this.recipes.Count; i++)
						{
							this.allRecipesCached.Add(this.recipes[i]);
						}
					}
					List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].recipeUsers != null && allDefsListForReading[j].recipeUsers.Contains(this))
						{
							this.allRecipesCached.Add(allDefsListForReading[j]);
						}
					}
				}
				return this.allRecipesCached;
			}
		}

		
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001E968 File Offset: 0x0001CB68
		public bool ConnectToPower
		{
			get
			{
				if (this.EverTransmitsPower)
				{
					return false;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == typeof(CompPowerBattery))
					{
						return true;
					}
					if (this.comps[i].compClass == typeof(CompPowerTrader))
					{
						return true;
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001E9DE File Offset: 0x0001CBDE
		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001E9F3 File Offset: 0x0001CBF3
		public FillCategory Fillage
		{
			get
			{
				if (this.fillPercent < 0.01f)
				{
					return FillCategory.None;
				}
				if (this.fillPercent > 0.99f)
				{
					return FillCategory.Full;
				}
				return FillCategory.Partial;
			}
		}

		
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001EA14 File Offset: 0x0001CC14
		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x0001EA20 File Offset: 0x0001CC20
		public bool CanOverlapZones
		{
			get
			{
				if (this.building != null && this.building.SupportsPlants)
				{
					return false;
				}
				if (this.passability == Traversability.Impassable && this.category != ThingCategory.Plant)
				{
					return false;
				}
				if (this.surfaceType >= SurfaceType.Item)
				{
					return false;
				}
				if (typeof(ISlotGroupParent).IsAssignableFrom(this.thingClass))
				{
					return false;
				}
				if (!this.canOverlapZones)
				{
					return false;
				}
				if (this.IsBlueprint || this.IsFrame)
				{
					ThingDef thingDef = this.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						return thingDef.CanOverlapZones;
					}
				}
				return true;
			}
		}

		
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001EAAD File Offset: 0x0001CCAD
		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority > ResourceCountPriority.Uncounted;
			}
		}

		
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0001EAB8 File Offset: 0x0001CCB8
		[Obsolete("Will be removed in a future version.")]
		public bool BlockPlanting
		{
			get
			{
				return this.BlocksPlanting(false);
			}
		}

		
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001EAC1 File Offset: 0x0001CCC1
		public List<VerbProperties> Verbs
		{
			get
			{
				if (this.verbs != null)
				{
					return this.verbs;
				}
				return ThingDef.EmptyVerbPropertiesList;
			}
		}

		
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001EAD8 File Offset: 0x0001CCD8
		public bool CanHaveFaction
		{
			get
			{
				if (this.IsBlueprint || this.IsFrame)
				{
					return true;
				}
				ThingCategory thingCategory = this.category;
				return thingCategory == ThingCategory.Pawn || thingCategory == ThingCategory.Building;
			}
		}

		
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001EB0D File Offset: 0x0001CD0D
		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable && !this.building.isNaturalRock;
			}
		}

		
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001EB34 File Offset: 0x0001CD34
		public ThingCategoryDef FirstThingCategory
		{
			get
			{
				if (this.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					return null;
				}
				return this.thingCategories[0];
			}
		}

		
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001EB51 File Offset: 0x0001CD51
		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.7f, 0.5f, 1f);
			}
		}

		
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001EB74 File Offset: 0x0001CD74
		public bool CanEverDeteriorate
		{
			get
			{
				return this.useHitPoints && (this.category == ThingCategory.Item || this == ThingDefOf.BurnedTree);
			}
		}

		
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001EB93 File Offset: 0x0001CD93
		public bool CanInteractThroughCorners
		{
			get
			{
				return this.category == ThingCategory.Building && this.holdsRoof && (this.building == null || !this.building.isNaturalRock || this.IsSmoothed);
			}
		}

		
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001EBCA File Offset: 0x0001CDCA
		public bool AffectsRegions
		{
			get
			{
				return this.passability == Traversability.Impassable || this.IsDoor;
			}
		}

		
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001EBDD File Offset: 0x0001CDDD
		public bool AffectsReachability
		{
			get
			{
				return this.AffectsRegions || (this.passability == Traversability.Impassable || this.IsDoor) || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this);
			}
		}

		
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001EC08 File Offset: 0x0001CE08
		public string DescriptionDetailed
		{
			get
			{
				if (this.descriptionDetailedCached == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.description);
					if (this.IsApparel)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(string.Format("{0}: {1}", "Layer".Translate(), this.apparel.GetLayersString()));
						stringBuilder.Append(string.Format("{0}: {1}", "Covers".Translate(), this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human)));
						if (this.equippedStatOffsets != null && this.equippedStatOffsets.Count > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
							for (int i = 0; i < this.equippedStatOffsets.Count; i++)
							{
								if (i > 0)
								{
									stringBuilder.AppendLine();
								}
								StatModifier statModifier = this.equippedStatOffsets[i];
								stringBuilder.Append(string.Format("{0}: {1}", statModifier.stat.LabelCap, statModifier.ValueToStringAsOffset));
							}
						}
					}
					this.descriptionDetailedCached = stringBuilder.ToString();
				}
				return this.descriptionDetailedCached;
			}
		}

		
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001ED32 File Offset: 0x0001CF32
		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0001ED3D File Offset: 0x0001CF3D
		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001ED54 File Offset: 0x0001CF54
		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0001ED6B File Offset: 0x0001CF6B
		public bool IsFrame
		{
			get
			{
				return this.isFrameInt;
			}
		}

		
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001ED73 File Offset: 0x0001CF73
		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001ED89 File Offset: 0x0001CF89
		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001ED94 File Offset: 0x0001CF94
		public bool IsMedicine
		{
			get
			{
				return this.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001EDA6 File Offset: 0x0001CFA6
		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001EDBD File Offset: 0x0001CFBD
		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001EDC8 File Offset: 0x0001CFC8
		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001EDD3 File Offset: 0x0001CFD3
		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.CachedNutrition > 0f;
			}
		}

		
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0001EDF1 File Offset: 0x0001CFF1
		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty<VerbProperties>() || !this.tools.NullOrEmpty<Tool>());
			}
		}

		
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001EE1B File Offset: 0x0001D01B
		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001EE32 File Offset: 0x0001D032
		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001EE49 File Offset: 0x0001D049
		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001EE60 File Offset: 0x0001D060
		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory > DrugCategory.None;
			}
		}

		
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001EE7A File Offset: 0x0001D07A
		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0f;
			}
		}

		
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001EE98 File Offset: 0x0001D098
		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001EEB5 File Offset: 0x0001D0B5
		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001EED2 File Offset: 0x0001D0D2
		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001EEE9 File Offset: 0x0001D0E9
		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0001EEF4 File Offset: 0x0001D0F4
		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.BuildingsArt);
			}
		}

		
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001EF01 File Offset: 0x0001D101
		public bool IsSmoothable
		{
			get
			{
				return this.building != null && this.building.smoothedThing != null;
			}
		}

		
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0001EF1B File Offset: 0x0001D11B
		public bool IsSmoothed
		{
			get
			{
				return this.building != null && this.building.unsmoothedThing != null;
			}
		}

		
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0001EF35 File Offset: 0x0001D135
		public bool IsMetal
		{
			get
			{
				return this.stuffProps != null && this.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
		}

		
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0001EF58 File Offset: 0x0001D158
		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0f;
			}
		}

		
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0001EF7E File Offset: 0x0001D17E
		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0001EFA3 File Offset: 0x0001D1A3
		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
		}

		
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0001EFC8 File Offset: 0x0001D1C8
		public bool IsRangedWeapon
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (!this.verbs[i].IsMeleeAttack)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0001F018 File Offset: 0x0001D218
		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001F030 File Offset: 0x0001D230
		public bool IsWeaponUsingProjectiles
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (this.verbs[i].LaunchesProjectile)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0001F080 File Offset: 0x0001D280
		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
			}
		}

		
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0001F0BC File Offset: 0x0001D2BC
		public bool IsNonResourceNaturalRock
		{
			get
			{
				return this.category == ThingCategory.Building && this.building.isNaturalRock && !this.building.isResourceRock && !this.IsSmoothed;
			}
		}

		
		public bool BlocksPlanting(bool canWipePlants = false)
		{
			return (this.building == null || !this.building.SupportsPlants) && (this.blockPlants || (!canWipePlants && this.category == ThingCategory.Plant) || this.Fillage > FillCategory.None || this.IsEdifice());
		}

		
		public bool EverStorable(bool willMinifyIfPossible)
		{
			if (typeof(MinifiedThing).IsAssignableFrom(this.thingClass))
			{
				return true;
			}
			if (!this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				if (this.category == ThingCategory.Item)
				{
					return true;
				}
				if (willMinifyIfPossible && this.Minifiable)
				{
					return true;
				}
			}
			return false;
		}

		
		public Thing GetConcreteExample(ThingDef stuff = null)
		{
			if (this.concreteExamplesInt == null)
			{
				this.concreteExamplesInt = new Dictionary<ThingDef, Thing>();
			}
			if (stuff == null)
			{
				stuff = ThingDefOf.Steel;
			}
			if (!this.concreteExamplesInt.ContainsKey(stuff))
			{
				if (this.race == null)
				{
					this.concreteExamplesInt[stuff] = ThingMaker.MakeThing(this, base.MadeFromStuff ? stuff : null);
				}
				else
				{
					this.concreteExamplesInt[stuff] = PawnGenerator.GeneratePawn((from pkd in DefDatabase<PawnKindDef>.AllDefsListForReading
					where pkd.race == this
					select pkd).FirstOrDefault<PawnKindDef>(), null);
				}
			}
			return this.concreteExamplesInt[stuff];
		}

		
		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(T));
		}

		
		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => typeof(T).IsAssignableFrom(c.compClass));
		}

		
		public bool HasComp(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == compType)
				{
					return true;
				}
			}
			return false;
		}

		
		public bool HasAssignableCompFrom(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (compType.IsAssignableFrom(this.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		
		public T GetCompProperties<T>() where T : CompProperties
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.graphicData.shaderType == null)
					{
						this.graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					this.graphic = this.graphicData.Graphic;
				});
			}
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			if (this.verbs != null && this.verbs.Count == 1)
			{
				this.verbs[0].label = this.label;
			}
			base.PostLoad();
			if (this.category == ThingCategory.Building && this.building == null)
			{
				this.building = new BuildingProperties();
			}
			if (this.building != null)
			{
				this.building.PostLoadSpecial(this);
			}
			if (this.plant != null)
			{
				this.plant.PostLoadSpecial(this);
			}
			if (this.comps != null)
			{
				foreach (CompProperties compProperties in this.comps)
				{
					compProperties.PostLoadSpecial(this);
				}
			}
		}

		
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			if (this.category == ThingCategory.Pawn)
			{
				if (!this.race.Humanlike)
				{
					PawnKindDef anyPawnKind = this.race.AnyPawnKind;
					if (anyPawnKind != null)
					{
						Material material = anyPawnKind.lifeStages.Last<PawnKindLifeStage>().bodyGraphicData.Graphic.MatAt(Rot4.East, null);
						this.uiIcon = (Texture2D)material.mainTexture;
						this.uiIconColor = material.color;
						return;
					}
				}
			}
			else
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(this);
				if (this.colorGenerator != null && (thingDef == null || thingDef.stuffProps.allowColorGenerators))
				{
					this.uiIconColor = this.colorGenerator.ExemplaryColor;
				}
				else if (thingDef != null)
				{
					this.uiIconColor = base.GetColorForStuff(thingDef);
				}
				else if (this.graphicData != null)
				{
					this.uiIconColor = this.graphicData.color;
				}
				if (this.rotatable && this.graphic != null && this.graphic != BaseContent.BadGraphic && this.graphic.ShouldDrawRotated && this.defaultPlacingRot == Rot4.South)
				{
					this.uiIconAngle = 180f + this.graphic.DrawRotatedExtraAngleOffset;
				}
			}
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.ingestible != null)
			{
				this.ingestible.parent = this;
			}
			if (this.stuffProps != null)
			{
				this.stuffProps.parent = this;
			}
			if (this.building != null)
			{
				this.building.ResolveReferencesSpecial();
			}
			if (this.graphicData != null)
			{
				this.graphicData.ResolveReferencesSpecial();
			}
			if (this.race != null)
			{
				this.race.ResolveReferencesSpecial();
			}
			if (this.stuffProps != null)
			{
				this.stuffProps.ResolveReferencesSpecial();
			}
			if (this.soundImpactDefault == null)
			{
				this.soundImpactDefault = SoundDefOf.BulletImpact_Ground;
			}
			if (this.soundDrop == null)
			{
				this.soundDrop = SoundDefOf.Standard_Drop;
			}
			if (this.soundPickup == null)
			{
				this.soundPickup = SoundDefOf.Standard_Pickup;
			}
			if (this.soundInteract == null)
			{
				this.soundInteract = SoundDefOf.Standard_Pickup;
			}
			if (this.inspectorTabs != null && this.inspectorTabs.Any<Type>())
			{
				this.inspectorTabsResolved = new List<InspectTabBase>();
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}), false);
					}
				}
			}
			if (this.comps != null)
			{
				for (int j = 0; j < this.comps.Count; j++)
				{
					this.comps[j].ResolveReferences(this);
				}
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.graphicData != null)
			{
				foreach (string text2 in this.graphicData.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			if (this.projectile != null)
			{
				foreach (string text3 in this.projectile.ConfigErrors(this))
				{
					yield return text3;
				}
				enumerator = null;
			}
			if (this.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						StatModifier statBase = enumerator2.Current;
						if ((from st in this.statBases
						where st.stat == statBase.stat
						select st).Count<StatModifier>() > 1)
						{
							yield return "defines the stat base " + statBase.stat + " more than once.";
						}
					}
				}
				
			}
			if (!BeautyUtility.BeautyRelevant(this.category) && this.StatBaseDefined(StatDefOf.Beauty))
			{
				yield return "Beauty stat base is defined, but Things of category " + this.category + " cannot have beauty.";
			}
			if (char.IsNumber(this.defName[this.defName.Length - 1]))
			{
				yield return "ends with a numerical digit, which is not allowed on ThingDefs.";
			}
			if (this.thingClass == null)
			{
				yield return "has null thingClass.";
			}
			if (this.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(this.thingClass))
			{
				yield return "has components but it's thingClass is not a ThingWithComps";
			}
			if (this.ConnectToPower && this.drawerType == DrawerType.RealtimeOnly && this.IsFrame)
			{
				yield return "connects to power but does not add to map mesh. Will not create wire meshes.";
			}
			if (this.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in this.costList)
				{
					if (thingDefCountClass.count == 0)
					{
						yield return "cost in " + thingDefCountClass.thingDef + " is zero.";
					}
				}
				List<ThingDefCountClass>.Enumerator enumerator3 = default(List<ThingDefCountClass>.Enumerator);
			}
			if (this.thingCategories != null)
			{
				ThingCategoryDef thingCategoryDef = this.thingCategories.FirstOrDefault((ThingCategoryDef cat) => this.thingCategories.Count((ThingCategoryDef c) => c == cat) > 1);
				if (thingCategoryDef != null)
				{
					yield return "has duplicate thingCategory " + thingCategoryDef + ".";
				}
			}
			if (this.Fillage == FillCategory.Full && this.category != ThingCategory.Building)
			{
				yield return "gives full cover but is not a building.";
			}
			if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompPowerTrader)) && this.drawerType == DrawerType.MapMeshOnly)
			{
				yield return "has PowerTrader comp but does not draw real time. It won't draw a needs-power overlay.";
			}
			if (this.equipmentType != EquipmentType.None)
			{
				if (this.techLevel == TechLevel.Undefined && !this.destroyOnDrop)
				{
					yield return "is equipment but has no tech level.";
				}
				if (!this.comps.Any((CompProperties c) => c.compClass == typeof(CompEquippable)))
				{
					yield return "is equipment but has no CompEquippable";
				}
			}
			if (this.thingClass == typeof(Bullet) && this.projectile.damageDef == null)
			{
				yield return " is a bullet but has no damageDef.";
			}
			if (this.destroyOnDrop)
			{
				if (!this.menuHidden)
				{
					yield return "destroyOnDrop but not menuHidden.";
				}
				if (this.tradeability != Tradeability.None)
				{
					yield return "destroyOnDrop but tradeability is " + this.tradeability;
				}
			}
			if (this.stackLimit > 1 && !this.drawGUIOverlay)
			{
				yield return "has stackLimit > 1 but also has drawGUIOverlay = false.";
			}
			if (this.damageMultipliers != null)
			{
				using (List<DamageMultiplier>.Enumerator enumerator4 = this.damageMultipliers.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						DamageMultiplier mult = enumerator4.Current;
						if ((from m in this.damageMultipliers
						where m.damageDef == mult.damageDef
						select m).Count<DamageMultiplier>() > 1)
						{
							yield return "has multiple damage multipliers for damageDef " + mult.damageDef;
							break;
						}
					}
				}
			
			}
			if (this.Fillage == FillCategory.Full && !this.IsEdifice())
			{
				yield return "fillPercent is 1.00 but is not edifice";
			}
			if (base.MadeFromStuff && this.constructEffect != null)
			{
				yield return "madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
			}
			if (base.MadeFromStuff && this.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				yield return "madeFromStuff but has no stuffCategories.";
			}
			if (this.costList.NullOrEmpty<ThingDefCountClass>() && this.costStuffCount <= 0 && this.recipeMaker != null)
			{
				yield return "has a recipeMaker but no costList or costStuffCount.";
			}
			if (this.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 1E-05f && !this.CanEverDeteriorate && !this.destroyOnDrop)
			{
				yield return "has >0 DeteriorationRate but can't deteriorate.";
			}
			if (this.drawerType == DrawerType.MapMeshOnly)
			{
				if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompForbiddable)))
				{
					yield return "drawerType=MapMeshOnly but has a CompForbiddable, which must draw in real time.";
				}
			}
			if (this.smeltProducts != null && this.smeltable)
			{
				yield return "has smeltProducts but has smeltable=false";
			}
			if (this.equipmentType != EquipmentType.None && this.verbs.NullOrEmpty<VerbProperties>() && this.tools.NullOrEmpty<Tool>())
			{
				yield return "is equipment but has no verbs or tools";
			}
			if (this.Minifiable && this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is minifiable but not in any thing category";
			}
			if (this.category == ThingCategory.Building && !this.Minifiable && !this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is not minifiable yet has thing categories (could be confusing in thing filters because it can't be moved/stored anyway)";
			}
			if (!this.destroyOnDrop && this != ThingDefOf.MinifiedThing && (this.EverHaulable || this.Minifiable))
			{
				if (!this.statBases.NullOrEmpty<StatModifier>())
				{
					if (this.statBases.Any((StatModifier s) => s.stat == StatDefOf.Mass))
					{
						goto IL_A96;
					}
				}
				yield return "is haulable, but does not have an authored mass value";
			}
			IL_A96:
			if (this.ingestible == null && this.GetStatValueAbstract(StatDefOf.Nutrition, null) != 0f)
			{
				yield return "has nutrition but ingestible properties are null";
			}
			if (this.BaseFlammability != 0f && !this.useHitPoints && this.category != ThingCategory.Pawn && !this.destroyOnDrop)
			{
				yield return "flammable but has no hitpoints (will burn indefinitely)";
			}
			if (this.graphicData != null && this.graphicData.shadowData != null && this.staticSunShadowHeight > 0f)
			{
				yield return "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
			}
			if (this.saveCompressible && this.Claimable)
			{
				yield return "claimable item is compressible; faction will be unset after load";
			}
			if (this.deepCommonality > 0f != this.deepLumpSizeRange.TrueMax > 0)
			{
				yield return "if deepCommonality or deepLumpSizeRange is set, the other also must be set";
			}
			if (this.deepCommonality > 0f && this.deepCountPerPortion <= 0)
			{
				yield return "deepCommonality > 0 but deepCountPerPortion is not set";
			}
			if (this.verbs != null)
			{
				int num;
				for (int i = 0; i < this.verbs.Count; i = num + 1)
				{
					foreach (string arg in this.verbs[i].ConfigErrors(this))
					{
						yield return string.Format("verb {0}: {1}", i, arg);
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.building != null)
			{
				foreach (string text4 in this.building.ConfigErrors(this))
				{
					yield return text4;
				}
				enumerator = null;
			}
			if (this.apparel != null)
			{
				foreach (string text5 in this.apparel.ConfigErrors(this))
				{
					yield return text5;
				}
				enumerator = null;
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string text6 in this.comps[i].ConfigErrors(this))
					{
						yield return text6;
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.race != null)
			{
				foreach (string text7 in this.race.ConfigErrors())
				{
					yield return text7;
				}
				enumerator = null;
			}
			if (this.race != null && this.tools != null)
			{
				//ThingDef.c__DisplayClass269_3 c__DisplayClass269_3 = new ThingDef.c__DisplayClass269_3();
				//c__DisplayClass269_3.4__this = this;
				//c__DisplayClass269_3.i = 0;
				//while (c__DisplayClass269_3.i < this.tools.Count)
				//{
				//	if (this.tools[c__DisplayClass269_3.i].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(c__DisplayClass269_3.4__this.tools[c__DisplayClass269_3.i].linkedBodyPartsGroup)))
				//	{
				//		yield return string.Concat(new object[]
				//		{
				//			"has tool with linkedBodyPartsGroup ",
				//			this.tools[c__DisplayClass269_3.i].linkedBodyPartsGroup,
				//			" but body ",
				//			this.race.body,
				//			" has no parts with that group."
				//		});
				//	}
				//	int num = c__DisplayClass269_3.i;
				//	c__DisplayClass269_3.i = num + 1;
				//}
				//c__DisplayClass269_3 = null;
			}
			if (this.ingestible != null)
			{
				foreach (string text8 in this.ingestible.ConfigErrors())
				{
					yield return text8;
				}
				enumerator = null;
			}
			if (this.plant != null)
			{
				foreach (string text9 in this.plant.ConfigErrors())
				{
					yield return text9;
				}
				enumerator = null;
			}
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return string.Format("duplicate thingdef tool id {0}", tool.id);
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text10 in tool2.ConfigErrors())
					{
						yield return text10;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator5 = default(List<Tool>.Enumerator);
			}
			yield break;
			yield break;
		}

		
		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0001F75D File Offset: 0x0001D95D
		public string LabelAsStuff
		{
			get
			{
				if (!this.stuffProps.stuffAdjective.NullOrEmpty())
				{
					return this.stuffProps.stuffAdjective;
				}
				return this.label;
			}
		}

		
		public bool IsWithinCategory(ThingCategoryDef category)
		{
			if (this.thingCategories == null)
			{
				return false;
			}
			for (int i = 0; i < this.thingCategories.Count; i++)
			{
				for (ThingCategoryDef thingCategoryDef = this.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
				{
					if (thingCategoryDef == category)
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{

			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.apparel != null)
			{
				string coveredOuterPartsString = this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredOuterPartsString, "Stat_Thing_Apparel_Covers_Desc".Translate(), 2750, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Layer".Translate(), this.apparel.GetLayersString(), "Stat_Thing_Apparel_Layer_Desc".Translate(), 2751, null, null, false);
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), "Stat_Thing_Drug_MedicineXpGainFactor_Desc".Translate(), 1000, null, null, false);
			}
			if (this.fillPercent > 0f && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), "CoverEffectivenessExplanation".Translate(), 2000, null, null, false);
				yield return statDrawEntry2;
			}
			if (this.constructionSkillPrerequisite > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Construction.LabelCap), this.constructionSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Construction.LabelCap), 1100, null, null, false);
			}
			if (this.artisticSkillPrerequisite > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Artistic.LabelCap), this.artisticSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Artistic.LabelCap), 1100, null, null, false);
			}
			string[] array = (from u in (from r in DefDatabase<RecipeDef>.AllDefsListForReading
			where r.recipeUsers != null && r.products.Count == 1 && r.products.Any((ThingDefCountClass p) => p.thingDef == this) && !r.IsSurgery
			select r).SelectMany((RecipeDef r) => r.recipeUsers)
			select u.label).ToArray<string>();
			if (array.Any<string>())
			{
				string valueString = array.ToCommaList(false).CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CreatedAt".Translate(), valueString, "Stat_Thing_CreatedAt_Desc".Translate(), 1103, null, null, false);
			}
			if (this.thingClass != null && typeof(Building_Bed).IsAssignableFrom(this.thingClass) && !this.statBases.StatListContains(StatDefOf.BedRestEffectiveness))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.BedRestEffectiveness, StatDefOf.BedRestEffectiveness.valueIfMissing, StatRequest.ForEmpty(), ToStringNumberSense.Undefined, null, false);
			}
			if (!this.verbs.NullOrEmpty<VerbProperties>())
			{
				VerbProperties verb = this.verbs.First((VerbProperties x) => x.isPrimary);
				StatCategoryDef verbStatCategory = (this.category == ThingCategory.Pawn) ? (verbStatCategory = StatCategoryDefOf.PawnCombat) : (verbStatCategory = StatCategoryDefOf.Weapon);
				float warmupTime = verb.warmupTime;
				if (warmupTime > 0f)
				{
					TaggedString taggedString = (this.category == ThingCategory.Pawn) ? "MeleeWarmupTime".Translate() : "WarmupTime".Translate();
					yield return new StatDrawEntry(verbStatCategory, taggedString, warmupTime.ToString("0.##") + " " + "LetterSecond".Translate(), "Stat_Thing_Weapon_MeleeWarmupTime_Desc".Translate(), 3555, null, null, false);
				}
				if (verb.defaultProjectile != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Stat_Thing_Damage_Desc".Translate());
					stringBuilder.AppendLine();
					float num = (float)verb.defaultProjectile.projectile.GetDamageAmount(req.Thing, stringBuilder);
					yield return new StatDrawEntry(verbStatCategory, "Damage".Translate(), num.ToString(), stringBuilder.ToString(), 5500, null, null, false);
					if (verb.defaultProjectile.projectile.damageDef.armorCategory != null)
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						float armorPenetration = verb.defaultProjectile.projectile.GetArmorPenetration(req.Thing, stringBuilder2);
						TaggedString taggedString2 = "ArmorPenetrationExplanation".Translate();
						if (stringBuilder2.Length != 0)
						{
							taggedString2 += "\n\n" + stringBuilder2;
						}
						yield return new StatDrawEntry(verbStatCategory, "ArmorPenetration".Translate(), armorPenetration.ToStringPercent(), taggedString2, 5400, null, null, false);
					}
				}
				if (verb.LaunchesProjectile)
				{
					int burstShotCount = verb.burstShotCount;
					float burstShotFireRate = 60f / verb.ticksBetweenBurstShots.TicksToSeconds();
					float range = verb.range;
					if (burstShotCount > 1)
					{
						yield return new StatDrawEntry(verbStatCategory, "BurstShotCount".Translate(), burstShotCount.ToString(), "Stat_Thing_Weapon_BurstShotCount_Desc".Translate(), 5391, null, null, false);
						yield return new StatDrawEntry(verbStatCategory, "BurstShotFireRate".Translate(), burstShotFireRate.ToString("0.##") + " rpm", "Stat_Thing_Weapon_BurstShotFireRate_Desc".Translate(), 5392, null, null, false);
					}
					yield return new StatDrawEntry(verbStatCategory, "Range".Translate(), range.ToString("F0"), "Stat_Thing_Weapon_Range_Desc".Translate(), 5390, null, null, false);
					if (verb.defaultProjectile != null && verb.defaultProjectile.projectile != null && verb.defaultProjectile.projectile.stoppingPower != 0f)
					{
						yield return new StatDrawEntry(verbStatCategory, "StoppingPower".Translate(), verb.defaultProjectile.projectile.stoppingPower.ToString("F1"), "StoppingPowerExplanation".Translate(), 5402, null, null, false);
					}
				}
				if (verb.forcedMissRadius > 0f)
				{
					yield return new StatDrawEntry(verbStatCategory, "MissRadius".Translate(), verb.forcedMissRadius.ToString("0.#"), "Stat_Thing_Weapon_MissRadius_Desc".Translate(), 3557, null, null, false);
					yield return new StatDrawEntry(verbStatCategory, "DirectHitChance".Translate(), (1f / (float)GenRadial.NumCellsInRadius(verb.forcedMissRadius)).ToStringPercent(), "Stat_Thing_Weapon_DirectHitChance_Desc".Translate(), 3560, null, null, false);
				}
				verb = null;
				verbStatCategory = null;
			}
			if (this.plant != null)
			{
				foreach (StatDrawEntry statDrawEntry3 in this.plant.SpecialDisplayStats())
				{
					yield return statDrawEntry3;
				}
				enumerator = null;
			}
			if (this.ingestible != null)
			{
				foreach (StatDrawEntry statDrawEntry4 in this.ingestible.SpecialDisplayStats())
				{
					yield return statDrawEntry4;
				}
				enumerator = null;
			}
			if (this.race != null)
			{
				foreach (StatDrawEntry statDrawEntry5 in this.race.SpecialDisplayStats(this, req))
				{
					yield return statDrawEntry5;
				}
				enumerator = null;
			}
			if (this.building != null)
			{
				foreach (StatDrawEntry statDrawEntry6 in this.building.SpecialDisplayStats(this, req))
				{
					yield return statDrawEntry6;
				}
				enumerator = null;
			}
			if (this.isTechHediff)
			{
				IEnumerable<RecipeDef> enumerable = from x in DefDatabase<RecipeDef>.AllDefs
				where x.addsHediff != null && x.IsIngredient(this)
				select x;
				bool multiple = enumerable.Count<RecipeDef>() >= 2;
				foreach (RecipeDef recipeDef in enumerable)
				{
					//ThingDef.c__DisplayClass274_0 c__DisplayClass274_ = new ThingDef.c__DisplayClass274_0();
					string extraLabelPart = multiple ? (" (" + recipeDef.addsHediff.label + ")") : "";
					//c__DisplayClass274_.diff = recipeDef.addsHediff;
					//if (c__DisplayClass274_.diff.addedPartProps != null)
					//{
					//	yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate() + extraLabelPart, c__DisplayClass274_.diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), "Stat_Thing_BodyPartEfficiency_Desc".Translate(), 4000, null, null, false);
					//}
					//foreach (StatDrawEntry statDrawEntry7 in c__DisplayClass274_.diff.SpecialDisplayStats(StatRequest.ForEmpty()))
					//{
					//	statDrawEntry7.category = StatCategoryDefOf.Implant;
					//	yield return statDrawEntry7;
					//}
					enumerator = null;
					HediffCompProperties_VerbGiver hediffCompProperties_VerbGiver = recipeDef.addsHediff.CompProps<HediffCompProperties_VerbGiver>();
					if (hediffCompProperties_VerbGiver != null)
					{
						if (!hediffCompProperties_VerbGiver.verbs.NullOrEmpty<VerbProperties>())
						{
							VerbProperties verb = hediffCompProperties_VerbGiver.verbs[0];
							if (!verb.IsMeleeAttack)
							{
								if (verb.defaultProjectile != null)
								{
									StringBuilder stringBuilder3 = new StringBuilder();
									stringBuilder3.AppendLine("Stat_Thing_Damage_Desc".Translate());
									stringBuilder3.AppendLine();
									int damageAmount = verb.defaultProjectile.projectile.GetDamageAmount(null, stringBuilder3);
									yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate() + extraLabelPart, damageAmount.ToString(), stringBuilder3.ToString(), 5500, null, null, false);
									if (verb.defaultProjectile.projectile.damageDef.armorCategory != null)
									{
										float armorPenetration2 = verb.defaultProjectile.projectile.GetArmorPenetration(null, null);
										yield return new StatDrawEntry(StatCategoryDefOf.Basics, "ArmorPenetration".Translate() + extraLabelPart, armorPenetration2.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false);
									}
								}
							}
							else
							{
								int meleeDamageBaseAmount = verb.meleeDamageBaseAmount;
								if (verb.meleeDamageDef.armorCategory != null)
								{
									float num2 = verb.meleeArmorPenetrationBase;
									if (num2 < 0f)
									{
										num2 = (float)meleeDamageBaseAmount * 0.015f;
									}
									yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "ArmorPenetration".Translate() + extraLabelPart, num2.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false);
								}
							}
							verb = null;
						}
						else if (!hediffCompProperties_VerbGiver.tools.NullOrEmpty<Tool>())
						{
							Tool tool = hediffCompProperties_VerbGiver.tools[0];
							if (ThingUtility.PrimaryMeleeWeaponDamageType(hediffCompProperties_VerbGiver.tools).armorCategory != null)
							{
								float num3 = tool.armorPenetration;
								if (num3 < 0f)
								{
									num3 = tool.power * 0.015f;
								}
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "ArmorPenetration".Translate() + extraLabelPart, num3.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false);
							}
						}
					}
					ThoughtDef thoughtDef = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((ThoughtDef x) => x.hediff == recipeDef.addsHediff);
					if (thoughtDef != null && thoughtDef.stages != null && thoughtDef.stages.Any<ThoughtStage>())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate() + extraLabelPart, thoughtDef.stages.First<ThoughtStage>().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), "Stat_Thing_MoodChange_Desc".Translate(), 3500, null, null, false);
					}
					extraLabelPart = null;
				}
				IEnumerator<RecipeDef> enumerator2 = null;
			}
			int num4;
			for (int i = 0; i < this.comps.Count; i = num4 + 1)
			{
				foreach (StatDrawEntry statDrawEntry8 in this.comps[i].SpecialDisplayStats(req))
				{
					yield return statDrawEntry8;
				}
				enumerator = null;
				num4 = i;
			}
			if (this.building != null)
			{
				if (this.building.mineableThing != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_MineableThing_Name".Translate(), this.building.mineableThing.LabelCap, "Stat_MineableThing_Desc".Translate(), 2200, null, new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(this.building.mineableThing, -1)
					}, false);
				}
				if (this.building.IsTurret)
				{
					ThingDef turret = this.building.turretGunDef;
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Weapon_Name".Translate(), turret.LabelCap, "Stat_Weapon_Desc".Translate(), 5389, null, new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(turret, -1)
					}, false);
					StatRequest request = StatRequest.For(turret, null, QualityCategory.Normal);
					foreach (StatDrawEntry statDrawEntry9 in turret.SpecialDisplayStats(request))
					{
						if (statDrawEntry9.category == StatCategoryDefOf.Weapon)
						{
							yield return statDrawEntry9;
						}
					}
					enumerator = null;
					for (int i = 0; i < turret.statBases.Count; i = num4 + 1)
					{
						StatModifier statModifier = turret.statBases[i];
						if (statModifier.stat.category == StatCategoryDefOf.Weapon)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Weapon, statModifier.stat, statModifier.value, request, ToStringNumberSense.Undefined, null, false);
						}
						num4 = i;
					}
					turret = null;
					request = default(StatRequest);
				}
			}
			if (this.IsMeat)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.race != null && thingDef.race.meatDef == this)
					{
						list.Add(thingDef);
					}
				}
				string valueString2 = string.Join(", ", (from p in list
				select p.label).ToArray<string>()).CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_SourceSpecies_Name".Translate(), valueString2, "Stat_SourceSpecies_Desc".Translate(), 1200, null, Dialog_InfoCard.DefsToHyperlinks(list), false);
			}
			if (this.IsLeather)
			{
				List<ThingDef> list2 = new List<ThingDef>();
				foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef2.race != null && thingDef2.race.leatherDef == this)
					{
						list2.Add(thingDef2);
					}
				}
				string valueString3 = string.Join(", ", (from p in list2
				select p.label).ToArray<string>()).CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_SourceSpecies_Name".Translate(), valueString3, "Stat_SourceSpecies_Desc".Translate(), 1200, null, Dialog_InfoCard.DefsToHyperlinks(list2), false);
			}
			if (!this.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < this.equippedStatOffsets.Count; i = num4 + 1)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, this.equippedStatOffsets[i].stat, this.equippedStatOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, true);
					num4 = i;
				}
			}
			if (this.IsDrug)
			{
				foreach (StatDrawEntry statDrawEntry10 in DrugStatsUtility.SpecialDisplayStats(this))
				{
					yield return statDrawEntry10;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		
		public Type thingClass;

		
		public ThingCategory category;

		
		public TickerType tickerType;

		
		public int stackLimit = 1;

		
		public IntVec2 size = new IntVec2(1, 1);

		
		public bool destroyable = true;

		
		public bool rotatable = true;

		
		public bool smallVolume;

		
		public bool useHitPoints = true;

		
		public bool receivesSignals;

		
		public List<CompProperties> comps = new List<CompProperties>();

		
		public List<ThingDefCountClass> killedLeavings;

		
		public List<ThingDefCountClass> butcherProducts;

		
		public List<ThingDefCountClass> smeltProducts;

		
		public bool smeltable;

		
		public bool burnableByRecipe;

		
		public bool randomizeRotationOnSpawn;

		
		public List<DamageMultiplier> damageMultipliers;

		
		public bool isTechHediff;

		
		public RecipeMakerProperties recipeMaker;

		
		public ThingDef minifiedDef;

		
		public bool isUnfinishedThing;

		
		public bool leaveResourcesWhenKilled;

		
		public ThingDef slagDef;

		
		public bool isFrameInt;

		
		public IntVec3 interactionCellOffset = IntVec3.Zero;

		
		public bool hasInteractionCell;

		
		public ThingDef interactionCellIcon;

		
		public bool interactionCellIconReverse;

		
		public ThingDef filthLeaving;

		
		public bool forceDebugSpawnable;

		
		public bool intricate;

		
		public bool scatterableOnMapGen = true;

		
		public float deepCommonality;

		
		public int deepCountPerCell = 300;

		
		public int deepCountPerPortion = -1;

		
		public IntRange deepLumpSizeRange = IntRange.zero;

		
		public float generateCommonality = 1f;

		
		public float generateAllowChance = 1f;

		
		private bool canOverlapZones = true;

		
		public FloatRange startingHpRange = FloatRange.One;

		
		[NoTranslate]
		public List<string> thingSetMakerTags;

		
		public bool alwaysFlee;

		
		public List<RecipeDef> recipes;

		
		public bool messageOnDeteriorateInStorage = true;

		
		public bool canLoadIntoCaravan = true;

		
		public bool isMechClusterThreat;

		
		public FloatRange displayNumbersBetweenSameDefDistRange = FloatRange.Zero;

		
		public int minRewardCount = 1;

		
		public GraphicData graphicData;

		
		public DrawerType drawerType = DrawerType.RealtimeOnly;

		
		public bool drawOffscreen;

		
		public ColorGenerator colorGenerator;

		
		public float hideAtSnowDepth = 99999f;

		
		public bool drawDamagedOverlay = true;

		
		public bool castEdgeShadows;

		
		public float staticSunShadowHeight;

		
		public bool useSameGraphicForGhost;

		
		public bool selectable;

		
		public bool neverMultiSelect;

		
		public bool isAutoAttackableMapObject;

		
		public bool hasTooltip;

		
		public List<Type> inspectorTabs;

		
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		
		public bool seeThroughFog;

		
		public bool drawGUIOverlay;

		
		public bool drawGUIOverlayQuality = true;

		
		public ResourceCountPriority resourceReadoutPriority;

		
		public bool resourceReadoutAlwaysShow;

		
		public bool drawPlaceWorkersWhileSelected;

		
		public bool drawPlaceWorkersWhileInstallBlueprintSelected;

		
		public ConceptDef storedConceptLearnOpportunity;

		
		public float uiIconScale = 1f;

		
		public bool hasCustomRectForSelector;

		
		public bool alwaysHaulable;

		
		public bool designateHaulable;

		
		public List<ThingCategoryDef> thingCategories;

		
		public bool mineable;

		
		public bool socialPropernessMatters;

		
		public bool stealable = true;

		
		public SoundDef soundDrop;

		
		public SoundDef soundPickup;

		
		public SoundDef soundInteract;

		
		public SoundDef soundImpactDefault;

		
		public SoundDef soundPlayInstrument;

		
		public bool saveCompressible;

		
		public bool isSaveable = true;

		
		public bool holdsRoof;

		
		public float fillPercent;

		
		public bool coversFloor;

		
		public bool neverOverlapFloors;

		
		public SurfaceType surfaceType;

		
		public bool blockPlants;

		
		public bool blockLight;

		
		public bool blockWind;

		
		public Tradeability tradeability = Tradeability.All;

		
		[NoTranslate]
		public List<string> tradeTags;

		
		public bool tradeNeverStack;

		
		public bool healthAffectsPrice = true;

		
		public ColorGenerator colorGeneratorInTraderStock;

		
		private List<VerbProperties> verbs;

		
		public List<Tool> tools;

		
		public float equippedAngleOffset;

		
		public EquipmentType equipmentType;

		
		public TechLevel techLevel;

		
		[NoTranslate]
		public List<string> weaponTags;

		
		[NoTranslate]
		public List<string> techHediffsTags;

		
		public bool destroyOnDrop;

		
		public List<StatModifier> equippedStatOffsets;

		
		public SoundDef meleeHitSound;

		
		public BuildableDef entityDefToBuild;

		
		public ThingDef projectileWhenLoaded;

		
		public IngestibleProperties ingestible;

		
		public FilthProperties filth;

		
		public GasProperties gas;

		
		public BuildingProperties building;

		
		public RaceProperties race;

		
		public ApparelProperties apparel;

		
		public MoteProperties mote;

		
		public PlantProperties plant;

		
		public ProjectileProperties projectile;

		
		public StuffProperties stuffProps;

		
		public SkyfallerProperties skyfaller;

		
		public bool canBeUsedUnderRoof = true;

		
		[Unsaved(false)]
		private string descriptionDetailedCached;

		
		[Unsaved(false)]
		public Graphic interactionCellGraphic;

		
		public const int SmallUnitPerVolume = 10;

		
		public const float SmallVolumePerUnit = 0.1f;

		
		private List<RecipeDef> allRecipesCached;

		
		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		
		private Dictionary<ThingDef, Thing> concreteExamplesInt;
	}
}
