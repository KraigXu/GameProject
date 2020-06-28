using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B99 RID: 2969
	public class Need_Rest : Need
	{
		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004592 RID: 17810 RVA: 0x00177EEA File Offset: 0x001760EA
		public RestCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RestCategory.Exhausted;
				}
				if (this.CurLevel < 0.14f)
				{
					return RestCategory.VeryTired;
				}
				if (this.CurLevel < 0.28f)
				{
					return RestCategory.Tired;
				}
				return RestCategory.Rested;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004593 RID: 17811 RVA: 0x00177F1C File Offset: 0x0017611C
		public float RestFallPerTick
		{
			get
			{
				switch (this.CurCategory)
				{
				case RestCategory.Rested:
					return 1.58333332E-05f * this.RestFallFactor;
				case RestCategory.Tired:
					return 1.58333332E-05f * this.RestFallFactor * 0.7f;
				case RestCategory.VeryTired:
					return 1.58333332E-05f * this.RestFallFactor * 0.3f;
				case RestCategory.Exhausted:
					return 1.58333332E-05f * this.RestFallFactor * 0.6f;
				default:
					return 999f;
				}
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004594 RID: 17812 RVA: 0x00177F93 File Offset: 0x00176193
		private float RestFallFactor
		{
			get
			{
				return this.pawn.health.hediffSet.RestFallFactor;
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004595 RID: 17813 RVA: 0x00177FAA File Offset: 0x001761AA
		public override int GUIChangeArrow
		{
			get
			{
				if (this.Resting)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004596 RID: 17814 RVA: 0x00177FB7 File Offset: 0x001761B7
		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x00177FBF File Offset: 0x001761BF
		private bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + 2;
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x00177FD8 File Offset: 0x001761D8
		public Need_Rest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.28f);
			this.threshPercents.Add(0.14f);
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x0017802D File Offset: 0x0017622D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x00178047 File Offset: 0x00176247
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00178060 File Offset: 0x00176260
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				if (this.Resting)
				{
					float num = this.lastRestEffectiveness;
					num *= this.pawn.GetStatValue(StatDefOf.RestRateMultiplier, true);
					if (num > 0f)
					{
						this.CurLevel += 0.005714286f * num;
					}
				}
				else
				{
					this.CurLevel -= this.RestFallPerTick * 150f;
				}
			}
			if (this.CurLevel < 0.0001f)
			{
				this.ticksAtZero += 150;
			}
			else
			{
				this.ticksAtZero = 0;
			}
			if (this.ticksAtZero > 1000 && this.pawn.Spawned)
			{
				float mtb;
				if (this.ticksAtZero < 15000)
				{
					mtb = 0.25f;
				}
				else if (this.ticksAtZero < 30000)
				{
					mtb = 0.125f;
				}
				else if (this.ticksAtZero < 45000)
				{
					mtb = 0.0833333358f;
				}
				else
				{
					mtb = 0.0625f;
				}
				if (Rand.MTBEventOccurs(mtb, 60000f, 150f) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown))
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, this.pawn.Position), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false, false);
					if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromCollapsingExhausted)
					{
						this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
					}
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageInvoluntarySleep".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					TaleRecorder.RecordTale(TaleDefOf.Exhausted, new object[]
					{
						this.pawn
					});
				}
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x0017826F File Offset: 0x0017646F
		public void TickResting(float restEffectiveness)
		{
			if (restEffectiveness <= 0f)
			{
				return;
			}
			this.lastRestTick = Find.TickManager.TicksGame;
			this.lastRestEffectiveness = restEffectiveness;
		}

		// Token: 0x040027F6 RID: 10230
		private int lastRestTick = -999;

		// Token: 0x040027F7 RID: 10231
		private float lastRestEffectiveness = 1f;

		// Token: 0x040027F8 RID: 10232
		private int ticksAtZero;

		// Token: 0x040027F9 RID: 10233
		private const float FullSleepHours = 10.5f;

		// Token: 0x040027FA RID: 10234
		public const float BaseRestGainPerTick = 3.809524E-05f;

		// Token: 0x040027FB RID: 10235
		private const float BaseRestFallPerTick = 1.58333332E-05f;

		// Token: 0x040027FC RID: 10236
		public const float ThreshTired = 0.28f;

		// Token: 0x040027FD RID: 10237
		public const float ThreshVeryTired = 0.14f;

		// Token: 0x040027FE RID: 10238
		public const float DefaultFallAsleepMaxLevel = 0.75f;

		// Token: 0x040027FF RID: 10239
		public const float DefaultNaturalWakeThreshold = 1f;

		// Token: 0x04002800 RID: 10240
		public const float CanWakeThreshold = 0.2f;

		// Token: 0x04002801 RID: 10241
		private const float BaseInvoluntarySleepMTBDays = 0.25f;
	}
}
