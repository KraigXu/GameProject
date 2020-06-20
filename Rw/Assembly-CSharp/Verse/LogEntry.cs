using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000114 RID: 276
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00023F50 File Offset: 0x00022150
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00023F63 File Offset: 0x00022163
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00023F6B File Offset: 0x0002216B
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00023F63 File Offset: 0x00022163
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00023F73 File Offset: 0x00022173
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00023FB0 File Offset: 0x000221B0
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00023FE8 File Offset: 0x000221E8
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

		// Token: 0x060007BA RID: 1978 RVA: 0x00024094 File Offset: 0x00022294
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000240AC File Offset: 0x000222AC
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000240C4 File Offset: 0x000222C4
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

		// Token: 0x060007BD RID: 1981 RVA: 0x000240FD File Offset: 0x000222FD
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x060007BE RID: 1982
		public abstract bool Concerns(Thing t);

		// Token: 0x060007BF RID: 1983
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x060007C0 RID: 1984 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00024124 File Offset: 0x00022324
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true)).CapitalizeFirst() + ".";
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00024166 File Offset: 0x00022366
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002416F File Offset: 0x0002236F
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x040006F8 RID: 1784
		protected int logID;

		// Token: 0x040006F9 RID: 1785
		protected int ticksAbs = -1;

		// Token: 0x040006FA RID: 1786
		public LogEntryDef def;

		// Token: 0x040006FB RID: 1787
		private WeakReference<Thing> cachedStringPov;

		// Token: 0x040006FC RID: 1788
		private string cachedString;

		// Token: 0x040006FD RID: 1789
		private float cachedHeightWidth;

		// Token: 0x040006FE RID: 1790
		private float cachedHeight;

		// Token: 0x040006FF RID: 1791
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x04000700 RID: 1792
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04000701 RID: 1793
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04000702 RID: 1794
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04000703 RID: 1795
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04000704 RID: 1796
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
