using System;
using Verse;

namespace RimWorld
{
	
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		
		// (get) Token: 0x06005E7E RID: 24190 RVA: 0x0020B1E9 File Offset: 0x002093E9
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
