using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5C RID: 3932
	[DefOf]
	public static class JobDefOf
	{
		// Token: 0x06006063 RID: 24675 RVA: 0x00216DF3 File Offset: 0x00214FF3
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}

		// Token: 0x04003627 RID: 13863
		public static JobDef Goto;

		// Token: 0x04003628 RID: 13864
		public static JobDef Wait;

		// Token: 0x04003629 RID: 13865
		public static JobDef Wait_MaintainPosture;

		// Token: 0x0400362A RID: 13866
		public static JobDef Wait_Downed;

		// Token: 0x0400362B RID: 13867
		public static JobDef GotoWander;

		// Token: 0x0400362C RID: 13868
		public static JobDef Wait_Wander;

		// Token: 0x0400362D RID: 13869
		public static JobDef GotoSafeTemperature;

		// Token: 0x0400362E RID: 13870
		public static JobDef Wait_SafeTemperature;

		// Token: 0x0400362F RID: 13871
		public static JobDef Wait_Combat;

		// Token: 0x04003630 RID: 13872
		public static JobDef Equip;

		// Token: 0x04003631 RID: 13873
		public static JobDef AttackMelee;

		// Token: 0x04003632 RID: 13874
		public static JobDef AttackStatic;

		// Token: 0x04003633 RID: 13875
		public static JobDef UseVerbOnThing;

		// Token: 0x04003634 RID: 13876
		public static JobDef CastAbilityOnThing;

		// Token: 0x04003635 RID: 13877
		public static JobDef TakeInventory;

		// Token: 0x04003636 RID: 13878
		public static JobDef Follow;

		// Token: 0x04003637 RID: 13879
		public static JobDef FollowClose;

		// Token: 0x04003638 RID: 13880
		public static JobDef Wear;

		// Token: 0x04003639 RID: 13881
		public static JobDef RemoveApparel;

		// Token: 0x0400363A RID: 13882
		public static JobDef DropEquipment;

		// Token: 0x0400363B RID: 13883
		public static JobDef Strip;

		// Token: 0x0400363C RID: 13884
		public static JobDef Open;

		// Token: 0x0400363D RID: 13885
		public static JobDef Hunt;

		// Token: 0x0400363E RID: 13886
		public static JobDef ManTurret;

		// Token: 0x0400363F RID: 13887
		public static JobDef EnterCryptosleepCasket;

		// Token: 0x04003640 RID: 13888
		public static JobDef UseNeurotrainer;

		// Token: 0x04003641 RID: 13889
		public static JobDef UseArtifact;

		// Token: 0x04003642 RID: 13890
		public static JobDef TriggerFirefoamPopper;

		// Token: 0x04003643 RID: 13891
		public static JobDef ClearSnow;

		// Token: 0x04003644 RID: 13892
		public static JobDef Vomit;

		// Token: 0x04003645 RID: 13893
		public static JobDef Flick;

		// Token: 0x04003646 RID: 13894
		public static JobDef DoBill;

		// Token: 0x04003647 RID: 13895
		public static JobDef Research;

		// Token: 0x04003648 RID: 13896
		public static JobDef Mine;

		// Token: 0x04003649 RID: 13897
		public static JobDef OperateDeepDrill;

		// Token: 0x0400364A RID: 13898
		public static JobDef OperateScanner;

		// Token: 0x0400364B RID: 13899
		public static JobDef Repair;

		// Token: 0x0400364C RID: 13900
		public static JobDef FixBrokenDownBuilding;

		// Token: 0x0400364D RID: 13901
		public static JobDef UseCommsConsole;

		// Token: 0x0400364E RID: 13902
		public static JobDef Clean;

		// Token: 0x0400364F RID: 13903
		public static JobDef TradeWithPawn;

		// Token: 0x04003650 RID: 13904
		public static JobDef Flee;

		// Token: 0x04003651 RID: 13905
		public static JobDef FleeAndCower;

		// Token: 0x04003652 RID: 13906
		public static JobDef Lovin;

		// Token: 0x04003653 RID: 13907
		public static JobDef SocialFight;

		// Token: 0x04003654 RID: 13908
		public static JobDef Maintain;

		// Token: 0x04003655 RID: 13909
		public static JobDef GiveToPackAnimal;

		// Token: 0x04003656 RID: 13910
		public static JobDef EnterTransporter;

		// Token: 0x04003657 RID: 13911
		public static JobDef Resurrect;

		// Token: 0x04003658 RID: 13912
		public static JobDef Insult;

		// Token: 0x04003659 RID: 13913
		public static JobDef HaulCorpseToPublicPlace;

		// Token: 0x0400365A RID: 13914
		public static JobDef InducePrisonerToEscape;

		// Token: 0x0400365B RID: 13915
		public static JobDef OfferHelp;

		// Token: 0x0400365C RID: 13916
		public static JobDef ApplyTechprint;

		// Token: 0x0400365D RID: 13917
		public static JobDef GotoMindControlled;

		// Token: 0x0400365E RID: 13918
		public static JobDef MarryAdjacentPawn;

		// Token: 0x0400365F RID: 13919
		public static JobDef SpectateCeremony;

		// Token: 0x04003660 RID: 13920
		public static JobDef StandAndBeSociallyActive;

		// Token: 0x04003661 RID: 13921
		public static JobDef GiveSpeech;

		// Token: 0x04003662 RID: 13922
		public static JobDef PrepareCaravan_GatherItems;

		// Token: 0x04003663 RID: 13923
		public static JobDef PrepareCaravan_GatherPawns;

		// Token: 0x04003664 RID: 13924
		public static JobDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x04003665 RID: 13925
		public static JobDef Ignite;

		// Token: 0x04003666 RID: 13926
		public static JobDef BeatFire;

		// Token: 0x04003667 RID: 13927
		public static JobDef ExtinguishSelf;

		// Token: 0x04003668 RID: 13928
		public static JobDef LayDown;

		// Token: 0x04003669 RID: 13929
		public static JobDef Ingest;

		// Token: 0x0400366A RID: 13930
		public static JobDef SocialRelax;

		// Token: 0x0400366B RID: 13931
		public static JobDef HaulToCell;

		// Token: 0x0400366C RID: 13932
		public static JobDef HaulToContainer;

		// Token: 0x0400366D RID: 13933
		public static JobDef Steal;

		// Token: 0x0400366E RID: 13934
		public static JobDef Refuel;

		// Token: 0x0400366F RID: 13935
		public static JobDef RefuelAtomic;

		// Token: 0x04003670 RID: 13936
		public static JobDef RearmTurret;

		// Token: 0x04003671 RID: 13937
		public static JobDef RearmTurretAtomic;

		// Token: 0x04003672 RID: 13938
		public static JobDef FillFermentingBarrel;

		// Token: 0x04003673 RID: 13939
		public static JobDef TakeBeerOutOfFermentingBarrel;

		// Token: 0x04003674 RID: 13940
		public static JobDef UnloadInventory;

		// Token: 0x04003675 RID: 13941
		public static JobDef UnloadYourInventory;

		// Token: 0x04003676 RID: 13942
		public static JobDef HaulToTransporter;

		// Token: 0x04003677 RID: 13943
		public static JobDef Rescue;

		// Token: 0x04003678 RID: 13944
		public static JobDef Arrest;

		// Token: 0x04003679 RID: 13945
		public static JobDef Capture;

		// Token: 0x0400367A RID: 13946
		public static JobDef TakeWoundedPrisonerToBed;

		// Token: 0x0400367B RID: 13947
		public static JobDef TakeToBedToOperate;

		// Token: 0x0400367C RID: 13948
		public static JobDef EscortPrisonerToBed;

		// Token: 0x0400367D RID: 13949
		public static JobDef CarryToCryptosleepCasket;

		// Token: 0x0400367E RID: 13950
		public static JobDef ReleasePrisoner;

		// Token: 0x0400367F RID: 13951
		public static JobDef Kidnap;

		// Token: 0x04003680 RID: 13952
		public static JobDef CarryDownedPawnToExit;

		// Token: 0x04003681 RID: 13953
		public static JobDef PlaceNoCostFrame;

		// Token: 0x04003682 RID: 13954
		public static JobDef FinishFrame;

		// Token: 0x04003683 RID: 13955
		public static JobDef Deconstruct;

		// Token: 0x04003684 RID: 13956
		public static JobDef Uninstall;

		// Token: 0x04003685 RID: 13957
		public static JobDef SmoothFloor;

		// Token: 0x04003686 RID: 13958
		public static JobDef RemoveFloor;

		// Token: 0x04003687 RID: 13959
		public static JobDef BuildRoof;

		// Token: 0x04003688 RID: 13960
		public static JobDef RemoveRoof;

		// Token: 0x04003689 RID: 13961
		public static JobDef SmoothWall;

		// Token: 0x0400368A RID: 13962
		public static JobDef PrisonerAttemptRecruit;

		// Token: 0x0400368B RID: 13963
		public static JobDef PrisonerExecution;

		// Token: 0x0400368C RID: 13964
		public static JobDef DeliverFood;

		// Token: 0x0400368D RID: 13965
		public static JobDef FeedPatient;

		// Token: 0x0400368E RID: 13966
		public static JobDef TendPatient;

		// Token: 0x0400368F RID: 13967
		public static JobDef VisitSickPawn;

		// Token: 0x04003690 RID: 13968
		public static JobDef Sow;

		// Token: 0x04003691 RID: 13969
		public static JobDef Harvest;

		// Token: 0x04003692 RID: 13970
		public static JobDef CutPlant;

		// Token: 0x04003693 RID: 13971
		public static JobDef HarvestDesignated;

		// Token: 0x04003694 RID: 13972
		public static JobDef CutPlantDesignated;

		// Token: 0x04003695 RID: 13973
		public static JobDef Slaughter;

		// Token: 0x04003696 RID: 13974
		public static JobDef Milk;

		// Token: 0x04003697 RID: 13975
		public static JobDef Shear;

		// Token: 0x04003698 RID: 13976
		public static JobDef Tame;

		// Token: 0x04003699 RID: 13977
		public static JobDef Train;

		// Token: 0x0400369A RID: 13978
		public static JobDef Nuzzle;

		// Token: 0x0400369B RID: 13979
		public static JobDef Mate;

		// Token: 0x0400369C RID: 13980
		public static JobDef LayEgg;

		// Token: 0x0400369D RID: 13981
		public static JobDef PredatorHunt;

		// Token: 0x0400369E RID: 13982
		[MayRequireRoyalty]
		public static JobDef Reign;

		// Token: 0x0400369F RID: 13983
		[MayRequireRoyalty]
		public static JobDef Meditate;

		// Token: 0x040036A0 RID: 13984
		[MayRequireRoyalty]
		public static JobDef Play_MusicalInstrument;

		// Token: 0x040036A1 RID: 13985
		[MayRequireRoyalty]
		public static JobDef LinkPsylinkable;
	}
}
