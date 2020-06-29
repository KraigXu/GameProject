﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class GameCondition : IExposable, ILoadReferenceable
	{
		
		
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		
		
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		
		
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		
		
		public virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		
		
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		
		
		public virtual bool ElectricityDisabled
		{
			get
			{
				return false;
			}
		}

		
		
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		
		
		public virtual string Description
		{
			get
			{
				return this.def.description;
			}
		}

		
		
		public virtual int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		
		
		
		public int TicksLeft
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654, false);
					return 360000000;
				}
				return this.Duration - this.TicksPassed;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		
		
		
		public bool Permanent
		{
			get
			{
				return this.permanent;
			}
			set
			{
				if (value)
				{
					this.duration = -1;
				}
				this.permanent = value;
			}
		}

		
		
		
		public int Duration
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867, false);
					return 360000000;
				}
				return this.duration;
			}
			set
			{
				this.permanent = false;
				this.duration = value;
			}
		}

		
		
		public virtual string TooltipString
		{
			get
			{
				string text = this.def.LabelCap;
				if (this.Permanent)
				{
					text += "\n" + "Permanent".Translate().CapitalizeFirst();
				}
				else
				{
					Vector2 location;
					if (this.SingleMap != null)
					{
						location = Find.WorldGrid.LongLatOf(this.SingleMap.Tile);
					}
					else if (Find.CurrentMap != null)
					{
						location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
					}
					else if (Find.AnyPlayerHomeMap != null)
					{
						location = Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
					}
					else
					{
						location = Vector2.zero;
					}
					text += "\n" + "Started".Translate() + ": " + GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick), location);
					text += "\n" + "Lasted".Translate() + ": " + this.TicksPassed.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor);
				}
				text += "\n";
				text = text + "\n" + this.Description;
				text += "\n";
				text += "\n";
				if (this.conditionCauser != null && CameraJumper.CanJump(this.conditionCauser))
				{
					text += this.def.jumpToSourceKey.Translate();
				}
				else if (this.quest != null)
				{
					text += "CausedByQuest".Translate(this.quest.name);
				}
				else
				{
					text += "SourceUnknown".Translate();
				}
				return text;
			}
		}

		
		
		public List<Map> AffectedMaps
		{
			get
			{
				if (!GenCollection.ListsEqual<Map>(this.cachedAffectedMapsForMaps, Find.Maps))
				{
					this.cachedAffectedMapsForMaps.Clear();
					this.cachedAffectedMapsForMaps.AddRange(Find.Maps);
					this.cachedAffectedMaps.Clear();
					if (this.gameConditionManager.ownerMap != null)
					{
						this.cachedAffectedMaps.Add(this.gameConditionManager.ownerMap);
					}
					GameCondition.tmpGameConditionManagers.Clear();
					this.gameConditionManager.GetChildren(GameCondition.tmpGameConditionManagers);
					for (int i = 0; i < GameCondition.tmpGameConditionManagers.Count; i++)
					{
						if (GameCondition.tmpGameConditionManagers[i].ownerMap != null)
						{
							this.cachedAffectedMaps.Add(GameCondition.tmpGameConditionManagers[i].ownerMap);
						}
					}
					GameCondition.tmpGameConditionManagers.Clear();
				}
				return this.cachedAffectedMaps;
			}
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<bool>(ref this.suppressEndMessage, "suppressEndMessage", false, false);
			Scribe_Defs.Look<GameConditionDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			BackCompatibility.PostExposeData(this);
		}

		
		public virtual void GameConditionTick()
		{
		}

		
		public virtual void GameConditionDraw(Map map)
		{
		}

		
		public virtual void Init()
		{
		}

		
		public virtual void End()
		{
			if (!this.suppressEndMessage && this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		
		public virtual WeatherDef ForcedWeather()
		{
			return null;
		}

		
		public virtual void PostMake()
		{
			this.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
		}

		
		public virtual void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
		}

		
		public string GetUniqueLoadID()
		{
			return string.Format("{0}_{1}", base.GetType().Name, this.uniqueID.ToString());
		}

		
		public GameConditionManager gameConditionManager;

		
		public Thing conditionCauser;

		
		public GameConditionDef def;

		
		public int uniqueID = -1;

		
		public int startTick;

		
		public bool suppressEndMessage;

		
		private int duration = -1;

		
		private bool permanent;

		
		private List<Map> cachedAffectedMaps = new List<Map>();

		
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		
		public Quest quest;

		
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
