﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	
	public class WorldObjectCompProperties_DefeatAllEnemiesQuest : WorldObjectCompProperties
	{
		
		public WorldObjectCompProperties_DefeatAllEnemiesQuest()
		{
			this.compClass = typeof(DefeatAllEnemiesQuestComp);
		}

		
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in this.n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_DefeatAllEnemiesQuest but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
