using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetSitePartDefsByTagsAndFaction : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate);
		}

		
		protected override void RunInt()
		{
			if (!this.TrySetVars(QuestGen.slate))
			{
				Log.Error("Could not resolve site parts.", false);
			}
		}


		private bool TrySetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			Faction faction = slate.Get<Faction>("enemyFaction", null, false);
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);

			for (int i = 0; i < 2; i++)
			{
				QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags.Clear();
				foreach (QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption sitePartOption in this.sitePartsTags.GetValue(slate))
				{
					if (Rand.Chance(sitePartOption.chance) && (i != 1 || sitePartOption.chance >= 1f))
					{
						QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags.Add(sitePartOption.tag);
					}
				}
				IEnumerable<string> source = from x in QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags
				where x != null
				select x;
				Func<string, IEnumerable<SitePartDef>> selector= delegate (string x)
				{
					IEnumerable<SitePartDef> enumerable = SiteMakerHelper.SitePartDefsWithTag(x);
					IEnumerable<SitePartDef> source2 = enumerable;
					Func<SitePartDef, bool> predicate=y => points >= y.minThreatPoints;
					IEnumerable<SitePartDef> enumerable2 = source2.Where(predicate);
					if (!enumerable2.Any<SitePartDef>())
					{
						return enumerable;
					}
					return enumerable2;
				};
				IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates = source.Select(selector);
				Faction factionToUse = faction;
				bool disallowNonHostileFactions = true;
				Predicate<Faction> extraFactionValidator=x=> (asker == null || asker.Faction == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)));
				List<SitePartDef> list;
				Faction var;
				if (SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out list, out var, factionToUse, disallowNonHostileFactions, extraFactionValidator))
				{
					slate.Set<List<SitePartDef>>(this.storeAs.GetValue(slate), list, false);
					slate.Set<int>("sitePartCount", list.Count, false);
					if (QuestGen.Working)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						for (int j = 0; j < list.Count; j++)
						{
							dictionary[list[j].defName + "_exists"] = "True";
						}
						QuestGen.AddQuestDescriptionConstants(dictionary);
					}
					if (!this.storeFactionAs.GetValue(slate).NullOrEmpty())
					{
						slate.Set<Faction>(this.storeFactionAs.GetValue(slate), var, false);
					}
					return true;
				}
			}
			return false;
		}

		
		public SlateRef<IEnumerable<QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption>> sitePartsTags;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		
		public SlateRef<Thing> mustBeHostileToFactionOf;

		
		private static List<string> tmpTags = new List<string>();

		
		public class SitePartOption
		{
			
			[NoTranslate]
			public string tag;

			
			public float chance = 1f;
		}
	}
}
