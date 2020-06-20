using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F14 RID: 3860
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06005E89 RID: 24201 RVA: 0x0020B21C File Offset: 0x0020941C
		// (set) Token: 0x06005E8A RID: 24202 RVA: 0x0020B3A5 File Offset: 0x002095A5
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
			set
			{
				Find.TutorialState.roomRect = value;
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06005E8B RID: 24203 RVA: 0x0020B3B4 File Offset: 0x002095B4
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (IEnumerator<IntVec3> enumerator = this.RoomRect.EdgeCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Wall))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06005E8C RID: 24204 RVA: 0x0020B424 File Offset: 0x00209624
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x0020B446 File Offset: 0x00209646
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x0020B464 File Offset: 0x00209664
		public override void LessonUpdate()
		{
			this.cachedEdgeCells.Clear();
			this.cachedEdgeCells.AddRange((from c in this.RoomRect.EdgeCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall)
			select c).ToList<IntVec3>());
			GenDraw.DrawFieldEdges((from c in this.cachedEdgeCells
			where c.GetEdifice(base.Map) == null
			select c).ToList<IntVec3>());
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x0020B4F7 File Offset: 0x002096F7
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Wall")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x04003369 RID: 13161
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();
	}
}
