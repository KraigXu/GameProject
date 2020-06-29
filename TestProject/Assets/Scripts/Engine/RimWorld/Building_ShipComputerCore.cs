using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Building_ShipComputerCore : Building
	{
		
		// (get) Token: 0x06004BD9 RID: 19417 RVA: 0x00198B03 File Offset: 0x00196D03
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			Command_Action command_Action = new Command_Action();
			command_Action.action = new Action(this.TryLaunch);
			command_Action.defaultLabel = "CommandShipLaunch".Translate();
			command_Action.defaultDesc = "CommandShipLaunchDesc".Translate();
			if (!this.CanLaunchNow)
			{
				command_Action.Disable(ShipUtility.LaunchFailReasons(this).First<string>());
			}
			if (ShipCountdown.CountingDown)
			{
				command_Action.Disable(null);
			}
			command_Action.hotKey = KeyBindingDefOf.Misc1;
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);
			yield return command_Action;
			yield break;
			yield break;
		}

		
		public void ForceLaunch()
		{
			ShipCountdown.InitiateCountdown(this);
			if (base.Spawned)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "LaunchedShip");
			}
		}

		
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				this.ForceLaunch();
			}
		}
	}
}
