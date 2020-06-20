using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCF RID: 3023
	public class Thought_Memory : Thought
	{
		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x0018525E File Offset: 0x0018345E
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x00185273 File Offset: 0x00183473
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x0018527B File Offset: 0x0018347B
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.def.DurationTicks;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060047B8 RID: 18360 RVA: 0x00185290 File Offset: 0x00183490
		public override string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null || this.cachedLabelCapForOtherPawn != this.otherPawn || this.cachedLabelCapForStageIndex != this.CurStageIndex)
				{
					if (this.otherPawn != null)
					{
						this.cachedLabelCap = base.CurStage.label.Formatted(this.otherPawn.LabelShort, this.otherPawn).CapitalizeFirst();
						if (this.def.Worker != null)
						{
							this.cachedLabelCap = this.def.Worker.PostProcessLabel(this.pawn, this.cachedLabelCap);
						}
					}
					else
					{
						this.cachedLabelCap = base.LabelCap;
					}
					this.cachedLabelCapForOtherPawn = this.otherPawn;
					this.cachedLabelCapForStageIndex = this.CurStageIndex;
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060047B9 RID: 18361 RVA: 0x00185368 File Offset: 0x00183568
		public override string LabelCapSocial
		{
			get
			{
				if (base.CurStage.labelSocial != null)
				{
					return base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				return base.LabelCapSocial;
			}
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x001853BE File Offset: 0x001835BE
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x001853C8 File Offset: 0x001835C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x00185426 File Offset: 0x00183626
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0018543A File Offset: 0x0018363A
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x00185444 File Offset: 0x00183644
		public virtual bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= this.def.stackLimit)
			{
				Thought_Memory thought_Memory = thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.def.DurationTicks / 2);
					thought_Memory.Renew();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x001854B4 File Offset: 0x001836B4
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x001854F9 File Offset: 0x001836F9
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return base.MoodOffset() * this.moodPowerFactor;
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x00185524 File Offset: 0x00183724
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				", moodPowerFactor=",
				this.moodPowerFactor,
				", age=",
				this.age,
				")"
			});
		}

		// Token: 0x04002930 RID: 10544
		public float moodPowerFactor = 1f;

		// Token: 0x04002931 RID: 10545
		public Pawn otherPawn;

		// Token: 0x04002932 RID: 10546
		public int age;

		// Token: 0x04002933 RID: 10547
		private int forcedStage;

		// Token: 0x04002934 RID: 10548
		private string cachedLabelCap;

		// Token: 0x04002935 RID: 10549
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04002936 RID: 10550
		private int cachedLabelCapForStageIndex = -1;
	}
}
