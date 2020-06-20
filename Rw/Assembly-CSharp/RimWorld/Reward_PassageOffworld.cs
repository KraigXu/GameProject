using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD7 RID: 4055
	[StaticConstructorOnStartup]
	public class Reward_PassageOffworld : Reward
	{
		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06006166 RID: 24934 RVA: 0x0021D182 File Offset: 0x0021B382
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PassageOffworld_Label".Translate(), Reward_PassageOffworld.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06006167 RID: 24935 RVA: 0x000255BF File Offset: 0x000237BF
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x000255BF File Offset: 0x000237BF
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006169 RID: 24937 RVA: 0x0021D194 File Offset: 0x0021B394
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PassageOffworld".Translate().Resolve();
		}

		// Token: 0x04003B42 RID: 15170
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Stars", true);
	}
}
