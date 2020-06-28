using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E33 RID: 3635
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x060057DB RID: 22491 RVA: 0x001D2650 File Offset: 0x001D0850
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageSlaughteringBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
