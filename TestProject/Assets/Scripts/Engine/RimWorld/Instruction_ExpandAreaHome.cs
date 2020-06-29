using System;
using Verse;

namespace RimWorld
{
	
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		
		// (get) Token: 0x06005EB0 RID: 24240 RVA: 0x0020BD70 File Offset: 0x00209F70
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
