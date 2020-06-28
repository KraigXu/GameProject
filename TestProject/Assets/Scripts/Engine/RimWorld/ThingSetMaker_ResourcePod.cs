using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD6 RID: 3286
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x06004FA0 RID: 20384 RVA: 0x001AD3C4 File Offset: 0x001AB5C4
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef(false);
			float num = Rand.Range(150f, 600f);
			do
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int num2 = Rand.Range(20, 40);
				if (num2 > thing.def.stackLimit)
				{
					num2 = thing.def.stackLimit;
				}
				if ((float)num2 * thing.def.BaseMarketValue > num)
				{
					num2 = Mathf.FloorToInt(num / thing.def.BaseMarketValue);
				}
				if (num2 == 0)
				{
					num2 = 1;
				}
				thing.stackCount = num2;
				outThings.Add(thing);
				num -= (float)num2 * thingDef.BaseMarketValue;
			}
			while (outThings.Count < 7 && num > thingDef.BaseMarketValue);
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x001AD46C File Offset: 0x001AB66C
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x001AD498 File Offset: 0x001AB698
		public static ThingDef RandomPodContentsDef(bool mustBeResource = false)
		{
			IEnumerable<ThingDef> source = ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
			if (mustBeResource)
			{
				source = from x in source
				where x.stackLimit > 1
				select x;
			}
			int numMeats = (from x in source
			where x.IsMeat
			select x).Count<ThingDef>();
			int numLeathers = (from x in source
			where x.IsLeather
			select x).Count<ThingDef>();
			return source.RandomElementByWeight((ThingDef d) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, numMeats, numLeathers));
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x001AD54C File Offset: 0x001AB74C
		[DebugOutput("Incidents", false)]
		private static void PodContentsPossibleDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef thingDef in ThingSetMaker_ResourcePod.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(thingDef.defName);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x001AD5BC File Offset: 0x001AB7BC
		[DebugOutput("Incidents", false)]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef(false).LabelCap);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001AD5FF File Offset: 0x001AB7FF
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		// Token: 0x04002C9B RID: 11419
		private const int MaxStacks = 7;

		// Token: 0x04002C9C RID: 11420
		private const float MaxMarketValue = 40f;

		// Token: 0x04002C9D RID: 11421
		private const float MinMoney = 150f;

		// Token: 0x04002C9E RID: 11422
		private const float MaxMoney = 600f;
	}
}
