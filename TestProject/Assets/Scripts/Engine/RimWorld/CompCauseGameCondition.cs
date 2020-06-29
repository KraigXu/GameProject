using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class CompCauseGameCondition : ThingComp
	{
		
		
		public CompProperties_CausesGameCondition Props
		{
			get
			{
				return (CompProperties_CausesGameCondition)this.props;
			}
		}

		
		
		public GameConditionDef ConditionDef
		{
			get
			{
				return this.Props.conditionDef;
			}
		}

		
		
		public IEnumerable<GameCondition> CausedConditions
		{
			get
			{
				return this.causedConditions.Values;
			}
		}

		
		
		public bool Active
		{
			get
			{
				return this.initiatableComp == null || this.initiatableComp.Initiated;
			}
		}

		
		
		public int MyTile
		{
			get
			{
				if (this.siteLink != null)
				{
					return this.siteLink.Tile;
				}
				if (this.parent.SpawnedOrAnyParentSpawned)
				{
					return this.parent.Tile;
				}
				return -1;
			}
		}

		
		public void LinkWithSite(Site site)
		{
			this.siteLink = site;
		}

		
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.CacheComps();
		}

		
		private void CacheComps()
		{
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => !Find.Maps.Contains(x.Key));
			}
			Scribe_References.Look<Site>(ref this.siteLink, "siteLink", false);
			Scribe_Collections.Look<Map, GameCondition>(ref this.causedConditions, "causedConditions", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => x.Value == null);
				foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
				{
					keyValuePair.Value.conditionCauser = this.parent;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CacheComps();
			}
		}

		
		public bool InAoE(int tile)
		{
			return this.MyTile != -1 && this.Active && Find.WorldGrid.TraversalDistanceBetween(this.MyTile, tile, true, this.Props.worldRange + 1) <= this.Props.worldRange;
		}

		
		protected GameCondition GetConditionInstance(Map map)
		{
			GameCondition activeCondition;
			if (!this.causedConditions.TryGetValue(map, out activeCondition) && this.Props.preventConditionStacking)
			{
				activeCondition = map.GameConditionManager.GetActiveCondition(this.Props.conditionDef);
				if (activeCondition != null)
				{
					this.causedConditions.Add(map, activeCondition);
					this.SetupCondition(activeCondition, map);
				}
			}
			return activeCondition;
		}

		
		public override void CompTick()
		{
			if (this.Active)
			{
				foreach (Map map in Find.Maps)
				{
					if (this.InAoE(map.Tile))
					{
						this.EnforceConditionOn(map);
					}
				}
			}
			CompCauseGameCondition.tmpDeadConditionMaps.Clear();
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				if (keyValuePair.Value.Expired || !keyValuePair.Key.GameConditionManager.ConditionIsActive(keyValuePair.Value.def))
				{
					CompCauseGameCondition.tmpDeadConditionMaps.Add(keyValuePair.Key);
				}
			}
			foreach (Map key in CompCauseGameCondition.tmpDeadConditionMaps)
			{
				this.causedConditions.Remove(key);
			}
		}

		
		private GameCondition EnforceConditionOn(Map map)
		{
			GameCondition gameCondition = this.GetConditionInstance(map);
			if (gameCondition == null)
			{
				gameCondition = this.CreateConditionOn(map);
			}
			else
			{
				gameCondition.TicksLeft = gameCondition.TransitionTicks;
			}
			return gameCondition;
		}

		
		protected virtual GameCondition CreateConditionOn(Map map)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.ConditionDef, -1);
			gameCondition.Duration = gameCondition.TransitionTicks;
			gameCondition.conditionCauser = this.parent;
			map.gameConditionManager.RegisterCondition(gameCondition);
			this.causedConditions.Add(map, gameCondition);
			this.SetupCondition(gameCondition, map);
			return gameCondition;
		}

		
		protected virtual void SetupCondition(GameCondition condition, Map map)
		{
			condition.suppressEndMessage = true;
		}

		
		protected void ReSetupAllConditions()
		{
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				this.SetupCondition(keyValuePair.Value, keyValuePair.Key);
			}
		}

		
		public override void PostDeSpawn(Map map)
		{
			Messages.Message("MessageConditionCauserDespawned".Translate(this.parent.def.LabelCap), new TargetInfo(this.parent.Position, map, false), MessageTypeDefOf.NeutralEvent, true);
		}

		
		public override string CompInspectStringExtra()
		{
			if (!Prefs.DevMode)
			{
				return base.CompInspectStringExtra();
			}
			GameCondition gameCondition = this.parent.Map.GameConditionManager.ActiveConditions.Find((GameCondition c) => c.def == this.Props.conditionDef);
			if (gameCondition == null)
			{
				return base.CompInspectStringExtra();
			}
			return string.Concat(new object[]
			{
				"[DEV] Current map condition\n[DEV] Ticks Passed: ",
				gameCondition.TicksPassed,
				"\n[DEV] Ticks Left: ",
				gameCondition.TicksLeft
			});
		}

		
		public virtual void RandomizeSettings()
		{
		}

		
		protected CompInitiatable initiatableComp;

		
		protected Site siteLink;

		
		private Dictionary<Map, GameCondition> causedConditions = new Dictionary<Map, GameCondition>();

		
		private static List<Map> tmpDeadConditionMaps = new List<Map>();
	}
}
