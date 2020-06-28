using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F23 RID: 3875
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06005EDA RID: 24282 RVA: 0x0020C434 File Offset: 0x0020A634
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count<Thing>() / (float)Find.TutorialState.startingItems.Count;
			}
		}

		// Token: 0x06005EDB RID: 24283 RVA: 0x0020C486 File Offset: 0x0020A686
		private IEnumerable<Thing> NeedUnforbidItems()
		{
			return from it in Find.TutorialState.startingItems
			where it.IsForbidden(Faction.OfPlayer) && !it.Destroyed
			select it;
		}

		// Token: 0x06005EDC RID: 24284 RVA: 0x0020C4B6 File Offset: 0x0020A6B6
		public override void PostDeactivated()
		{
			base.PostDeactivated();
			Find.TutorialState.startingItems.RemoveAll((Thing it) => !Instruction_EquipWeapons.IsWeapon(it));
		}

		// Token: 0x06005EDD RID: 24285 RVA: 0x0020C4F0 File Offset: 0x0020A6F0
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06005EDE RID: 24286 RVA: 0x0020C54C File Offset: 0x0020A74C
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
			foreach (Thing thing in this.NeedUnforbidItems())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
		}
	}
}
