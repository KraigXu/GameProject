using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00023F50 File Offset: 0x00022150
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00023F63 File Offset: 0x00022163
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00023F6B File Offset: 0x0002216B
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00023F63 File Offset: 0x00022163
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		
		public string ToGameStringFromPOV(Thing pov, bool forceLog = false)
		{
			if (this.cachedString == null || pov == null != (this.cachedStringPov == null) || (this.cachedStringPov != null && pov != this.cachedStringPov.Target) || DebugViewSettings.logGrammarResolution || forceLog)
			{
				Rand.PushState();
				try
				{
					Rand.Seed = this.logID;
					this.cachedStringPov = ((pov != null) ? new WeakReference<Thing>(pov) : null);
					this.cachedString = this.ToGameStringFromPOV_Worker(pov, forceLog);
					this.cachedHeightWidth = 0f;
					this.cachedHeight = 0f;
				}
				finally
				{
					Rand.PopState();
				}
			}
			return this.cachedString;
		}

		
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		
		public float GetTextHeight(Thing pov, float width)
		{
			string text = this.ToGameStringFromPOV(pov, false);
			if (this.cachedHeightWidth != width)
			{
				this.cachedHeightWidth = width;
				this.cachedHeight = Text.CalcHeight(text, width);
			}
			return this.cachedHeight;
		}

		
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		
		public abstract bool Concerns(Thing t);

		
		public abstract IEnumerable<Thing> GetConcerns();

		
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true)).CapitalizeFirst() + ".";
		}

		
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		
		protected int logID;

		
		protected int ticksAbs = -1;

		
		public LogEntryDef def;

		
		private WeakReference<Thing> cachedStringPov;

		
		private string cachedString;

		
		private float cachedHeightWidth;

		
		private float cachedHeight;

		
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
