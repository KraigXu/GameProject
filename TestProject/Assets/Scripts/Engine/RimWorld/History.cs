using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public sealed class History : IExposable
	{
		
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

		
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
		}

		
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		
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

		
		public void Notify_PsylinkAvailable()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		
		public void FinalizeInit()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		
		private void AddOrRemoveHistoryRecorderGroups()
		{
			if (this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorder groups were null.", false);
			}
			IEnumerator<HistoryAutoRecorderGroupDef> enumerator = DefDatabase<HistoryAutoRecorderGroupDef>.AllDefs.GetEnumerator();
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

		
		public Archive archive = new Archive();

		
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		
		public SimpleCurveDrawerStyle curveDrawerStyle;

		
		public int lastPsylinkAvailable = -999999;
	}
}
