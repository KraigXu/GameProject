using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BE RID: 2494
	public sealed class GameConditionManager : IExposable
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003B83 RID: 15235 RVA: 0x0013A59F File Offset: 0x0013879F
		public List<GameCondition> ActiveConditions
		{
			get
			{
				return this.activeConditions;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x0013A5A7 File Offset: 0x001387A7
		public GameConditionManager Parent
		{
			get
			{
				if (this.ownerMap != null)
				{
					return Find.World.gameConditionManager;
				}
				return null;
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x0013A5C0 File Offset: 0x001387C0
		public bool ElectricityDisabled
		{
			get
			{
				using (List<GameCondition>.Enumerator enumerator = this.activeConditions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ElectricityDisabled)
						{
							return true;
						}
					}
				}
				return this.Parent != null && this.Parent.ElectricityDisabled;
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0013A630 File Offset: 0x00138830
		public GameConditionManager(Map map)
		{
			this.ownerMap = map;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0013A64A File Offset: 0x0013884A
		public GameConditionManager(World world)
		{
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0013A65D File Offset: 0x0013885D
		public void RegisterCondition(GameCondition cond)
		{
			this.activeConditions.Add(cond);
			cond.startTick = Mathf.Max(cond.startTick, Find.TickManager.TicksGame);
			cond.gameConditionManager = this;
			cond.Init();
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0013A694 File Offset: 0x00138894
		public void ExposeData()
		{
			Scribe_Collections.Look<GameCondition>(ref this.activeConditions, "activeConditions", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.activeConditions.Count; i++)
				{
					this.activeConditions[i].gameConditionManager = this;
				}
			}
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0013A6E8 File Offset: 0x001388E8
		public void GameConditionManagerTick()
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (gameCondition.Expired)
				{
					gameCondition.End();
				}
				else
				{
					gameCondition.GameConditionTick();
				}
			}
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x0013A730 File Offset: 0x00138930
		public void GameConditionManagerDraw(Map map)
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				this.activeConditions[i].GameConditionDraw(map);
			}
			if (this.Parent != null)
			{
				this.Parent.GameConditionManagerDraw(map);
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0013A77C File Offset: 0x0013897C
		public void DoSteadyEffects(IntVec3 c, Map map)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				this.activeConditions[i].DoCellSteadyEffects(c, map);
			}
			if (this.Parent != null)
			{
				this.Parent.DoSteadyEffects(c, map);
			}
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0013A7C7 File Offset: 0x001389C7
		public bool ConditionIsActive(GameConditionDef def)
		{
			return this.GetActiveCondition(def) != null;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x0013A7D4 File Offset: 0x001389D4
		public GameCondition GetActiveCondition(GameConditionDef def)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (def == this.activeConditions[i].def)
				{
					return this.activeConditions[i];
				}
			}
			if (this.Parent != null)
			{
				return this.Parent.GetActiveCondition(def);
			}
			return null;
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x0013A830 File Offset: 0x00138A30
		public T GetActiveCondition<T>() where T : GameCondition
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				T t = this.activeConditions[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			if (this.Parent != null)
			{
				return this.Parent.GetActiveCondition<T>();
			}
			return default(T);
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0013A894 File Offset: 0x00138A94
		public PsychicDroneLevel GetHighestPsychicDroneLevelFor(Gender gender)
		{
			PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
			for (int i = 0; i < this.ActiveConditions.Count; i++)
			{
				GameCondition_PsychicEmanation gameCondition_PsychicEmanation = this.activeConditions[i] as GameCondition_PsychicEmanation;
				if (gameCondition_PsychicEmanation != null && gameCondition_PsychicEmanation.gender == gender && gameCondition_PsychicEmanation.level > psychicDroneLevel)
				{
					psychicDroneLevel = gameCondition_PsychicEmanation.level;
				}
			}
			return psychicDroneLevel;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0013A8E8 File Offset: 0x00138AE8
		public void GetChildren(List<GameConditionManager> outChildren)
		{
			if (this == Find.World.gameConditionManager)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					outChildren.Add(maps[i].gameConditionManager);
				}
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0013A92C File Offset: 0x00138B2C
		public float TotalHeightAt(float width)
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += Text.CalcHeight(this.activeConditions[i].LabelCap, width - 6f);
			}
			if (this.Parent != null)
			{
				num += this.Parent.TotalHeightAt(width);
			}
			return num;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0013A990 File Offset: 0x00138B90
		public void DoConditionsUI(Rect rect)
		{
			GUI.BeginGroup(rect);
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				string labelCap = this.activeConditions[i].LabelCap;
				Rect rect2 = new Rect(0f, num, rect.width, Text.CalcHeight(labelCap, rect.width - 6f));
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.DrawHighlightIfMouseover(rect2);
				Rect rect3 = rect2;
				rect3.width -= 6f;
				Widgets.Label(rect3, labelCap);
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, new TipSignal(this.activeConditions[i].TooltipString, 976090154 ^ i));
				}
				if (Widgets.ButtonInvisible(rect2, true))
				{
					if (this.activeConditions[i].conditionCauser != null && CameraJumper.CanJump(this.activeConditions[i].conditionCauser))
					{
						CameraJumper.TryJumpAndSelect(this.activeConditions[i].conditionCauser);
					}
					else if (this.activeConditions[i].quest != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.activeConditions[i].quest);
					}
				}
				num += rect2.height;
			}
			rect.yMin += num;
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.Parent != null)
			{
				this.Parent.DoConditionsUI(rect);
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0013AB34 File Offset: 0x00138D34
		public void GetAllGameConditionsAffectingMap(Map map, List<GameCondition> listToFill)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				listToFill.Add(this.activeConditions[i]);
			}
			if (this.Parent != null)
			{
				this.Parent.GetAllGameConditionsAffectingMap(map, listToFill);
			}
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0013AB80 File Offset: 0x00138D80
		internal float AggregateTemperatureOffset()
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += this.activeConditions[i].TemperatureOffset();
			}
			if (this.Parent != null)
			{
				num += this.Parent.AggregateTemperatureOffset();
			}
			return num;
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x0013ABD4 File Offset: 0x00138DD4
		internal float AggregateAnimalDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].AnimalDensityFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateAnimalDensityFactor(map);
			}
			return num;
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x0013AC2C File Offset: 0x00138E2C
		internal float AggregatePlantDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].PlantDensityFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregatePlantDensityFactor(map);
			}
			return num;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0013AC84 File Offset: 0x00138E84
		internal float AggregateSkyGazeJoyGainFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].SkyGazeJoyGainFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeJoyGainFactor(map);
			}
			return num;
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x0013ACDC File Offset: 0x00138EDC
		internal float AggregateSkyGazeChanceFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].SkyGazeChanceFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeChanceFactor(map);
			}
			return num;
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x0013AD34 File Offset: 0x00138F34
		internal bool AllowEnjoyableOutsideNow(Map map)
		{
			GameConditionDef gameConditionDef;
			return this.AllowEnjoyableOutsideNow(map, out gameConditionDef);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0013AD4C File Offset: 0x00138F4C
		internal bool AllowEnjoyableOutsideNow(Map map, out GameConditionDef reason)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (!gameCondition.AllowEnjoyableOutsideNow(map))
				{
					reason = gameCondition.def;
					return false;
				}
			}
			reason = null;
			return this.Parent == null || this.Parent.AllowEnjoyableOutsideNow(map, out reason);
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x0013ADAC File Offset: 0x00138FAC
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameCondition saveable in this.activeConditions)
			{
				stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(saveable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400232E RID: 9006
		public Map ownerMap;

		// Token: 0x0400232F RID: 9007
		private List<GameCondition> activeConditions = new List<GameCondition>();

		// Token: 0x04002330 RID: 9008
		private const float TextPadding = 6f;
	}
}
