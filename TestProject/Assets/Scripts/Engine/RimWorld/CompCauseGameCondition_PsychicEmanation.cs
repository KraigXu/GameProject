using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
	{
		
		
		public new CompProperties_CausesGameCondition_PsychicEmanation Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_PsychicEmanation)this.props;
			}
		}

		
		
		public PsychicDroneLevel Level
		{
			get
			{
				return this.droneLevel;
			}
		}

		
		
		private bool DroneLevelIncreases
		{
			get
			{
				return this.Props.droneLevelIncreaseInterval != int.MinValue;
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
			this.droneLevel = this.Props.droneLevel;
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.DroneLevelIncreases)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		
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

		
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)condition;
			gameCondition_PsychicEmanation.gender = this.gender;
			gameCondition_PsychicEmanation.level = this.Level;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		
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

		
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(this.droneLevel.GetLabelCap()));
		}

		
		public override void RandomizeSettings()
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		
		public Gender gender;

		
		private int ticksToIncreaseDroneLevel;

		
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;
	}
}
