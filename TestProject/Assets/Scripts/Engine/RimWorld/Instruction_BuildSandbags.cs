using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F15 RID: 3861
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06005E93 RID: 24211 RVA: 0x0020B560 File Offset: 0x00209760
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (List<IntVec3>.Enumerator enumerator = this.sandbagCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Sandbags))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x0020B5D0 File Offset: 0x002097D0
		public override void OnActivated()
		{
			base.OnActivated();
			Find.TutorialState.sandbagsRect = TutorUtility.FindUsableRect(7, 7, base.Map, 0f, false);
			this.sandbagCells = new List<IntVec3>();
			foreach (IntVec3 intVec in Find.TutorialState.sandbagsRect.EdgeCells)
			{
				if (intVec.x != Find.TutorialState.sandbagsRect.CenterCell.x && intVec.z != Find.TutorialState.sandbagsRect.CenterCell.z)
				{
					this.sandbagCells.Add(intVec);
				}
			}
			foreach (IntVec3 c in Find.TutorialState.sandbagsRect.ContractedBy(1))
			{
				if (!Find.TutorialState.sandbagsRect.ContractedBy(2).Contains(c))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing.def.passability != Traversability.Standable && (thing.def.category == ThingCategory.Plant || thing.def.category == ThingCategory.Item))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x0020B764 File Offset: 0x00209964
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x0020B782 File Offset: 0x00209982
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x0020B7A8 File Offset: 0x002099A8
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges((from c in this.sandbagCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags)
			select c).ToList<IntVec3>());
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.sandbagCells), false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x0020B7FE File Offset: 0x002099FE
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Sandbags")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400336A RID: 13162
		private List<IntVec3> sandbagCells;
	}
}
