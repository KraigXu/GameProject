using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StoryState : IExposable
	{
		
		// (get) Token: 0x06003D78 RID: 15736 RVA: 0x001451D8 File Offset: 0x001433D8
		public IIncidentTarget Target
		{
			get
			{
				return this.target;
			}
		}

		
		// (get) Token: 0x06003D79 RID: 15737 RVA: 0x001451E0 File Offset: 0x001433E0
		public List<QuestScriptDef> RecentRandomQuests
		{
			get
			{
				return this.recentRandomQuests;
			}
		}

		
		// (get) Token: 0x06003D7A RID: 15738 RVA: 0x001451E8 File Offset: 0x001433E8
		public List<QuestScriptDef> RecentRandomDecrees
		{
			get
			{
				return this.recentRandomDecrees;
			}
		}

		
		// (get) Token: 0x06003D7B RID: 15739 RVA: 0x001451F0 File Offset: 0x001433F0
		public int LastRoyalFavorQuestTick
		{
			get
			{
				return this.lastRoyalFavorQuestTick;
			}
		}

		
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x001451F8 File Offset: 0x001433F8
		public int LastThreatBigTick
		{
			get
			{
				if (this.lastThreatBigTick > Find.TickManager.TicksGame + 1000)
				{
					Log.Error(string.Concat(new object[]
					{
						"Latest big threat queue time was ",
						this.lastThreatBigTick,
						" at tick ",
						Find.TickManager.TicksGame,
						". This is too far in the future. Resetting."
					}), false);
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomQuests, "recentRandomQuests", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomDecrees, "recentRandomDecrees", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<int, int>(ref this.colonistCountTicks, "colonistCountTicks", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<int>(ref this.lastRoyalFavorQuestTick, "lastRoyalFavorQuestTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.recentRandomQuests == null)
				{
					this.recentRandomQuests = new List<QuestScriptDef>();
				}
				if (this.recentRandomDecrees == null)
				{
					this.recentRandomDecrees = new List<QuestScriptDef>();
				}
				if (this.colonistCountTicks == null)
				{
					this.colonistCountTicks = new Dictionary<int, int>();
				}
				this.RecordPopulationIncrease();
			}
		}

		
		public void Notify_IncidentFired(FiringIncident fi)
		{
			if (fi.parms.forced || fi.parms.target != this.target)
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (fi.def.category == IncidentCategoryDefOf.ThreatBig)
			{
				this.lastThreatBigTick = ticksGame;
				Find.StoryWatcher.statsRecord.numThreatBigs++;
			}
			if (this.lastFireTicks.ContainsKey(fi.def))
			{
				this.lastFireTicks[fi.def] = ticksGame;
			}
			else
			{
				this.lastFireTicks.Add(fi.def, ticksGame);
			}
			if (fi.def == IncidentDefOf.GiveQuest_Random)
			{
				this.RecordRandomQuestFired(fi.parms.questScriptDef);
			}
		}

		
		public void RecordRandomQuestFired(QuestScriptDef questScript)
		{
			this.recentRandomQuests.Insert(0, questScript);
			while (this.recentRandomQuests.Count > 5)
			{
				this.recentRandomQuests.RemoveAt(this.recentRandomQuests.Count - 1);
			}
			if (questScript.canGiveRoyalFavor)
			{
				this.lastRoyalFavorQuestTick = Find.TickManager.TicksGame;
			}
		}

		
		public void RecordDecreeFired(QuestScriptDef questScript)
		{
			this.recentRandomDecrees.Insert(0, questScript);
			while (this.recentRandomDecrees.Count > 5)
			{
				this.recentRandomDecrees.RemoveAt(this.recentRandomDecrees.Count - 1);
			}
		}

		
		public void RecordPopulationIncrease()
		{
			int count = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
		}

		
		public int GetTicksFromColonistCount(int count)
		{
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
			return this.colonistCountTicks[count];
		}

		
		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
			other.RecentRandomQuests.Clear();
			other.recentRandomQuests.AddRange(this.RecentRandomQuests);
			other.RecentRandomDecrees.Clear();
			other.RecentRandomDecrees.AddRange(this.RecentRandomDecrees);
			other.lastRoyalFavorQuestTick = this.lastRoyalFavorQuestTick;
			other.colonistCountTicks.Clear();
			foreach (KeyValuePair<int, int> keyValuePair2 in this.colonistCountTicks)
			{
				other.colonistCountTicks.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
		}

		
		private IIncidentTarget target;

		
		private int lastThreatBigTick = -1;

		
		private Dictionary<int, int> colonistCountTicks = new Dictionary<int, int>();

		
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		
		private List<QuestScriptDef> recentRandomQuests = new List<QuestScriptDef>();

		
		private List<QuestScriptDef> recentRandomDecrees = new List<QuestScriptDef>();

		
		private int lastRoyalFavorQuestTick = -1;

		
		private const int RecentRandomQuestsMaxStorage = 5;
	}
}
