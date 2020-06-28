using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F21 RID: 3873
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x0020C2F4 File Offset: 0x0020A4F4
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x06005ED3 RID: 24275 RVA: 0x0020C32F File Offset: 0x0020A52F
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06005ED4 RID: 24276 RVA: 0x0020C357 File Offset: 0x0020A557
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
