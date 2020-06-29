﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompPawnSpawnOnWakeup : ThingComp
	{
		
		// (get) Token: 0x0600521E RID: 21022 RVA: 0x001B6E7D File Offset: 0x001B507D
		private CompProperties_PawnSpawnOnWakeup Props
		{
			get
			{
				return (CompProperties_PawnSpawnOnWakeup)this.props;
			}
		}

		
		// (get) Token: 0x0600521F RID: 21023 RVA: 0x001B6E8A File Offset: 0x001B508A
		public bool CanSpawn
		{
			get
			{
				return this.points > 0f;
			}
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.points = this.Props.points.RandomInRange;
		}

		
		public override void CompTick()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
			if (this.points == 0f)
			{
				return;
			}
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			bool flag = comp == null || comp.Awake;
			if (this.points > 0f && flag && this.parent.Spawned)
			{
				this.Spawn();
			}
		}

		
		private IntVec3 GetSpawnPosition()
		{
			if (!this.Props.dropInPods)
			{
				return this.parent.Position;
			}
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!DropCellFinder.IsGoodDropSpot(c, this.parent.MapHeld, false, true, true))
				{
					return false;
				}
				float num = c.DistanceTo(this.parent.Position);
				return num >= (float)this.Props.pawnSpawnRadius.min && num <= (float)this.Props.pawnSpawnRadius.max;
			};
			IntVec3 result;
			if (CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.MapHeld, this.Props.pawnSpawnRadius.max, validator, out result, -1))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		
		private List<Thing> GeneratePawns()
		{
			List<Thing> list = new List<Thing>();
			float pointsLeft;
			
			PawnKindDef pawnKindDef;
			for (pointsLeft = this.points; pointsLeft > 0f; pointsLeft -= pawnKindDef.combatPower)
			{
				IEnumerable<PawnKindDef> spawnablePawnKinds = this.Props.spawnablePawnKinds;
				Func<PawnKindDef, bool> predicate;
				if ((predicate ) == null)
				{
					predicate = (9__0 = ((PawnKindDef p) => p.combatPower <= pointsLeft));
				}
				if (!spawnablePawnKinds.Where(predicate).TryRandomElement(out pawnKindDef))
				{
					break;
				}
				int index = pawnKindDef.lifeStages.Count - 1;
				list.Add(PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, this.parent.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, new float?(pawnKindDef.race.race.lifeStageAges[index].minAge), null, null, null, null, null, null)));
			}
			this.points = 0f;
			return list;
		}

		
		private void Spawn()
		{
			Lord lord = CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, delegate(Thing spawner)
			{
				CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = spawner.TryGetComp<CompPawnSpawnOnWakeup>();
				if (compPawnSpawnOnWakeup != null)
				{
					return compPawnSpawnOnWakeup.spawnedPawns;
				}
				return null;
			});
			if (lord == null)
			{
				lord = CompSpawnerPawn.CreateNewLord(this.parent, this.Props.aggressive, this.Props.defendRadius, this.Props.lordJob);
			}
			IntVec3 spawnPosition = this.GetSpawnPosition();
			if (!spawnPosition.IsValid)
			{
				return;
			}
			List<Thing> list = this.GeneratePawns();
			if (this.Props.dropInPods)
			{
				DropPodUtility.DropThingsNear(spawnPosition, this.parent.MapHeld, list, 110, false, false, true, true);
			}
			List<IntVec3> occupiedCells = new List<IntVec3>();
			Predicate<IntVec3> 9__1;
			foreach (Thing thing in list)
			{
				if (!this.Props.dropInPods)
				{
					IntVec3 root = spawnPosition;
					Map map = this.parent.Map;
					int randomInRange = this.Props.pawnSpawnRadius.RandomInRange;
					Predicate<IntVec3> extraValidator;
					if ((extraValidator ) == null)
					{
						extraValidator = (9__1 = ((IntVec3 c) => !occupiedCells.Contains(c)));
					}
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(root, map, randomInRange, extraValidator);
					if (!intVec.IsValid)
					{
						intVec = CellFinder.RandomClosewalkCellNear(spawnPosition, this.parent.Map, this.Props.pawnSpawnRadius.RandomInRange, null);
					}
					GenSpawn.Spawn(thing, intVec, this.parent.Map, WipeMode.Vanish);
					occupiedCells.Add(intVec);
				}
				lord.AddPawn((Pawn)thing);
				this.spawnedPawns.Add((Pawn)thing);
				CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.WakeUp();
				}
			}
			if (this.Props.spawnEffecter != null)
			{
				Effecter effecter = new Effecter(this.Props.spawnEffecter);
				effecter.Trigger(this.parent, TargetInfo.Invalid);
				effecter.Cleanup();
			}
			if (this.Props.spawnSound != null)
			{
				this.Props.spawnSound.PlayOneShot(this.parent);
			}
			if (this.Props.activatedMessageKey != null)
			{
				Messages.Message(this.Props.activatedMessageKey.Translate(), this.spawnedPawns, MessageTypeDefOf.ThreatBig, true);
			}
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Spawn",
				action = new Action(this.Spawn)
			};
			yield break;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public float points;

		
		public List<Pawn> spawnedPawns = new List<Pawn>();
	}
}
