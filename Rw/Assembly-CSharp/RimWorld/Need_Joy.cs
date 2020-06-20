using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B93 RID: 2963
	public class Need_Joy : Need
	{
		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004579 RID: 17785 RVA: 0x00177540 File Offset: 0x00175740
		public JoyCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return JoyCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return JoyCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return JoyCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return JoyCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return JoyCategory.High;
				}
				return JoyCategory.Extreme;
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x0017759C File Offset: 0x0017579C
		private float FallPerInterval
		{
			get
			{
				switch (this.CurCategory)
				{
				case JoyCategory.Empty:
					return 0.0015f;
				case JoyCategory.VeryLow:
					return 0.0006f;
				case JoyCategory.Low:
					return 0.00105f;
				case JoyCategory.Satisfied:
					return 0.0015f;
				case JoyCategory.High:
					return 0.0015f;
				case JoyCategory.Extreme:
					return 0.0015f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x0600457B RID: 17787 RVA: 0x001775F9 File Offset: 0x001757F9
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (!this.GainingJoy)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x0600457C RID: 17788 RVA: 0x00177610 File Offset: 0x00175810
		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 10;
			}
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x00177628 File Offset: 0x00175828
		public Need_Joy(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0017769D File Offset: 0x0017589D
		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x001776B0 File Offset: 0x001758B0
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.6f);
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x001776C8 File Offset: 0x001758C8
		public void GainJoy(float amount, JoyKindDef joyKind)
		{
			if (amount <= 0f)
			{
				return;
			}
			amount *= this.tolerances.JoyFactorFromTolerance(joyKind);
			amount = Mathf.Min(amount, 1f - this.CurLevel);
			this.curLevelInt += amount;
			if (joyKind != null)
			{
				this.tolerances.Notify_JoyGained(amount, joyKind);
			}
			this.lastGainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x00177730 File Offset: 0x00175930
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.tolerances.NeedInterval(this.pawn);
				if (!this.GainingJoy)
				{
					this.CurLevel -= this.FallPerInterval;
				}
			}
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00177768 File Offset: 0x00175968
		public override string GetTipString()
		{
			string text = base.GetTipString();
			string text2 = this.tolerances.TolerancesString();
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (this.pawn.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(this.pawn);
				text += "\n\n" + "CurrentExpectationsAndRecreation".Translate(expectationDef.label, expectationDef.joyToleranceDropPerDay.ToStringPercent(), expectationDef.joyKindsNeeded);
				text = text + "\n\n" + JoyUtility.JoyKindsOnMapString(this.pawn.MapHeld);
			}
			else
			{
				Caravan caravan = this.pawn.GetCaravan();
				if (caravan != null)
				{
					float num = caravan.needs.GetCurrentJoyGainPerTick(this.pawn) * 2500f;
					if (num > 0f)
					{
						text += "\n\n" + "GainingJoyBecauseCaravanNotMoving".Translate() + ": +" + num.ToStringPercent() + "/" + "LetterHour".Translate();
					}
				}
			}
			return text;
		}

		// Token: 0x040027DC RID: 10204
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		// Token: 0x040027DD RID: 10205
		private int lastGainTick = -999;
	}
}
