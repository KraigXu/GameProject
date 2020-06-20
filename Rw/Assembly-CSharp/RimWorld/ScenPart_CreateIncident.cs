using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C19 RID: 3097
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x060049D5 RID: 18901 RVA: 0x0018FEBE File Offset: 0x0018E0BE
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x060049D6 RID: 18902 RVA: 0x0018FEC5 File Offset: 0x0018E0C5
		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x0018FED4 File Offset: 0x0018E0D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x0018FF38 File Offset: 0x0018E138
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "intervalDays".Translate(), ref this.intervalDays, ref this.intervalDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect3, "repeat".Translate(), ref this.repeat, false, null, null, false);
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00190044 File Offset: 0x0018E244
		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = 15f * Rand.Gaussian(0f, 1f) + 30f;
			if (this.intervalDays < 0f)
			{
				this.intervalDays = 0f;
			}
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x001900A2 File Offset: 0x0018E2A2
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x001900AB File Offset: 0x0018E2AB
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x001900CC File Offset: 0x0018E2CC
		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap == null)
			{
				return;
			}
			if (this.isFinished)
			{
				return;
			}
			if (this.incident == null)
			{
				Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null", false);
				this.isFinished = true;
				return;
			}
			if ((float)Find.TickManager.TicksGame >= this.occurTick)
			{
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.incident.category, (from x in Find.Maps
				where x.IsPlayerHome
				select x).RandomElement<Map>());
				if (!this.incident.Worker.TryExecute(parms))
				{
					this.isFinished = true;
					return;
				}
				if (this.repeat && this.intervalDays > 0f)
				{
					this.occurTick += this.IntervalTicks;
					return;
				}
				this.isFinished = true;
			}
		}

		// Token: 0x040029FD RID: 10749
		private const float IntervalMidpoint = 30f;

		// Token: 0x040029FE RID: 10750
		private const float IntervalDeviation = 15f;

		// Token: 0x040029FF RID: 10751
		private float intervalDays;

		// Token: 0x04002A00 RID: 10752
		private bool repeat;

		// Token: 0x04002A01 RID: 10753
		private string intervalDaysBuffer;

		// Token: 0x04002A02 RID: 10754
		private float occurTick;

		// Token: 0x04002A03 RID: 10755
		private bool isFinished;
	}
}
