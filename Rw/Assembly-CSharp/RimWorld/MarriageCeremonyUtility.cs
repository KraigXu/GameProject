using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200077F RID: 1919
	public static class MarriageCeremonyUtility
	{
		// Token: 0x0600322C RID: 12844 RVA: 0x001179EC File Offset: 0x00115BEC
		public static bool AcceptableGameConditionsToStartCeremony(Map map)
		{
			if (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) < 5 || GenLocalDate.HourInteger(map) > 16)
			{
				return false;
			}
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				return false;
			}
			if (map.dangerWatcher.DangerRating != StoryDanger.None)
			{
				return false;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Drafted)
					{
						num++;
					}
				}
			}
			return (float)num / (float)map.mapPawns.FreeColonistsSpawnedCount < 0.5f;
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x00117A9C File Offset: 0x00115C9C
		public static bool FianceReadyToStartCeremony(Pawn pawn, Pawn otherPawn)
		{
			return MarriageCeremonyUtility.FianceCanContinueCeremony(pawn, otherPawn) && pawn.health.hediffSet.BleedRateTotal <= 0f && !HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn) && !MarriageCeremonyUtility.IsCurrentlyMarryingSomeone(pawn) && pawn.GetLord() == null && (!pawn.Drafted && !pawn.InMentalState && pawn.Awake() && !pawn.IsBurning()) && !pawn.InBed();
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x00117B20 File Offset: 0x00115D20
		public static bool FianceCanContinueCeremony(Pawn pawn, Pawn otherPawn)
		{
			return GatheringsUtility.PawnCanStartOrContinueGathering(pawn) && !pawn.HostileTo(otherPawn) && (pawn.Spawned && !pawn.Downed) && !pawn.InMentalState;
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x00117B52 File Offset: 0x00115D52
		public static bool ShouldGuestKeepAttendingCeremony(Pawn p)
		{
			return GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x00117B5C File Offset: 0x00115D5C
		public static void Married(Pawn firstPawn, Pawn secondPawn)
		{
			LovePartnerRelationUtility.ChangeSpouseRelationsToExSpouse(firstPawn);
			LovePartnerRelationUtility.ChangeSpouseRelationsToExSpouse(secondPawn);
			firstPawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, secondPawn);
			firstPawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.ExSpouse, secondPawn);
			firstPawn.relations.AddDirectRelation(PawnRelationDefOf.Spouse, secondPawn);
			MarriageCeremonyUtility.AddNewlyMarriedThoughts(firstPawn, secondPawn);
			MarriageCeremonyUtility.AddNewlyMarriedThoughts(secondPawn, firstPawn);
			if (firstPawn.needs.mood != null)
			{
				firstPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.DivorcedMe, secondPawn);
			}
			if (secondPawn.needs.mood != null)
			{
				secondPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.DivorcedMe, firstPawn);
			}
			if (firstPawn.relations.nextMarriageNameChange != secondPawn.relations.nextMarriageNameChange)
			{
				Log.Warning("Marriage name change is different on marrying pawns. This is weird, but not harmful.", false);
			}
			SpouseRelationUtility.ChangeNameAfterMarriage(firstPawn, secondPawn, firstPawn.relations.nextMarriageNameChange);
			LovePartnerRelationUtility.TryToShareBed(firstPawn, secondPawn);
			TaleRecorder.RecordTale(TaleDefOf.Marriage, new object[]
			{
				firstPawn,
				secondPawn
			});
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x00117C68 File Offset: 0x00115E68
		private static void AddNewlyMarriedThoughts(Pawn pawn, Pawn otherPawn)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GotMarried, otherPawn);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HoneymoonPhase, otherPawn);
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x00117CC4 File Offset: 0x00115EC4
		private static bool IsCurrentlyMarryingSomeone(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Lord> lords = p.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_Joinable_MarriageCeremony lordJob_Joinable_MarriageCeremony = lords[i].LordJob as LordJob_Joinable_MarriageCeremony;
				if (lordJob_Joinable_MarriageCeremony != null && (lordJob_Joinable_MarriageCeremony.firstPawn == p || lordJob_Joinable_MarriageCeremony.secondPawn == p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
