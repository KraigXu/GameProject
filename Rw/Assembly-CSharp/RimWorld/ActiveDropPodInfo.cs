using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBD RID: 3261
	public class ActiveDropPodInfo : IThingHolder, IExposable
	{
		// Token: 0x17000E00 RID: 3584
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

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004F0F RID: 20239 RVA: 0x001A9C9B File Offset: 0x001A7E9B
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x001A9CA3 File Offset: 0x001A7EA3
		public ActiveDropPodInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x001A9CD5 File Offset: 0x001A7ED5
		public ActiveDropPodInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x001A9D10 File Offset: 0x001A7F10
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

		// Token: 0x06004F13 RID: 20243 RVA: 0x001A9EC1 File Offset: 0x001A80C1
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x001A9EC9 File Offset: 0x001A80C9
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04002C5F RID: 11359
		public IThingHolder parent;

		// Token: 0x04002C60 RID: 11360
		public ThingOwner innerContainer;

		// Token: 0x04002C61 RID: 11361
		public int openDelay = 110;

		// Token: 0x04002C62 RID: 11362
		public bool leaveSlag;

		// Token: 0x04002C63 RID: 11363
		public bool savePawnsWithReferenceMode;

		// Token: 0x04002C64 RID: 11364
		public bool despawnPodBeforeSpawningThing;

		// Token: 0x04002C65 RID: 11365
		public WipeMode? spawnWipeMode;

		// Token: 0x04002C66 RID: 11366
		public Rot4? setRotation;

		// Token: 0x04002C67 RID: 11367
		public bool moveItemsAsideBeforeSpawning;

		// Token: 0x04002C68 RID: 11368
		public const int DefaultOpenDelay = 110;

		// Token: 0x04002C69 RID: 11369
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04002C6A RID: 11370
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
