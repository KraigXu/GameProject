using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C58 RID: 3160
	public class Building_BlastingCharge : Building
	{
		// Token: 0x06004B78 RID: 19320 RVA: 0x00197294 File Offset: 0x00195494
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true);
			command_Action.defaultDesc = "CommandDetonateDesc".Translate();
			command_Action.action = new Action(this.Command_Detonate);
			if (base.GetComp<CompExplosive>().wickStarted)
			{
				command_Action.Disable(null);
			}
			command_Action.defaultLabel = "CommandDetonateLabel".Translate();
			yield return command_Action;
			yield break;
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x001972A4 File Offset: 0x001954A4
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
