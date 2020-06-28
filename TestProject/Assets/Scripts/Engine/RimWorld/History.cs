using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092A RID: 2346
	public sealed class History : IExposable
	{
		// Token: 0x060037C1 RID: 14273 RVA: 0x0012B22C File Offset: 0x0012942C
		public History()
		{
			this.autoRecorderGroups = new List<HistoryAutoRecorderGroup>();
			this.AddOrRemoveHistoryRecorderGroups();
			this.curveDrawerStyle = new SimpleCurveDrawerStyle();
			this.curveDrawerStyle.DrawMeasures = true;
			this.curveDrawerStyle.DrawPoints = false;
			this.curveDrawerStyle.DrawBackground = true;
			this.curveDrawerStyle.DrawBackgroundLines = false;
			this.curveDrawerStyle.DrawLegend = true;
			this.curveDrawerStyle.DrawCurveMousePoint = true;
			this.curveDrawerStyle.OnlyPositiveValues = true;
			this.curveDrawerStyle.UseFixedSection = true;
			this.curveDrawerStyle.UseAntiAliasedLines = true;
			this.curveDrawerStyle.PointsRemoveOptimization = true;
			this.curveDrawerStyle.MeasureLabelsXCount = 10;
			this.curveDrawerStyle.MeasureLabelsYCount = 5;
			this.curveDrawerStyle.XIntegersOnly = true;
			this.curveDrawerStyle.LabelX = "Day".Translate();
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0012B328 File Offset: 0x00129528
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0012B35C File Offset: 0x0012955C
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x0012B364 File Offset: 0x00129564
		public void ExposeData()
		{
			Scribe_Deep.Look<Archive>(ref this.archive, "archive", Array.Empty<object>());
			Scribe_Collections.Look<HistoryAutoRecorderGroup>(ref this.autoRecorderGroups, "autoRecorderGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastPsylinkAvailable, "lastPsylinkAvailable", -999999, false);
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorderGroups();
				if (this.lastPsylinkAvailable == -999999)
				{
					this.lastPsylinkAvailable = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0012B3E3 File Offset: 0x001295E3
		public void Notify_PsylinkAvailable()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0012B3E3 File Offset: 0x001295E3
		public void FinalizeInit()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x0012B3F8 File Offset: 0x001295F8
		private void AddOrRemoveHistoryRecorderGroups()
		{
			if (this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorder groups were null.", false);
			}
			using (IEnumerator<HistoryAutoRecorderGroupDef> enumerator = DefDatabase<HistoryAutoRecorderGroupDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderGroupDef def = enumerator.Current;
					if (!this.autoRecorderGroups.Any((HistoryAutoRecorderGroup x) => x.def == def))
					{
						HistoryAutoRecorderGroup historyAutoRecorderGroup = new HistoryAutoRecorderGroup();
						historyAutoRecorderGroup.def = def;
						historyAutoRecorderGroup.AddOrRemoveHistoryRecorders();
						this.autoRecorderGroups.Add(historyAutoRecorderGroup);
					}
				}
			}
			this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x.def == null);
		}

		// Token: 0x04002105 RID: 8453
		public Archive archive = new Archive();

		// Token: 0x04002106 RID: 8454
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x04002107 RID: 8455
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x04002108 RID: 8456
		public int lastPsylinkAvailable = -999999;
	}
}
