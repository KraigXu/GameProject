using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEE RID: 3054
	public class FactionRelation : IExposable
	{
		// Token: 0x060048A2 RID: 18594 RVA: 0x0018B280 File Offset: 0x00189480
		public void CheckKindThresholds(Faction faction, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			FactionRelationKind previousKind = this.kind;
			sentLetter = false;
			if (this.kind != FactionRelationKind.Hostile && this.goodwill <= -75)
			{
				this.kind = FactionRelationKind.Hostile;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind != FactionRelationKind.Ally && this.goodwill >= 75)
			{
				this.kind = FactionRelationKind.Ally;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Hostile && this.goodwill >= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Ally && this.goodwill <= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x0018B348 File Offset: 0x00189548
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<int>(ref this.goodwill, "goodwill", 0, false);
			Scribe_Values.Look<FactionRelationKind>(ref this.kind, "kind", FactionRelationKind.Neutral, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x0018B388 File Offset: 0x00189588
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.other,
				", goodwill=",
				this.goodwill.ToString("F1"),
				", kind=",
				this.kind,
				")"
			});
		}

		// Token: 0x0400299E RID: 10654
		public Faction other;

		// Token: 0x0400299F RID: 10655
		public int goodwill = 100;

		// Token: 0x040029A0 RID: 10656
		public FactionRelationKind kind = FactionRelationKind.Neutral;
	}
}
