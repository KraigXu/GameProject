using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000955 RID: 2389
	public class QuestPart_FactionGoodwillForMoodChange : QuestPartActivable
	{
		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06003880 RID: 14464 RVA: 0x0012E417 File Offset: 0x0012C617
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestAveragePawnMood".Translate(120000.ToStringTicksToPeriod(true, false, true, true), this.cachedMovingAverage.ToStringPercent());
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x0012E44C File Offset: 0x0012C64C
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestAveragePawnMoodTargets".Translate((from p in this.pawns
				select p.LabelShort).ToCommaList(true), 120000.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003882 RID: 14466 RVA: 0x0012E4B0 File Offset: 0x0012C6B0
		private float AveragePawnMoodPercent
		{
			get
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].needs != null && this.pawns[i].needs.mood != null)
					{
						num += this.pawns[i].needs.mood.CurLevelPercentage;
						num2++;
					}
				}
				if (num2 == 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003883 RID: 14467 RVA: 0x0012E538 File Offset: 0x0012C738
		private float MovingAveragePawnMoodPercent
		{
			get
			{
				if (this.movingAverage.Count == 0)
				{
					return this.AveragePawnMoodPercent;
				}
				float num = 0f;
				for (int i = 0; i < this.movingAverage.Count; i++)
				{
					num += this.movingAverage[i];
				}
				return num / (float)this.movingAverage.Count;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003884 RID: 14468 RVA: 0x0012E592 File Offset: 0x0012C792
		public int SampleSize
		{
			get
			{
				return Mathf.FloorToInt(48f);
			}
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0012E5A0 File Offset: 0x0012C7A0
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			this.currentInterval++;
			if (this.currentInterval >= 2500)
			{
				this.currentInterval = 0;
				while (this.movingAverage.Count >= this.SampleSize)
				{
					this.movingAverage.RemoveLast<float>();
				}
				this.movingAverage.Insert(0, this.AveragePawnMoodPercent);
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x0012E614 File Offset: 0x0012C814
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!this.inSignal.NullOrEmpty() && signal.tag == this.inSignal)
			{
				float movingAveragePawnMoodPercent = this.MovingAveragePawnMoodPercent;
				int num = Mathf.RoundToInt(QuestPart_FactionGoodwillForMoodChange.GoodwillFromAverageMoodCurve.Evaluate(movingAveragePawnMoodPercent));
				SignalArgs args = new SignalArgs(num.Named("GOODWILL"), movingAveragePawnMoodPercent.ToStringPercent().Named("AVERAGEMOOD"));
				if (num > 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, args));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignalFailed, args));
			}
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0012E6BC File Offset: 0x0012C8BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailed, "outSignalFailed", null, false);
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<float>(ref this.movingAverage, "movingAverage", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.movingAverage == null)
				{
					this.movingAverage = new List<float>();
				}
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x04002163 RID: 8547
		public string inSignal;

		// Token: 0x04002164 RID: 8548
		public string outSignalSuccess;

		// Token: 0x04002165 RID: 8549
		public string outSignalFailed;

		// Token: 0x04002166 RID: 8550
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002167 RID: 8551
		private int currentInterval = 2500;

		// Token: 0x04002168 RID: 8552
		private List<float> movingAverage = new List<float>();

		// Token: 0x04002169 RID: 8553
		private float cachedMovingAverage;

		// Token: 0x0400216A RID: 8554
		private static readonly SimpleCurve GoodwillFromAverageMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 20f),
				true
			}
		};

		// Token: 0x0400216B RID: 8555
		public const int Interval = 2500;

		// Token: 0x0400216C RID: 8556
		public const int Range = 120000;
	}
}
