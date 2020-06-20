using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F19 RID: 3865
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06005EAA RID: 24234
		protected abstract Area MyArea { get; }

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06005EAB RID: 24235 RVA: 0x0020BD0C File Offset: 0x00209F0C
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x0020BD2E File Offset: 0x00209F2E
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x0020BD47 File Offset: 0x00209F47
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x0020B89D File Offset: 0x00209A9D
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400336C RID: 13164
		private int startingAreaCount = -1;
	}
}
