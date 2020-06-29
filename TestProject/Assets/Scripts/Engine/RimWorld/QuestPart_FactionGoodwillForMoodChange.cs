using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_FactionGoodwillForMoodChange : QuestPartActivable
	{
		
		
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestAveragePawnMood".Translate(120000.ToStringTicksToPeriod(true, false, true, true), this.cachedMovingAverage.ToStringPercent());
			}
		}

		
		
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestAveragePawnMoodTargets".Translate((from p in this.pawns
				select p.LabelShort).ToCommaList(true), 120000.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		
		
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

		
		
		public int SampleSize
		{
			get
			{
				return Mathf.FloorToInt(48f);
			}
		}

		
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

		
		public string inSignal;

		
		public string outSignalSuccess;

		
		public string outSignalFailed;

		
		public List<Pawn> pawns = new List<Pawn>();

		
		private int currentInterval = 2500;

		
		private List<float> movingAverage = new List<float>();

		
		private float cachedMovingAverage;

		
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

		
		public const int Interval = 2500;

		
		public const int Range = 120000;
	}
}
