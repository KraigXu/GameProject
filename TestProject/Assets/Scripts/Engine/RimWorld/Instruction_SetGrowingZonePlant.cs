using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		
		// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x0020C2F4 File Offset: 0x0020A4F4
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
