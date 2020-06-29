using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Reward_PassageOffworld : Reward
	{
		
		// (get) Token: 0x06006166 RID: 24934 RVA: 0x0021D182 File Offset: 0x0021B382
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PassageOffworld_Label".Translate(), Reward_PassageOffworld.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
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
			return "Reward_PassageOffworld".Translate().Resolve();
		}

		
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Stars", true);
	}
}
