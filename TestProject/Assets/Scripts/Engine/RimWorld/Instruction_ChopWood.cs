using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Instruction_ChopWood : Lesson_Instruction
	{
		
		// (get) Token: 0x06005E9B RID: 24219 RVA: 0x0020B844 File Offset: 0x00209A44
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
