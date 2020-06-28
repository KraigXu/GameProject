using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200113C RID: 4412
	public class QuestNode_GetRandomFactionForSite : QuestNode
	{
		// Token: 0x06006710 RID: 26384 RVA: 0x002415B9 File Offset: 0x0023F7B9
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate, true);
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x002415C3 File Offset: 0x0023F7C3
		protected override void RunInt()
		{
			this.TrySetVars(QuestGen.slate, false);
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x002415D4 File Offset: 0x0023F7D4
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

		// Token: 0x04003F36 RID: 16182
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04003F37 RID: 16183
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F38 RID: 16184
		public SlateRef<Thing> mustBeHostileToFactionOf;
	}
}
