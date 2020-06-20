using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A8A RID: 2698
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06003FD5 RID: 16341 RVA: 0x00153C81 File Offset: 0x00151E81
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile != -1;
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x00153C8F File Offset: 0x00151E8F
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00153CAC File Offset: 0x00151EAC
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", -1, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x00153D2C File Offset: 0x00151F2C
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (!this.map.Parent.def.isTempIncidentMapOwner)
			{
				return;
			}
			this.Set(caravan);
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x00153D50 File Offset: 0x00151F50
		public void Notify_CaravanFormed(Caravan caravan)
		{
			if (this.shouldPassStoryState)
			{
				this.shouldPassStoryState = false;
				this.map.StoryState.CopyTo(caravan.StoryState);
			}
			if (this.nextTile != -1 && this.nextTile != caravan.Tile && caravan.CanReach(this.nextTile))
			{
				caravan.pather.StartPath(this.nextTile, null, true, true);
				caravan.pather.nextTileCostLeft = caravan.pather.nextTileCostTotal * this.nextTileCostLeftPct;
				caravan.pather.Paused = this.paused;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (this.HasDestinationTile && this.destinationTile != caravan.Tile)
			{
				caravan.pather.StartPath(this.destinationTile, this.arrivalAction, true, true);
				this.destinationTile = -1;
				this.arrivalAction = null;
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x00153E34 File Offset: 0x00152034
		private void Set(Caravan caravan)
		{
			caravan.StoryState.CopyTo(this.map.StoryState);
			this.shouldPassStoryState = true;
			if (caravan.pather.Moving)
			{
				this.nextTile = caravan.pather.nextTile;
				this.nextTileCostLeftPct = caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				this.paused = caravan.pather.Paused;
				this.destinationTile = caravan.pather.Destination;
				this.arrivalAction = caravan.pather.ArrivalAction;
				return;
			}
			this.nextTile = -1;
			this.nextTileCostLeftPct = 0f;
			this.paused = false;
			this.destinationTile = -1;
			this.arrivalAction = null;
		}

		// Token: 0x04002525 RID: 9509
		private Map map;

		// Token: 0x04002526 RID: 9510
		private bool shouldPassStoryState;

		// Token: 0x04002527 RID: 9511
		private int nextTile = -1;

		// Token: 0x04002528 RID: 9512
		private float nextTileCostLeftPct;

		// Token: 0x04002529 RID: 9513
		private bool paused;

		// Token: 0x0400252A RID: 9514
		private int destinationTile = -1;

		// Token: 0x0400252B RID: 9515
		private CaravanArrivalAction arrivalAction;
	}
}
