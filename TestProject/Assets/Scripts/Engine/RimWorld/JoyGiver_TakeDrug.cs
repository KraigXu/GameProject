using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006FF RID: 1791
	public class JoyGiver_TakeDrug : JoyGiver_Ingest
	{
		// Token: 0x06002F5F RID: 12127 RVA: 0x0010A860 File Offset: 0x00108A60
		protected override Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			if (pawn.drugs == null)
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing t) => this.CanIngestForJoy(pawn, t) && (extraValidator == null || extraValidator(t));
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (predicate(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			bool flag = false;
			if (pawn.story != null && (pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || pawn.InMentalState))
			{
				flag = true;
			}
			JoyGiver_TakeDrug.takeableDrugs.Clear();
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int j = 0; j < currentPolicy.Count; j++)
			{
				if (flag || currentPolicy[j].allowedForJoy)
				{
					JoyGiver_TakeDrug.takeableDrugs.Add(currentPolicy[j].drug);
				}
			}
			JoyGiver_TakeDrug.takeableDrugs.Shuffle<ThingDef>();
			for (int k = 0; k < JoyGiver_TakeDrug.takeableDrugs.Count; k++)
			{
				List<Thing> list = pawn.Map.listerThings.ThingsOfDef(JoyGiver_TakeDrug.takeableDrugs[k]);
				if (list.Count > 0)
				{
					Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, list, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, predicate, null);
					if (thing != null)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x0010AA08 File Offset: 0x00108C08
		public override float GetChance(Pawn pawn)
		{
			int num = 0;
			if (pawn.story != null)
			{
				num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			}
			if (num < 0)
			{
				return 0f;
			}
			float num2 = this.def.baseChance;
			if (num == 1)
			{
				num2 *= 2f;
			}
			if (num == 2)
			{
				num2 *= 5f;
			}
			return num2;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x0010AA64 File Offset: 0x00108C64
		protected override Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return DrugAIUtility.IngestAndTakeToInventoryJob(ingestible, pawn, 9999);
		}

		// Token: 0x04001ABC RID: 6844
		private static List<ThingDef> takeableDrugs = new List<ThingDef>();
	}
}
