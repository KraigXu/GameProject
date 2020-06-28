using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020003F6 RID: 1014
	public class BackCompatibilityConverter_1_0 : BackCompatibilityConverter
	{
		// Token: 0x06001E2B RID: 7723 RVA: 0x000BB8B3 File Offset: 0x000B9AB3
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer <= 1 && (majorVer == 0 || minorVer == 0);
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000BB8C4 File Offset: 0x000B9AC4
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef))
			{
				if (defName == "CrashedPoisonShipPart" || defName == "CrashedPsychicEmanatorShipPart")
				{
					return "MechCapsule";
				}
				if (defName == "PoisonSpreader")
				{
					return "Defoliator";
				}
				if (defName == "PoisonSpreaderShipPart")
				{
					return "DefoliatorShipPart";
				}
				if (defName == "MechSerumNeurotrainer")
				{
					XmlNode xmlNode = (node != null) ? node.ParentNode : null;
					if (xmlNode != null && xmlNode.HasChildNodes)
					{
						foreach (object obj in xmlNode.ChildNodes)
						{
							XmlNode xmlNode2 = (XmlNode)obj;
							if (xmlNode2.Name == "skill")
							{
								return NeurotrainerDefGenerator.NeurotrainerDefPrefix + "_" + xmlNode2.InnerText;
							}
						}
					}
					ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefsListForReading
					where def.thingCategories != null && def.thingCategories.Contains(ThingCategoryDefOf.Neurotrainers)
					select def).RandomElementWithFallback(null);
					if (thingDef == null)
					{
						return null;
					}
					return thingDef.defName;
				}
			}
			else if ((defType == typeof(QuestScriptDef) || defType == typeof(TaleDef)) && defName == "JourneyOffer")
			{
				return "EndGame_ShipEscape";
			}
			return null;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000BBA40 File Offset: 0x000B9C40
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (baseType == typeof(Thing))
			{
				if (providedClassName == "Building_CrashedShipPart" && node["def"] != null)
				{
					BackCompatibilityConverter_1_0.oldCrashedShipParts.Add(node);
					return ThingDefOf.MechCapsule.thingClass;
				}
			}
			else if (baseType == typeof(LordJob) && providedClassName == "LordJob_MechanoidsDefendShip")
			{
				XmlElement xmlElement = node["shipPart"];
				if (xmlElement != null)
				{
					xmlElement.InnerText = xmlElement.InnerText.Replace("Thing_CrashedPsychicEmanatorShipPart", "Thing_MechCapsule").Replace("Thing_CrashedPoisonShipPart", "Thing_MechCapsule");
				}
				return typeof(LordJob_MechanoidsDefend);
			}
			return null;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000BBAF8 File Offset: 0x000B9CF8
		public override int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (body != BodyDefOf.MechanicalCentipede)
			{
				return index;
			}
			switch (index)
			{
			case 9:
				return 10;
			case 10:
				return 12;
			case 11:
				return 14;
			case 12:
				return 15;
			case 13:
				return 9;
			case 14:
				return 11;
			case 15:
				return 13;
			default:
				return index;
			}
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000BBB4C File Offset: 0x000B9D4C
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.questManager == null)
				{
					game.questManager = new QuestManager();
				}
				Zone zone = obj as Zone;
				if (zone != null && zone.ID == -1)
				{
					zone.ID = Find.UniqueIDsManager.GetNextZoneID();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.royalty == null)
				{
					pawn.royalty = new Pawn_RoyaltyTracker(pawn);
				}
				Pawn_NativeVerbs pawn_NativeVerbs = obj as Pawn_NativeVerbs;
				if (pawn_NativeVerbs != null && pawn_NativeVerbs.verbTracker == null)
				{
					pawn_NativeVerbs.verbTracker = new VerbTracker(pawn_NativeVerbs);
				}
				Thing thing = obj as Thing;
				if (thing != null)
				{
					if (thing.def.defName == "Sandbags" && thing.Stuff == null)
					{
						thing.SetStuffDirect(ThingDefOf.Cloth);
					}
					if (thing.def == ThingDefOf.MechCapsule)
					{
						foreach (XmlNode xmlNode in BackCompatibilityConverter_1_0.oldCrashedShipParts)
						{
							XmlElement xmlElement = xmlNode["def"];
							XmlElement xmlElement2 = xmlNode["id"];
							if (xmlElement != null && xmlElement2 != null && Thing.IDNumberFromThingID(xmlElement2.InnerText) == thing.thingIDNumber)
							{
								BackCompatibilityConverter_1_0.upgradedCrashedShipParts.Add(new BackCompatibilityConverter_1_0.UpgradedCrashedShipPart
								{
									originalDefName = xmlElement.InnerText,
									thing = thing
								});
							}
						}
					}
				}
				StoryWatcher storyWatcher = obj as StoryWatcher;
				if (storyWatcher != null)
				{
					if (storyWatcher.watcherAdaptation == null)
					{
						storyWatcher.watcherAdaptation = new StoryWatcher_Adaptation();
					}
					if (storyWatcher.watcherPopAdaptation == null)
					{
						storyWatcher.watcherPopAdaptation = new StoryWatcher_PopAdaptation();
					}
				}
				FoodRestrictionDatabase foodRestrictionDatabase = obj as FoodRestrictionDatabase;
				if (foodRestrictionDatabase != null && VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) < 2057)
				{
					List<FoodRestriction> allFoodRestrictions = foodRestrictionDatabase.AllFoodRestrictions;
					for (int i = 0; i < allFoodRestrictions.Count; i++)
					{
						allFoodRestrictions[i].filter.SetAllow(ThingCategoryDefOf.CorpsesHumanlike, true, null, null);
						allFoodRestrictions[i].filter.SetAllow(ThingCategoryDefOf.CorpsesAnimal, true, null, null);
					}
				}
				SitePart sitePart = obj as SitePart;
				if (sitePart != null)
				{
					sitePart.hidden = sitePart.def.defaultHidden;
				}
			}
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000BBD9C File Offset: 0x000B9F9C
		public static Quest MakeAndAddWorldObjectQuest(WorldObject destination, string description)
		{
			Quest quest = Quest.MakeRaw();
			quest.SetInitiallyAccepted();
			QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, quest.InitiateSignal, QuestEndOutcome.Unknown, null).worldObject = destination;
			quest.description = description;
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000BBDE4 File Offset: 0x000B9FE4
		public static Quest MakeAndAddTradeRequestQuest(WorldObject destination, string description, TradeRequestComp tradeRequest)
		{
			Quest quest = Quest.MakeRaw();
			quest.SetInitiallyAccepted();
			string text = "Quest" + quest.id + ".TradeRequestSite";
			QuestUtility.AddQuestTag(ref destination.questTags, text);
			QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, quest.InitiateSignal, QuestEndOutcome.Unknown, null).worldObject = destination;
			QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, text + ".TradeRequestFulfilled", QuestEndOutcome.Success, null);
			if (destination.rewards != null)
			{
				QuestPart_GiveToCaravan questPart_GiveToCaravan = new QuestPart_GiveToCaravan
				{
					inSignal = text + ".TradeRequestFulfilled",
					Things = destination.rewards
				};
				foreach (Thing thing in questPart_GiveToCaravan.Things)
				{
					thing.holdingOwner = null;
				}
				quest.AddPart(questPart_GiveToCaravan);
			}
			quest.description = description;
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000BBED4 File Offset: 0x000BA0D4
		public override void PreLoadSavegame(string loadingVersion)
		{
			BackCompatibilityConverter_1_0.oldCrashedShipParts.Clear();
			BackCompatibilityConverter_1_0.upgradedCrashedShipParts.Clear();
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000BBEEC File Offset: 0x000BA0EC
		public override void PostLoadSavegame(string loadingVersion)
		{
			BackCompatibilityConverter_1_0.<>c__DisplayClass11_0 <>c__DisplayClass11_ = new BackCompatibilityConverter_1_0.<>c__DisplayClass11_0();
			BackCompatibilityConverter_1_0.oldCrashedShipParts.Clear();
			foreach (BackCompatibilityConverter_1_0.UpgradedCrashedShipPart upgradedCrashedShipPart in BackCompatibilityConverter_1_0.upgradedCrashedShipParts)
			{
				Thing thing = upgradedCrashedShipPart.thing;
				IntVec3 intVec = IntVec3.Invalid;
				Map map;
				if (thing.Spawned)
				{
					intVec = thing.Position;
					map = thing.Map;
				}
				else
				{
					Skyfaller skyfaller = thing.ParentHolder as Skyfaller;
					if (skyfaller == null)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
					intVec = skyfaller.Position;
					map = skyfaller.Map;
				}
				if (!(intVec == IntVec3.Invalid))
				{
					intVec = new IntVec3(intVec.x - Mathf.CeilToInt((float)thing.def.size.x / 2f), intVec.y, intVec.z);
					Thing item = null;
					if (upgradedCrashedShipPart.originalDefName == "CrashedPoisonShipPart" || upgradedCrashedShipPart.originalDefName == "PoisonSpreaderShipPart")
					{
						item = ThingMaker.MakeThing(ThingDefOf.DefoliatorShipPart, null);
					}
					else if (upgradedCrashedShipPart.originalDefName == "CrashedPsychicEmanatorShipPart")
					{
						item = ThingMaker.MakeThing(ThingDefOf.PsychicDronerShipPart, null);
					}
					ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
					activeDropPodInfo.innerContainer.TryAdd(item, 1, true);
					activeDropPodInfo.openDelay = 60;
					activeDropPodInfo.leaveSlag = false;
					activeDropPodInfo.despawnPodBeforeSpawningThing = true;
					activeDropPodInfo.spawnWipeMode = new WipeMode?(WipeMode.Vanish);
					DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
				}
			}
			BackCompatibilityConverter_1_0.upgradedCrashedShipParts.Clear();
			<>c__DisplayClass11_.sites = Find.WorldObjects.Sites;
			int k;
			int j;
			for (k = 0; k < <>c__DisplayClass11_.sites.Count; k = j + 1)
			{
				if (!Find.QuestManager.QuestsListForReading.Any((Quest x) => x.QuestLookTargets.Contains(<>c__DisplayClass11_.sites[k])))
				{
					Quest quest = Quest.MakeRaw();
					QuestUtility.GenerateBackCompatibilityNameFor(quest);
					quest.SetInitiallyAccepted();
					quest.appearanceTick = <>c__DisplayClass11_.sites[k].creationGameTicks;
					TimeoutComp component = <>c__DisplayClass11_.sites[k].GetComponent<TimeoutComp>();
					if (component != null && component.Active && !<>c__DisplayClass11_.sites[k].HasMap)
					{
						QuestPartUtility.MakeAndAddQuestTimeoutDelay(quest, component.TicksLeft, <>c__DisplayClass11_.sites[k]);
						component.StopTimeout();
					}
					QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, quest.InitiateSignal, QuestEndOutcome.Unknown, null).worldObject = <>c__DisplayClass11_.sites[k];
					ChoiceLetter choiceLetter = Find.Archive.ArchivablesListForReading.OfType<ChoiceLetter>().FirstOrDefault((ChoiceLetter x) => x.lookTargets != null && x.lookTargets.targets.Contains(<>c__DisplayClass11_.sites[k]));
					if (choiceLetter != null)
					{
						quest.description = choiceLetter.text;
					}
					Find.QuestManager.Add(quest);
				}
				j = k;
			}
			<>c__DisplayClass11_.worldObjects = Find.WorldObjects.AllWorldObjects;
			int l;
			Predicate<QuestPart> <>9__3;
			for (l = 0; l < <>c__DisplayClass11_.worldObjects.Count; l = j + 1)
			{
				if (<>c__DisplayClass11_.worldObjects[l].def == WorldObjectDefOf.EscapeShip && !Find.QuestManager.QuestsListForReading.Any(delegate(Quest x)
				{
					List<QuestPart> partsListForReading = x.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = ((QuestPart y) => y is QuestPart_NoWorldObject && ((QuestPart_NoWorldObject)y).worldObject == <>c__DisplayClass11_.worldObjects[l]));
					}
					return partsListForReading.Any(predicate);
				}))
				{
					BackCompatibilityConverter_1_0.MakeAndAddWorldObjectQuest(<>c__DisplayClass11_.worldObjects[l], null);
				}
				j = l;
			}
			int m;
			Predicate<QuestPart> <>9__5;
			for (m = 0; m < <>c__DisplayClass11_.worldObjects.Count; m = j + 1)
			{
				if (<>c__DisplayClass11_.worldObjects[m] is PeaceTalks && !Find.QuestManager.QuestsListForReading.Any(delegate(Quest x)
				{
					List<QuestPart> partsListForReading = x.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__5) == null)
					{
						predicate = (<>9__5 = ((QuestPart y) => y is QuestPart_NoWorldObject && ((QuestPart_NoWorldObject)y).worldObject == <>c__DisplayClass11_.worldObjects[m]));
					}
					return partsListForReading.Any(predicate);
				}))
				{
					Quest quest2 = BackCompatibilityConverter_1_0.MakeAndAddWorldObjectQuest(<>c__DisplayClass11_.worldObjects[m], null);
					ChoiceLetter choiceLetter2 = Find.Archive.ArchivablesListForReading.OfType<ChoiceLetter>().FirstOrDefault((ChoiceLetter x) => x.lookTargets != null && x.lookTargets.targets.Contains(<>c__DisplayClass11_.worldObjects[m]));
					if (choiceLetter2 != null)
					{
						quest2.description = choiceLetter2.text;
					}
				}
				j = m;
			}
			int i;
			Predicate<QuestPart> <>9__8;
			for (i = 0; i < <>c__DisplayClass11_.worldObjects.Count; i = j + 1)
			{
				TradeRequestComp component2 = <>c__DisplayClass11_.worldObjects[i].GetComponent<TradeRequestComp>();
				if (component2 != null && component2.ActiveRequest && !Find.QuestManager.QuestsListForReading.Any(delegate(Quest x)
				{
					List<QuestPart> partsListForReading = x.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__8) == null)
					{
						predicate = (<>9__8 = ((QuestPart y) => y is QuestPart_NoWorldObject && ((QuestPart_NoWorldObject)y).worldObject == <>c__DisplayClass11_.worldObjects[i]));
					}
					return partsListForReading.Any(predicate);
				}))
				{
					Quest quest3 = BackCompatibilityConverter_1_0.MakeAndAddTradeRequestQuest(<>c__DisplayClass11_.worldObjects[i], null, component2);
					ChoiceLetter choiceLetter3 = Find.Archive.ArchivablesListForReading.OfType<ChoiceLetter>().FirstOrDefault((ChoiceLetter x) => x.lookTargets != null && x.lookTargets.targets.Contains(<>c__DisplayClass11_.worldObjects[i]));
					if (choiceLetter3 != null)
					{
						quest3.description = choiceLetter3.text;
					}
				}
				j = i;
			}
		}

		// Token: 0x0400126D RID: 4717
		private static List<XmlNode> oldCrashedShipParts = new List<XmlNode>();

		// Token: 0x0400126E RID: 4718
		private static List<BackCompatibilityConverter_1_0.UpgradedCrashedShipPart> upgradedCrashedShipParts = new List<BackCompatibilityConverter_1_0.UpgradedCrashedShipPart>();

		// Token: 0x02001656 RID: 5718
		private struct UpgradedCrashedShipPart
		{
			// Token: 0x040055BE RID: 21950
			public string originalDefName;

			// Token: 0x040055BF RID: 21951
			public Thing thing;
		}
	}
}
