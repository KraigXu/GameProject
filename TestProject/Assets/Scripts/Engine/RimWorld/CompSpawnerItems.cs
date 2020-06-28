using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5E RID: 3422
	public class CompSpawnerItems : ThingComp
	{
		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x001BE112 File Offset: 0x001BC312
		public CompProperties_SpawnerItems Props
		{
			get
			{
				return (CompProperties_SpawnerItems)this.props;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005353 RID: 21331 RVA: 0x001BE120 File Offset: 0x001BC320
		public bool Active
		{
			get
			{
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x001BE144 File Offset: 0x001BC344
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

		// Token: 0x06005355 RID: 21333 RVA: 0x001BE154 File Offset: 0x001BC354
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

		// Token: 0x06005356 RID: 21334 RVA: 0x001BE1C4 File Offset: 0x001BC3C4
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

		// Token: 0x06005357 RID: 21335 RVA: 0x001BE21D File Offset: 0x001BC41D
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x001BE234 File Offset: 0x001BC434
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "NextSpawnedResourceIn".Translate() + ": " + (this.Props.spawnInterval - this.ticksPassed).ToStringTicksToPeriod(true, false, true, true);
			}
			return null;
		}

		// Token: 0x04002E04 RID: 11780
		private int ticksPassed;
	}
}
