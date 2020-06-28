using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CE RID: 206
	public class PawnKindDef : Def
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001C01A File Offset: 0x0001A21A
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001C028 File Offset: 0x0001A228
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001C062 File Offset: 0x0001A262
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001C08C File Offset: 0x0001A28C
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.RaceProps.wildness) + this.race.BaseMarketValue;
		}

		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.backstoryFilters != null && this.backstoryFiltersOverride != null)
			{
				yield return "both backstoryCategories and backstoryCategoriesOverride are defined";
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			if (this.baseRecruitDifficulty > 1.0001f)
			{
				yield return this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float num = 999999f;
				int num2;
				int i;
				for (i = 0; i < this.weaponTags.Count; i = num2 + 1)
				{
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						num = Mathf.Min(num, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
					}
					num2 = i;
				}
				if (num < 999999f && num > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						num,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				int num2;
				for (int k = 0; k < this.apparelRequired.Count; k = num2 + 1)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j = num2 + 1)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
						num2 = j;
					}
					num2 = k;
				}
			}
			if (this.alternateGraphics != null)
			{
				using (List<AlternateGraphic>.Enumerator enumerator2 = this.alternateGraphics.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Weight < 0f)
						{
							yield return "alternate graphic has negative weight.";
						}
					}
				}

			}
			yield break;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001C157 File Offset: 0x0001A357
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x04000484 RID: 1156
		public ThingDef race;

		// Token: 0x04000485 RID: 1157
		public FactionDef defaultFactionType;

		// Token: 0x04000486 RID: 1158
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x04000487 RID: 1159
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		// Token: 0x04000488 RID: 1160
		[NoTranslate]
		public List<string> backstoryCategories;

		// Token: 0x04000489 RID: 1161
		[MustTranslate]
		public string labelPlural;

		// Token: 0x0400048A RID: 1162
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x0400048B RID: 1163
		public List<AlternateGraphic> alternateGraphics;

		// Token: 0x0400048C RID: 1164
		public List<TraitDef> disallowedTraits;

		// Token: 0x0400048D RID: 1165
		public float alternateGraphicChance;

		// Token: 0x0400048E RID: 1166
		public float backstoryCryptosleepCommonality;

		// Token: 0x0400048F RID: 1167
		public int minGenerationAge;

		// Token: 0x04000490 RID: 1168
		public int maxGenerationAge = 999999;

		// Token: 0x04000491 RID: 1169
		public bool factionLeader;

		// Token: 0x04000492 RID: 1170
		public bool destroyGearOnDrop;

		// Token: 0x04000493 RID: 1171
		public float defendPointRadius = -1f;

		// Token: 0x04000494 RID: 1172
		public float royalTitleChance;

		// Token: 0x04000495 RID: 1173
		public RoyalTitleDef titleRequired;

		// Token: 0x04000496 RID: 1174
		public RoyalTitleDef minTitleRequired;

		// Token: 0x04000497 RID: 1175
		public List<RoyalTitleDef> titleSelectOne;

		// Token: 0x04000498 RID: 1176
		public bool allowRoyalRoomRequirements = true;

		// Token: 0x04000499 RID: 1177
		public bool allowRoyalApparelRequirements = true;

		// Token: 0x0400049A RID: 1178
		public bool isFighter = true;

		// Token: 0x0400049B RID: 1179
		public float combatPower = -1f;

		// Token: 0x0400049C RID: 1180
		public bool canArriveManhunter = true;

		// Token: 0x0400049D RID: 1181
		public bool canBeSapper;

		// Token: 0x0400049E RID: 1182
		public float baseRecruitDifficulty = 0.5f;

		// Token: 0x0400049F RID: 1183
		public bool aiAvoidCover;

		// Token: 0x040004A0 RID: 1184
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x040004A1 RID: 1185
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x040004A2 RID: 1186
		public bool forceNormalGearQuality;

		// Token: 0x040004A3 RID: 1187
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x040004A4 RID: 1188
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x040004A5 RID: 1189
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x040004A6 RID: 1190
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x040004A7 RID: 1191
		public List<ThingDef> apparelRequired;

		// Token: 0x040004A8 RID: 1192
		[NoTranslate]
		public List<string> apparelTags;

		// Token: 0x040004A9 RID: 1193
		[NoTranslate]
		public List<string> apparelDisallowTags;

		// Token: 0x040004AA RID: 1194
		[NoTranslate]
		public List<string> hairTags;

		// Token: 0x040004AB RID: 1195
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x040004AC RID: 1196
		public bool apparelIgnoreSeasons;

		// Token: 0x040004AD RID: 1197
		public Color apparelColor = Color.white;

		// Token: 0x040004AE RID: 1198
		public List<SpecificApparelRequirement> specificApparelRequirements;

		// Token: 0x040004AF RID: 1199
		public List<ThingDef> techHediffsRequired;

		// Token: 0x040004B0 RID: 1200
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x040004B1 RID: 1201
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x040004B2 RID: 1202
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		// Token: 0x040004B3 RID: 1203
		public float techHediffsChance;

		// Token: 0x040004B4 RID: 1204
		public int techHediffsMaxAmount = 1;

		// Token: 0x040004B5 RID: 1205
		public float biocodeWeaponChance;

		// Token: 0x040004B6 RID: 1206
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x040004B7 RID: 1207
		public PawnInventoryOption inventoryOptions;

		// Token: 0x040004B8 RID: 1208
		public float invNutrition;

		// Token: 0x040004B9 RID: 1209
		public ThingDef invFoodDef;

		// Token: 0x040004BA RID: 1210
		public float chemicalAddictionChance;

		// Token: 0x040004BB RID: 1211
		public float combatEnhancingDrugsChance;

		// Token: 0x040004BC RID: 1212
		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		// Token: 0x040004BD RID: 1213
		public bool trader;

		// Token: 0x040004BE RID: 1214
		public List<SkillRange> skills;

		// Token: 0x040004BF RID: 1215
		public WorkTags requiredWorkTags;

		// Token: 0x040004C0 RID: 1216
		[MustTranslate]
		public string labelMale;

		// Token: 0x040004C1 RID: 1217
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x040004C2 RID: 1218
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040004C3 RID: 1219
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x040004C4 RID: 1220
		public IntRange wildGroupSize = IntRange.one;

		// Token: 0x040004C5 RID: 1221
		public float ecoSystemWeight = 1f;

		// Token: 0x040004C6 RID: 1222
		private const int MaxWeaponMoney = 999999;
	}
}
