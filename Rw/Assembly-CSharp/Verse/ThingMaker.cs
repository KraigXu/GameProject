﻿using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000311 RID: 785
	public static class ThingMaker
	{
		// Token: 0x0600166E RID: 5742 RVA: 0x000820A0 File Offset: 0x000802A0
		public static Thing MakeThing(ThingDef def, ThingDef stuff = null)
		{
			if (stuff != null && !stuff.IsStuff)
			{
				Log.Error(string.Concat(new object[]
				{
					"MakeThing error: Tried to make ",
					def,
					" from ",
					stuff,
					" which is not a stuff. Assigning default."
				}), false);
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (def.MadeFromStuff && stuff == null)
			{
				Log.Error("MakeThing error: " + def + " is madeFromStuff but stuff=null. Assigning default.", false);
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (!def.MadeFromStuff && stuff != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"MakeThing error: ",
					def,
					" is not madeFromStuff but stuff=",
					stuff,
					". Setting to null."
				}), false);
				stuff = null;
			}
			Thing thing = (Thing)Activator.CreateInstance(def.thingClass);
			thing.def = def;
			thing.SetStuffDirect(stuff);
			thing.PostMake();
			return thing;
		}
	}
}
