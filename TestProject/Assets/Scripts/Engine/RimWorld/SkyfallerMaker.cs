using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC1 RID: 3265
	public static class SkyfallerMaker
	{
		// Token: 0x06004F36 RID: 20278 RVA: 0x001AAF71 File Offset: 0x001A9171
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001AAF80 File Offset: 0x001A9180
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001AAF9C File Offset: 0x001A919C
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, Thing innerThing)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (innerThing != null && !skyfaller2.innerContainer.TryAdd(innerThing, true))
			{
				Log.Error("Could not add " + innerThing.ToStringSafe<Thing>() + " to a skyfaller.", false);
				innerThing.Destroy(DestroyMode.Vanish);
			}
			return skyfaller2;
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x001AAFE8 File Offset: 0x001A91E8
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x001AB00E File Offset: 0x001A920E
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x001AB023 File Offset: 0x001A9223
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x001AB039 File Offset: 0x001A9239
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x001AB04F File Offset: 0x001A924F
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, things), pos, map, WipeMode.Vanish);
		}
	}
}
