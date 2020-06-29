using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompGatherSpot : ThingComp
	{
		
		
		
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

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		
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

		
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		
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

		
		private bool active = true;
	}
}
