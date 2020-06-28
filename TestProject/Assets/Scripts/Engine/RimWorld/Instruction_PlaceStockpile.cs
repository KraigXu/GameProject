using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F20 RID: 3872
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x06005ECB RID: 24267 RVA: 0x0020C210 File Offset: 0x0020A410
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06005ECC RID: 24268 RVA: 0x0020C24B File Offset: 0x0020A44B
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06005ECD RID: 24269 RVA: 0x0020C263 File Offset: 0x0020A463
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x06005ECE RID: 24270 RVA: 0x0020C28A File Offset: 0x0020A48A
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x0020C2A8 File Offset: 0x0020A4A8
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x06005ED0 RID: 24272 RVA: 0x0020C2C6 File Offset: 0x0020A4C6
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAddStockpile_Resources")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x04003371 RID: 13169
		private CellRect stockpileRect;

		// Token: 0x04003372 RID: 13170
		private List<IntVec3> cachedCells;
	}
}
