using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C12 RID: 3090
	public static class PawnGenerationContextUtility
	{
		// Token: 0x06004994 RID: 18836 RVA: 0x0018F588 File Offset: 0x0018D788
		public static string ToStringHuman(this PawnGenerationContext context)
		{
			switch (context)
			{
			case PawnGenerationContext.All:
				return "PawnGenerationContext_All".Translate();
			case PawnGenerationContext.PlayerStarter:
				return "PawnGenerationContext_PlayerStarter".Translate();
			case PawnGenerationContext.NonPlayer:
				return "PawnGenerationContext_NonPlayer".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0018F5DE File Offset: 0x0018D7DE
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x0018F5EC File Offset: 0x0018D7EC
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x0018F620 File Offset: 0x0018D820
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
