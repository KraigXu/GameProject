using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092C RID: 2348
	public class HistoryAutoRecorderGroup : IExposable
	{
		// Token: 0x060037CD RID: 14285 RVA: 0x0012B638 File Offset: 0x00129838
		public float GetMaxDay()
		{
			float num = 0f;
			foreach (HistoryAutoRecorder historyAutoRecorder in this.recorders)
			{
				int count = historyAutoRecorder.records.Count;
				if (count != 0)
				{
					float num2 = (float)((count - 1) * historyAutoRecorder.def.recordTicksFrequency) / 60000f;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x0012B6BC File Offset: 0x001298BC
		public void Tick()
		{
			for (int i = 0; i < this.recorders.Count; i++)
			{
				this.recorders[i].Tick();
			}
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0012B6F0 File Offset: 0x001298F0
		public void DrawGraph(Rect graphRect, Rect legendRect, FloatRange section, List<CurveMark> marks)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame != this.cachedGraphTickCount)
			{
				this.cachedGraphTickCount = ticksGame;
				this.curves.Clear();
				for (int i = 0; i < this.recorders.Count; i++)
				{
					HistoryAutoRecorder historyAutoRecorder = this.recorders[i];
					SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo();
					simpleCurveDrawInfo.color = historyAutoRecorder.def.graphColor;
					simpleCurveDrawInfo.label = historyAutoRecorder.def.LabelCap;
					simpleCurveDrawInfo.valueFormat = historyAutoRecorder.def.valueFormat;
					simpleCurveDrawInfo.curve = new SimpleCurve();
					for (int j = 0; j < historyAutoRecorder.records.Count; j++)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint((float)j * (float)historyAutoRecorder.def.recordTicksFrequency / 60000f, historyAutoRecorder.records[j]), false);
					}
					simpleCurveDrawInfo.curve.SortPoints();
					if (historyAutoRecorder.records.Count == 1)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint(1.66666669E-05f, historyAutoRecorder.records[0]), true);
					}
					this.curves.Add(simpleCurveDrawInfo);
				}
			}
			if (Mathf.Approximately(section.min, section.max))
			{
				section.max += 1.66666669E-05f;
			}
			SimpleCurveDrawerStyle curveDrawerStyle = Find.History.curveDrawerStyle;
			curveDrawerStyle.FixedSection = section;
			curveDrawerStyle.UseFixedScale = this.def.useFixedScale;
			curveDrawerStyle.FixedScale = this.def.fixedScale;
			curveDrawerStyle.YIntegersOnly = this.def.integersOnly;
			curveDrawerStyle.OnlyPositiveValues = this.def.onlyPositiveValues;
			SimpleCurveDrawer.DrawCurves(graphRect, this.curves, curveDrawerStyle, marks, legendRect);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0012B8C5 File Offset: 0x00129AC5
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderGroupDef>(ref this.def, "def");
			Scribe_Collections.Look<HistoryAutoRecorder>(ref this.recorders, "recorders", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorders();
			}
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0012B8FC File Offset: 0x00129AFC
		public void AddOrRemoveHistoryRecorders()
		{
			if (this.recorders.RemoveAll((HistoryAutoRecorder x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorders were null.", false);
			}
			using (List<HistoryAutoRecorderDef>.Enumerator enumerator = this.def.historyAutoRecorderDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderDef recorderDef = enumerator.Current;
					if (!this.recorders.Any((HistoryAutoRecorder x) => x.def == recorderDef))
					{
						HistoryAutoRecorder historyAutoRecorder = new HistoryAutoRecorder();
						historyAutoRecorder.def = recorderDef;
						this.recorders.Add(historyAutoRecorder);
					}
				}
			}
			this.recorders.RemoveAll((HistoryAutoRecorder x) => x.def == null);
		}

		// Token: 0x0400210B RID: 8459
		public HistoryAutoRecorderGroupDef def;

		// Token: 0x0400210C RID: 8460
		public List<HistoryAutoRecorder> recorders = new List<HistoryAutoRecorder>();

		// Token: 0x0400210D RID: 8461
		private List<SimpleCurveDrawInfo> curves = new List<SimpleCurveDrawInfo>();

		// Token: 0x0400210E RID: 8462
		private int cachedGraphTickCount = -1;
	}
}
