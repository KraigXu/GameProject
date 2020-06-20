using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CE0 RID: 3296
	public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
	{
		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06004FEE RID: 20462 RVA: 0x001AF5E5 File Offset: 0x001AD7E5
		public new CompProperties_CausesGameCondition_PsychicEmanation Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_PsychicEmanation)this.props;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06004FEF RID: 20463 RVA: 0x001AF5F2 File Offset: 0x001AD7F2
		public PsychicDroneLevel Level
		{
			get
			{
				return this.droneLevel;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06004FF0 RID: 20464 RVA: 0x001AF5FA File Offset: 0x001AD7FA
		private bool DroneLevelIncreases
		{
			get
			{
				return this.Props.droneLevelIncreaseInterval != int.MinValue;
			}
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001AF611 File Offset: 0x001AD811
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
			this.droneLevel = this.Props.droneLevel;
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001AF632 File Offset: 0x001AD832
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.DroneLevelIncreases)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x001AF66C File Offset: 0x001AD86C
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned || !this.DroneLevelIncreases || !base.Active)
			{
				return;
			}
			this.ticksToIncreaseDroneLevel--;
			if (this.ticksToIncreaseDroneLevel <= 0)
			{
				this.IncreaseDroneLevel();
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
			}
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x001AF6CC File Offset: 0x001AD8CC
		private void IncreaseDroneLevel()
		{
			if (this.droneLevel == PsychicDroneLevel.BadExtreme)
			{
				return;
			}
			this.droneLevel += 1;
			TaggedString taggedString = "LetterPsychicDroneLevelIncreased".Translate();
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), taggedString, LetterDefOf.NegativeEvent, null);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			base.ReSetupAllConditions();
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x001AF73D File Offset: 0x001AD93D
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)condition;
			gameCondition_PsychicEmanation.gender = this.gender;
			gameCondition_PsychicEmanation.level = this.Level;
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x001AF764 File Offset: 0x001AD964
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001AF7A2 File Offset: 0x001AD9A2
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.gender.GetLabel(false),
				action = delegate
				{
					if (this.gender == Gender.Female)
					{
						this.gender = Gender.Male;
					}
					else
					{
						this.gender = Gender.Female;
					}
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Action
			{
				defaultLabel = this.droneLevel.GetLabel(),
				action = delegate
				{
					this.IncreaseDroneLevel();
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			yield break;
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001AF7B4 File Offset: 0x001AD9B4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(this.droneLevel.GetLabelCap()));
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x001AF83C File Offset: 0x001ADA3C
		public override void RandomizeSettings()
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x04002CB2 RID: 11442
		public Gender gender;

		// Token: 0x04002CB3 RID: 11443
		private int ticksToIncreaseDroneLevel;

		// Token: 0x04002CB4 RID: 11444
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;
	}
}
