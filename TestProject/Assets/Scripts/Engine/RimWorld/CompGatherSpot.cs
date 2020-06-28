using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D10 RID: 3344
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x001B46E2 File Offset: 0x001B28E2
		// (set) Token: 0x06005156 RID: 20822 RVA: 0x001B46EC File Offset: 0x001B28EC
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (value == this.active)
				{
					return;
				}
				this.active = value;
				if (this.parent.Spawned)
				{
					if (this.active)
					{
						this.parent.Map.gatherSpotLister.RegisterActivated(this);
						return;
					}
					this.parent.Map.gatherSpotLister.RegisterDeactivated(this);
				}
			}
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001B474C File Offset: 0x001B294C
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001B4760 File Offset: 0x001B2960
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.parent.Faction != Faction.OfPlayer && !respawningAfterLoad)
			{
				this.active = false;
			}
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001B47AE File Offset: 0x001B29AE
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001B47CB File Offset: 0x001B29CB
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_TogglePower;
			command_Toggle.defaultLabel = "CommandGatherSpotToggleLabel".Translate();
			command_Toggle.icon = TexCommand.GatherSpotActive;
			command_Toggle.isActive = (() => this.Active);
			command_Toggle.toggleAction = delegate
			{
				this.Active = !this.Active;
			};
			if (this.Active)
			{
				command_Toggle.defaultDesc = "CommandGatherSpotToggleDescActive".Translate();
			}
			else
			{
				command_Toggle.defaultDesc = "CommandGatherSpotToggleDescInactive".Translate();
			}
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x04002D0A RID: 11530
		private bool active = true;
	}
}
