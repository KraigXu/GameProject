using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B3F RID: 2879
	public static class ResurrectionUtility
	{
		// Token: 0x060043B5 RID: 17333 RVA: 0x0016C7C0 File Offset: 0x0016A9C0
		public static void Resurrect(Pawn pawn)
		{
			if (!pawn.Dead)
			{
				Log.Error("Tried to resurrect a pawn who is not dead: " + pawn.ToStringSafe<Pawn>(), false);
				return;
			}
			if (pawn.Discarded)
			{
				Log.Error("Tried to resurrect a discarded pawn: " + pawn.ToStringSafe<Pawn>(), false);
				return;
			}
			Corpse corpse = pawn.Corpse;
			bool flag = false;
			IntVec3 loc = IntVec3.Invalid;
			Map map = null;
			if (corpse != null)
			{
				flag = corpse.Spawned;
				loc = corpse.Position;
				map = corpse.Map;
				corpse.InnerPawn = null;
				corpse.Destroy(DestroyMode.Vanish);
			}
			if (flag && pawn.IsWorldPawn())
			{
				Find.WorldPawns.RemovePawn(pawn);
			}
			pawn.ForceSetStateToUnspawned();
			PawnComponentsUtility.CreateInitialComponents(pawn);
			pawn.health.Notify_Resurrected();
			if (pawn.Faction != null && pawn.Faction.IsPlayer)
			{
				if (pawn.workSettings != null)
				{
					pawn.workSettings.EnableAndInitialize();
				}
				Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(pawn, PopAdaptationEvent.GainedColonist);
			}
			if (flag)
			{
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				for (int i = 0; i < 10; i++)
				{
					MoteMaker.ThrowAirPuffUp(pawn.DrawPos, map);
				}
				if (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.HostileTo(Faction.OfPlayer))
				{
					LordMaker.MakeNewLord(pawn.Faction, new LordJob_AssaultColony(pawn.Faction, true, true, false, false, true), pawn.Map, Gen.YieldSingle<Pawn>(pawn));
				}
				if (pawn.apparel != null)
				{
					List<Apparel> wornApparel = pawn.apparel.WornApparel;
					for (int j = 0; j < wornApparel.Count; j++)
					{
						wornApparel[j].Notify_PawnResurrected();
					}
				}
			}
			PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(pawn);
			if (pawn.royalty != null)
			{
				pawn.royalty.Notify_Resurrected();
			}
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x0016C974 File Offset: 0x0016AB74
		public static void ResurrectWithSideEffects(Pawn pawn)
		{
			Corpse corpse = pawn.Corpse;
			float x2;
			if (corpse != null)
			{
				x2 = corpse.GetComp<CompRottable>().RotProgress / 60000f;
			}
			else
			{
				x2 = 0f;
			}
			ResurrectionUtility.Resurrect(pawn);
			BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ResurrectionSickness, pawn, null);
			if (!pawn.health.WouldDieAfterAddingHediff(hediff))
			{
				pawn.health.AddHediff(hediff, null, null, null);
			}
			if (Rand.Chance(ResurrectionUtility.DementiaChancePerRotDaysCurve.Evaluate(x2)) && brain != null)
			{
				Hediff hediff2 = HediffMaker.MakeHediff(HediffDefOf.Dementia, pawn, brain);
				if (!pawn.health.WouldDieAfterAddingHediff(hediff2))
				{
					pawn.health.AddHediff(hediff2, null, null, null);
				}
			}
			if (Rand.Chance(ResurrectionUtility.BlindnessChancePerRotDaysCurve.Evaluate(x2)))
			{
				foreach (BodyPartRecord bodyPartRecord in from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where x.def == BodyPartDefOf.Eye
				select x)
				{
					if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord))
					{
						Hediff hediff3 = HediffMaker.MakeHediff(HediffDefOf.Blindness, pawn, bodyPartRecord);
						pawn.health.AddHediff(hediff3, null, null, null);
					}
				}
			}
			if (brain != null && Rand.Chance(ResurrectionUtility.ResurrectionPsychosisChancePerRotDaysCurve.Evaluate(x2)))
			{
				Hediff hediff4 = HediffMaker.MakeHediff(HediffDefOf.ResurrectionPsychosis, pawn, brain);
				if (!pawn.health.WouldDieAfterAddingHediff(hediff4))
				{
					pawn.health.AddHediff(hediff4, null, null, null);
				}
			}
			if (pawn.Dead)
			{
				Log.Error("The pawn has died while being resurrected.", false);
				ResurrectionUtility.Resurrect(pawn);
			}
		}

		// Token: 0x040026C1 RID: 9921
		private static SimpleCurve DementiaChancePerRotDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.8f),
				true
			}
		};

		// Token: 0x040026C2 RID: 9922
		private static SimpleCurve BlindnessChancePerRotDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.8f),
				true
			}
		};

		// Token: 0x040026C3 RID: 9923
		private static SimpleCurve ResurrectionPsychosisChancePerRotDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.02f),
				true
			},
			{
				new CurvePoint(5f, 0.8f),
				true
			}
		};
	}
}
