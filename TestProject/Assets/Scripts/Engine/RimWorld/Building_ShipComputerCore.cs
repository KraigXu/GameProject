using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C60 RID: 3168
	public class Building_ShipComputerCore : Building
	{
		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06004BD9 RID: 19417 RVA: 0x00198B03 File Offset: 0x00196D03
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x00198B13 File Offset: 0x00196D13
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
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

		// Token: 0x06004BDB RID: 19419 RVA: 0x00198B23 File Offset: 0x00196D23
		public void ForceLaunch()
		{
			ShipCountdown.InitiateCountdown(this);
			if (base.Spawned)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "LaunchedShip");
			}
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00198B4D File Offset: 0x00196D4D
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				this.ForceLaunch();
			}
		}
	}
}
