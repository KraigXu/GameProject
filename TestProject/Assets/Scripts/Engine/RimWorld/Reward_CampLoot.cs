using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Reward_CampLoot : Reward
	{
		
		// (get) Token: 0x06006149 RID: 24905 RVA: 0x0021C9BB File Offset: 0x0021ABBB
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_CampLoot_Label".Translate(), Reward_CampLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_CampLoot".Translate().Resolve();
		}

		
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
