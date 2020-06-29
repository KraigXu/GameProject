﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		
		// (get) Token: 0x0600516B RID: 20843
		protected abstract int GatherResourcesIntervalDays { get; }

		
		// (get) Token: 0x0600516C RID: 20844
		protected abstract int ResourceAmount { get; }

		
		// (get) Token: 0x0600516D RID: 20845
		protected abstract ThingDef ResourceDef { get; }

		
		// (get) Token: 0x0600516E RID: 20846
		protected abstract string SaveKey { get; }

		
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x001B49FC File Offset: 0x001B2BFC
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x001B4A04 File Offset: 0x001B2C04
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null;
			}
		}

		
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x001B4A16 File Offset: 0x001B2C16
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (float)(this.GatherResourcesIntervalDays * 60000);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.fullness += num;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent, false);
			}
			if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield, true)))
			{
				MoteMaker.ThrowText((doer.DrawPos + this.parent.DrawPos) / 2f, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int i = GenMath.RoundRandom((float)this.ResourceAmount * this.fullness);
				while (i > 0)
				{
					int num = Mathf.Clamp(i, 1, this.ResourceDef.stackLimit);
					i -= num;
					Thing thing = ThingMaker.MakeThing(this.ResourceDef, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
			this.fullness = 0f;
		}

		
		protected float fullness;
	}
}
