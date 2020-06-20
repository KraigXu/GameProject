using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000860 RID: 2144
	public class BuildingProperties
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060034F7 RID: 13559 RVA: 0x0012228A File Offset: 0x0012048A
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x00122295 File Offset: 0x00120495
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060034F9 RID: 13561 RVA: 0x001222A0 File Offset: 0x001204A0
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		// Token: 0x17000973 RID: 2419
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

		// Token: 0x060034FB RID: 13563 RVA: 0x001223D0 File Offset: 0x001205D0
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

		// Token: 0x060034FC RID: 13564 RVA: 0x00002681 File Offset: 0x00000881
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x001223E8 File Offset: 0x001205E8
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

		// Token: 0x060034FE RID: 13566 RVA: 0x001224B8 File Offset: 0x001206B8
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

		// Token: 0x060034FF RID: 13567 RVA: 0x0012256D File Offset: 0x0012076D
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

		// Token: 0x04001BD8 RID: 7128
		public bool isEdifice = true;

		// Token: 0x04001BD9 RID: 7129
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		// Token: 0x04001BDA RID: 7130
		public bool isInert;

		// Token: 0x04001BDB RID: 7131
		private bool deconstructible = true;

		// Token: 0x04001BDC RID: 7132
		public bool alwaysDeconstructible;

		// Token: 0x04001BDD RID: 7133
		public bool claimable = true;

		// Token: 0x04001BDE RID: 7134
		public bool isSittable;

		// Token: 0x04001BDF RID: 7135
		public SoundDef soundAmbient;

		// Token: 0x04001BE0 RID: 7136
		public ConceptDef spawnedConceptLearnOpportunity;

		// Token: 0x04001BE1 RID: 7137
		public ConceptDef boughtConceptLearnOpportunity;

		// Token: 0x04001BE2 RID: 7138
		public bool expandHomeArea = true;

		// Token: 0x04001BE3 RID: 7139
		public Type blueprintClass = typeof(Blueprint_Build);

		// Token: 0x04001BE4 RID: 7140
		public GraphicData blueprintGraphicData;

		// Token: 0x04001BE5 RID: 7141
		public float uninstallWork = 200f;

		// Token: 0x04001BE6 RID: 7142
		public bool forceShowRoomStats;

		// Token: 0x04001BE7 RID: 7143
		public bool wantsHopperAdjacent;

		// Token: 0x04001BE8 RID: 7144
		public bool allowWireConnection = true;

		// Token: 0x04001BE9 RID: 7145
		public bool shipPart;

		// Token: 0x04001BEA RID: 7146
		public bool canPlaceOverImpassablePlant = true;

		// Token: 0x04001BEB RID: 7147
		public float heatPerTickWhileWorking;

		// Token: 0x04001BEC RID: 7148
		public bool canBuildNonEdificesUnder = true;

		// Token: 0x04001BED RID: 7149
		public bool canPlaceOverWall;

		// Token: 0x04001BEE RID: 7150
		public bool allowAutoroof = true;

		// Token: 0x04001BEF RID: 7151
		public bool preventDeteriorationOnTop;

		// Token: 0x04001BF0 RID: 7152
		public bool preventDeteriorationInside;

		// Token: 0x04001BF1 RID: 7153
		public bool isMealSource;

		// Token: 0x04001BF2 RID: 7154
		public bool isNaturalRock;

		// Token: 0x04001BF3 RID: 7155
		public bool isResourceRock;

		// Token: 0x04001BF4 RID: 7156
		public bool repairable = true;

		// Token: 0x04001BF5 RID: 7157
		public float roofCollapseDamageMultiplier = 1f;

		// Token: 0x04001BF6 RID: 7158
		public bool hasFuelingPort;

		// Token: 0x04001BF7 RID: 7159
		public ThingDef smoothedThing;

		// Token: 0x04001BF8 RID: 7160
		[Unsaved(false)]
		public ThingDef unsmoothedThing;

		// Token: 0x04001BF9 RID: 7161
		public TerrainDef naturalTerrain;

		// Token: 0x04001BFA RID: 7162
		public TerrainDef leaveTerrain;

		// Token: 0x04001BFB RID: 7163
		public float combatPower;

		// Token: 0x04001BFC RID: 7164
		public int minMechClusterPoints;

		// Token: 0x04001BFD RID: 7165
		public bool isPlayerEjectable;

		// Token: 0x04001BFE RID: 7166
		public GraphicData fullGraveGraphicData;

		// Token: 0x04001BFF RID: 7167
		public float bed_healPerDay;

		// Token: 0x04001C00 RID: 7168
		public bool bed_defaultMedical;

		// Token: 0x04001C01 RID: 7169
		public bool bed_showSleeperBody;

		// Token: 0x04001C02 RID: 7170
		public bool bed_humanlike = true;

		// Token: 0x04001C03 RID: 7171
		public float bed_maxBodySize = 9999f;

		// Token: 0x04001C04 RID: 7172
		public bool bed_caravansCanUse;

		// Token: 0x04001C05 RID: 7173
		public float nutritionCostPerDispense;

		// Token: 0x04001C06 RID: 7174
		public SoundDef soundDispense;

		// Token: 0x04001C07 RID: 7175
		public ThingDef turretGunDef;

		// Token: 0x04001C08 RID: 7176
		public float turretBurstWarmupTime;

		// Token: 0x04001C09 RID: 7177
		public float turretBurstCooldownTime = -1f;

		// Token: 0x04001C0A RID: 7178
		public float turretInitialCooldownTime;

		// Token: 0x04001C0B RID: 7179
		[Unsaved(false)]
		public Material turretTopMat;

		// Token: 0x04001C0C RID: 7180
		public float turretTopDrawSize = 2f;

		// Token: 0x04001C0D RID: 7181
		public Vector2 turretTopOffset;

		// Token: 0x04001C0E RID: 7182
		public bool ai_combatDangerous;

		// Token: 0x04001C0F RID: 7183
		public bool ai_chillDestination = true;

		// Token: 0x04001C10 RID: 7184
		public bool ai_neverTrashThis;

		// Token: 0x04001C11 RID: 7185
		public SoundDef soundDoorOpenPowered;

		// Token: 0x04001C12 RID: 7186
		public SoundDef soundDoorClosePowered;

		// Token: 0x04001C13 RID: 7187
		public SoundDef soundDoorOpenManual;

		// Token: 0x04001C14 RID: 7188
		public SoundDef soundDoorCloseManual;

		// Token: 0x04001C15 RID: 7189
		[NoTranslate]
		public string sowTag;

		// Token: 0x04001C16 RID: 7190
		public ThingDef defaultPlantToGrow;

		// Token: 0x04001C17 RID: 7191
		public ThingDef mineableThing;

		// Token: 0x04001C18 RID: 7192
		public int mineableYield = 1;

		// Token: 0x04001C19 RID: 7193
		public float mineableNonMinedEfficiency = 0.7f;

		// Token: 0x04001C1A RID: 7194
		public float mineableDropChance = 1f;

		// Token: 0x04001C1B RID: 7195
		public bool mineableYieldWasteable = true;

		// Token: 0x04001C1C RID: 7196
		public float mineableScatterCommonality;

		// Token: 0x04001C1D RID: 7197
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		// Token: 0x04001C1E RID: 7198
		public StorageSettings fixedStorageSettings;

		// Token: 0x04001C1F RID: 7199
		public StorageSettings defaultStorageSettings;

		// Token: 0x04001C20 RID: 7200
		public bool ignoreStoredThingsBeauty;

		// Token: 0x04001C21 RID: 7201
		public bool isTrap;

		// Token: 0x04001C22 RID: 7202
		public bool trapDestroyOnSpring;

		// Token: 0x04001C23 RID: 7203
		public float trapPeacefulWildAnimalsSpringChanceFactor = 1f;

		// Token: 0x04001C24 RID: 7204
		public DamageArmorCategoryDef trapDamageCategory;

		// Token: 0x04001C25 RID: 7205
		public GraphicData trapUnarmedGraphicData;

		// Token: 0x04001C26 RID: 7206
		[Unsaved(false)]
		public Graphic trapUnarmedGraphic;

		// Token: 0x04001C27 RID: 7207
		public float unpoweredWorkTableWorkSpeedFactor;

		// Token: 0x04001C28 RID: 7208
		public IntRange watchBuildingStandDistanceRange = IntRange.one;

		// Token: 0x04001C29 RID: 7209
		public int watchBuildingStandRectWidth = 3;

		// Token: 0x04001C2A RID: 7210
		public bool watchBuildingInSameRoom;

		// Token: 0x04001C2B RID: 7211
		public JoyKindDef joyKind;

		// Token: 0x04001C2C RID: 7212
		public int haulToContainerDuration;

		// Token: 0x04001C2D RID: 7213
		public float instrumentRange;

		// Token: 0x04001C2E RID: 7214
		public int minDistanceToSameTypeOfBuilding;

		// Token: 0x04001C2F RID: 7215
		public bool artificialForMeditationPurposes = true;
	}
}
