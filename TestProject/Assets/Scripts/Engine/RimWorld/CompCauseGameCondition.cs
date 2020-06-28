using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDB RID: 3291
	public class CompCauseGameCondition : ThingComp
	{
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06004FCC RID: 20428 RVA: 0x001AEE5C File Offset: 0x001AD05C
		public CompProperties_CausesGameCondition Props
		{
			get
			{
				return (CompProperties_CausesGameCondition)this.props;
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x001AEE69 File Offset: 0x001AD069
		public GameConditionDef ConditionDef
		{
			get
			{
				return this.Props.conditionDef;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x001AEE76 File Offset: 0x001AD076
		public IEnumerable<GameCondition> CausedConditions
		{
			get
			{
				return this.causedConditions.Values;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06004FCF RID: 20431 RVA: 0x001AEE83 File Offset: 0x001AD083
		public bool Active
		{
			get
			{
				return this.initiatableComp == null || this.initiatableComp.Initiated;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x001AEE9A File Offset: 0x001AD09A
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

		// Token: 0x06004FD1 RID: 20433 RVA: 0x001AEECA File Offset: 0x001AD0CA
		public void LinkWithSite(Site site)
		{
			this.siteLink = site;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x001AEED3 File Offset: 0x001AD0D3
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.CacheComps();
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x001AEEE1 File Offset: 0x001AD0E1
		private void CacheComps()
		{
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x001AEEF4 File Offset: 0x001AD0F4
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

		// Token: 0x06004FD5 RID: 20437 RVA: 0x001AEFEC File Offset: 0x001AD1EC
		public bool InAoE(int tile)
		{
			return this.MyTile != -1 && this.Active && Find.WorldGrid.TraversalDistanceBetween(this.MyTile, tile, true, this.Props.worldRange + 1) <= this.Props.worldRange;
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x001AF03C File Offset: 0x001AD23C
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

		// Token: 0x06004FD7 RID: 20439 RVA: 0x001AF098 File Offset: 0x001AD298
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

		// Token: 0x06004FD8 RID: 20440 RVA: 0x001AF1D0 File Offset: 0x001AD3D0
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

		// Token: 0x06004FD9 RID: 20441 RVA: 0x001AF200 File Offset: 0x001AD400
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

		// Token: 0x06004FDA RID: 20442 RVA: 0x001AF254 File Offset: 0x001AD454
		protected virtual void SetupCondition(GameCondition condition, Map map)
		{
			condition.suppressEndMessage = true;
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x001AF260 File Offset: 0x001AD460
		protected void ReSetupAllConditions()
		{
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				this.SetupCondition(keyValuePair.Value, keyValuePair.Key);
			}
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x001AF2C0 File Offset: 0x001AD4C0
		public override void PostDeSpawn(Map map)
		{
			Messages.Message("MessageConditionCauserDespawned".Translate(this.parent.def.LabelCap), new TargetInfo(this.parent.Position, map, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x001AF314 File Offset: 0x001AD514
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

		// Token: 0x06004FDE RID: 20446 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void RandomizeSettings()
		{
		}

		// Token: 0x04002CA4 RID: 11428
		protected CompInitiatable initiatableComp;

		// Token: 0x04002CA5 RID: 11429
		protected Site siteLink;

		// Token: 0x04002CA6 RID: 11430
		private Dictionary<Map, GameCondition> causedConditions = new Dictionary<Map, GameCondition>();

		// Token: 0x04002CA7 RID: 11431
		private static List<Map> tmpDeadConditionMaps = new List<Map>();
	}
}
