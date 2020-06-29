using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompSpawnerItems : ThingComp
	{
		
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x001BE112 File Offset: 0x001BC312
		public CompProperties_SpawnerItems Props
		{
			get
			{
				return (CompProperties_SpawnerItems)this.props;
			}
		}

		
		// (get) Token: 0x06005353 RID: 21331 RVA: 0x001BE120 File Offset: 0x001BC320
		public bool Active
		{
			get
			{
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				defaultLabel = "DEV: Spawn items",
				action = delegate
				{
					this.SpawnItems();
				}
			};
			yield break;
		}

		
		private void SpawnItems()
		{
			ThingDef thingDef;
			if (this.Props.MatchingItems.TryRandomElement(out thingDef))
			{
				int stackCount = Mathf.CeilToInt(this.Props.approxMarketValuePerDay / thingDef.BaseMarketValue);
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, default(Rot4));
			}
		}

		
		public override void CompTickRare()
		{
			if (!this.Active)
			{
				return;
			}
			this.ticksPassed += 250;
			if (this.ticksPassed >= this.Props.spawnInterval)
			{
				this.SpawnItems();
				this.ticksPassed -= this.Props.spawnInterval;
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "NextSpawnedResourceIn".Translate() + ": " + (this.Props.spawnInterval - this.ticksPassed).ToStringTicksToPeriod(true, false, true, true);
			}
			return null;
		}

		
		private int ticksPassed;
	}
}
