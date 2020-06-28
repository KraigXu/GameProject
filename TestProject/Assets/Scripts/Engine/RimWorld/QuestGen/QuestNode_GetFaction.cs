using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001124 RID: 4388
	public class QuestNode_GetFaction : QuestNode
	{
		// Token: 0x060066A5 RID: 26277 RVA: 0x0023EE4C File Offset: 0x0023D04C
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			if (slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, slate))
			{
				return true;
			}
			if (this.TryFindFaction(out faction, slate))
			{
				slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
				return true;
			}
			return false;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x0023EEA0 File Offset: 0x0023D0A0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Faction faction;
			if (QuestGen.slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, QuestGen.slate))
			{
				return;
			}
			if (this.TryFindFaction(out faction, QuestGen.slate))
			{
				QuestGen.slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
				if (!faction.def.hidden)
				{
					QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
					questPart_InvolvedFactions.factions.Add(faction);
					QuestGen.quest.AddPart(questPart_InvolvedFactions);
				}
			}
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x0023EF30 File Offset: 0x0023D130
		private bool TryFindFaction(out Faction faction, Slate slate)
		{
			return (from x in Find.FactionManager.GetFactions(true, false, true, TechLevel.Undefined)
			where this.IsGoodFaction(x, slate)
			select x).TryRandomElement(out faction);
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x0023EF78 File Offset: 0x0023D178
		private bool IsGoodFaction(Faction faction, Slate slate)
		{
			if (faction.def.hidden && (this.allowedHiddenFactions.GetValue(slate) == null || !this.allowedHiddenFactions.GetValue(slate).Contains(faction)))
			{
				return false;
			}
			if (this.ofPawn.GetValue(slate) != null && faction != this.ofPawn.GetValue(slate).Faction)
			{
				return false;
			}
			if (this.exclude.GetValue(slate) != null && this.exclude.GetValue(slate).Contains(faction))
			{
				return false;
			}
			if (this.mustBePermanentEnemy.GetValue(slate) && !faction.def.permanentEnemy)
			{
				return false;
			}
			if (!this.allowEnemy.GetValue(slate) && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (!this.allowNeutral.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				return false;
			}
			if (!this.allowAlly.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return false;
			}
			if (!(this.allowPermanentEnemy.GetValue(slate) ?? true) && faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.playerCantBeAttackingCurrently.GetValue(slate) && SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))
			{
				return false;
			}
			if (this.peaceTalksCantExist.GetValue(slate))
			{
				if (this.PeaceTalksExist(faction))
				{
					return false;
				}
				string tag = QuestNode_QuestUnique.GetProcessedTag("PeaceTalks", faction);
				if (Find.QuestManager.questsInDisplayOrder.Any((Quest q) => q.tags.Contains(tag)))
				{
					return false;
				}
			}
			if (this.leaderMustBeSafe.GetValue(slate) && (faction.leader == null || faction.leader.Spawned || faction.leader.IsPrisoner))
			{
				return false;
			}
			Thing value = this.mustBeHostileToFactionOf.GetValue(slate);
			return value == null || value.Faction == null || (value.Faction != faction && faction.HostileTo(value.Faction));
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x0023F168 File Offset: 0x0023D368
		private bool PeaceTalksExist(Faction faction)
		{
			List<PeaceTalks> peaceTalks = Find.WorldObjects.PeaceTalks;
			for (int i = 0; i < peaceTalks.Count; i++)
			{
				if (peaceTalks[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003EC8 RID: 16072
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EC9 RID: 16073
		public SlateRef<bool> allowEnemy;

		// Token: 0x04003ECA RID: 16074
		public SlateRef<bool> allowNeutral;

		// Token: 0x04003ECB RID: 16075
		public SlateRef<bool> allowAlly;

		// Token: 0x04003ECC RID: 16076
		public SlateRef<bool> allowAskerFaction;

		// Token: 0x04003ECD RID: 16077
		public SlateRef<bool?> allowPermanentEnemy;

		// Token: 0x04003ECE RID: 16078
		public SlateRef<bool> mustBePermanentEnemy;

		// Token: 0x04003ECF RID: 16079
		public SlateRef<bool> playerCantBeAttackingCurrently;

		// Token: 0x04003ED0 RID: 16080
		public SlateRef<bool> peaceTalksCantExist;

		// Token: 0x04003ED1 RID: 16081
		public SlateRef<bool> leaderMustBeSafe;

		// Token: 0x04003ED2 RID: 16082
		public SlateRef<Pawn> ofPawn;

		// Token: 0x04003ED3 RID: 16083
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x04003ED4 RID: 16084
		public SlateRef<IEnumerable<Faction>> exclude;

		// Token: 0x04003ED5 RID: 16085
		public SlateRef<IEnumerable<Faction>> allowedHiddenFactions;
	}
}
