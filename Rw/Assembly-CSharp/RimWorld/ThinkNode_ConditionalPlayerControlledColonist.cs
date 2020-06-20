using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CF RID: 1999
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x0600338F RID: 13199 RVA: 0x0011D9E8 File Offset: 0x0011BBE8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
