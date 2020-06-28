using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C6 RID: 2246
	public class GatheringWorker_MarriageCeremony : GatheringWorker
	{
		// Token: 0x06003617 RID: 13847 RVA: 0x001259B4 File Offset: 0x00123BB4
		private static void FindFiancees(Pawn organizer, out Pawn firstFiance, out Pawn secondFiance)
		{
			firstFiance = organizer;
			secondFiance = organizer.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x001259CC File Offset: 0x00123BCC
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			Pawn firstPawn;
			Pawn secondPawn;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstPawn, out secondPawn);
			return new LordJob_Joinable_MarriageCeremony(firstPawn, secondPawn, spot);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x001259EC File Offset: 0x00123BEC
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Pawn firstFiance;
			Pawn secondFiance;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstFiance, out secondFiance);
			return RCellFinder.TryFindMarriageSite(firstFiance, secondFiance, out spot);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x00125A0C File Offset: 0x00123C0C
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Pawn pawn;
			Pawn pawn2;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
			Messages.Message("MessageNewMarriageCeremony".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("PAWN1"), pawn2.Named("PAWN2")), new TargetInfo(spot, pawn.Map, false), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x00125A7C File Offset: 0x00123C7C
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer != null)
			{
				Pawn pawn;
				Pawn pawn2;
				GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
				if (!GatheringsUtility.PawnCanStartOrContinueGathering(pawn) || !GatheringsUtility.PawnCanStartOrContinueGathering(pawn2))
				{
					return false;
				}
			}
			return base.CanExecute(map, organizer);
		}
	}
}
