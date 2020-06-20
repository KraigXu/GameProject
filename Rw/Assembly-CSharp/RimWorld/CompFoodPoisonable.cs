using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0D RID: 3341
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x0600513B RID: 20795 RVA: 0x001B4082 File Offset: 0x001B2282
		public float PoisonPercent
		{
			get
			{
				return this.poisonPct;
			}
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x001B408A File Offset: 0x001B228A
		public void SetPoisoned(FoodPoisonCause newCause)
		{
			this.poisonPct = 1f;
			this.cause = newCause;
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x001B409E File Offset: 0x001B229E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
			Scribe_Values.Look<FoodPoisonCause>(ref this.cause, "cause", FoodPoisonCause.Unknown, false);
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x001B40CE File Offset: 0x001B22CE
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
			compFoodPoisonable.cause = this.cause;
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x001B40F4 File Offset: 0x001B22F4
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			if (this.cause == FoodPoisonCause.Unknown && compFoodPoisonable.cause != FoodPoisonCause.Unknown)
			{
				this.cause = compFoodPoisonable.cause;
			}
			else if (compFoodPoisonable.cause != FoodPoisonCause.Unknown || this.cause != FoodPoisonCause.Unknown)
			{
				float num = this.poisonPct * (float)this.parent.stackCount;
				float num2 = compFoodPoisonable.poisonPct * (float)count;
				this.cause = ((num > num2) ? this.cause : compFoodPoisonable.cause);
			}
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x001B4199 File Offset: 0x001B2399
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, this.cause);
			}
		}

		// Token: 0x04002D07 RID: 11527
		private float poisonPct;

		// Token: 0x04002D08 RID: 11528
		public FoodPoisonCause cause;
	}
}
