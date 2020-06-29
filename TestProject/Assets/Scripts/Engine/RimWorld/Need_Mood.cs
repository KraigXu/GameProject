﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Need_Mood : Need_Seeker
	{
		
		
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

		
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		
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

		
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		
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

		
		public ThoughtHandler thoughts;

		
		public PawnObserver observer;

		
		public PawnRecentMemory recentMemory;
	}
}
