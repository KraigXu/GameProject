using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F51 RID: 3921
	[DefOf]
	public static class SoundDefOf
	{
		// Token: 0x06006059 RID: 24665 RVA: 0x00216D49 File Offset: 0x00214F49
		static SoundDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
		}

		// Token: 0x0400352A RID: 13610
		public static SoundDef Tick_High;

		// Token: 0x0400352B RID: 13611
		public static SoundDef Tick_Low;

		// Token: 0x0400352C RID: 13612
		public static SoundDef Tick_Tiny;

		// Token: 0x0400352D RID: 13613
		public static SoundDef Crunch;

		// Token: 0x0400352E RID: 13614
		public static SoundDef Click;

		// Token: 0x0400352F RID: 13615
		public static SoundDef ClickReject;

		// Token: 0x04003530 RID: 13616
		public static SoundDef CancelMode;

		// Token: 0x04003531 RID: 13617
		public static SoundDef TabClose;

		// Token: 0x04003532 RID: 13618
		public static SoundDef TabOpen;

		// Token: 0x04003533 RID: 13619
		public static SoundDef Checkbox_TurnedOff;

		// Token: 0x04003534 RID: 13620
		public static SoundDef Checkbox_TurnedOn;

		// Token: 0x04003535 RID: 13621
		public static SoundDef RowTabSelect;

		// Token: 0x04003536 RID: 13622
		public static SoundDef ArchitectCategorySelect;

		// Token: 0x04003537 RID: 13623
		public static SoundDef ExecuteTrade;

		// Token: 0x04003538 RID: 13624
		public static SoundDef FloatMenu_Open;

		// Token: 0x04003539 RID: 13625
		public static SoundDef FloatMenu_Cancel;

		// Token: 0x0400353A RID: 13626
		public static SoundDef DialogBoxAppear;

		// Token: 0x0400353B RID: 13627
		public static SoundDef TutorMessageAppear;

		// Token: 0x0400353C RID: 13628
		public static SoundDef TinyBell;

		// Token: 0x0400353D RID: 13629
		public static SoundDef PageChange;

		// Token: 0x0400353E RID: 13630
		public static SoundDef DragSlider;

		// Token: 0x0400353F RID: 13631
		public static SoundDef Lesson_Activated;

		// Token: 0x04003540 RID: 13632
		public static SoundDef Lesson_Deactivated;

		// Token: 0x04003541 RID: 13633
		public static SoundDef DraftOn;

		// Token: 0x04003542 RID: 13634
		public static SoundDef DraftOff;

		// Token: 0x04003543 RID: 13635
		public static SoundDef CommsWindow_Open;

		// Token: 0x04003544 RID: 13636
		public static SoundDef CommsWindow_Close;

		// Token: 0x04003545 RID: 13637
		public static SoundDef RadioComms_Ambience;

		// Token: 0x04003546 RID: 13638
		public static SoundDef InfoCard_Open;

		// Token: 0x04003547 RID: 13639
		public static SoundDef InfoCard_Close;

		// Token: 0x04003548 RID: 13640
		public static SoundDef Clock_Stop;

		// Token: 0x04003549 RID: 13641
		public static SoundDef Clock_Normal;

		// Token: 0x0400354A RID: 13642
		public static SoundDef Clock_Fast;

		// Token: 0x0400354B RID: 13643
		public static SoundDef Clock_Superfast;

		// Token: 0x0400354C RID: 13644
		public static SoundDef Quest_Accepted;

		// Token: 0x0400354D RID: 13645
		public static SoundDef Quest_Succeded;

		// Token: 0x0400354E RID: 13646
		public static SoundDef Quest_Concluded;

		// Token: 0x0400354F RID: 13647
		public static SoundDef Quest_Failed;

		// Token: 0x04003550 RID: 13648
		public static SoundDef Mouseover_Standard;

		// Token: 0x04003551 RID: 13649
		public static SoundDef Mouseover_Thump;

		// Token: 0x04003552 RID: 13650
		public static SoundDef Mouseover_Category;

		// Token: 0x04003553 RID: 13651
		public static SoundDef Mouseover_Command;

		// Token: 0x04003554 RID: 13652
		public static SoundDef Mouseover_ButtonToggle;

		// Token: 0x04003555 RID: 13653
		public static SoundDef Mouseover_Tab;

		// Token: 0x04003556 RID: 13654
		public static SoundDef ThingSelected;

		// Token: 0x04003557 RID: 13655
		public static SoundDef MapSelected;

		// Token: 0x04003558 RID: 13656
		public static SoundDef ColonistSelected;

		// Token: 0x04003559 RID: 13657
		public static SoundDef ColonistOrdered;

		// Token: 0x0400355A RID: 13658
		public static SoundDef LetterArrive_BadUrgent;

		// Token: 0x0400355B RID: 13659
		public static SoundDef LetterArrive;

		// Token: 0x0400355C RID: 13660
		public static SoundDef Designate_DragStandard;

		// Token: 0x0400355D RID: 13661
		public static SoundDef Designate_DragStandard_Changed;

		// Token: 0x0400355E RID: 13662
		public static SoundDef Designate_DragBuilding;

		// Token: 0x0400355F RID: 13663
		public static SoundDef Designate_DragAreaAdd;

		// Token: 0x04003560 RID: 13664
		public static SoundDef Designate_DragAreaDelete;

		// Token: 0x04003561 RID: 13665
		public static SoundDef Designate_Failed;

		// Token: 0x04003562 RID: 13666
		public static SoundDef Designate_ZoneAdd;

		// Token: 0x04003563 RID: 13667
		public static SoundDef Designate_ZoneDelete;

		// Token: 0x04003564 RID: 13668
		public static SoundDef Designate_Cancel;

		// Token: 0x04003565 RID: 13669
		public static SoundDef Designate_Haul;

		// Token: 0x04003566 RID: 13670
		public static SoundDef Designate_Mine;

		// Token: 0x04003567 RID: 13671
		public static SoundDef Designate_SmoothSurface;

		// Token: 0x04003568 RID: 13672
		public static SoundDef Designate_PlanRemove;

		// Token: 0x04003569 RID: 13673
		public static SoundDef Designate_PlanAdd;

		// Token: 0x0400356A RID: 13674
		public static SoundDef Designate_Claim;

		// Token: 0x0400356B RID: 13675
		public static SoundDef Designate_Deconstruct;

		// Token: 0x0400356C RID: 13676
		public static SoundDef Designate_Hunt;

		// Token: 0x0400356D RID: 13677
		public static SoundDef Designate_PlaceBuilding;

		// Token: 0x0400356E RID: 13678
		public static SoundDef Designate_CutPlants;

		// Token: 0x0400356F RID: 13679
		public static SoundDef Designate_Harvest;

		// Token: 0x04003570 RID: 13680
		public static SoundDef Standard_Drop;

		// Token: 0x04003571 RID: 13681
		public static SoundDef Standard_Pickup;

		// Token: 0x04003572 RID: 13682
		public static SoundDef BulletImpact_Ground;

		// Token: 0x04003573 RID: 13683
		public static SoundDef Ambient_AltitudeWind;

		// Token: 0x04003574 RID: 13684
		public static SoundDef Ambient_Space;

		// Token: 0x04003575 RID: 13685
		public static SoundDef Power_OnSmall;

		// Token: 0x04003576 RID: 13686
		public static SoundDef Power_OffSmall;

		// Token: 0x04003577 RID: 13687
		public static SoundDef Thunder_OnMap;

		// Token: 0x04003578 RID: 13688
		public static SoundDef Thunder_OffMap;

		// Token: 0x04003579 RID: 13689
		public static SoundDef Interact_CleanFilth;

		// Token: 0x0400357A RID: 13690
		public static SoundDef Interact_Sow;

		// Token: 0x0400357B RID: 13691
		public static SoundDef Interact_Tend;

		// Token: 0x0400357C RID: 13692
		public static SoundDef Interact_BeatFire;

		// Token: 0x0400357D RID: 13693
		public static SoundDef Interact_Ignite;

		// Token: 0x0400357E RID: 13694
		public static SoundDef Roof_Start;

		// Token: 0x0400357F RID: 13695
		public static SoundDef Roof_Finish;

		// Token: 0x04003580 RID: 13696
		public static SoundDef Roof_Collapse;

		// Token: 0x04003581 RID: 13697
		public static SoundDef PsychicPulseGlobal;

		// Token: 0x04003582 RID: 13698
		public static SoundDef PsychicSootheGlobal;

		// Token: 0x04003583 RID: 13699
		public static SoundDef GeyserSpray;

		// Token: 0x04003584 RID: 13700
		public static SoundDef TurretAcquireTarget;

		// Token: 0x04003585 RID: 13701
		public static SoundDef FlickSwitch;

		// Token: 0x04003586 RID: 13702
		public static SoundDef PlayBilliards;

		// Token: 0x04003587 RID: 13703
		public static SoundDef Building_Complete;

		// Token: 0x04003588 RID: 13704
		public static SoundDef RawMeat_Eat;

		// Token: 0x04003589 RID: 13705
		public static SoundDef HissSmall;

		// Token: 0x0400358A RID: 13706
		public static SoundDef HissJet;

		// Token: 0x0400358B RID: 13707
		public static SoundDef MetalHitImportant;

		// Token: 0x0400358C RID: 13708
		public static SoundDef Door_OpenPowered;

		// Token: 0x0400358D RID: 13709
		public static SoundDef Door_ClosePowered;

		// Token: 0x0400358E RID: 13710
		public static SoundDef Door_OpenManual;

		// Token: 0x0400358F RID: 13711
		public static SoundDef Door_CloseManual;

		// Token: 0x04003590 RID: 13712
		public static SoundDef EnergyShield_AbsorbDamage;

		// Token: 0x04003591 RID: 13713
		public static SoundDef EnergyShield_Reset;

		// Token: 0x04003592 RID: 13714
		public static SoundDef EnergyShield_Broken;

		// Token: 0x04003593 RID: 13715
		public static SoundDef Pawn_Melee_Punch_HitPawn;

		// Token: 0x04003594 RID: 13716
		public static SoundDef Pawn_Melee_Punch_HitBuilding;

		// Token: 0x04003595 RID: 13717
		public static SoundDef Pawn_Melee_Punch_Miss;

		// Token: 0x04003596 RID: 13718
		public static SoundDef Artillery_ShellLoaded;

		// Token: 0x04003597 RID: 13719
		public static SoundDef TechMedicineUsed;

		// Token: 0x04003598 RID: 13720
		public static SoundDef OrbitalBeam;

		// Token: 0x04003599 RID: 13721
		public static SoundDef DropPod_Open;

		// Token: 0x0400359A RID: 13722
		public static SoundDef Building_Deconstructed;

		// Token: 0x0400359B RID: 13723
		public static SoundDef CryptosleepCasket_Accept;

		// Token: 0x0400359C RID: 13724
		public static SoundDef CryptosleepCasket_Eject;

		// Token: 0x0400359D RID: 13725
		public static SoundDef TrapSpring;

		// Token: 0x0400359E RID: 13726
		public static SoundDef TrapArm;

		// Token: 0x0400359F RID: 13727
		public static SoundDef FireBurning;

		// Token: 0x040035A0 RID: 13728
		public static SoundDef Vomit;

		// Token: 0x040035A1 RID: 13729
		public static SoundDef ResearchStart;

		// Token: 0x040035A2 RID: 13730
		public static SoundDef ThingUninstalled;

		// Token: 0x040035A3 RID: 13731
		public static SoundDef ShipTakeoff;

		// Token: 0x040035A4 RID: 13732
		public static SoundDef Corpse_Drop;

		// Token: 0x040035A5 RID: 13733
		public static SoundDef Tornado;

		// Token: 0x040035A6 RID: 13734
		public static SoundDef Tunnel;

		// Token: 0x040035A7 RID: 13735
		public static SoundDef Hive_Spawn;

		// Token: 0x040035A8 RID: 13736
		public static SoundDef Interceptor_BlockProjectile;

		// Token: 0x040035A9 RID: 13737
		public static SoundDef MechanoidsWakeUp;

		// Token: 0x040035AA RID: 13738
		public static SoundDef FlashstormAmbience;

		// Token: 0x040035AB RID: 13739
		public static SoundDef MechSerumUsed;

		// Token: 0x040035AC RID: 13740
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicEffect;

		// Token: 0x040035AD RID: 13741
		[MayRequireRoyalty]
		public static SoundDef PsycastSkipEffect;

		// Token: 0x040035AE RID: 13742
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicPulse;

		// Token: 0x040035AF RID: 13743
		[MayRequireRoyalty]
		public static SoundDef PsycastSkipPulse;

		// Token: 0x040035B0 RID: 13744
		[MayRequireRoyalty]
		public static SoundDef PsycastCastLoop;

		// Token: 0x040035B1 RID: 13745
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyOverloaded;

		// Token: 0x040035B2 RID: 13746
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyHyperloaded;

		// Token: 0x040035B3 RID: 13747
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainCharring;

		// Token: 0x040035B4 RID: 13748
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainRoasting;

		// Token: 0x040035B5 RID: 13749
		[MayRequireRoyalty]
		public static SoundDef MeditationGainPsyfocus;

		// Token: 0x040035B6 RID: 13750
		[MayRequireRoyalty]
		public static SoundDef MechClusterDefeated;

		// Token: 0x040035B7 RID: 13751
		[MayRequireRoyalty]
		public static SoundDef TechprintApplied;

		// Token: 0x040035B8 RID: 13752
		public static SoundDef GameStartSting;

		// Token: 0x040035B9 RID: 13753
		public static SoundDef PlanetkillerImpact;
	}
}
