using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetRandomFactionForSite : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate, true);
		}

		
		protected override void RunInt()
		{
			this.TrySetVars(QuestGen.slate, false);
		}

		
		private bool TrySetVars(Slate slate, bool test)
		{
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(this.sitePartDefs.GetValue(slate), out faction, true, (Faction x) => (asker == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))))
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.allowViolentQuests && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
			if (!test && faction != null && !faction.def.hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			return true;
		}

		
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Thing> mustBeHostileToFactionOf;
	}
}
