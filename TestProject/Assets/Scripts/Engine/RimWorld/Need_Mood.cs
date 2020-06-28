using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B94 RID: 2964
	public class Need_Mood : Need_Seeker
	{
		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004583 RID: 17795 RVA: 0x001778A0 File Offset: 0x00175AA0
		public override float CurInstantLevel
		{
			get
			{
				float num = this.thoughts.TotalMoodOffset();
				if (this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
				{
					num += Find.Storyteller.difficulty.colonistMoodOffset;
				}
				return Mathf.Clamp01(this.def.baseLevel + num / 100f);
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x00177900 File Offset: 0x00175B00
		public string MoodString
		{
			get
			{
				if (this.pawn.MentalStateDef != null)
				{
					return "Mood_MentalState".Translate();
				}
				float breakThresholdExtreme = this.pawn.mindState.mentalBreaker.BreakThresholdExtreme;
				if (this.CurLevel < breakThresholdExtreme)
				{
					return "Mood_AboutToBreak".Translate();
				}
				if (this.CurLevel < breakThresholdExtreme + 0.05f)
				{
					return "Mood_OnEdge".Translate();
				}
				if (this.CurLevel < this.pawn.mindState.mentalBreaker.BreakThresholdMinor)
				{
					return "Mood_Stressed".Translate();
				}
				if (this.CurLevel < 0.65f)
				{
					return "Mood_Neutral".Translate();
				}
				if (this.CurLevel < 0.9f)
				{
					return "Mood_Content".Translate();
				}
				return "Mood_Happy".Translate();
			}
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x001779EE File Offset: 0x00175BEE
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x00177A1C File Offset: 0x00175C1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThoughtHandler>(ref this.thoughts, "thoughts", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PawnRecentMemory>(ref this.recentMemory, "recentMemory", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x00177A6D File Offset: 0x00175C6D
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x00177A98 File Offset: 0x00175C98
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetTipString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("MentalBreakThresholdExtreme".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdExtreme.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMajor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMajor.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMinor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMinor.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x00177B80 File Offset: 0x00175D80
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMajor);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x040027DE RID: 10206
		public ThoughtHandler thoughts;

		// Token: 0x040027DF RID: 10207
		public PawnObserver observer;

		// Token: 0x040027E0 RID: 10208
		public PawnRecentMemory recentMemory;
	}
}
