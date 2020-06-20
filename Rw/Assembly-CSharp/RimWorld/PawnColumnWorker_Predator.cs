using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDE RID: 3806
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x06005D46 RID: 23878 RVA: 0x0020495B File Offset: 0x00202B5B
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn.RaceProps.predator)
			{
				return PawnColumnWorker_Predator.Icon;
			}
			return null;
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x00204971 File Offset: 0x00202B71
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x040032C2 RID: 12994
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
