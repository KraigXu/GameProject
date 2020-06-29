using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class BuildingProperties
	{
		
		// (get) Token: 0x060034F7 RID: 13559 RVA: 0x0012228A File Offset: 0x0012048A
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x00122295 File Offset: 0x00120495
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		
		// (get) Token: 0x060034F9 RID: 13561 RVA: 0x001222A0 File Offset: 0x001204A0
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x001222BC File Offset: 0x001204BC
		public bool IsMortar
		{
			get
			{
				if (!this.IsTurret)
				{
					return false;
				}
				List<VerbProperties> verbs = this.turretGunDef.Verbs;
				for (int i = 0; i < verbs.Count; i++)
				{
					if (verbs[i].isPrimary && verbs[i].defaultProjectile != null && verbs[i].defaultProjectile.projectile.flyOverhead)
					{
						return true;
					}
				}
				if (this.turretGunDef.HasComp(typeof(CompChangeableProjectile)))
				{
					if (this.turretGunDef.building.fixedStorageSettings.filter.Allows(ThingDefOf.Shell_HighExplosive))
					{
						return true;
					}
					foreach (ThingDef thingDef in this.turretGunDef.building.fixedStorageSettings.filter.AllowedThingDefs)
					{
						if (thingDef.projectileWhenLoaded != null && thingDef.projectileWhenLoaded.projectile.flyOverhead)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.isTrap && !this.isEdifice)
			{
				yield return "isTrap but is not edifice. Code will break.";
			}
			if (this.alwaysDeconstructible && !this.deconstructible)
			{
				yield return "alwaysDeconstructible=true but deconstructible=false";
			}
			if (parent.holdsRoof && !this.isEdifice)
			{
				yield return "holds roof but is not an edifice.";
			}
			if (this.buildingTags.Contains("MechClusterCombatThreat") && this.combatPower <= 0f)
			{
				yield return "has MechClusterCombatThreat tag but 0 combatPower and thus no points cost; this will make an infinite loop during mech cluster building selection";
			}
			yield break;
		}

		
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		
		public void ResolveReferencesSpecial()
		{
			if (this.soundDoorOpenPowered == null)
			{
				this.soundDoorOpenPowered = SoundDefOf.Door_OpenPowered;
			}
			if (this.soundDoorClosePowered == null)
			{
				this.soundDoorClosePowered = SoundDefOf.Door_ClosePowered;
			}
			if (this.soundDoorOpenManual == null)
			{
				this.soundDoorOpenManual = SoundDefOf.Door_OpenManual;
			}
			if (this.soundDoorCloseManual == null)
			{
				this.soundDoorCloseManual = SoundDefOf.Door_CloseManual;
			}
			if (this.turretGunDef != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopMat = MaterialPool.MatFrom(this.turretGunDef.graphicData.texPath);
				});
			}
			if (this.fixedStorageSettings != null)
			{
				this.fixedStorageSettings.filter.ResolveReferences();
			}
			if (this.defaultStorageSettings == null && this.fixedStorageSettings != null)
			{
				this.defaultStorageSettings = new StorageSettings();
				this.defaultStorageSettings.CopyFrom(this.fixedStorageSettings);
			}
			if (this.defaultStorageSettings != null)
			{
				this.defaultStorageSettings.filter.ResolveReferences();
			}
		}

		
		public static void FinalizeInit()
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingDef.building != null && thingDef.building.smoothedThing != null)
				{
					ThingDef thingDef2 = thingDef.building.smoothedThing;
					if (thingDef2.building == null)
					{
						Log.Error(string.Format("{0} is smoothable to non-building {1}", thingDef, thingDef2), false);
					}
					else if (thingDef2.building.unsmoothedThing == null || thingDef2.building.unsmoothedThing == thingDef)
					{
						thingDef2.building.unsmoothedThing = thingDef;
					}
					else
					{
						Log.Error(string.Format("{0} and {1} both smooth to {2}", thingDef, thingDef2.building.unsmoothedThing, thingDef2), false);
					}
				}
			}
		}

		
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			if (this.joyKind != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Stat_RecreationType_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Stat_JoyKind_AllTypes".Translate() + ":");
				foreach (JoyKindDef joyKindDef in DefDatabase<JoyKindDef>.AllDefs)
				{
					stringBuilder.AppendLine("  - " + joyKindDef.LabelCap);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_JoyKind".Translate(), this.joyKind.LabelCap, stringBuilder.ToString(), 4750, this.joyKind.LabelCap, null, false);
			}
			if (parentDef.Minifiable)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_WorkToUninstall".Translate(), this.uninstallWork.ToStringWorkAmount(), "Stat_Thing_WorkToUninstall_Desc".Translate(), 1102, null, null, false);
			}
			if (typeof(Building_TrapDamager).IsAssignableFrom(parentDef.thingClass))
			{
				float f = StatDefOf.TrapMeleeDamage.Worker.GetValue(req, true) * 0.015f;
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "TrapArmorPenetration".Translate(), f.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 3000, null, null, false);
			}
			yield break;
		}

		
		public bool isEdifice = true;

		
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		
		public bool isInert;

		
		private bool deconstructible = true;

		
		public bool alwaysDeconstructible;

		
		public bool claimable = true;

		
		public bool isSittable;

		
		public SoundDef soundAmbient;

		
		public ConceptDef spawnedConceptLearnOpportunity;

		
		public ConceptDef boughtConceptLearnOpportunity;

		
		public bool expandHomeArea = true;

		
		public Type blueprintClass = typeof(Blueprint_Build);

		
		public GraphicData blueprintGraphicData;

		
		public float uninstallWork = 200f;

		
		public bool forceShowRoomStats;

		
		public bool wantsHopperAdjacent;

		
		public bool allowWireConnection = true;

		
		public bool shipPart;

		
		public bool canPlaceOverImpassablePlant = true;

		
		public float heatPerTickWhileWorking;

		
		public bool canBuildNonEdificesUnder = true;

		
		public bool canPlaceOverWall;

		
		public bool allowAutoroof = true;

		
		public bool preventDeteriorationOnTop;

		
		public bool preventDeteriorationInside;

		
		public bool isMealSource;

		
		public bool isNaturalRock;

		
		public bool isResourceRock;

		
		public bool repairable = true;

		
		public float roofCollapseDamageMultiplier = 1f;

		
		public bool hasFuelingPort;

		
		public ThingDef smoothedThing;

		
		[Unsaved(false)]
		public ThingDef unsmoothedThing;

		
		public TerrainDef naturalTerrain;

		
		public TerrainDef leaveTerrain;

		
		public float combatPower;

		
		public int minMechClusterPoints;

		
		public bool isPlayerEjectable;

		
		public GraphicData fullGraveGraphicData;

		
		public float bed_healPerDay;

		
		public bool bed_defaultMedical;

		
		public bool bed_showSleeperBody;

		
		public bool bed_humanlike = true;

		
		public float bed_maxBodySize = 9999f;

		
		public bool bed_caravansCanUse;

		
		public float nutritionCostPerDispense;

		
		public SoundDef soundDispense;

		
		public ThingDef turretGunDef;

		
		public float turretBurstWarmupTime;

		
		public float turretBurstCooldownTime = -1f;

		
		public float turretInitialCooldownTime;

		
		[Unsaved(false)]
		public Material turretTopMat;

		
		public float turretTopDrawSize = 2f;

		
		public Vector2 turretTopOffset;

		
		public bool ai_combatDangerous;

		
		public bool ai_chillDestination = true;

		
		public bool ai_neverTrashThis;

		
		public SoundDef soundDoorOpenPowered;

		
		public SoundDef soundDoorClosePowered;

		
		public SoundDef soundDoorOpenManual;

		
		public SoundDef soundDoorCloseManual;

		
		[NoTranslate]
		public string sowTag;

		
		public ThingDef defaultPlantToGrow;

		
		public ThingDef mineableThing;

		
		public int mineableYield = 1;

		
		public float mineableNonMinedEfficiency = 0.7f;

		
		public float mineableDropChance = 1f;

		
		public bool mineableYieldWasteable = true;

		
		public float mineableScatterCommonality;

		
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		
		public StorageSettings fixedStorageSettings;

		
		public StorageSettings defaultStorageSettings;

		
		public bool ignoreStoredThingsBeauty;

		
		public bool isTrap;

		
		public bool trapDestroyOnSpring;

		
		public float trapPeacefulWildAnimalsSpringChanceFactor = 1f;

		
		public DamageArmorCategoryDef trapDamageCategory;

		
		public GraphicData trapUnarmedGraphicData;

		
		[Unsaved(false)]
		public Graphic trapUnarmedGraphic;

		
		public float unpoweredWorkTableWorkSpeedFactor;

		
		public IntRange watchBuildingStandDistanceRange = IntRange.one;

		
		public int watchBuildingStandRectWidth = 3;

		
		public bool watchBuildingInSameRoom;

		
		public JoyKindDef joyKind;

		
		public int haulToContainerDuration;

		
		public float instrumentRange;

		
		public int minDistanceToSameTypeOfBuilding;

		
		public bool artificialForMeditationPurposes = true;
	}
}
