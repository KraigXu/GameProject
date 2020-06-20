using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020008F0 RID: 2288
	public class QuestScriptDef : Def
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x00128034 File Offset: 0x00126234
		public bool IsRootRandomSelected
		{
			get
			{
				return this.rootSelectionWeight != 0f;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x00128046 File Offset: 0x00126246
		public bool IsRootDecree
		{
			get
			{
				return this.decreeSelectionWeight != 0f;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x00128058 File Offset: 0x00126258
		public bool IsRootAny
		{
			get
			{
				return this.IsRootRandomSelected || this.IsRootDecree || this.isRootSpecial;
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x00128074 File Offset: 0x00126274
		public void Run()
		{
			if (this.questDescriptionRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionRules);
			}
			if (this.questNameRules != null)
			{
				QuestGen.AddQuestNameRules(this.questNameRules);
			}
			if (this.questDescriptionAndNameRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionAndNameRules);
				QuestGen.AddQuestNameRules(this.questDescriptionAndNameRules);
			}
			this.root.Run();
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x001280D0 File Offset: 0x001262D0
		public bool CanRun(Slate slate)
		{
			return this.root.TestRun(slate.DeepCopy());
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x001280E4 File Offset: 0x001262E4
		public bool CanRun(float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return this.CanRun(slate);
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x0012810B File Offset: 0x0012630B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.rootSelectionWeight > 0f && !this.autoAccept && this.expireDaysRange.TrueMax <= 0f)
			{
				yield return "rootSelectionWeight > 0 but expireDaysRange not set";
			}
			if (this.autoAccept && this.expireDaysRange.TrueMax > 0f)
			{
				yield return "autoAccept but there is an expireDaysRange set";
			}
			if (this.defaultChallengeRating > 0 && !this.IsRootAny)
			{
				yield return "non-root quest has defaultChallengeRating";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001F4B RID: 8011
		public QuestNode root;

		// Token: 0x04001F4C RID: 8012
		public float rootSelectionWeight;

		// Token: 0x04001F4D RID: 8013
		public float rootMinPoints;

		// Token: 0x04001F4E RID: 8014
		public float rootMinProgressScore;

		// Token: 0x04001F4F RID: 8015
		public bool rootIncreasesPopulation;

		// Token: 0x04001F50 RID: 8016
		public float decreeSelectionWeight;

		// Token: 0x04001F51 RID: 8017
		public List<string> decreeTags;

		// Token: 0x04001F52 RID: 8018
		public RulePack questDescriptionRules;

		// Token: 0x04001F53 RID: 8019
		public RulePack questNameRules;

		// Token: 0x04001F54 RID: 8020
		public RulePack questDescriptionAndNameRules;

		// Token: 0x04001F55 RID: 8021
		public bool autoAccept;

		// Token: 0x04001F56 RID: 8022
		public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

		// Token: 0x04001F57 RID: 8023
		public bool nameMustBeUnique;

		// Token: 0x04001F58 RID: 8024
		public int defaultChallengeRating = -1;

		// Token: 0x04001F59 RID: 8025
		public bool isRootSpecial;

		// Token: 0x04001F5A RID: 8026
		public bool canGiveRoyalFavor;
	}
}
