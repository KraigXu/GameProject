using System;
using Verse.Sound;

namespace Verse
{
	
	public static class GenDrop
	{
		
		[Obsolete("Only used for mod compatibility")]
		public static bool TryDropSpawn(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			return GenDrop.TryDropSpawn_NewTmp(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator, true);
		}

		
		public static bool TryDropSpawn_NewTmp(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (map == null)
			{
				Log.Error("Dropped " + thing + " in a null map.", false);
				resultingThing = null;
				return false;
			}
			if (!dropCell.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Dropped ",
					thing,
					" out of bounds at ",
					dropCell
				}), false);
				resultingThing = null;
				return false;
			}
			if (thing.def.destroyOnDrop)
			{
				thing.Destroy(DestroyMode.Vanish);
				resultingThing = null;
				return true;
			}
			if (playDropSound && thing.def.soundDrop != null)
			{
				thing.def.soundDrop.PlayOneShot(new TargetInfo(dropCell, map, false));
			}
			return GenPlace.TryPlaceThing(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator, default(Rot4));
		}
	}
}
