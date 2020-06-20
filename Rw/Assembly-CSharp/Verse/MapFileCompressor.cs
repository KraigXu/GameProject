using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000153 RID: 339
	public class MapFileCompressor : IExposable
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x000342CD File Offset: 0x000324CD
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x000342DC File Offset: 0x000324DC
		public void ExposeData()
		{
			DataExposeUtility.ByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x000342EE File Offset: 0x000324EE
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0003432C File Offset: 0x0003252C
		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing thing in this.map.thingGrid.ThingsAt(curSq))
			{
				if (thing.IsSaveCompressible())
				{
					if (num != 0)
					{
						Log.Error(string.Concat(new object[]
						{
							"Found two compressible things in ",
							curSq,
							". The last was ",
							thing
						}), false);
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000343C8 File Offset: 0x000325C8
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDefsByShortHash.ContainsKey(thingDef.shortHash))
				{
					Log.Error(string.Concat(new object[]
					{
						"Hash collision between ",
						thingDef,
						" and  ",
						thingDefsByShortHash[thingDef.shortHash],
						": both have short hash ",
						thingDef.shortHash
					}), false);
				}
				else
				{
					thingDefsByShortHash.Add(thingDef.shortHash, thingDef);
				}
			}
			int major = VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
			int minor = VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, major, minor);
				if (thingDef2 == null)
				{
					try
					{
						thingDef2 = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef3 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef3 != null)
						{
							thingDef2 = thingDef3;
							thingDefsByShortHash.Add(val, thingDef3);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.", false);
							thingDefsByShortHash.Add(val, null);
						}
					}
				}
				if (thingDef2 != null)
				{
					try
					{
						Thing thing = ThingMaker.MakeThing(thingDef2, null);
						thing.SetPositionDirect(c);
						loadables.Add(thing);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate compressed thing: " + arg, false);
					}
				}
			});
			return loadables;
		}

		// Token: 0x040007DE RID: 2014
		private Map map;

		// Token: 0x040007DF RID: 2015
		private byte[] compressedData;

		// Token: 0x040007E0 RID: 2016
		public CompressibilityDecider compressibilityDecider;
	}
}
