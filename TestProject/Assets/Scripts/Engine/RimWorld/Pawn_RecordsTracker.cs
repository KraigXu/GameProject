using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Pawn_RecordsTracker : IExposable
	{
		
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x0017CB47 File Offset: 0x0017AD47
		public float StoryRelevance
		{
			get
			{
				return (float)this.storyRelevance + this.storyRelevanceBonus;
			}
		}

		
		// (get) Token: 0x0600467F RID: 18047 RVA: 0x0017CB58 File Offset: 0x0017AD58
		public Battle BattleActive
		{
			get
			{
				if (this.battleExitTick < Find.TickManager.TicksGame)
				{
					return null;
				}
				if (this.battleActive == null)
				{
					return null;
				}
				while (this.battleActive.AbsorbedBy != null)
				{
					this.battleActive = this.battleActive.AbsorbedBy;
				}
				return this.battleActive;
			}
		}

		
		// (get) Token: 0x06004680 RID: 18048 RVA: 0x0017CBA7 File Offset: 0x0017ADA7
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 681;
			this.storyRelevanceBonus = Rand.Range(0f, 100f);
			Rand.PopState();
		}

		
		public void RecordsTick()
		{
			if (this.pawn.Dead)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(80))
			{
				this.RecordsTickUpdate(80);
				this.battleActive = this.BattleActive;
			}
		}

		
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		
		private void RecordsTickUpdate(int interval)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Time && allDefsListForReading[i].Worker.ShouldMeasureTimeNow(this.pawn))
				{
					DefMap<RecordDef, float> defMap = this.records;
					RecordDef def = allDefsListForReading[i];
					defMap[def] += (float)interval;
				}
			}
			this.storyRelevance *= Math.Pow(0.20000000298023224, 0.0);
		}

		
		public void Increment(RecordDef def)
		{
			if (def.type != RecordType.Int)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to increment record \"",
					def.defName,
					"\" whose record type is \"",
					def.type,
					"\"."
				}), false);
				return;
			}
			this.records[def] = Mathf.Round(this.records[def] + 1f);
		}

		
		public void AddTo(RecordDef def, float value)
		{
			if (def.type == RecordType.Int)
			{
				this.records[def] = Mathf.Round(this.records[def] + Mathf.Round(value));
				return;
			}
			if (def.type == RecordType.Float)
			{
				DefMap<RecordDef, float> defMap = this.records;
				defMap[def] += value;
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Tried to add value to record \"",
				def.defName,
				"\" whose record type is \"",
				def.type,
				"\"."
			}), false);
		}

		
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				return Mathf.Round(num);
			}
			return num;
		}

		
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		
		public void AccumulateStoryEvent(StoryEventDef def)
		{
			this.storyRelevance += (double)def.importance;
		}

		
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
			Scribe_Values.Look<double>(ref this.storyRelevance, "storyRelevance", 0.0, false);
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		
		public Pawn pawn;

		
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		
		private double storyRelevance;

		
		private Battle battleActive;

		
		private int battleExitTick;

		
		private float storyRelevanceBonus;

		
		private const int UpdateTimeRecordsIntervalTicks = 80;

		
		private const float StoryRelevanceBonusRange = 100f;

		
		private const float StoryRelevanceMultiplierPerYear = 0.2f;
	}
}
