using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F11 RID: 3857
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06005E7C RID: 24188 RVA: 0x0020B1CF File Offset: 0x002093CF
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
