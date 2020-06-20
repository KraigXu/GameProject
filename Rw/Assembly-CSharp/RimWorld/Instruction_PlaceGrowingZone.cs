using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1F RID: 3871
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x06005EC4 RID: 24260 RVA: 0x0020C128 File Offset: 0x0020A328
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x0020C163 File Offset: 0x0020A363
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x0020C17B File Offset: 0x0020A37B
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x0020C1A3 File Offset: 0x0020A3A3
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x0020C1C1 File Offset: 0x0020A3C1
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06005EC9 RID: 24265 RVA: 0x0020C1DF File Offset: 0x0020A3DF
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAdd_Growing")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400336F RID: 13167
		private CellRect growingZoneRect;

		// Token: 0x04003370 RID: 13168
		private List<IntVec3> cachedCells;
	}
}
