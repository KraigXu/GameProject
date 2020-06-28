using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001283 RID: 4739
	public static class WorldObjectMaker
	{
		// Token: 0x06006F32 RID: 28466 RVA: 0x0026B2B5 File Offset: 0x002694B5
		public static WorldObject MakeWorldObject(WorldObjectDef def)
		{
			WorldObject worldObject = (WorldObject)Activator.CreateInstance(def.worldObjectClass);
			worldObject.def = def;
			worldObject.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
			worldObject.creationGameTicks = Find.TickManager.TicksGame;
			worldObject.PostMake();
			return worldObject;
		}
	}
}
