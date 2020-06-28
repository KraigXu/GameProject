using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C20 RID: 3104
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x06004A02 RID: 18946 RVA: 0x00190935 File Offset: 0x0018EB35
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			if (method == PlayerPawnsArriveMethod.Standing)
			{
				return "PlayerPawnsArriveMethod_Standing".Translate();
			}
			if (method != PlayerPawnsArriveMethod.DropPods)
			{
				throw new NotImplementedException();
			}
			return "PlayerPawnsArriveMethod_DropPods".Translate();
		}
	}
}
