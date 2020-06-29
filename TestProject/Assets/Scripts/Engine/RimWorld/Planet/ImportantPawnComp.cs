﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		
		
		protected abstract string PawnSaveKey { get; }

		
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		
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

		
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.RemovePawnOnWorldObjectRemoved();
		}

		
		protected abstract void RemovePawnOnWorldObjectRemoved();

		
		public ThingOwner<Pawn> pawn;

		
		private const float AutoFoodLevel = 0.8f;
	}
}
