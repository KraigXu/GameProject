using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200127B RID: 4731
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06006EEC RID: 28396
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06006EED RID: 28397 RVA: 0x0026A933 File Offset: 0x00268B33
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x06006EEE RID: 28398 RVA: 0x0026A949 File Offset: 0x00268B49
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006EEF RID: 28399 RVA: 0x0026A972 File Offset: 0x00268B72
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006EF0 RID: 28400 RVA: 0x0026A980 File Offset: 0x00268B80
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x0026A988 File Offset: 0x00268B88
		public override void CompTick()
		{
			base.CompTick();
			bool any = this.pawn.Any;
			this.pawn.ThingOwnerTick(true);
			if (any && !base.ParentHasMap)
			{
				if (!this.pawn.Any || this.pawn[0].Destroyed)
				{
					this.parent.Destroy();
					return;
				}
				Pawn pawn = this.pawn[0];
				if (pawn.needs.food != null)
				{
					pawn.needs.food.CurLevelPercentage = 0.8f;
				}
			}
		}

		// Token: 0x06006EF2 RID: 28402 RVA: 0x0026AA17 File Offset: 0x00268C17
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06006EF3 RID: 28403
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x04004449 RID: 17481
		public ThingOwner<Pawn> pawn;

		// Token: 0x0400444A RID: 17482
		private const float AutoFoodLevel = 0.8f;
	}
}
