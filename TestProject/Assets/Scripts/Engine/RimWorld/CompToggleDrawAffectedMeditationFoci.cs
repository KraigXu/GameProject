﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class CompToggleDrawAffectedMeditationFoci : ThingComp
	{
		
		
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		
		
		public CompProperties_ToggleDrawAffectedMeditationFoci Props
		{
			get
			{
				return (CompProperties_ToggleDrawAffectedMeditationFoci)this.props;
			}
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.enabled = this.Props.defaultEnabled;
			}
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.defaultLabel = "CommandWarnInBuildingRadius".Translate();
			command_Toggle.defaultDesc = "CommandWarnInBuildingRadiusDesc".Translate();
			command_Toggle.icon = CompToggleDrawAffectedMeditationFoci.CommandTex;
			command_Toggle.isActive = (() => this.enabled);
			Command_Toggle command_Toggle2 = command_Toggle;
			command_Toggle2.toggleAction = (Action)Delegate.Combine(command_Toggle2.toggleAction, new Action(delegate
			{
				this.enabled = !this.enabled;
			}));
			yield return command_Toggle;
			yield break;
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", false, false);
		}

		
		private bool enabled;

		
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);
	}
}
