using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D72 RID: 3442
	[StaticConstructorOnStartup]
	public class CompToggleDrawAffectedMeditationFoci : ThingComp
	{
		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x060053C8 RID: 21448 RVA: 0x001BFD47 File Offset: 0x001BDF47
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x060053C9 RID: 21449 RVA: 0x001BFD4F File Offset: 0x001BDF4F
		public CompProperties_ToggleDrawAffectedMeditationFoci Props
		{
			get
			{
				return (CompProperties_ToggleDrawAffectedMeditationFoci)this.props;
			}
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x001BFD5C File Offset: 0x001BDF5C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.enabled = this.Props.defaultEnabled;
			}
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x001BFD72 File Offset: 0x001BDF72
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

		// Token: 0x060053CC RID: 21452 RVA: 0x001BFD82 File Offset: 0x001BDF82
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", false, false);
		}

		// Token: 0x04002E45 RID: 11845
		private bool enabled;

		// Token: 0x04002E46 RID: 11846
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);
	}
}
