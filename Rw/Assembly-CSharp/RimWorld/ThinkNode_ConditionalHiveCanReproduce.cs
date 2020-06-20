using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F2 RID: 2034
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x060033DA RID: 13274 RVA: 0x0011E010 File Offset: 0x0011C210
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
