using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestManager : IExposable
	{
		
		
		public List<Quest> QuestsListForReading
		{
			get
			{
				return this.quests;
			}
		}

		
		
		public List<QuestPart_SituationalThought> SituationalThoughtQuestParts
		{
			get
			{
				return this.cachedSituationalThoughtQuestParts;
			}
		}

		
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

		
		public bool Contains(Quest quest)
		{
			return this.quests.Contains(quest);
		}

		
		public void QuestManagerTick()
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].QuestTick();
			}
		}

		
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

		
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			for (int i = 0; i < this.quests.Count; i++)
			{
				this.quests[i].Notify_PawnDiscarded(pawn);
			}
		}

		
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

		
		private List<Quest> quests = new List<Quest>();

		
		public List<Quest> questsInDisplayOrder = new List<Quest>();

		
		private List<QuestPart_SituationalThought> cachedSituationalThoughtQuestParts = new List<QuestPart_SituationalThought>();
	}
}
