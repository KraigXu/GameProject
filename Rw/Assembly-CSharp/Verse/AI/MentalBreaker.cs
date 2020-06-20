using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x0200053F RID: 1343
	public class MentalBreaker : IExposable
	{
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x000E23E8 File Offset: 0x000E05E8
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) * 0.142857149f;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002662 RID: 9826 RVA: 0x000E2401 File Offset: 0x000E0601
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) * 0.5714286f;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x000E241A File Offset: 0x000E061A
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x000E242D File Offset: 0x000E062D
		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x000E245D File Offset: 0x000E065D
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002666 RID: 9830 RVA: 0x000E247C File Offset: 0x000E067C
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x000E24A3 File Offset: 0x000E06A3
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x000E24D2 File Offset: 0x000E06D2
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x000E24FF File Offset: 0x000E06FF
		public float CurMood
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return 0.5f;
				}
				return this.pawn.needs.mood.CurLevel;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x0600266A RID: 9834 RVA: 0x000E252E File Offset: 0x000E072E
		private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
		{
			get
			{
				MentalBreakIntensity intensity;
				Func<MentalBreakDef, bool> <>9__0;
				MentalBreakIntensity intensity2;
				for (intensity = this.CurrentDesiredMoodBreakIntensity; intensity != MentalBreakIntensity.None; intensity = (MentalBreakIntensity)(intensity2 - MentalBreakIntensity.Minor))
				{
					IEnumerable<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
					Func<MentalBreakDef, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((MentalBreakDef d) => d.intensity == intensity && d.Worker.BreakCanOccur(this.pawn)));
					}
					IEnumerable<MentalBreakDef> enumerable = allDefsListForReading.Where(predicate);
					bool flag = false;
					foreach (MentalBreakDef mentalBreakDef in enumerable)
					{
						yield return mentalBreakDef;
						flag = true;
					}
					IEnumerator<MentalBreakDef> enumerator = null;
					if (flag)
					{
						yield break;
					}
					intensity2 = intensity;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x0600266B RID: 9835 RVA: 0x000E253E File Offset: 0x000E073E
		private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
		{
			get
			{
				if (this.ticksBelowExtreme >= 2000)
				{
					return MentalBreakIntensity.Extreme;
				}
				if (this.ticksBelowMajor >= 2000)
				{
					return MentalBreakIntensity.Major;
				}
				if (this.ticksBelowMinor >= 2000)
				{
					return MentalBreakIntensity.Minor;
				}
				return MentalBreakIntensity.None;
			}
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public MentalBreaker()
		{
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000E256E File Offset: 0x000E076E
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000E257D File Offset: 0x000E077D
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
			this.ticksBelowMinor = 0;
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000E2594 File Offset: 0x000E0794
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalBreak, "ticksUntilCanDoMentalBreak", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000E25EC File Offset: 0x000E07EC
		public void MentalBreakerTick()
		{
			if (this.ticksUntilCanDoMentalBreak > 0 && this.pawn.Awake())
			{
				this.ticksUntilCanDoMentalBreak--;
			}
			if (this.CanDoRandomMentalBreaks && this.pawn.MentalStateDef == null && this.pawn.IsHashIntervalTick(150))
			{
				if (!DebugSettings.enableRandomMentalStates)
				{
					return;
				}
				if (this.CurMood < this.BreakThresholdExtreme)
				{
					this.ticksBelowExtreme += 150;
				}
				else
				{
					this.ticksBelowExtreme = 0;
				}
				if (this.CurMood < this.BreakThresholdMajor)
				{
					this.ticksBelowMajor += 150;
				}
				else
				{
					this.ticksBelowMajor = 0;
				}
				if (this.CurMood < this.BreakThresholdMinor)
				{
					this.ticksBelowMinor += 150;
				}
				else
				{
					this.ticksBelowMinor = 0;
				}
				if (this.TestMoodMentalBreak() && this.TryDoRandomMoodCausedMentalBreak())
				{
					return;
				}
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int i = 0; i < allTraits.Count; i++)
					{
						if (allTraits[i].CurrentData.MentalStateGiver.CheckGive(this.pawn, 150))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000E273C File Offset: 0x000E093C
		private bool TestMoodMentalBreak()
		{
			if (this.ticksUntilCanDoMentalBreak > 0)
			{
				return false;
			}
			if (this.ticksBelowExtreme > 2000)
			{
				return Rand.MTBEventOccurs(0.5f, 60000f, 150f);
			}
			if (this.ticksBelowMajor > 2000)
			{
				return Rand.MTBEventOccurs(0.8f, 60000f, 150f);
			}
			return this.ticksBelowMinor > 2000 && Rand.MTBEventOccurs(4f, 60000f, 150f);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000E27BC File Offset: 0x000E09BC
		public bool TryDoRandomMoodCausedMentalBreak()
		{
			if (!this.CanDoRandomMentalBreaks || this.pawn.Downed || !this.pawn.Awake() || this.pawn.InMentalState)
			{
				return false;
			}
			if (this.pawn.Faction != Faction.OfPlayer && this.CurrentDesiredMoodBreakIntensity != MentalBreakIntensity.Extreme)
			{
				return false;
			}
			if (QuestUtility.AnyQuestDisablesRandomMoodCausedMentalBreaksFor(this.pawn))
			{
				return false;
			}
			MentalBreakDef mentalBreakDef;
			if (!this.CurrentPossibleMoodBreaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out mentalBreakDef))
			{
				return false;
			}
			Thought thought = this.RandomFinalStraw();
			TaggedString taggedString = "MentalStateReason_Mood".Translate();
			if (thought != null)
			{
				taggedString += "\n\n" + "FinalStraw".Translate(thought.LabelCap);
			}
			return mentalBreakDef.Worker.TryStart(this.pawn, taggedString, true);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000E2898 File Offset: 0x000E0A98
		private Thought RandomFinalStraw()
		{
			this.pawn.needs.mood.thoughts.GetAllMoodThoughts(MentalBreaker.tmpThoughts);
			float num = 0f;
			for (int i = 0; i < MentalBreaker.tmpThoughts.Count; i++)
			{
				float num2 = MentalBreaker.tmpThoughts[i].MoodOffset();
				if (num2 < num)
				{
					num = num2;
				}
			}
			float maxMoodOffset = num * 0.5f;
			Thought result = null;
			(from x in MentalBreaker.tmpThoughts
			where x.MoodOffset() <= maxMoodOffset
			select x).TryRandomElementByWeight((Thought x) => -x.MoodOffset(), out result);
			MentalBreaker.tmpThoughts.Clear();
			return result;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000E2956 File Offset: 0x000E0B56
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalBreak = 15000;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000E2963 File Offset: 0x000E0B63
		public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
		{
			switch (intensity)
			{
			case MentalBreakIntensity.Minor:
				return this.BreakThresholdMinor;
			case MentalBreakIntensity.Major:
				return this.BreakThresholdMajor;
			case MentalBreakIntensity.Extreme:
				return this.BreakThresholdExtreme;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000E2998 File Offset: 0x000E0B98
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn.ToString());
			stringBuilder.AppendLine("   ticksUntilCanDoMentalBreak=" + this.ticksUntilCanDoMentalBreak);
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowExtreme=",
				this.ticksBelowExtreme,
				"/",
				2000
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowSerious=",
				this.ticksBelowMajor,
				"/",
				2000
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowMinor=",
				this.ticksBelowMinor,
				"/",
				2000
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current desired mood break intensity: " + this.CurrentDesiredMoodBreakIntensity.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current possible mood breaks:");
			float num = (from d in this.CurrentPossibleMoodBreaks
			select d.Worker.CommonalityFor(this.pawn, true)).Sum();
			foreach (MentalBreakDef mentalBreakDef in this.CurrentPossibleMoodBreaks)
			{
				float num2 = mentalBreakDef.Worker.CommonalityFor(this.pawn, true);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					mentalBreakDef,
					"     ",
					(num2 / num).ToStringPercent()
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000E2B74 File Offset: 0x000E0D74
		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn + " current possible mood mental breaks:");
			stringBuilder.AppendLine("CurrentDesiredMoodBreakIntensity: " + this.CurrentDesiredMoodBreakIntensity);
			foreach (MentalBreakDef arg in this.CurrentPossibleMoodBreaks)
			{
				stringBuilder.AppendLine("  " + arg);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04001720 RID: 5920
		private Pawn pawn;

		// Token: 0x04001721 RID: 5921
		private int ticksUntilCanDoMentalBreak;

		// Token: 0x04001722 RID: 5922
		private int ticksBelowExtreme;

		// Token: 0x04001723 RID: 5923
		private int ticksBelowMajor;

		// Token: 0x04001724 RID: 5924
		private int ticksBelowMinor;

		// Token: 0x04001725 RID: 5925
		private const int CheckInterval = 150;

		// Token: 0x04001726 RID: 5926
		private const float ExtremeBreakMTBDays = 0.5f;

		// Token: 0x04001727 RID: 5927
		private const float MajorBreakMTBDays = 0.8f;

		// Token: 0x04001728 RID: 5928
		private const float MinorBreakMTBDays = 4f;

		// Token: 0x04001729 RID: 5929
		private const int MinTicksBelowToBreak = 2000;

		// Token: 0x0400172A RID: 5930
		private const int MinTicksSinceRecoveryToBreak = 15000;

		// Token: 0x0400172B RID: 5931
		private const float MajorBreakMoodFraction = 0.5714286f;

		// Token: 0x0400172C RID: 5932
		private const float ExtremeBreakMoodFraction = 0.142857149f;

		// Token: 0x0400172D RID: 5933
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
