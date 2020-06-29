﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ActiveDropPodInfo : IThingHolder, IExposable
	{
		
		// (get) Token: 0x06004F0D RID: 20237 RVA: 0x001A9C4A File Offset: 0x001A7E4A
		// (set) Token: 0x06004F0E RID: 20238 RVA: 0x001A9C80 File Offset: 0x001A7E80
		public Thing SingleContainedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				if (this.innerContainer.Count > 1)
				{
					Log.Error("ContainedThing used on a DropPodInfo holding > 1 thing.", false);
				}
				return this.innerContainer[0];
			}
			set
			{
				this.innerContainer.Clear();
				this.innerContainer.TryAdd(value, true);
			}
		}

		
		// (get) Token: 0x06004F0F RID: 20239 RVA: 0x001A9C9B File Offset: 0x001A7E9B
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		
		public ActiveDropPodInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		
		public ActiveDropPodInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		
		public void ExposeData()
		{
			if (this.savePawnsWithReferenceMode && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Values.Look<bool>(ref this.savePawnsWithReferenceMode, "savePawnsWithReferenceMode", false, false);
			if (this.savePawnsWithReferenceMode)
			{
				Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			}
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.openDelay, "openDelay", 110, false);
			Scribe_Values.Look<bool>(ref this.leaveSlag, "leaveSlag", false, false);
			Scribe_Values.Look<WipeMode?>(ref this.spawnWipeMode, "spawnWipeMode", null, false);
			Scribe_Values.Look<bool>(ref this.despawnPodBeforeSpawningThing, "despawnPodBeforeSpawningThing", false, false);
			Scribe_Values.Look<Rot4?>(ref this.setRotation, "setRotation", null, false);
			Scribe_Values.Look<bool>(ref this.moveItemsAsideBeforeSpawning, "moveItemsAsideBeforeSpawning", false, false);
			if (this.savePawnsWithReferenceMode && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public IThingHolder parent;

		
		public ThingOwner innerContainer;

		
		public int openDelay = 110;

		
		public bool leaveSlag;

		
		public bool savePawnsWithReferenceMode;

		
		public bool despawnPodBeforeSpawningThing;

		
		public WipeMode? spawnWipeMode;

		
		public Rot4? setRotation;

		
		public bool moveItemsAsideBeforeSpawning;

		
		public const int DefaultOpenDelay = 110;

		
		private List<Thing> tmpThings = new List<Thing>();

		
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
