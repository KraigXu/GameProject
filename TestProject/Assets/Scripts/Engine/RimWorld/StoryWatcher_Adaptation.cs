using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2A RID: 2602
	public class StoryWatcher_Adaptation : IExposable
	{
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x001457C7 File Offset: 0x001439C7
		public float TotalThreatPointsFactor
		{
			get
			{
				return this.StorytellerDef.pointsFactorFromAdaptDays.Evaluate(this.adaptDays);
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x001457DF File Offset: 0x001439DF
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x001457E7 File Offset: 0x001439E7
		private int Population
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x00144F07 File Offset: 0x00143107
		private StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x001457F4 File Offset: 0x001439F4
		public void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist)
			{
				return;
			}
			if (ev != AdaptationEvent.Downed)
			{
				this.ResolvePawnEvent(p, ev);
				return;
			}
			if (dinfo == null || !dinfo.Value.Def.ExternalViolenceFor(p))
			{
				return;
			}
			this.pawnsJustDownedThisTick.Add(p);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x00145850 File Offset: 0x00143A50
		private void ResolvePawnEvent(Pawn p, AdaptationEvent ev)
		{
			float num;
			if (ev == AdaptationEvent.Downed)
			{
				num = this.StorytellerDef.adaptDaysLossFromColonistViolentlyDownedByPopulation.Evaluate((float)this.Population);
			}
			else
			{
				if (this.pawnsJustDownedThisTick.Contains(p))
				{
					this.pawnsJustDownedThisTick.Remove(p);
				}
				int num2 = this.Population - 1;
				num = this.StorytellerDef.adaptDaysLossFromColonistLostByPostPopulation.Evaluate((float)num2);
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message(string.Concat(new object[]
				{
					"Adaptation event: ",
					p,
					" ",
					ev,
					". Loss: ",
					num.ToString("F1"),
					" from ",
					this.adaptDays.ToString("F1")
				}), false);
			}
			this.adaptDays = Mathf.Max(this.StorytellerDef.adaptDaysMin, this.adaptDays - num);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x00145938 File Offset: 0x00143B38
		public void AdaptationWatcherTick()
		{
			for (int i = 0; i < this.pawnsJustDownedThisTick.Count; i++)
			{
				this.ResolvePawnEvent(this.pawnsJustDownedThisTick[i], AdaptationEvent.Downed);
			}
			this.pawnsJustDownedThisTick.Clear();
			if (Find.TickManager.TicksGame % 30000 == 0)
			{
				if (this.adaptDays >= 0f && (float)GenDate.DaysPassed < this.StorytellerDef.adaptDaysGameStartGraceDays)
				{
					return;
				}
				float num = 0.5f * this.StorytellerDef.adaptDaysGrowthRateCurve.Evaluate(this.adaptDays);
				if (this.adaptDays > 0f)
				{
					num *= Find.Storyteller.difficulty.adaptationGrowthRateFactorOverZero;
				}
				this.adaptDays += num;
				this.adaptDays = Mathf.Min(this.adaptDays, this.StorytellerDef.adaptDaysMax);
			}
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x00145A15 File Offset: 0x00143C15
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x00145A2D File Offset: 0x00143C2D
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x040023F6 RID: 9206
		private float adaptDays;

		// Token: 0x040023F7 RID: 9207
		private List<Pawn> pawnsJustDownedThisTick = new List<Pawn>();

		// Token: 0x040023F8 RID: 9208
		private const int UpdateInterval = 30000;
	}
}
