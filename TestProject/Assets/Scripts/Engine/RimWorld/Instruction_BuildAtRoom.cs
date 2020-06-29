using System;
using Verse;

namespace RimWorld
{
	
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		
		// (get) Token: 0x06005E73 RID: 24179
		protected abstract CellRect BuildableRect { get; }

		
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

		
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
