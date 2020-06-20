using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C48 RID: 3144
	public static class MinifyUtility
	{
		// Token: 0x06004B02 RID: 19202 RVA: 0x0019526C File Offset: 0x0019346C
		public static MinifiedThing MakeMinified(this Thing thing)
		{
			if (!thing.def.Minifiable)
			{
				Log.Warning("Tried to minify " + thing + " which is not minifiable.", false);
				return null;
			}
			if (thing.Spawned)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			if (thing.holdingOwner != null)
			{
				Log.Warning("Can't minify thing which is in a ThingOwner because we don't know how to handle it. Remove it from the container first. holder=" + thing.ParentHolder, false);
				return null;
			}
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(thing);
			MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(thing.def.minifiedDef, null);
			minifiedThing.InnerThing = thing;
			if (blueprint_Install != null)
			{
				blueprint_Install.SetThingToInstallFromMinified(minifiedThing);
			}
			if (minifiedThing.InnerThing.stackCount > 1)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to minify ",
					thing.LabelCap,
					" with stack count ",
					minifiedThing.InnerThing.stackCount,
					". Clamped stack count to 1."
				}), false);
				minifiedThing.InnerThing.stackCount = 1;
			}
			return minifiedThing;
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x0019535C File Offset: 0x0019355C
		public static Thing TryMakeMinified(this Thing thing)
		{
			if (thing.def.Minifiable)
			{
				return thing.MakeMinified();
			}
			return thing;
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x00195374 File Offset: 0x00193574
		public static Thing GetInnerIfMinified(this Thing outerThing)
		{
			MinifiedThing minifiedThing = outerThing as MinifiedThing;
			if (minifiedThing != null)
			{
				return minifiedThing.InnerThing;
			}
			return outerThing;
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x00195394 File Offset: 0x00193594
		public static MinifiedThing Uninstall(this Thing th)
		{
			if (!th.Spawned)
			{
				Log.Warning("Can't uninstall unspawned thing " + th, false);
				return null;
			}
			Map map = th.Map;
			MinifiedThing minifiedThing = th.MakeMinified();
			GenPlace.TryPlaceThing(minifiedThing, th.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
			SoundDefOf.ThingUninstalled.PlayOneShot(new TargetInfo(th.Position, map, false));
			return minifiedThing;
		}
	}
}
