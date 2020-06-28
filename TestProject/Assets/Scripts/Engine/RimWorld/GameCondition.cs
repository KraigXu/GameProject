using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020009B2 RID: 2482
	public class GameCondition : IExposable, ILoadReferenceable
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x00138FBC File Offset: 0x001371BC
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003B15 RID: 15125 RVA: 0x00138FC9 File Offset: 0x001371C9
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x00138FD6 File Offset: 0x001371D6
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003B17 RID: 15127 RVA: 0x00138FE9 File Offset: 0x001371E9
		public virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x00138FF6 File Offset: 0x001371F6
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ElectricityDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x0013901B File Offset: 0x0013721B
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003B1B RID: 15131 RVA: 0x0013902E File Offset: 0x0013722E
		public virtual string Description
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x0013903B File Offset: 0x0013723B
		public virtual int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003B1D RID: 15133 RVA: 0x00139042 File Offset: 0x00137242
		// (set) Token: 0x06003B1E RID: 15134 RVA: 0x0013906F File Offset: 0x0013726F
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

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x0013907F File Offset: 0x0013727F
		// (set) Token: 0x06003B20 RID: 15136 RVA: 0x00139087 File Offset: 0x00137287
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

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x0013909A File Offset: 0x0013729A
		// (set) Token: 0x06003B22 RID: 15138 RVA: 0x001390C0 File Offset: 0x001372C0
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

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x001390D0 File Offset: 0x001372D0
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

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x001392C4 File Offset: 0x001374C4
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

		// Token: 0x06003B25 RID: 15141 RVA: 0x0013939C File Offset: 0x0013759C
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

		// Token: 0x06003B26 RID: 15142 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Init()
		{
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0013942A File Offset: 0x0013762A
		public virtual void End()
		{
			if (!this.suppressEndMessage && this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0013946C File Offset: 0x0013766C
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual WeatherDef ForcedWeather()
		{
			return null;
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x00139482 File Offset: 0x00137682
		public virtual void PostMake()
		{
			this.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x00139494 File Offset: 0x00137694
		public string GetUniqueLoadID()
		{
			return string.Format("{0}_{1}", base.GetType().Name, this.uniqueID.ToString());
		}

		// Token: 0x040022FA RID: 8954
		public GameConditionManager gameConditionManager;

		// Token: 0x040022FB RID: 8955
		public Thing conditionCauser;

		// Token: 0x040022FC RID: 8956
		public GameConditionDef def;

		// Token: 0x040022FD RID: 8957
		public int uniqueID = -1;

		// Token: 0x040022FE RID: 8958
		public int startTick;

		// Token: 0x040022FF RID: 8959
		public bool suppressEndMessage;

		// Token: 0x04002300 RID: 8960
		private int duration = -1;

		// Token: 0x04002301 RID: 8961
		private bool permanent;

		// Token: 0x04002302 RID: 8962
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x04002303 RID: 8963
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x04002304 RID: 8964
		public Quest quest;

		// Token: 0x04002305 RID: 8965
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
