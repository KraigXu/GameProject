using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F12 RID: 3858
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06005E7E RID: 24190 RVA: 0x0020B1E9 File Offset: 0x002093E9
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06005E7F RID: 24191 RVA: 0x0020B1FC File Offset: 0x002093FC
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
