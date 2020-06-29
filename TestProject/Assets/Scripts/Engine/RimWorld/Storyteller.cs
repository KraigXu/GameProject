using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Storyteller : IExposable
	{
		
		// (get) Token: 0x06003CF0 RID: 15600 RVA: 0x00142900 File Offset: 0x00140B00
		public List<IIncidentTarget> AllIncidentTargets
		{
			get
			{
				Storyteller.tmpAllIncidentTargets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Storyteller.tmpAllIncidentTargets.Add(maps[i]);
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled)
					{
						Storyteller.tmpAllIncidentTargets.Add(caravans[j]);
					}
				}
				Storyteller.tmpAllIncidentTargets.Add(Find.World);
				return Storyteller.tmpAllIncidentTargets;
			}
		}

		
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		
		public Storyteller()
		{
		}

		
		public Storyteller(StorytellerDef def, DifficultyDef difficulty)
		{
			this.def = def;
			this.difficulty = difficulty;
			this.InitializeStorytellerComps();
		}

		
		private void InitializeStorytellerComps()
		{
			this.storytellerComps = new List<StorytellerComp>();
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				if (this.def.comps[i].Enabled)
				{
					StorytellerComp storytellerComp = (StorytellerComp)Activator.CreateInstance(this.def.comps[i].compClass);
					storytellerComp.props = this.def.comps[i];
					storytellerComp.Initialize();
					this.storytellerComps.Add(storytellerComp);
				}
			}
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<StorytellerDef>(ref this.def, "def");
			Scribe_Defs.Look<DifficultyDef>(ref this.difficulty, "difficulty");
			Scribe_Deep.Look<IncidentQueue>(ref this.incidentQueue, "incidentQueue", Array.Empty<object>());
			if (this.difficulty == null)
			{
				Log.Error("Loaded storyteller without difficulty", false);
				this.difficulty = DefDatabase<DifficultyDef>.AllDefsListForReading[3];
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.InitializeStorytellerComps();
			}
		}

		
		public void StorytellerTick()
		{
			this.incidentQueue.IncidentQueueTick();
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				if (!DebugSettings.enableStoryteller)
				{
					return;
				}
				foreach (FiringIncident fi in this.MakeIncidentsForInterval())
				{
					this.TryFire(fi);
				}
			}
		}

		
		public bool TryFire(FiringIncident fi)
		{
			if (fi.def.Worker.CanFireNow(fi.parms, false) && fi.def.Worker.TryExecute(fi.parms))
			{
				fi.parms.target.StoryState.Notify_IncidentFired(fi);
				return true;
			}
			return false;
		}

		
		public IEnumerable<FiringIncident> MakeIncidentsForInterval()
		{
			List<IIncidentTarget> targets = this.AllIncidentTargets;
			int num;
			for (int i = 0; i < this.storytellerComps.Count; i = num + 1)
			{
				foreach (FiringIncident firingIncident in this.MakeIncidentsForInterval(this.storytellerComps[i], targets))
				{
					yield return firingIncident;
				}
				IEnumerator<FiringIncident> enumerator = null;
				num = i;
			}
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (quests[i].State == QuestState.Ongoing)
				{
					List<QuestPart> parts = quests[i].PartsListForReading;
					for (int j = 0; j < parts.Count; j = num + 1)
					{
						IIncidentMakerQuestPart incidentMakerQuestPart = parts[j] as IIncidentMakerQuestPart;
						if (incidentMakerQuestPart != null && ((QuestPartActivable)parts[j]).State == QuestPartState.Enabled)
						{
							foreach (FiringIncident firingIncident2 in incidentMakerQuestPart.MakeIntervalIncidents())
							{
								firingIncident2.sourceQuestPart = parts[j];
								firingIncident2.parms.quest = quests[i];
								yield return firingIncident2;
							}
							IEnumerator<FiringIncident> enumerator = null;
						}
						num = j;
					}
					parts = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		
		public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
		{
			if (GenDate.DaysPassedFloat <= comp.props.minDaysPassed)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < targets.Count; i = num + 1)
			{
				IIncidentTarget incidentTarget = targets[i];
				bool flag = false;
				bool flag2 = comp.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>();
				foreach (IncidentTargetTagDef item in incidentTarget.IncidentTargetTags())
				{
					if (!comp.props.disallowedTargetTags.NullOrEmpty<IncidentTargetTagDef>() && comp.props.disallowedTargetTags.Contains(item))
					{
						flag = true;
						break;
					}
					if (!flag2 && comp.props.allowedTargetTags.Contains(item))
					{
						flag2 = true;
					}
				}
				if (!flag && flag2)
				{
					foreach (FiringIncident firingIncident in comp.MakeIntervalIncidents(incidentTarget))
					{
						if (Find.Storyteller.difficulty.allowBigThreats || firingIncident.def.category != IncidentCategoryDefOf.ThreatBig)
						{
							yield return firingIncident;
						}
					}
					IEnumerator<FiringIncident> enumerator2 = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		
		public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
			}
		}

		
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		
		public string DebugString()
		{
			if (Time.frameCount % 60 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("GLOBAL STORYTELLER STATS");
				stringBuilder.AppendLine("------------------------");
				stringBuilder.AppendLine("Storyteller: ".PadRight(40) + this.def.label);
				stringBuilder.AppendLine("Adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.AdaptDays.ToString("F1"));
				stringBuilder.AppendLine("Adapt points factor: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor.ToString("F2"));
				stringBuilder.AppendLine("Time points factor: ".PadRight(40) + Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassed).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Ally incident fraction (neutral or ally): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(false).ToString("F2"));
				stringBuilder.AppendLine("Ally incident fraction (ally only): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(true).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(StorytellerUtilityPopulation.DebugReadout().TrimEndNewlines());
				IIncidentTarget incidentTarget = Find.WorldSelector.SingleSelectedObject as IIncidentTarget;
				if (incidentTarget == null)
				{
					incidentTarget = Find.CurrentMap;
				}
				if (incidentTarget != null)
				{
					Map map = incidentTarget as Map;
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("STATS FOR INCIDENT TARGET: " + incidentTarget);
					stringBuilder.AppendLine("------------------------");
					stringBuilder.AppendLine("Progress score: ".PadRight(40) + StorytellerUtility.GetProgressScore(incidentTarget).ToString("F2"));
					stringBuilder.AppendLine("Base points: ".PadRight(40) + StorytellerUtility.DefaultThreatPointsNow(incidentTarget).ToString("F0"));
					stringBuilder.AppendLine("Points factor random range: ".PadRight(40) + incidentTarget.IncidentPointsRandomFactorRange);
					stringBuilder.AppendLine("Wealth: ".PadRight(40) + incidentTarget.PlayerWealthForStoryteller.ToString("F0"));
					if (map != null)
					{
						stringBuilder.AppendLine("- Items: ".PadRight(40) + map.wealthWatcher.WealthItems.ToString("F0"));
						stringBuilder.AppendLine("- Buildings: ".PadRight(40) + map.wealthWatcher.WealthBuildings.ToString("F0"));
						stringBuilder.AppendLine("- Floors: ".PadRight(40) + map.wealthWatcher.WealthFloorsOnly.ToString("F0"));
						stringBuilder.AppendLine("- Pawns: ".PadRight(40) + map.wealthWatcher.WealthPawns.ToString("F0"));
					}
					stringBuilder.AppendLine("Pawn count human: ".PadRight(40) + (from p in incidentTarget.PlayerPawnsForStoryteller
					where p.def.race.Humanlike
					select p).Count<Pawn>());
					stringBuilder.AppendLine("Pawn count animal: ".PadRight(40) + (from p in incidentTarget.PlayerPawnsForStoryteller
					where p.def.race.Animal
					select p).Count<Pawn>());
					if (map != null)
					{
						stringBuilder.AppendLine("StoryDanger: ".PadRight(40) + map.dangerWatcher.DangerRating);
						stringBuilder.AppendLine("FireDanger: ".PadRight(40) + map.fireWatcher.FireDanger.ToString("F2"));
						stringBuilder.AppendLine("LastThreatBigTick days ago: ".PadRight(40) + (Find.TickManager.TicksGame - map.storyState.LastThreatBigTick).ToStringTicksToDays("F1"));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("LIST OF ALL INCIDENT TARGETS");
				stringBuilder.AppendLine("------------------------");
				for (int i = 0; i < this.AllIncidentTargets.Count; i++)
				{
					stringBuilder.AppendLine(i + ". " + this.AllIncidentTargets[i].ToString());
				}
				this.debugStringCached = stringBuilder.ToString();
			}
			return this.debugStringCached;
		}

		
		public StorytellerDef def;

		
		public DifficultyDef difficulty;

		
		public List<StorytellerComp> storytellerComps;

		
		public IncidentQueue incidentQueue = new IncidentQueue();

		
		public static readonly Vector2 PortraitSizeTiny = new Vector2(116f, 124f);

		
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		
		public const int IntervalsPerDay = 60;

		
		public const int CheckInterval = 1000;

		
		private static List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		
		private string debugStringCached = "Generating data...";
	}
}
