using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F13 RID: 3859
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06005E81 RID: 24193 RVA: 0x0020B21C File Offset: 0x0020941C
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06005E82 RID: 24194 RVA: 0x0020B228 File Offset: 0x00209428
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x0020B26C File Offset: 0x0020946C
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005E84 RID: 24196 RVA: 0x0020B28C File Offset: 0x0020948C
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x06005E85 RID: 24197 RVA: 0x0020B2AD File Offset: 0x002094AD
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06005E86 RID: 24198 RVA: 0x0020B2DB File Offset: 0x002094DB
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04003368 RID: 13160
		private List<IntVec3> allowedPlaceCells;
	}
}
