using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF6 RID: 3062
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x060048D0 RID: 18640 RVA: 0x0018C65C File Offset: 0x0018A85C
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x0018C669 File Offset: 0x0018A869
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0018C676 File Offset: 0x0018A876
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x0018C683 File Offset: 0x0018A883
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0018C690 File Offset: 0x0018A890
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x0018C69D File Offset: 0x0018A89D
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0018C6AA File Offset: 0x0018A8AA
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0018C6B7 File Offset: 0x0018A8B7
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x0018C6C4 File Offset: 0x0018A8C4
		public int GetNextCaravanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCaravanID);
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x0018C6D1 File Offset: 0x0018A8D1
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0018C6DE File Offset: 0x0018A8DE
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0018C6EB File Offset: 0x0018A8EB
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x0018C6F8 File Offset: 0x0018A8F8
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x0018C705 File Offset: 0x0018A905
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x0018C712 File Offset: 0x0018A912
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x0018C71F File Offset: 0x0018A91F
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x0018C72C File Offset: 0x0018A92C
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x0018C739 File Offset: 0x0018A939
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0018C746 File Offset: 0x0018A946
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x0018C753 File Offset: 0x0018A953
		public int GetNextArchivedDialogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextArchivedDialogID);
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x0018C760 File Offset: 0x0018A960
		public int GetNextMessageID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMessageID);
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0018C76D File Offset: 0x0018A96D
		public int GetNextZoneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextZoneID);
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0018C77A File Offset: 0x0018A97A
		public int GetNextQuestID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextQuestID);
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x0018C787 File Offset: 0x0018A987
		public int GetNextGameConditionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGameConditionID);
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x0018C794 File Offset: 0x0018A994
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x0018C7AD File Offset: 0x0018A9AD
		private static int GetNextID(ref int nextID)
		{
			if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Warning("Getting next unique ID during saving or loading. This may cause bugs.", false);
			}
			int result = nextID;
			nextID++;
			if (nextID == 2147483647)
			{
				Log.Warning("Next ID is at max value. Resetting to 0. This may cause bugs.", false);
				nextID = 0;
			}
			return result;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x0018C7EC File Offset: 0x0018A9EC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextThingID, "nextThingID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBillID, "nextBillID", 0, false);
			Scribe_Values.Look<int>(ref this.nextFactionID, "nextFactionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLordID, "nextLordID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTaleID, "nextTaleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPassingShipID, "nextPassingShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldObjectID, "nextWorldObjectID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMapID, "nextMapID", 0, false);
			Scribe_Values.Look<int>(ref this.nextCaravanID, "nextCaravanID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAreaID, "nextAreaID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransporterGroupID, "nextTransporterGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAncientCryptosleepCasketGroupID, "nextAncientCryptosleepCasketGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextJobID, "nextJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextSignalTagID, "nextSignalTagID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldFeatureID, "nextWorldFeatureID", 0, false);
			Scribe_Values.Look<int>(ref this.nextHediffID, "nextHediffID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBattleID, "nextBattleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLogID, "nextLogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLetterID, "nextLetterID", 0, false);
			Scribe_Values.Look<int>(ref this.nextArchivedDialogID, "nextArchivedDialogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMessageID, "nextMessageID", 0, false);
			Scribe_Values.Look<int>(ref this.nextZoneID, "nextZoneID", 0, false);
			Scribe_Values.Look<int>(ref this.nextQuestID, "nextQuestID", 0, false);
			Scribe_Values.Look<int>(ref this.nextGameConditionID, "nextGameConditionID", 0, false);
		}

		// Token: 0x040029AB RID: 10667
		private int nextThingID;

		// Token: 0x040029AC RID: 10668
		private int nextBillID;

		// Token: 0x040029AD RID: 10669
		private int nextFactionID;

		// Token: 0x040029AE RID: 10670
		private int nextLordID;

		// Token: 0x040029AF RID: 10671
		private int nextTaleID;

		// Token: 0x040029B0 RID: 10672
		private int nextPassingShipID;

		// Token: 0x040029B1 RID: 10673
		private int nextWorldObjectID;

		// Token: 0x040029B2 RID: 10674
		private int nextMapID;

		// Token: 0x040029B3 RID: 10675
		private int nextCaravanID;

		// Token: 0x040029B4 RID: 10676
		private int nextAreaID;

		// Token: 0x040029B5 RID: 10677
		private int nextTransporterGroupID;

		// Token: 0x040029B6 RID: 10678
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x040029B7 RID: 10679
		private int nextJobID;

		// Token: 0x040029B8 RID: 10680
		private int nextSignalTagID;

		// Token: 0x040029B9 RID: 10681
		private int nextWorldFeatureID;

		// Token: 0x040029BA RID: 10682
		private int nextHediffID;

		// Token: 0x040029BB RID: 10683
		private int nextBattleID;

		// Token: 0x040029BC RID: 10684
		private int nextLogID;

		// Token: 0x040029BD RID: 10685
		private int nextLetterID;

		// Token: 0x040029BE RID: 10686
		private int nextArchivedDialogID;

		// Token: 0x040029BF RID: 10687
		private int nextMessageID;

		// Token: 0x040029C0 RID: 10688
		private int nextZoneID;

		// Token: 0x040029C1 RID: 10689
		private int nextQuestID;

		// Token: 0x040029C2 RID: 10690
		private int nextGameConditionID;
	}
}
