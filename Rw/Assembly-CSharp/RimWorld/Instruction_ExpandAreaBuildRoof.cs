using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1B RID: 3867
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x0020BD8A File Offset: 0x00209F8A
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
