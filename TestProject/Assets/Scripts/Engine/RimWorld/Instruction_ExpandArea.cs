using System;
using Verse;

namespace RimWorld
{
	
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		
		// (get) Token: 0x06005EAA RID: 24234
		protected abstract Area MyArea { get; }

		
		// (get) Token: 0x06005EAB RID: 24235 RVA: 0x0020BD0C File Offset: 0x00209F0C
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		
		private int startingAreaCount = -1;
	}
}
