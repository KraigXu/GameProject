using System;
using Verse;

namespace RimWorld
{
	
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		
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
