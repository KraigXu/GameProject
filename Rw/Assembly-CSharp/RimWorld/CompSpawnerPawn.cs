﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D61 RID: 3425
	public class CompSpawnerPawn : ThingComp
	{
		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x0600535D RID: 21341 RVA: 0x001BE2EC File Offset: 0x001BC4EC
		private CompProperties_SpawnerPawn Props
		{
			get
			{
				return (CompProperties_SpawnerPawn)this.props;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x001BE2F9 File Offset: 0x001BC4F9
		public Lord Lord
		{
			get
			{
				return CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, null);
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x0600535F RID: 21343 RVA: 0x001BE320 File Offset: 0x001BC520
		private float SpawnedPawnsPoints
		{
			get
			{
				this.FilterOutUnspawnedPawns();
				float num = 0f;
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					num += this.spawnedPawns[i].kindDef.combatPower;
				}
				return num;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005360 RID: 21344 RVA: 0x001BE369 File Offset: 0x001BC569
		public bool Active
		{
			get
			{
				return this.pawnsLeftToSpawn != 0 && !this.Dormant;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005361 RID: 21345 RVA: 0x001BE380 File Offset: 0x001BC580
		public CompCanBeDormant DormancyComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormancyCompCached) == null)
				{
					result = (this.dormancyCompCached = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06005362 RID: 21346 RVA: 0x001BE3AB File Offset: 0x001BC5AB
		public bool Dormant
		{
			get
			{
				return this.DormancyComp != null && !this.DormancyComp.Awake;
			}
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x001BE3C8 File Offset: 0x001BC5C8
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.chosenKind == null)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.Props.maxPawnsToSpawn != IntRange.zero)
			{
				this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
			}
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x001BE420 File Offset: 0x001BC620
		public static Lord FindLordToJoin(Thing spawner, Type lordJobType, bool shouldTryJoinParentLord, Func<Thing, List<Pawn>> spawnedPawnSelector = null)
		{
			if (spawner.Spawned)
			{
				if (shouldTryJoinParentLord)
				{
					Building building = spawner as Building;
					Lord lord = (building != null) ? building.GetLord() : null;
					if (lord != null)
					{
						return lord;
					}
				}
				if (spawnedPawnSelector == null)
				{
					spawnedPawnSelector = delegate(Thing s)
					{
						CompSpawnerPawn compSpawnerPawn = s.TryGetComp<CompSpawnerPawn>();
						if (compSpawnerPawn != null)
						{
							return compSpawnerPawn.spawnedPawns;
						}
						return null;
					};
				}
				Predicate<Pawn> hasJob = delegate(Pawn x)
				{
					Lord lord2 = x.GetLord();
					return lord2 != null && lord2.LordJob.GetType() == lordJobType;
				};
				Pawn foundPawn = null;
				RegionTraverser.BreadthFirstTraverse(spawner.GetRegion(RegionType.Set_Passable), (Region from, Region to) => true, delegate(Region r)
				{
					List<Thing> list = r.ListerThings.ThingsOfDef(spawner.def);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Faction == spawner.Faction)
						{
							List<Pawn> list2 = spawnedPawnSelector(list[i]);
							if (list2 != null)
							{
								foundPawn = list2.Find(hasJob);
							}
							if (foundPawn != null)
							{
								return true;
							}
						}
					}
					return false;
				}, 40, RegionType.Set_Passable);
				if (foundPawn != null)
				{
					return foundPawn.GetLord();
				}
			}
			return null;
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x001BE538 File Offset: 0x001BC738
		public static Lord CreateNewLord(Thing byThing, bool aggressive, float defendRadius, Type lordJobType)
		{
			IntVec3 invalid;
			if (!CellFinder.TryFindRandomCellNear(byThing.Position, byThing.Map, 5, (IntVec3 c) => c.Standable(byThing.Map) && byThing.Map.reachability.CanReach(c, byThing, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)), out invalid, -1))
			{
				Log.Error("Found no place for mechanoids to defend " + byThing, false);
				invalid = IntVec3.Invalid;
			}
			return LordMaker.MakeNewLord(byThing.Faction, Activator.CreateInstance(lordJobType, new object[]
			{
				new SpawnedPawnParams
				{
					aggressive = aggressive,
					defendRadius = defendRadius,
					defSpot = invalid,
					spawnerThing = byThing
				}
			}) as LordJob, byThing.Map, null);
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x001BE5F4 File Offset: 0x001BC7F4
		private void SpawnInitialPawns()
		{
			int num = 0;
			Pawn pawn;
			while (num < this.Props.initialPawnsCount && this.TrySpawnPawn(out pawn))
			{
				num++;
			}
			this.SpawnPawnsUntilPoints(this.Props.initialPawnsPoints);
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x001BE638 File Offset: 0x001BC838
		public void SpawnPawnsUntilPoints(float points)
		{
			int num = 0;
			while (this.SpawnedPawnsPoints < points)
			{
				num++;
				if (num > 1000)
				{
					Log.Error("Too many iterations.", false);
					break;
				}
				Pawn pawn;
				if (!this.TrySpawnPawn(out pawn))
				{
					break;
				}
			}
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x001BE67B File Offset: 0x001BC87B
		private void CalculateNextPawnSpawnTick()
		{
			this.CalculateNextPawnSpawnTick(this.Props.pawnSpawnIntervalDays.RandomInRange * 60000f);
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x001BE69C File Offset: 0x001BC89C
		public void CalculateNextPawnSpawnTick(float delayTicks)
		{
			float num = GenMath.LerpDouble(0f, 5f, 1f, 0.5f, (float)this.spawnedPawns.Count);
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(delayTicks / (num * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x001BE6F4 File Offset: 0x001BC8F4
		private void FilterOutUnspawnedPawns()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x001BE738 File Offset: 0x001BC938
		private PawnKindDef RandomPawnKindDef()
		{
			float curPoints = this.SpawnedPawnsPoints;
			IEnumerable<PawnKindDef> source = this.Props.spawnablePawnKinds;
			if (this.Props.maxSpawnedPawnsPoints > -1f)
			{
				source = from x in source
				where curPoints + x.combatPower <= this.Props.maxSpawnedPawnsPoints
				select x;
			}
			PawnKindDef result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x001BE79C File Offset: 0x001BC99C
		private bool TrySpawnPawn(out Pawn pawn)
		{
			if (!this.canSpawnPawns)
			{
				pawn = null;
				return false;
			}
			if (!this.Props.chooseSingleTypeToSpawn)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.chosenKind == null)
			{
				pawn = null;
				return false;
			}
			int index = this.chosenKind.lifeStages.Count - 1;
			pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.chosenKind, this.parent.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, new float?(this.chosenKind.race.race.lifeStageAges[index].minAge), null, null, null, null, null, null));
			this.spawnedPawns.Add(pawn);
			GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.parent.Position, this.parent.Map, this.Props.pawnSpawnRadius, null), this.parent.Map, WipeMode.Vanish);
			Lord lord = this.Lord;
			if (lord == null)
			{
				lord = CompSpawnerPawn.CreateNewLord(this.parent, this.aggressive, this.Props.defendRadius, this.Props.lordJob);
			}
			lord.AddPawn(pawn);
			if (this.Props.spawnSound != null)
			{
				this.Props.spawnSound.PlayOneShot(this.parent);
			}
			if (this.pawnsLeftToSpawn > 0)
			{
				this.pawnsLeftToSpawn--;
			}
			this.SendMessage();
			return true;
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x001BE944 File Offset: 0x001BCB44
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.Active && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x001BE968 File Offset: 0x001BCB68
		public override void CompTick()
		{
			if (this.Active && this.parent.Spawned && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
			if (this.parent.Spawned)
			{
				this.FilterOutUnspawnedPawns();
				if (this.Active && Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
				{
					Pawn pawn;
					if ((this.Props.maxSpawnedPawnsPoints < 0f || this.SpawnedPawnsPoints < this.Props.maxSpawnedPawnsPoints) && this.TrySpawnPawn(out pawn) && pawn.caller != null)
					{
						pawn.caller.DoCall();
					}
					this.CalculateNextPawnSpawnTick();
				}
			}
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x001BEA10 File Offset: 0x001BCC10
		public void SendMessage()
		{
			if (!this.Props.spawnMessageKey.NullOrEmpty() && MessagesRepeatAvoider.MessageShowAllowed(this.Props.spawnMessageKey, 0.1f))
			{
				Messages.Message(this.Props.spawnMessageKey.Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x001BEA71 File Offset: 0x001BCC71
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Spawn pawn",
					icon = TexCommand.ReleaseAnimals,
					action = delegate
					{
						Pawn pawn;
						this.TrySpawnPawn(out pawn);
					}
				};
			}
			yield break;
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x001BEA84 File Offset: 0x001BCC84
		public override string CompInspectStringExtra()
		{
			if (!this.Props.showNextSpawnInInspect || this.nextPawnSpawnTick <= 0 || this.chosenKind == null)
			{
				return null;
			}
			if (this.pawnsLeftToSpawn == 0 && !this.Props.noPawnsLeftToSpawnKey.NullOrEmpty())
			{
				return this.Props.noPawnsLeftToSpawnKey.Translate();
			}
			string text;
			if (!this.Dormant)
			{
				text = (this.Props.nextSpawnInspectStringKey ?? "SpawningNextPawnIn").Translate(this.chosenKind.LabelCap, (this.nextPawnSpawnTick - Find.TickManager.TicksGame).ToStringTicksToDays("F1"));
			}
			else
			{
				if (this.Props.nextSpawnInspectStringKeyDormant == null)
				{
					return null;
				}
				text = this.Props.nextSpawnInspectStringKeyDormant.Translate() + ": " + this.chosenKind.LabelCap;
			}
			if (this.pawnsLeftToSpawn > 0 && !this.Props.pawnsLeftToSpawnKey.NullOrEmpty())
			{
				text = text + ("\n" + this.Props.pawnsLeftToSpawnKey.Translate() + ": ") + this.pawnsLeftToSpawn;
			}
			return text;
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x001BEBD4 File Offset: 0x001BCDD4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.pawnsLeftToSpawn, "pawnsLeftToSpawn", -1, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.chosenKind, "chosenKind");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
				if (this.pawnsLeftToSpawn == -1 && this.Props.maxPawnsToSpawn != IntRange.zero)
				{
					this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
				}
			}
		}

		// Token: 0x04002E1B RID: 11803
		public int nextPawnSpawnTick = -1;

		// Token: 0x04002E1C RID: 11804
		public int pawnsLeftToSpawn = -1;

		// Token: 0x04002E1D RID: 11805
		public List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x04002E1E RID: 11806
		public bool aggressive = true;

		// Token: 0x04002E1F RID: 11807
		public bool canSpawnPawns = true;

		// Token: 0x04002E20 RID: 11808
		private PawnKindDef chosenKind;

		// Token: 0x04002E21 RID: 11809
		private CompCanBeDormant dormancyCompCached;
	}
}
