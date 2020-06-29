using System;
using Verse;

namespace RimWorld
{
	
	public static class GameConditionMaker
	{
		
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1);
			gameCondition.Permanent = true;
			gameCondition.startTick -= 180000;
			return gameCondition;
		}

		
		public static GameCondition MakeCondition(GameConditionDef def, int duration = -1)
		{
			GameCondition gameCondition = (GameCondition)Activator.CreateInstance(def.conditionClass);
			gameCondition.startTick = Find.TickManager.TicksGame;
			gameCondition.def = def;
			gameCondition.Duration = duration;
			gameCondition.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
			gameCondition.PostMake();
			return gameCondition;
		}
	}
}
