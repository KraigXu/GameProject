using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CBA RID: 3258
	public class ActiveDropPod : Thing, IActiveDropPod, IThingHolder
	{
		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06004EF9 RID: 20217 RVA: 0x001A95B5 File Offset: 0x001A77B5
		// (set) Token: 0x06004EFA RID: 20218 RVA: 0x001A95BD File Offset: 0x001A77BD
		public ActiveDropPodInfo Contents
		{
			get
			{
				return this.contents;
			}
			set
			{
				if (this.contents != null)
				{
					this.contents.parent = null;
				}
				if (value != null)
				{
					value.parent = this;
				}
				this.contents = value;
			}
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x001A95E4 File Offset: 0x001A77E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveDropPodInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00019EA1 File Offset: 0x000180A1
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x001A9618 File Offset: 0x001A7818
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001A963C File Offset: 0x001A783C
		public override void Tick()
		{
			if (this.contents == null)
			{
				return;
			}
			this.contents.innerContainer.ThingOwnerTick(true);
			if (base.Spawned)
			{
				this.age++;
				if (this.age > this.contents.openDelay)
				{
					this.PodOpen();
				}
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x001A9694 File Offset: 0x001A7894
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				for (int i = 0; i < 1; i++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001A96FC File Offset: 0x001A78FC
		private void PodOpen()
		{
			Map map = base.Map;
			if (this.contents.despawnPodBeforeSpawningThing)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			for (int i = this.contents.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.contents.innerContainer[i];
				Rot4 rot = (this.contents.setRotation != null) ? this.contents.setRotation.Value : Rot4.North;
				if (this.contents.moveItemsAsideBeforeSpawning)
				{
					GenSpawn.CheckMoveItemsAside(base.Position, rot, thing.def, map);
				}
				Thing thing2;
				if (this.contents.spawnWipeMode == null)
				{
					GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, out thing2, delegate(Thing placedThing, int count)
					{
						if (Find.TickManager.TicksGame < 1200 && TutorSystem.TutorialMode && placedThing.def.category == ThingCategory.Item)
						{
							Find.TutorialState.AddStartingItem(placedThing);
						}
					}, null, rot);
				}
				else if (this.contents.setRotation != null)
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.setRotation.Value, this.contents.spawnWipeMode.Value, false);
				}
				else
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.spawnWipeMode.Value);
				}
				Pawn pawn = thing2 as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						TaleRecorder.RecordTale(TaleDefOf.LandedInPod, new object[]
						{
							pawn
						});
					}
					if (pawn.IsColonist && pawn.Spawned && !map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
					if (pawn.guest != null && pawn.guest.IsPrisoner)
					{
						pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
				}
			}
			this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.contents.leaveSlag)
			{
				for (int j = 0; j < 1; j++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
			SoundDefOf.DropPod_Open.PlayOneShot(new TargetInfo(base.Position, map, false));
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04002C58 RID: 11352
		public int age;

		// Token: 0x04002C59 RID: 11353
		private ActiveDropPodInfo contents;
	}
}
