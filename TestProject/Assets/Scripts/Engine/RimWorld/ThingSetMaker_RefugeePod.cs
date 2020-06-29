using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn, true);
		}

		
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		
		private const float RelationWithColonistWeight = 20f;
	}
}
