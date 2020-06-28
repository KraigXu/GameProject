using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB1 RID: 2993
	public class Pawn_RecordsTracker : IExposable
	{
		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x0017CB47 File Offset: 0x0017AD47
		public float StoryRelevance
		{
			get
			{
				return (float)this.storyRelevance + this.storyRelevanceBonus;
			}
		}

		// Token: 0x17000C8D RID: 3213
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

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06004680 RID: 18048 RVA: 0x0017CBA7 File Offset: 0x0017ADA7
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x0017CBB0 File Offset: 0x0017ADB0
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 681;
			this.storyRelevanceBonus = Rand.Range(0f, 100f);
			Rand.PopState();
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x0017CC05 File Offset: 0x0017AE05
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

		// Token: 0x06004683 RID: 18051 RVA: 0x0017CC38 File Offset: 0x0017AE38
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x0017CC44 File Offset: 0x0017AE44
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

		// Token: 0x06004685 RID: 18053 RVA: 0x0017CCD4 File Offset: 0x0017AED4
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

		// Token: 0x06004686 RID: 18054 RVA: 0x0017CD50 File Offset: 0x0017AF50
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

		// Token: 0x06004687 RID: 18055 RVA: 0x0017CDF0 File Offset: 0x0017AFF0
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				return Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x0017CE23 File Offset: 0x0017B023
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x0017CE36 File Offset: 0x0017B036
		public void AccumulateStoryEvent(StoryEventDef def)
		{
			this.storyRelevance += (double)def.importance;
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x0017CE4C File Offset: 0x0017B04C
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0017CE6C File Offset: 0x0017B06C
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
			Scribe_Values.Look<double>(ref this.storyRelevance, "storyRelevance", 0.0, false);
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0400287B RID: 10363
		public Pawn pawn;

		// Token: 0x0400287C RID: 10364
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		// Token: 0x0400287D RID: 10365
		private double storyRelevance;

		// Token: 0x0400287E RID: 10366
		private Battle battleActive;

		// Token: 0x0400287F RID: 10367
		private int battleExitTick;

		// Token: 0x04002880 RID: 10368
		private float storyRelevanceBonus;

		// Token: 0x04002881 RID: 10369
		private const int UpdateTimeRecordsIntervalTicks = 80;

		// Token: 0x04002882 RID: 10370
		private const float StoryRelevanceBonusRange = 100f;

		// Token: 0x04002883 RID: 10371
		private const float StoryRelevanceMultiplierPerYear = 0.2f;
	}
}
