﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000332 RID: 818
	public static class DebugActionsIncidents
	{
		// Token: 0x06001804 RID: 6148 RVA: 0x00088E7F File Offset: 0x0008707F
		[DebugActionYielder]
		private static IEnumerable<Dialog_DebugActionsMenu.DebugActionOption> IncidentsYielder()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				yield break;
			}
			IIncidentTarget target = WorldRendererUtility.WorldRenderedNow ? (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) : null;
			if (target == null)
			{
				target = Find.CurrentMap;
			}
			if (target != null)
			{
				yield return DebugActionsIncidents.GetIncidentDebugAction(target);
				yield return DebugActionsIncidents.GetIncidents10DebugAction(target);
				yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction(target);
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				yield return DebugActionsIncidents.GetIncidentDebugAction(Find.World);
				yield return DebugActionsIncidents.GetIncidentWithPointsDebugAction(Find.World);
			}
			yield break;
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00088E88 File Offset: 0x00087088
		[DebugAction("Incidents", "Execute raid with points...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithPoints()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(true))
			{
				float localP = localP2;
				list.Add(new FloatMenuOption(localP.ToString() + " points", delegate
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = localP;
					IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00088F28 File Offset: 0x00087128
		[DebugAction("Incidents", "Execute raid with faction...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithFaction()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Func<RaidStrategyDef, bool> <>9__3;
			Func<PawnsArrivalModeDef, bool> <>9__5;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate
						{
							parms.points = localPoints;
							IEnumerable<RaidStrategyDef> allDefs = DefDatabase<RaidStrategyDef>.AllDefs;
							Func<RaidStrategyDef, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((RaidStrategyDef s) => s.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat)));
							}
							List<RaidStrategyDef> source = allDefs.Where(predicate).ToList<RaidStrategyDef>();
							Log.Message("Available strategies: " + string.Join(", ", (from s in source
							select s.defName).ToArray<string>()), false);
							parms.raidStrategy = source.RandomElement<RaidStrategyDef>();
							Log.Message("Strategy: " + parms.raidStrategy.defName, false);
							IEnumerable<PawnsArrivalModeDef> allDefs2 = DefDatabase<PawnsArrivalModeDef>.AllDefs;
							Func<PawnsArrivalModeDef, bool> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((PawnsArrivalModeDef a) => a.Worker.CanUseWith(parms) && parms.raidStrategy.arriveModes.Contains(a)));
							}
							List<PawnsArrivalModeDef> source2 = allDefs2.Where(predicate2).ToList<PawnsArrivalModeDef>();
							Log.Message("Available arrival modes: " + string.Join(", ", (from s in source2
							select s.defName).ToArray<string>()), false);
							parms.raidArrivalMode = source2.RandomElement<PawnsArrivalModeDef>();
							Log.Message("Arrival mode: " + parms.raidArrivalMode.defName, false);
							DebugActionsIncidents.DoRaid(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00089034 File Offset: 0x00087234
		[DebugAction("Incidents", "Execute raid with specifics...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExecuteRaidWithSpecifics()
		{
			StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
			IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action <>9__4;
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate
				{
					parms.faction = localFac;
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate
						{
							parms.points = localPoints;
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							foreach (RaidStrategyDef localStrat2 in DefDatabase<RaidStrategyDef>.AllDefs)
							{
								RaidStrategyDef localStrat = localStrat2;
								string text = localStrat.defName;
								if (!localStrat.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat))
								{
									text += " [NO]";
								}
								list3.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
								{
									parms.raidStrategy = localStrat;
									List<DebugMenuOption> list4 = new List<DebugMenuOption>();
									List<DebugMenuOption> list5 = list4;
									string label = "-Random-";
									DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
									Action method;
									if ((method = <>9__4) == null)
									{
										method = (<>9__4 = delegate
										{
											DebugActionsIncidents.DoRaid(parms);
										});
									}
									list5.Add(new DebugMenuOption(label, mode, method));
									foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
									{
										PawnsArrivalModeDef localArrival = localArrival2;
										string text2 = localArrival.defName;
										if (!localArrival.Worker.CanUseWith(parms) || !localStrat.arriveModes.Contains(localArrival))
										{
											text2 += " [NO]";
										}
										list4.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate
										{
											parms.raidArrivalMode = localArrival;
											DebugActionsIncidents.DoRaid(parms);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00089140 File Offset: 0x00087340
		private static string GetIncidentTargetLabel(IIncidentTarget target)
		{
			if (target == null)
			{
				return "null target";
			}
			if (target is Map)
			{
				return "Map";
			}
			if (target is World)
			{
				return "World";
			}
			if (target is Caravan)
			{
				return ((Caravan)target).LabelCap;
			}
			return target.ToString();
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0008918C File Offset: 0x0008738C
		private static Dialog_DebugActionsMenu.DebugActionOption GetIncidentDebugAction(IIncidentTarget target)
		{
			return new Dialog_DebugActionsMenu.DebugActionOption
			{
				action = delegate
				{
					DebugActionsIncidents.DoIncidentDebugAction(target, 1);
				},
				actionType = DebugActionType.Action,
				category = "Incidents",
				label = "Do incident (" + DebugActionsIncidents.GetIncidentTargetLabel(target) + ")..."
			};
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000891F8 File Offset: 0x000873F8
		private static Dialog_DebugActionsMenu.DebugActionOption GetIncidents10DebugAction(IIncidentTarget target)
		{
			return new Dialog_DebugActionsMenu.DebugActionOption
			{
				action = delegate
				{
					DebugActionsIncidents.DoIncidentDebugAction(target, 10);
				},
				actionType = DebugActionType.Action,
				category = "Incidents",
				label = "Do incident x10 (" + DebugActionsIncidents.GetIncidentTargetLabel(target) + ")..."
			};
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00089264 File Offset: 0x00087464
		private static void DoIncidentDebugAction(IIncidentTarget target, int iterations = 1)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<IncidentDef> allDefs = DefDatabase<IncidentDef>.AllDefs;
			Func<IncidentDef, bool> <>9__0;
			Func<IncidentDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((IncidentDef d) => d.TargetAllowed(target)));
			}
			foreach (IncidentDef localDef2 in from d in allDefs.Where(predicate)
			orderby d.defName
			select d)
			{
				IncidentDef localDef = localDef2;
				string text = localDef.defName;
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
				if (!localDef.Worker.CanFireNow(parms, false))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
				{
					for (int i = 0; i < iterations; i++)
					{
						IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
						if (localDef.pointsScaleable)
						{
							parms = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain).GenerateParms(localDef.category, parms.target);
						}
						localDef.Worker.TryExecute(parms);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000893B0 File Offset: 0x000875B0
		private static Dialog_DebugActionsMenu.DebugActionOption GetIncidentWithPointsDebugAction(IIncidentTarget target)
		{
			return new Dialog_DebugActionsMenu.DebugActionOption
			{
				action = delegate
				{
					DebugActionsIncidents.DoIncidentWithPointsAction(target);
				},
				actionType = DebugActionType.Action,
				category = "Incidents",
				label = "Do incident w/ points (" + DebugActionsIncidents.GetIncidentTargetLabel(target) + ")..."
			};
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0008941C File Offset: 0x0008761C
		private static void DoIncidentWithPointsAction(IIncidentTarget target)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<IncidentDef> allDefs = DefDatabase<IncidentDef>.AllDefs;
			Func<IncidentDef, bool> <>9__0;
			Func<IncidentDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((IncidentDef d) => d.TargetAllowed(target) && d.pointsScaleable));
			}
			foreach (IncidentDef localDef2 in from d in allDefs.Where(predicate)
			orderby d.defName
			select d)
			{
				IncidentDef localDef = localDef2;
				string text = localDef.defName;
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
				if (!localDef.Worker.CanFireNow(parms, false))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float num in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = num;
						list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate
						{
							parms.points = localPoints;
							localDef.Worker.TryExecute(parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00089554 File Offset: 0x00087754
		private static void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				incidentDef = IncidentDefOf.RaidEnemy;
			}
			else
			{
				incidentDef = IncidentDefOf.RaidFriendly;
			}
			incidentDef.Worker.TryExecute(parms);
		}
	}
}
