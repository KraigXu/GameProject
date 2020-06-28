using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093F RID: 2367
	public class QuestManager : IExposable
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x0012CF4C File Offset: 0x0012B14C
		public List<Quest> QuestsListForReading
		{
			get
			{
				return this.quests;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x0012CF54 File Offset: 0x0012B154
		public List<QuestPart_SituationalThought> SituationalThoughtQuestParts
		{
			get
			{
				return this.cachedSituationalThoughtQuestParts;
			}
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x0012CF5C File Offset: 0x0012B15C
		public void Add(Quest quest)
		{
			if (quest == null)
			{
				Log.Error("Tried to add a null quest.", false);
				return;
			}
			if (this.Contains(quest))
			{
				Log.Error("Tried to add the same quest twice: " + quest.ToStringSafe<Quest>(), false);
				return;
			}
			this.quests.Add(quest);
			this.AddToCache(quest);
			Find.SignalManager.RegisterReceiver(quest);
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				partsListForReading[i].PostQuestAdded();
			}
			if (quest.initiallyAccepted)
			{
				quest.Initiate();
			}
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x0012CFE8 File Offset: 0x0012B1E8
		public void Remove(Quest quest)
		{
			if (!this.Contains(quest))
			{
				Log.Error("Tried to remove non-existent quest: " + quest.ToStringSafe<Quest>(), false);
				return;
			}
			this.quests.Remove(quest);
			this.RemoveFromCache(quest);
			Find.SignalManager.DeregisterReceiver(quest);
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x0012D034 File Offset: 0x0012B234
		public bool Contains(Quest quest)
		{
			return this.quests.Contains(quest);
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x0012D044 File Offset: 0x0012B244
		public void QuestManagerTick()
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].QuestTick();
			}
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x0012D078 File Offset: 0x0012B278
		public bool IsReservedByAnyQuest(Pawn p)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].QuestReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x0012D0B4 File Offset: 0x0012B2B4
		private void AddToCache(Quest quest)
		{
			this.questsInDisplayOrder.Add(quest);
			this.questsInDisplayOrder.SortBy((Quest x) => x.TicksSinceAppeared);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Add(questPart_SituationalThought);
				}
			}
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x0012D130 File Offset: 0x0012B330
		private void RemoveFromCache(Quest quest)
		{
			this.questsInDisplayOrder.Remove(quest);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Remove(questPart_SituationalThought);
				}
			}
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x0012D184 File Offset: 0x0012B384
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x0012D1BC File Offset: 0x0012B3BC
		public void ExposeData()
		{
			Scribe_Collections.Look<Quest>(ref this.quests, "quests", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = this.quests.RemoveAll((Quest x) => x == null);
				if (num != 0)
				{
					Log.Error(num + " quest(s) were null after loading.", false);
				}
				this.cachedSituationalThoughtQuestParts.Clear();
				this.questsInDisplayOrder.Clear();
				for (int i = 0; i < this.quests.Count; i++)
				{
					this.AddToCache(this.quests[i]);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = 0; j < this.quests.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.quests[j]);
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x0012D2A8 File Offset: 0x0012B4A8
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].State == QuestState.Ongoing)
				{
					this.quests[i].Notify_ThingsProduced(worker, things);
				}
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x0012D2F4 File Offset: 0x0012B4F4
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].State == QuestState.Ongoing)
				{
					this.quests[i].Notify_PlantHarvested(worker, harvested);
				}
			}
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x0012D340 File Offset: 0x0012B540
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				if (this.quests[i].State == QuestState.Ongoing)
				{
					this.quests[i].Notify_PawnKilled(pawn, dinfo);
				}
			}
		}

		// Token: 0x04002138 RID: 8504
		private List<Quest> quests = new List<Quest>();

		// Token: 0x04002139 RID: 8505
		public List<Quest> questsInDisplayOrder = new List<Quest>();

		// Token: 0x0400213A RID: 8506
		private List<QuestPart_SituationalThought> cachedSituationalThoughtQuestParts = new List<QuestPart_SituationalThought>();
	}
}
