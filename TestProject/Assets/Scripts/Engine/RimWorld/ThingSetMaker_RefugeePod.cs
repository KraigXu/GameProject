using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD5 RID: 3285
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x06004F9D RID: 20381 RVA: 0x001AD334 File Offset: 0x001AB534
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn, true);
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x001AD3B8 File Offset: 0x001AB5B8
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		// Token: 0x04002C9A RID: 11418
		private const float RelationWithColonistWeight = 20f;
	}
}
