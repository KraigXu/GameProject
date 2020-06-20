using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public abstract class GatheringWorker
	{
		// Token: 0x06003610 RID: 13840 RVA: 0x001258A4 File Offset: 0x00123AA4
		public virtual bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec) && GatheringsUtility.PawnCanStartOrContinueGathering(organizer);
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x001258DC File Offset: 0x00123ADC
		public virtual bool TryExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			if (organizer == null)
			{
				return false;
			}
			IntVec3 spot;
			if (!this.TryFindGatherSpot(organizer, out spot))
			{
				return false;
			}
			LordJob lordJob = this.CreateLordJob(spot, organizer);
			Faction faction = organizer.Faction;
			LordJob lordJob2 = lordJob;
			Map map2 = organizer.Map;
			object startingPawns;
			if (!lordJob.OrganizerIsStartingPawn)
			{
				startingPawns = null;
			}
			else
			{
				(startingPawns = new Pawn[1])[0] = organizer;
			}
			LordMaker.MakeNewLord(faction, lordJob2, map2, startingPawns);
			this.SendLetter(spot, organizer);
			return true;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00125944 File Offset: 0x00123B44
		protected virtual void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x001259A1 File Offset: 0x00123BA1
		protected virtual Pawn FindOrganizer(Map map)
		{
			return GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, map, this.def);
		}

		// Token: 0x06003614 RID: 13844
		protected abstract bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot);

		// Token: 0x06003615 RID: 13845
		protected abstract LordJob CreateLordJob(IntVec3 spot, Pawn organizer);

		// Token: 0x04001E4B RID: 7755
		public GatheringDef def;
	}
}
