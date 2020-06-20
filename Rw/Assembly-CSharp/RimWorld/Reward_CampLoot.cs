using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD4 RID: 4052
	[StaticConstructorOnStartup]
	public class Reward_CampLoot : Reward
	{
		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06006149 RID: 24905 RVA: 0x0021C9BB File Offset: 0x0021ABBB
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_CampLoot_Label".Translate(), Reward_CampLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x000255BF File Offset: 0x000237BF
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x000255BF File Offset: 0x000237BF
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x0021C9CC File Offset: 0x0021ABCC
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_CampLoot".Translate().Resolve();
		}

		// Token: 0x04003B3B RID: 15163
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
