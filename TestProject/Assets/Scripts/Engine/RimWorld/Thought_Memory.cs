﻿using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_Memory : Thought
	{
		
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x0018525E File Offset: 0x0018345E
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x00185273 File Offset: 0x00183473
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x0018527B File Offset: 0x0018347B
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.def.DurationTicks;
			}
		}

		
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

		
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		
		public void Renew()
		{
			this.age = 0;
		}

		
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

		
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return base.MoodOffset() * this.moodPowerFactor;
		}

		
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

		
		public float moodPowerFactor = 1f;

		
		public Pawn otherPawn;

		
		public int age;

		
		private int forcedStage;

		
		private string cachedLabelCap;

		
		private Pawn cachedLabelCapForOtherPawn;

		
		private int cachedLabelCapForStageIndex = -1;
	}
}
