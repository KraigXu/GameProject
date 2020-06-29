﻿using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class QuestScriptDef : Def
	{
		
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x00128034 File Offset: 0x00126234
		public bool IsRootRandomSelected
		{
			get
			{
				return this.rootSelectionWeight != 0f;
			}
		}

		
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x00128046 File Offset: 0x00126246
		public bool IsRootDecree
		{
			get
			{
				return this.decreeSelectionWeight != 0f;
			}
		}

		
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x00128058 File Offset: 0x00126258
		public bool IsRootAny
		{
			get
			{
				return this.IsRootRandomSelected || this.IsRootDecree || this.isRootSpecial;
			}
		}

		
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

		
		public bool CanRun(Slate slate)
		{
			return this.root.TestRun(slate.DeepCopy());
		}

		
		public bool CanRun(float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return this.CanRun(slate);
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public QuestNode root;

		
		public float rootSelectionWeight;

		
		public float rootMinPoints;

		
		public float rootMinProgressScore;

		
		public bool rootIncreasesPopulation;

		
		public float decreeSelectionWeight;

		
		public List<string> decreeTags;

		
		public RulePack questDescriptionRules;

		
		public RulePack questNameRules;

		
		public RulePack questDescriptionAndNameRules;

		
		public bool autoAccept;

		
		public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

		
		public bool nameMustBeUnique;

		
		public int defaultChallengeRating = -1;

		
		public bool isRootSpecial;

		
		public bool canGiveRoyalFavor;
	}
}
