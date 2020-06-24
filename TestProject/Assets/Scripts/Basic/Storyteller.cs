using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A03 RID: 2563
	public class Storyteller : IExposable
	{
		// Token: 0x17000ACF RID: 2767
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

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0014298E File Offset: 0x00140B8E
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0014299A File Offset: 0x00140B9A
		public Storyteller()
		{
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x001429B8 File Offset: 0x00140BB8
		public Storyteller(StorytellerDef def, DifficultyDef difficulty)
		{
			this.def = def;
			this.difficulty = difficulty;
			this.InitializeStorytellerComps();
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x001429EC File Offset: 0x00140BEC
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

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00142A84 File Offset: 0x00140C84
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

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00142AF8 File Offset: 0x00140CF8
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

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00142B6C File Offset: 0x00140D6C
		public bool TryFire(FiringIncident fi)
		{
			if (fi.def.Worker.CanFireNow(fi.parms, false) && fi.def.Worker.TryExecute(fi.parms))
			{
				fi.parms.target.StoryState.Notify_IncidentFired(fi);
				return true;
			}
			return false;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00142BC3 File Offset: 0x00140DC3
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

		// Token: 0x06003CF9 RID: 15609 RVA: 0x00142BD3 File Offset: 0x00140DD3
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

		// Token: 0x06003CFA RID: 15610 RVA: 0x00142BEC File Offset: 0x00140DEC
		public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x00142C35 File Offset: 0x00140E35
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x00142C40 File Offset: 0x00140E40
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

		// Token: 0x04002399 RID: 9113
		public StorytellerDef def;

		// Token: 0x0400239A RID: 9114
		public DifficultyDef difficulty;

		// Token: 0x0400239B RID: 9115
		public List<StorytellerComp> storytellerComps;

		// Token: 0x0400239C RID: 9116
		public IncidentQueue incidentQueue = new IncidentQueue();

		// Token: 0x0400239D RID: 9117
		public static readonly Vector2 PortraitSizeTiny = new Vector2(116f, 124f);

		// Token: 0x0400239E RID: 9118
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		// Token: 0x0400239F RID: 9119
		public const int IntervalsPerDay = 60;

		// Token: 0x040023A0 RID: 9120
		public const int CheckInterval = 1000;

		// Token: 0x040023A1 RID: 9121
		private static List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		// Token: 0x040023A2 RID: 9122
		private string debugStringCached = "Generating data...";
	}
}
