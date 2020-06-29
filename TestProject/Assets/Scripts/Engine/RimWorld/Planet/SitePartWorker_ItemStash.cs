﻿using System;
using System.Collections.Generic;
using RimWorld.QuestGenNew;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	
	public class SitePartWorker_ItemStash : SitePartWorker
	{
		
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			ThingDef thingDef = slate.Get<ThingDef>("itemStashSingleThing", null, false);
			IEnumerable<ThingDef> enumerable = slate.Get<IEnumerable<ThingDef>>("itemStashThings", null, false);
			List<Thing> list;
			if (thingDef != null)
			{
				list = new List<Thing>();
				list.Add(ThingMaker.MakeThing(thingDef, null));
			}
			else
			{
				if (enumerable != null)
				{
					list = new List<Thing>();
					IEnumerator<ThingDef> enumerator = enumerable.GetEnumerator();
					{
						while (enumerator.MoveNext())
						{
							ThingDef def = enumerator.Current;
							list.Add(ThingMaker.MakeThing(def, null));
						}
						goto IL_D7;
					}
				}
				float x = slate.Get<float>("points", 0f, false);
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.totalMarketValueRange = new FloatRange?(new FloatRange(0.7f, 1.3f) * QuestTuning.PointsToRewardMarketValueCurve.Evaluate(x));
				list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
			}
			IL_D7:
			part.things = new ThingOwner<Thing>(part, false, LookMode.Deep);
			part.things.TryAddRangeOrTransfer(list, false, false);
			slate.Set<List<Thing>>("generatedItemStashThings", list, false);
			outExtraDescriptionRules.Add(new Rule_String("itemStashContents", GenLabel.ThingsLabel(list, "  - ")));
			outExtraDescriptionRules.Add(new Rule_String("itemStashContentsValue", GenThing.GetMarketValue(list).ToStringMoney(null)));
		}

		
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			string text = base.GetPostProcessedThreatLabel(site, sitePart);
			if (site.HasWorldObjectTimeout)
			{
				text += " (" + "DurationLeft".Translate(site.WorldObjectTimeoutTicksLeft.ToStringTicksToPeriod(true, false, true, true)) + ")";
			}
			return text;
		}
	}
}
