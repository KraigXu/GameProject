﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker
	{
		
		
		public virtual float BaseChanceThisGame
		{
			get
			{
				if (ModsConfig.RoyaltyActive && this.def.baseChanceWithRoyalty >= 0f)
				{
					return this.def.baseChanceWithRoyalty;
				}
				return this.def.baseChance;
			}
		}

		
		public bool CanFireNow(IncidentParms parms, bool forced = false)
		{
			if (!parms.forced)
			{
				if (!this.def.TargetAllowed(parms.target))
				{
					return false;
				}
				if (GenDate.DaysPassed < this.def.earliestDay)
				{
					return false;
				}
				if (Find.Storyteller.difficulty.difficulty < this.def.minDifficulty)
				{
					return false;
				}
				if (parms.points >= 0f && parms.points < this.def.minThreatPoints)
				{
					return false;
				}
				if (this.def.allowedBiomes != null)
				{
					BiomeDef biome = Find.WorldGrid[parms.target.Tile].biome;
					if (!this.def.allowedBiomes.Contains(biome))
					{
						return false;
					}
				}
				Scenario scenario = Find.Scenario;
				for (int i = 0; i < scenario.parts.Count; i++)
				{
					ScenPart_DisableIncident scenPart_DisableIncident = scenario.parts[i] as ScenPart_DisableIncident;
					if (scenPart_DisableIncident != null && scenPart_DisableIncident.Incident == this.def)
					{
						return false;
					}
				}
				if (this.def.minPopulation > 0 && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>() < this.def.minPopulation)
				{
					return false;
				}
				if (this.FiredTooRecently(parms.target))
				{
					return false;
				}
				if (this.def.minGreatestPopulation > 0 && Find.StoryWatcher.statsRecord.greatestPopulation < this.def.minGreatestPopulation)
				{
					return false;
				}
			}
			return this.CanFireNowSub(parms);
		}

		
		public bool FiredTooRecently(IIncidentTarget target)
		{
			Dictionary<IncidentDef, int> lastFireTicks = target.StoryState.lastFireTicks;
			int ticksGame = Find.TickManager.TicksGame;
			int num;
			if (lastFireTicks.TryGetValue(this.def, out num) && (float)(ticksGame - num) / 60000f < this.def.minRefireDays)
			{
				return true;
			}
			List<IncidentDef> refireCheckIncidents = this.def.RefireCheckIncidents;
			if (refireCheckIncidents != null)
			{
				for (int i = 0; i < refireCheckIncidents.Count; i++)
				{
					if (lastFireTicks.TryGetValue(refireCheckIncidents[i], out num) && (float)(ticksGame - num) / 60000f < this.def.minRefireDays)
					{
						return true;
					}
				}
			}
			return false;
		}

		
		protected virtual bool CanFireNowSub(IncidentParms parms)
		{
			return true;
		}

		
		public bool TryExecute(IncidentParms parms)
		{
			Map map;
			if ((map = (parms.target as Map)) != null && this.def.requireColonistsPresent && map.mapPawns.FreeColonistsSpawnedCount == 0)
			{
				return true;
			}
			bool flag = this.TryExecuteWorker(parms);
			if (flag)
			{
				if (this.def.tale != null)
				{
					Pawn pawn = null;
					if (parms.target is Caravan)
					{
						pawn = ((Caravan)parms.target).RandomOwner();
					}
					else if (parms.target is Map)
					{
						pawn = ((Map)parms.target).mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
					}
					else if (parms.target is World)
					{
						pawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.RandomElementWithFallback(null);
					}
					if (pawn != null)
					{
						TaleRecorder.RecordTale(this.def.tale, new object[]
						{
							pawn
						});
					}
				}
				if (this.def.category.tale != null)
				{
					Tale tale = TaleRecorder.RecordTale(this.def.category.tale, Array.Empty<object>());
					if (tale != null)
					{
						tale.customLabel = this.def.label;
					}
				}
			}
			return flag;
		}

		
		protected virtual bool TryExecuteWorker(IncidentParms parms)
		{
			Log.Error("Unimplemented incident " + this, false);
			return false;
		}

		
		protected void SendStandardLetter(IncidentParms parms, LookTargets lookTargets, params NamedArgument[] textArgs)
		{
			this.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, lookTargets, textArgs);
		}

		
		protected void SendStandardLetter(TaggedString baseLetterLabel, TaggedString baseLetterText, LetterDef baseLetterDef, IncidentParms parms, LookTargets lookTargets, params NamedArgument[] textArgs)
		{
			if (baseLetterLabel.NullOrEmpty() || baseLetterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.", false);
			}
			TaggedString taggedString = baseLetterText.Formatted(textArgs);
			TaggedString text;
			if (parms.customLetterText.NullOrEmpty())
			{
				text = taggedString;
			}
			else
			{
				List<NamedArgument> list = new List<NamedArgument>();
				if (textArgs != null)
				{
					list.AddRange(textArgs);
				}
				list.Add(taggedString.Named("BASETEXT"));
				text = parms.customLetterText.Formatted(list.ToArray());
			}
			TaggedString taggedString2 = baseLetterLabel.Formatted(textArgs);
			TaggedString label;
			if (parms.customLetterLabel.NullOrEmpty())
			{
				label = taggedString2;
			}
			else
			{
				List<NamedArgument> list2 = new List<NamedArgument>();
				if (textArgs != null)
				{
					list2.AddRange(textArgs);
				}
				list2.Add(taggedString2.Named("BASELABEL"));
				label = parms.customLetterLabel.Formatted(list2.ToArray());
			}
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, parms.customLetterDef ?? baseLetterDef, lookTargets, parms.faction, parms.quest, parms.letterHyperlinkThingDefs);
			List<HediffDef> list3 = new List<HediffDef>();
			if (!parms.letterHyperlinkHediffDefs.NullOrEmpty<HediffDef>())
			{
				list3.AddRange(parms.letterHyperlinkHediffDefs);
			}
			if (!this.def.letterHyperlinkHediffDefs.NullOrEmpty<HediffDef>())
			{
				if (list3 == null)
				{
					list3 = new List<HediffDef>();
				}
				list3.AddRange(this.def.letterHyperlinkHediffDefs);
			}
			choiceLetter.hyperlinkHediffDefs = list3;
			Find.LetterStack.ReceiveLetter(choiceLetter, null);
		}

		
		public IncidentDef def;
	}
}
