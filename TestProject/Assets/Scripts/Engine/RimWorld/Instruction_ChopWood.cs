using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F16 RID: 3862
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x170010F8 RID: 4344
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

		// Token: 0x06005E9C RID: 24220 RVA: 0x0020B89D File Offset: 0x00209A9D
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
