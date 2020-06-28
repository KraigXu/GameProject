using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200104F RID: 4175
	public sealed class PlaySettings : IExposable
	{
		// Token: 0x060063B7 RID: 25527 RVA: 0x002291C4 File Offset: 0x002273C4
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.showLearningHelper, "showLearningHelper", false, false);
			Scribe_Values.Look<bool>(ref this.showZones, "showZones", false, false);
			Scribe_Values.Look<bool>(ref this.showBeauty, "showBeauty", false, false);
			Scribe_Values.Look<bool>(ref this.showRoomStats, "showRoomStats", false, false);
			Scribe_Values.Look<bool>(ref this.showColonistBar, "showColonistBar", false, false);
			Scribe_Values.Look<bool>(ref this.showRoofOverlay, "showRoofOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showFertilityOverlay, "showFertilityOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showTerrainAffordanceOverlay, "showTerrainAffordanceOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.autoHomeArea, "autoHomeArea", false, false);
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
			Scribe_Values.Look<bool>(ref this.lockNorthUp, "lockNorthUp", false, false);
			Scribe_Values.Look<bool>(ref this.usePlanetDayNightSystem, "usePlanetDayNightSystem", false, false);
			Scribe_Values.Look<bool>(ref this.showExpandingIcons, "showExpandingIcons", false, false);
			Scribe_Values.Look<bool>(ref this.showWorldFeatures, "showWorldFeatures", false, false);
			Scribe_Values.Look<bool>(ref this.useWorkPriorities, "useWorkPriorities", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyHumanlike, "defaultCareForHumanlikeColonists", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyAnimal, "defaultCareForAnimalColonists", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyPrisoner, "defaultCareForHumanlikeColonistPrisoners", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralFaction, "defaultCareForHumanlikeNeutrals", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralAnimal, "defaultCareForAnimalNeutrals", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForHostileFaction, "defaultCareForHumanlikeEnemies", MedicalCareCategory.NoCare, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060063B8 RID: 25528 RVA: 0x00229354 File Offset: 0x00227554
		public void DoPlaySettingsGlobalControls(WidgetRow row, bool worldView)
		{
			bool flag = this.showColonistBar;
			if (worldView)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				}
				bool flag2 = this.lockNorthUp;
				row.ToggleableIcon(ref this.lockNorthUp, TexButton.LockNorthUp, "LockNorthUpToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				if (flag2 != this.lockNorthUp && this.lockNorthUp)
				{
					Find.WorldCameraDriver.RotateSoNorthIsUp(true);
				}
				row.ToggleableIcon(ref this.usePlanetDayNightSystem, TexButton.UsePlanetDayNightSystem, "UsePlanetDayNightSystemToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showExpandingIcons, TexButton.ShowExpandingIcons, "ShowExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showWorldFeatures, TexButton.ShowWorldFeatures, "ShowWorldFeaturesToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			else
			{
				row.ToggleableIcon(ref this.showLearningHelper, TexButton.ShowLearningHelper, "ShowLearningHelperWhenEmptyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showZones, TexButton.ShowZones, "ZoneVisibilityToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showBeauty, TexButton.ShowBeauty, "ShowBeautyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				this.CheckKeyBindingToggle(KeyBindingDefOf.ToggleBeautyDisplay, ref this.showBeauty);
				row.ToggleableIcon(ref this.showRoomStats, TexButton.ShowRoomStats, "ShowRoomStatsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, "InspectRoomStats");
				this.CheckKeyBindingToggle(KeyBindingDefOf.ToggleRoomStatsDisplay, ref this.showRoomStats);
				row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showRoofOverlay, TexButton.ShowRoofOverlay, "ShowRoofOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showFertilityOverlay, TexButton.ShowFertilityOverlay, "ShowFertilityOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showTerrainAffordanceOverlay, TexButton.ShowTerrainAffordanceOverlay, "ShowTerrainAffordanceOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.autoHomeArea, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.autoRebuild, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				bool resourceReadoutCategorized = Prefs.ResourceReadoutCategorized;
				bool flag3 = resourceReadoutCategorized;
				row.ToggleableIcon(ref resourceReadoutCategorized, TexButton.CategorizedResourceReadout, "CategorizedResourceReadoutToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				if (resourceReadoutCategorized != flag3)
				{
					Prefs.ResourceReadoutCategorized = resourceReadoutCategorized;
				}
			}
			if (flag != this.showColonistBar)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x060063B9 RID: 25529 RVA: 0x00229641 File Offset: 0x00227841
		private void CheckKeyBindingToggle(KeyBindingDef keyBinding, ref bool value)
		{
			if (keyBinding.KeyDownEvent)
			{
				value = !value;
				if (value)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x04003CC7 RID: 15559
		public bool showLearningHelper = true;

		// Token: 0x04003CC8 RID: 15560
		public bool showZones = true;

		// Token: 0x04003CC9 RID: 15561
		public bool showBeauty;

		// Token: 0x04003CCA RID: 15562
		public bool showRoomStats;

		// Token: 0x04003CCB RID: 15563
		public bool showColonistBar = true;

		// Token: 0x04003CCC RID: 15564
		public bool showRoofOverlay;

		// Token: 0x04003CCD RID: 15565
		public bool showFertilityOverlay;

		// Token: 0x04003CCE RID: 15566
		public bool showTerrainAffordanceOverlay;

		// Token: 0x04003CCF RID: 15567
		public bool autoHomeArea = true;

		// Token: 0x04003CD0 RID: 15568
		public bool autoRebuild;

		// Token: 0x04003CD1 RID: 15569
		public bool lockNorthUp = true;

		// Token: 0x04003CD2 RID: 15570
		public bool usePlanetDayNightSystem = true;

		// Token: 0x04003CD3 RID: 15571
		public bool showExpandingIcons = true;

		// Token: 0x04003CD4 RID: 15572
		public bool showWorldFeatures = true;

		// Token: 0x04003CD5 RID: 15573
		public bool useWorkPriorities;

		// Token: 0x04003CD6 RID: 15574
		public MedicalCareCategory defaultCareForColonyHumanlike = MedicalCareCategory.Best;

		// Token: 0x04003CD7 RID: 15575
		public MedicalCareCategory defaultCareForColonyAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04003CD8 RID: 15576
		public MedicalCareCategory defaultCareForColonyPrisoner = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04003CD9 RID: 15577
		public MedicalCareCategory defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04003CDA RID: 15578
		public MedicalCareCategory defaultCareForNeutralAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04003CDB RID: 15579
		public MedicalCareCategory defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
	}
}
