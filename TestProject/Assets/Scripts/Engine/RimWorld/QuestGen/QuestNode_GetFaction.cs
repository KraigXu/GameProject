using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetFaction : QuestNode
	{
		
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

		
		private bool TryFindFaction(out Faction faction, Slate slate)
		{
			return (from x in Find.FactionManager.GetFactions(true, false, true, TechLevel.Undefined)
			where this.IsGoodFaction(x, slate)
			select x).TryRandomElement(out faction);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<bool> allowEnemy;

		
		public SlateRef<bool> allowNeutral;

		
		public SlateRef<bool> allowAlly;

		
		public SlateRef<bool> allowAskerFaction;

		
		public SlateRef<bool?> allowPermanentEnemy;

		
		public SlateRef<bool> mustBePermanentEnemy;

		
		public SlateRef<bool> playerCantBeAttackingCurrently;

		
		public SlateRef<bool> peaceTalksCantExist;

		
		public SlateRef<bool> leaderMustBeSafe;

		
		public SlateRef<Pawn> ofPawn;

		
		public SlateRef<Thing> mustBeHostileToFactionOf;

		
		public SlateRef<IEnumerable<Faction>> exclude;

		
		public SlateRef<IEnumerable<Faction>> allowedHiddenFactions;
	}
}
