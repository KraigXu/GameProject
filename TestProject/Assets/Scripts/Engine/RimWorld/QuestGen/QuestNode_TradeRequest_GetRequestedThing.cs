using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_TradeRequest_GetRequestedThing : QuestNode
	{
		
		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			Rand.PushState(Find.TickManager.TicksGame ^ thingDef.GetHashCode() ^ 876093659);
			float num = (float)QuestNode_TradeRequest_GetRequestedThing.BaseValueWantedRange.RandomInRange;
			Rand.PopState();
			num *= QuestNode_TradeRequest_GetRequestedThing.ValueWantedFactorFromWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
			return ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue)));
		}

		
		private static bool TryFindRandomRequestedThingDef(Map map, out ThingDef thingDef, out int count)
		{
			QuestNode_TradeRequest_GetRequestedThing.requestCountDict.Clear();
			Func<ThingDef, bool> globalValidator = delegate(ThingDef td)
			{
				if (td.BaseMarketValue / td.BaseMass < 5f)
				{
					return false;
				}
				if (!td.alwaysHaulable)
				{
					return false;
				}
				CompProperties_Rottable compProperties = td.GetCompProperties<CompProperties_Rottable>();
				if (compProperties != null && compProperties.daysToRotStart < 10f)
				{
					return false;
				}
				if (td.ingestible != null && td.ingestible.HumanEdible)
				{
					return false;
				}
				if (td == ThingDefOf.Silver)
				{
					return false;
				}
				if (!td.PlayerAcquirable)
				{
					return false;
				}
				int num = QuestNode_TradeRequest_GetRequestedThing.RandomRequestCount(td, map);
				QuestNode_TradeRequest_GetRequestedThing.requestCountDict.Add(td, num);
				return PlayerItemAccessibilityUtility.PossiblyAccessible(td, num, map) && PlayerItemAccessibilityUtility.PlayerCanMake(td, map) && (td.thingSetMakerTags == null || !td.thingSetMakerTags.Contains("RewardStandardHighFreq"));
			};
			if ((from td in ThingSetMakerUtility.allGeneratableItems
			where globalValidator(td)
			select td).TryRandomElement(out thingDef))
			{
				count = QuestNode_TradeRequest_GetRequestedThing.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				slate.Set<bool>(this.storeHasQualityAs.GetValue(slate), thingDef.HasComp(typeof(CompQuality)), false);
			}
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				return true;
			}
			return false;
		}

		
		private static readonly IntRange BaseValueWantedRange = new IntRange(500, 2500);

		
		private static readonly SimpleCurve ValueWantedFactorFromWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(50000f, 1f),
				true
			},
			{
				new CurvePoint(300000f, 2f),
				true
			}
		};

		
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();

		
		[NoTranslate]
		public SlateRef<string> storeThingAs;

		
		[NoTranslate]
		public SlateRef<string> storeThingCountAs;

		
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		
		[NoTranslate]
		public SlateRef<string> storeHasQualityAs;
	}
}
