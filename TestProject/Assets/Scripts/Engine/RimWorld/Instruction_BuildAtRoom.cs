using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F10 RID: 3856
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06005E73 RID: 24179
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06005E74 RID: 24180 RVA: 0x0020B051 File Offset: 0x00209251
		protected override float ProgressPercent
		{
			get
			{
				if (this.def.targetCount <= 1)
				{
					return -1f;
				}
				return (float)this.NumPlaced() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x0020B07C File Offset: 0x0020927C
		protected int NumPlaced()
		{
			int num = 0;
			using (CellRect.Enumerator enumerator = this.BuildableRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, this.def.thingDef))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x0020B0EC File Offset: 0x002092EC
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x0020B120 File Offset: 0x00209320
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x06005E78 RID: 24184 RVA: 0x0020B144 File Offset: 0x00209344
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x0020B194 File Offset: 0x00209394
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x0020B1B0 File Offset: 0x002093B0
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
