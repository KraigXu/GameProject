using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD3 RID: 3283
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x06004F8C RID: 20364 RVA: 0x001ACD9B File Offset: 0x001AAF9B
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && x != ThingDefOf.RaisedRocks && !x.IsSmoothed
			select x);
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x001ACDDC File Offset: 0x001AAFDC
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int randomInRange = (parms.countRange ?? ThingSetMaker_Meteorite.MineablesCountRange).RandomInRange;
			ThingDef def = this.FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def, null);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x001ACE48 File Offset: 0x001AB048
		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			if (value < 0.4f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			if (value < 0.75f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
			select x).RandomElement<ThingDef>();
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x001ACEF7 File Offset: 0x001AB0F7
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x04002C96 RID: 11414
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x04002C97 RID: 11415
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x04002C98 RID: 11416
		private const float PreciousMineableMarketValue = 5f;
	}
}
