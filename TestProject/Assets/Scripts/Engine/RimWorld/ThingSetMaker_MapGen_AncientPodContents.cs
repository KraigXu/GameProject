using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThingSetMaker_MapGen_AncientPodContents : ThingSetMaker
	{
		
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PodContentsType podContentsType = parms.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(true);
			switch (podContentsType)
			{
			case PodContentsType.Empty:
				break;
			case PodContentsType.AncientFriendly:
				outThings.Add(this.GenerateFriendlyAncient());
				return;
			case PodContentsType.AncientIncapped:
				outThings.Add(this.GenerateIncappedAncient());
				return;
			case PodContentsType.AncientHalfEaten:
				outThings.Add(this.GenerateHalfEatenAncient());
				outThings.AddRange(this.GenerateScarabs());
				return;
			case PodContentsType.AncientHostile:
				outThings.Add(this.GenerateAngryAncient());
				return;
			case PodContentsType.Slave:
				outThings.Add(this.GenerateSlave());
				return;
			default:
				Log.Error("Pod contents type not handled: " + podContentsType, false);
				break;
			}
		}

		
		private Pawn GenerateFriendlyAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		
		private Pawn GenerateIncappedAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, true);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		
		private Pawn GenerateSlave()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, true);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			if (Rand.Value < 0.5f)
			{
				HealthUtility.DamageUntilDead(pawn);
			}
			return pawn;
		}

		
		private Pawn GenerateAngryAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		
		private Pawn GenerateHalfEatenAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			int num = Rand.Range(6, 10);
			for (int i = 0; i < num; i++)
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, (float)Rand.Range(3, 8), 0f, -1f, pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		
		private List<Thing> GenerateScarabs()
		{
			List<Thing> list = new List<Thing>();
			int num = Rand.Range(3, 6);
			for (int i = 0; i < num; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megascarab, null);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
				list.Add(pawn);
			}
			return list;
		}

		
		private void GiveRandomLootInventoryForTombPawn(Pawn p)
		{
			if (Rand.Value < 0.65f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Gold, Rand.Range(10, 50));
			}
			else
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Plasteel, Rand.Range(10, 50));
			}
			if (Rand.Value < 0.7f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentIndustrial, Rand.Range(-2, 4));
				return;
			}
			this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentSpacer, Rand.Range(-2, 4));
		}

		
		private void MakeIntoContainer(ThingOwner container, ThingDef def, int count)
		{
			if (count <= 0)
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(def, null);
			thing.stackCount = count;
			container.TryAdd(thing, true);
		}

		
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.AncientSoldier.race;
			yield return PawnKindDefOf.Slave.race;
			yield return PawnKindDefOf.Megascarab.race;
			yield return ThingDefOf.Gold;
			yield return ThingDefOf.Plasteel;
			yield return ThingDefOf.ComponentIndustrial;
			yield return ThingDefOf.ComponentSpacer;
			yield break;
		}
	}
}
