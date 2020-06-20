using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001144 RID: 4420
	public class QuestNode_GetSitePartDefsByTagsAndFaction : QuestNode
	{
		// Token: 0x06006731 RID: 26417 RVA: 0x00241D64 File Offset: 0x0023FF64
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate);
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x00241D6D File Offset: 0x0023FF6D
		protected override void RunInt()
		{
			if (!this.TrySetVars(QuestGen.slate))
			{
				Log.Error("Could not resolve site parts.", false);
			}
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x00241D88 File Offset: 0x0023FF88
		private bool TrySetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			Faction faction = slate.Get<Faction>("enemyFaction", null, false);
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Func<SitePartDef, bool> <>9__3;
			Func<string, IEnumerable<SitePartDef>> <>9__1;
			Predicate<Faction> <>9__2;
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
				Func<string, IEnumerable<SitePartDef>> selector;
				if ((selector = <>9__1) == null)
				{
					selector = (<>9__1 = delegate(string x)
					{
						IEnumerable<SitePartDef> enumerable = SiteMakerHelper.SitePartDefsWithTag(x);
						IEnumerable<SitePartDef> source2 = enumerable;
						Func<SitePartDef, bool> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = ((SitePartDef y) => points >= y.minThreatPoints));
						}
						IEnumerable<SitePartDef> enumerable2 = source2.Where(predicate);
						if (!enumerable2.Any<SitePartDef>())
						{
							return enumerable;
						}
						return enumerable2;
					});
				}
				IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates = source.Select(selector);
				Faction factionToUse = faction;
				bool disallowNonHostileFactions = true;
				Predicate<Faction> extraFactionValidator;
				if ((extraFactionValidator = <>9__2) == null)
				{
					extraFactionValidator = (<>9__2 = ((Faction x) => (asker == null || asker.Faction == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))));
				}
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

		// Token: 0x04003F4F RID: 16207
		public SlateRef<IEnumerable<QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption>> sitePartsTags;

		// Token: 0x04003F50 RID: 16208
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F51 RID: 16209
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x04003F52 RID: 16210
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x04003F53 RID: 16211
		private static List<string> tmpTags = new List<string>();

		// Token: 0x02001F3D RID: 7997
		public class SitePartOption
		{
			// Token: 0x0400752C RID: 29996
			[NoTranslate]
			public string tag;

			// Token: 0x0400752D RID: 29997
			public float chance = 1f;
		}
	}
}
