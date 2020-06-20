using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000857 RID: 2135
	public class NeurotrainerDefGenerator
	{
		// Token: 0x060034D2 RID: 13522 RVA: 0x001211CC File Offset: 0x0011F3CC
		public static IEnumerable<ThingDef> ImpliedThingDefs()
		{
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				if (typeof(Psycast).IsAssignableFrom(abilityDef.abilityClass))
				{
					ThingDef thingDef = NeurotrainerDefGenerator.BaseNeurotrainer();
					thingDef.defName = NeurotrainerDefGenerator.PsytrainerDefPrefix + "_" + abilityDef.defName;
					thingDef.label = "PsycastNeurotrainerLabel".Translate(abilityDef.label);
					thingDef.description = "PsycastNeurotrainerDescription".Translate();
					thingDef.comps.Add(new CompProperties_Neurotrainer
					{
						compClass = typeof(CompNeurotrainer),
						useJob = JobDefOf.UseNeurotrainer,
						useLabel = "PsycastNeurotrainerUseLabel".Translate(abilityDef.label),
						ability = abilityDef
					});
					thingDef.comps.Add(new CompProperties_UseEffect
					{
						compClass = typeof(CompUseEffect_GainAbility)
					});
					thingDef.statBases.Add(new StatModifier
					{
						stat = StatDefOf.MarketValue,
						value = Mathf.Round(Mathf.Lerp(100f, 1000f, (float)abilityDef.level / 6f))
					});
					thingDef.thingCategories = new List<ThingCategoryDef>
					{
						ThingCategoryDefOf.NeurotrainersPsycast
					};
					thingDef.thingSetMakerTags = new List<string>
					{
						"RewardStandardLowFreq"
					};
					thingDef.modContentPack = abilityDef.modContentPack;
					thingDef.descriptionHyperlinks = new List<DefHyperlink>
					{
						new DefHyperlink(abilityDef)
					};
					yield return thingDef;
				}
			}
			IEnumerator<AbilityDef> enumerator = null;
			foreach (SkillDef skillDef in DefDatabase<SkillDef>.AllDefs)
			{
				ThingDef thingDef2 = NeurotrainerDefGenerator.BaseNeurotrainer();
				thingDef2.defName = NeurotrainerDefGenerator.NeurotrainerDefPrefix + "_" + skillDef.defName;
				thingDef2.label = "SkillNeurotrainerLabel".Translate(skillDef.label);
				thingDef2.description = "SkillNeurotrainerDescription".Translate();
				thingDef2.comps.Add(new CompProperties_Neurotrainer
				{
					compClass = typeof(CompNeurotrainer),
					useJob = JobDefOf.UseNeurotrainer,
					useLabel = "SkillNeurotrainerUseLabel".Translate(skillDef.label),
					skill = skillDef
				});
				thingDef2.comps.Add(new CompProperties_UseEffect
				{
					compClass = typeof(CompUseEffect_LearnSkill)
				});
				thingDef2.statBases.Add(new StatModifier
				{
					stat = StatDefOf.MarketValue,
					value = 750f
				});
				thingDef2.thingCategories = new List<ThingCategoryDef>
				{
					ThingCategoryDefOf.Neurotrainers
				};
				thingDef2.thingSetMakerTags = new List<string>
				{
					"RewardStandardHighFreq",
					"SkillNeurotrainer"
				};
				thingDef2.modContentPack = skillDef.modContentPack;
				yield return thingDef2;
			}
			IEnumerator<SkillDef> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x001211D8 File Offset: 0x0011F3D8
		private static ThingDef BaseNeurotrainer()
		{
			return new ThingDef
			{
				category = ThingCategory.Item,
				selectable = true,
				thingClass = typeof(ThingWithComps),
				comps = new List<CompProperties>
				{
					new CompProperties_UseEffectPlaySound
					{
						soundOnUsed = SoundDefOf.MechSerumUsed
					},
					new CompProperties_UseEffect
					{
						compClass = typeof(CompUseEffect_DestroySelf)
					},
					new CompProperties_Forbiddable()
				},
				graphicData = new GraphicData
				{
					texPath = "Things/Item/Special/MechSerumNeurotrainer",
					graphicClass = typeof(Graphic_Single)
				},
				drawGUIOverlay = false,
				statBases = new List<StatModifier>
				{
					new StatModifier
					{
						stat = StatDefOf.MaxHitPoints,
						value = 80f
					},
					new StatModifier
					{
						stat = StatDefOf.Mass,
						value = 0.2f
					},
					new StatModifier
					{
						stat = StatDefOf.DeteriorationRate,
						value = 2f
					},
					new StatModifier
					{
						stat = StatDefOf.Flammability,
						value = 0.2f
					}
				},
				techLevel = TechLevel.Ultra,
				altitudeLayer = AltitudeLayer.Item,
				alwaysHaulable = true,
				rotatable = false,
				pathCost = 15,
				tradeTags = new List<string>
				{
					"ExoticMisc"
				},
				stackLimit = 1,
				tradeNeverStack = true,
				forceDebugSpawnable = true
			};
		}

		// Token: 0x04001BBB RID: 7099
		public static string NeurotrainerDefPrefix = "Neurotrainer";

		// Token: 0x04001BBC RID: 7100
		public static string PsytrainerDefPrefix = "Psytrainer";

		// Token: 0x04001BBD RID: 7101
		private const int MaxAbilityLevel = 6;
	}
}
