using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001275 RID: 4725
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06006EC3 RID: 28355 RVA: 0x0026A330 File Offset: 0x00268530
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06006EC4 RID: 28356 RVA: 0x0026A338 File Offset: 0x00268538
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x0026A34C File Offset: 0x0026854C
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x0026A383 File Offset: 0x00268583
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x0026A3A0 File Offset: 0x002685A0
		public override void CompTick()
		{
			base.CompTick();
			if (this.active)
			{
				MapParent mapParent = this.parent as MapParent;
				if (mapParent != null)
				{
					this.CheckAllEnemiesDefeated(mapParent);
				}
			}
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x0026A3D1 File Offset: 0x002685D1
		private void CheckAllEnemiesDefeated(MapParent mapParent)
		{
			if (!mapParent.HasMap)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true))
			{
				return;
			}
			this.GiveRewardsAndSendLetter();
			this.StopQuest();
		}

		// Token: 0x06006EC9 RID: 28361 RVA: 0x0026A3F8 File Offset: 0x002685F8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.relationsImprovement, "relationsImprovement", 0, false);
			Scribe_References.Look<Faction>(ref this.requestingFaction, "requestingFaction", false);
			Scribe_Deep.Look<ThingOwner>(ref this.rewards, "rewards", new object[]
			{
				this
			});
		}

		// Token: 0x06006ECA RID: 28362 RVA: 0x0026A45C File Offset: 0x0026865C
		private void GiveRewardsAndSendLetter()
		{
			Map map = Find.AnyPlayerHomeMap ?? ((MapParent)this.parent).Map;
			DefeatAllEnemiesQuestComp.tmpRewards.AddRange(this.rewards);
			this.rewards.Clear();
			IntVec3 intVec = DropCellFinder.TradeDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, DefeatAllEnemiesQuestComp.tmpRewards, 110, false, false, false, true);
			DefeatAllEnemiesQuestComp.tmpRewards.Clear();
			FactionRelationKind playerRelationKind = this.requestingFaction.PlayerRelationKind;
			TaggedString text = "LetterDefeatAllEnemiesQuestCompleted".Translate(this.requestingFaction.Name, this.relationsImprovement.ToString());
			this.requestingFaction.TryAffectGoodwillWith(Faction.OfPlayer, this.relationsImprovement, false, false, null, null);
			this.requestingFaction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, this.requestingFaction.PlayerRelationKind, null);
			Find.LetterStack.ReceiveLetter("LetterLabelDefeatAllEnemiesQuestCompleted".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(intVec, map, false), this.requestingFaction, null, null, null);
		}

		// Token: 0x06006ECB RID: 28363 RVA: 0x0026A566 File Offset: 0x00268766
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006ECC RID: 28364 RVA: 0x0026A574 File Offset: 0x00268774
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x06006ECD RID: 28365 RVA: 0x0026A57C File Offset: 0x0026877C
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06006ECE RID: 28366 RVA: 0x0026A590 File Offset: 0x00268790
		public override string CompInspectStringExtra()
		{
			if (this.active)
			{
				string value = GenThing.ThingsToCommaList(this.rewards, true, true, 5).CapitalizeFirst();
				return "QuestTargetDestroyInspectString".Translate(this.requestingFaction.Name, value, GenThing.GetMarketValue(this.rewards).ToStringMoney(null)).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x04004442 RID: 17474
		private bool active;

		// Token: 0x04004443 RID: 17475
		public Faction requestingFaction;

		// Token: 0x04004444 RID: 17476
		public int relationsImprovement;

		// Token: 0x04004445 RID: 17477
		public ThingOwner rewards;

		// Token: 0x04004446 RID: 17478
		private static List<Thing> tmpRewards = new List<Thing>();
	}
}
