using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Building_BlastingCharge : Building
	{
		
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

		
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
