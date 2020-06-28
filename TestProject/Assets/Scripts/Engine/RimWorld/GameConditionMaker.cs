using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B3 RID: 2483
	public static class GameConditionMaker
	{
		// Token: 0x06003B3A RID: 15162 RVA: 0x001394EE File Offset: 0x001376EE
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1);
			gameCondition.Permanent = true;
			gameCondition.startTick -= 180000;
			return gameCondition;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x00139510 File Offset: 0x00137710
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
