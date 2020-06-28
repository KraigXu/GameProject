using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000771 RID: 1905
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x060031C8 RID: 12744 RVA: 0x00115606 File Offset: 0x00113806
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
