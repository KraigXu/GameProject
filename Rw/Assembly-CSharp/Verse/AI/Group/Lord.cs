using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020005C6 RID: 1478
	[StaticConstructorOnStartup]
	public class Lord : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x000F2AF3 File Offset: 0x000F0CF3
		public Map Map
		{
			get
			{
				return this.lordManager.map;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x000F2B00 File Offset: 0x000F0D00
		public StateGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x000F2B08 File Offset: 0x000F0D08
		public LordToil CurLordToil
		{
			get
			{
				return this.curLordToil;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x000F2B10 File Offset: 0x000F0D10
		public LordJob LordJob
		{
			get
			{
				return this.curJob;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x000F2B18 File Offset: 0x000F0D18
		private bool CanExistWithoutPawns
		{
			get
			{
				return this.curJob is LordJob_VoluntarilyJoinable;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x000F2B28 File Offset: 0x000F0D28
		private bool ShouldExist
		{
			get
			{
				return this.ownedPawns.Count > 0 || this.CanExistWithoutPawns || this.ownedBuildings.Count > 0;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x000F2B50 File Offset: 0x000F0D50
		public bool AnyActivePawn
		{
			get
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					if (this.ownedPawns[i].mindState != null && this.ownedPawns[i].mindState.Active)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000F2BA1 File Offset: 0x000F0DA1
		private void Init()
		{
			this.initialized = true;
			this.initialColonyHealthTotal = this.Map.wealthWatcher.HealthTotal;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000F2BC0 File Offset: 0x000F0DC0
		public string GetUniqueLoadID()
		{
			return "Lord_" + this.loadID;
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000F2BD8 File Offset: 0x000F0DD8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.extraForbiddenThings, "extraForbiddenThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.ownedPawns, "ownedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Building>(ref this.ownedBuildings, "ownedBuildings", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<LordJob>(ref this.curJob, "lordJob", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
			Scribe_Values.Look<int>(ref this.ticksInToil, "ticksInToil", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsEverGained, "numPawnsEverGained", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsLostViolently, "numPawnsLostViolently", 0, false);
			Scribe_Values.Look<int>(ref this.initialColonyHealthTotal, "initialColonyHealthTotal", 0, false);
			Scribe_Values.Look<int>(ref this.lastPawnHarmTick, "lastPawnHarmTick", -99999, false);
			Scribe_Values.Look<string>(ref this.inSignalLeave, "inSignalLeave", null, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.extraForbiddenThings.RemoveAll((Thing x) => x == null);
				this.ownedPawns.RemoveAll((Pawn x) => x == null);
				this.ownedBuildings.RemoveAll((Building x) => x == null);
			}
			this.ExposeData_StateGraph();
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000F2D8C File Offset: 0x000F0F8C
		private void ExposeData_StateGraph()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpLordToilData.Clear();
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					if (this.graph.lordToils[i].data != null)
					{
						this.tmpLordToilData.Add(i, this.graph.lordToils[i].data);
					}
				}
				this.tmpTriggerData.Clear();
				int num = 0;
				for (int j = 0; j < this.graph.transitions.Count; j++)
				{
					for (int k = 0; k < this.graph.transitions[j].triggers.Count; k++)
					{
						if (this.graph.transitions[j].triggers[k].data != null)
						{
							this.tmpTriggerData.Add(num, this.graph.transitions[j].triggers[k].data);
						}
						num++;
					}
				}
				this.tmpCurLordToilIdx = this.graph.lordToils.IndexOf(this.curLordToil);
			}
			Scribe_Collections.Look<int, LordToilData>(ref this.tmpLordToilData, "lordToilData", LookMode.Value, LookMode.Deep);
			Scribe_Collections.Look<int, TriggerData>(ref this.tmpTriggerData, "triggerData", LookMode.Value, LookMode.Deep);
			Scribe_Values.Look<int>(ref this.tmpCurLordToilIdx, "curLordToilIdx", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curJob.LostImportantReferenceDuringLoading)
				{
					this.lordManager.RemoveLord(this);
					return;
				}
				LordJob job = this.curJob;
				this.curJob = null;
				this.SetJob(job);
				foreach (KeyValuePair<int, LordToilData> keyValuePair in this.tmpLordToilData)
				{
					if (keyValuePair.Key < 0 || keyValuePair.Key >= this.graph.lordToils.Count)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not find lord toil for lord toil data of type \"",
							keyValuePair.Value.GetType(),
							"\" (lord job: \"",
							this.curJob.GetType(),
							"\"), because lord toil index is out of bounds: ",
							keyValuePair.Key
						}), false);
					}
					else
					{
						this.graph.lordToils[keyValuePair.Key].data = keyValuePair.Value;
					}
				}
				this.tmpLordToilData.Clear();
				foreach (KeyValuePair<int, TriggerData> keyValuePair2 in this.tmpTriggerData)
				{
					Trigger triggerByIndex = this.GetTriggerByIndex(keyValuePair2.Key);
					if (triggerByIndex == null)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not find trigger for trigger data of type \"",
							keyValuePair2.Value.GetType(),
							"\" (lord job: \"",
							this.curJob.GetType(),
							"\"), because trigger index is out of bounds: ",
							keyValuePair2.Key
						}), false);
					}
					else
					{
						triggerByIndex.data = keyValuePair2.Value;
					}
				}
				this.tmpTriggerData.Clear();
				if (this.tmpCurLordToilIdx < 0 || this.tmpCurLordToilIdx >= this.graph.lordToils.Count)
				{
					Log.Error(string.Concat(new object[]
					{
						"Current lord toil index out of bounds (lord job: \"",
						this.curJob.GetType(),
						"\"): ",
						this.tmpCurLordToilIdx
					}), false);
					return;
				}
				this.curLordToil = this.graph.lordToils[this.tmpCurLordToilIdx];
			}
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000F3168 File Offset: 0x000F1368
		public void SetJob(LordJob lordJob)
		{
			if (this.curJob != null)
			{
				this.curJob.Cleanup();
			}
			this.curJob = lordJob;
			this.curLordToil = null;
			lordJob.lord = this;
			Rand.PushState();
			Rand.Seed = this.loadID * 193;
			this.graph = lordJob.CreateGraph();
			Rand.PopState();
			this.graph.ErrorCheck();
			if (this.faction != null && !this.faction.IsPlayer && this.faction.def.autoFlee && lordJob.AddFleeToil)
			{
				LordToil_PanicFlee lordToil_PanicFlee = new LordToil_PanicFlee();
				lordToil_PanicFlee.useAvoidGrid = true;
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					Transition transition = new Transition(this.graph.lordToils[i], lordToil_PanicFlee, false, true);
					transition.AddPreAction(new TransitionAction_Message("MessageFightersFleeing".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
					transition.AddTrigger(new Trigger_FractionPawnsLost(this.faction.def.attackersDownPercentageRangeForAutoFlee.RandomInRangeSeeded(this.loadID)));
					this.graph.AddTransition(transition, true);
				}
				this.graph.AddToil(lordToil_PanicFlee);
			}
			for (int j = 0; j < this.graph.lordToils.Count; j++)
			{
				this.graph.lordToils[j].lord = this;
			}
			for (int k = 0; k < this.ownedPawns.Count; k++)
			{
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[k]);
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000F3344 File Offset: 0x000F1544
		public void Cleanup()
		{
			this.curJob.Cleanup();
			if (this.curLordToil != null)
			{
				this.curLordToil.Cleanup();
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].mindState != null)
				{
					this.ownedPawns[i].mindState.duty = null;
				}
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[i]);
				if (this.ownedPawns[i].Spawned && this.ownedPawns[i].CurJob != null)
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000F3410 File Offset: 0x000F1610
		public void AddPawn(Pawn p)
		{
			if (this.ownedPawns.Contains(p))
			{
				Log.Error(string.Concat(new object[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					p,
					" whom it already controls."
				}), false);
				return;
			}
			if (p.GetLord() != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add pawn ",
					p,
					" to lord ",
					this,
					" but this pawn is already a member of lord ",
					p.GetLord(),
					". Pawns can't be members of more than one lord at the same time."
				}), false);
				return;
			}
			this.ownedPawns.Add(p);
			this.numPawnsEverGained++;
			this.Map.attackTargetsCache.UpdateTarget(p);
			this.curLordToil.UpdateAllDuties();
			this.curJob.Notify_PawnAdded(p);
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000F34F4 File Offset: 0x000F16F4
		public void AddBuilding(Building b)
		{
			if (this.ownedBuildings.Contains(b))
			{
				Log.Error(string.Concat(new object[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					b,
					" which it already controls."
				}), false);
				return;
			}
			this.ownedBuildings.Add(b);
			this.curLordToil.UpdateAllDuties();
			this.curJob.Notify_BuildingAdded(b);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000F356E File Offset: 0x000F176E
		private void RemovePawn(Pawn p)
		{
			this.ownedPawns.Remove(p);
			if (p.mindState != null)
			{
				p.mindState.duty = null;
			}
			this.Map.attackTargetsCache.UpdateTarget(p);
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000F35A4 File Offset: 0x000F17A4
		public void GotoToil(LordToil newLordToil)
		{
			LordToil previousToil = this.curLordToil;
			if (this.curLordToil != null)
			{
				this.curLordToil.Cleanup();
			}
			this.curLordToil = newLordToil;
			this.ticksInToil = 0;
			if (this.curLordToil.lord != this)
			{
				Log.Error("curLordToil lord is " + ((this.curLordToil.lord == null) ? "null (forgot to add toil to graph?)" : this.curLordToil.lord.ToString()), false);
				this.curLordToil.lord = this;
			}
			this.curLordToil.Init();
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil))
				{
					this.graph.transitions[i].SourceToilBecameActive(this.graph.transitions[i], previousToil);
				}
			}
			this.curLordToil.UpdateAllDuties();
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000F36A3 File Offset: 0x000F18A3
		public void LordTick()
		{
			if (!this.initialized)
			{
				this.Init();
			}
			this.curJob.LordJobTick();
			this.curLordToil.LordToilTick();
			this.CheckTransitionOnSignal(TriggerSignal.ForTick);
			this.ticksInToil++;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000F36E4 File Offset: 0x000F18E4
		private Trigger GetTriggerByIndex(int index)
		{
			int num = 0;
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				for (int j = 0; j < this.graph.transitions[i].triggers.Count; j++)
				{
					if (num == index)
					{
						return this.graph.transitions[i].triggers[j];
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000F375A File Offset: 0x000F195A
		public void ReceiveMemo(string memo)
		{
			this.CheckTransitionOnSignal(TriggerSignal.ForMemo(memo));
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000F376C File Offset: 0x000F196C
		public void Notify_FactionRelationsChanged(Faction otherFaction, FactionRelationKind previousRelationKind)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.FactionRelationsChanged,
				faction = otherFaction,
				previousRelationKind = new FactionRelationKind?(previousRelationKind)
			});
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].Spawned)
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000F37E8 File Offset: 0x000F19E8
		private void Destroy()
		{
			this.lordManager.RemoveLord(this);
			this.curJob.Notify_LordDestroyed();
			if (this.faction != null)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated");
			}
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000F381C File Offset: 0x000F1A1C
		public void Notify_PawnLost(Pawn pawn, PawnLostCondition cond, DamageInfo? dinfo = null)
		{
			if (this.ownedPawns.Contains(pawn))
			{
				this.RemovePawn(pawn);
				if (cond == PawnLostCondition.IncappedOrKilled || cond == PawnLostCondition.MadePrisoner)
				{
					this.numPawnsLostViolently++;
				}
				this.curJob.Notify_PawnLost(pawn, cond);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_PawnLost(pawn, cond);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.PawnLost;
					signal.thing = pawn;
					signal.condition = cond;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Lord lost pawn ",
				pawn,
				" it didn't have. Condition=",
				cond
			}), false);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000F3900 File Offset: 0x000F1B00
		public void Notify_BuildingLost(Building building, DamageInfo? dinfo = null)
		{
			if (this.ownedBuildings.Contains(building))
			{
				this.ownedBuildings.Remove(building);
				this.curJob.Notify_BuildingLost(building);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_BuildingLost(building);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.BuildingLost;
					signal.thing = building;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost building " + building + " it didn't have.", false);
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000F39B4 File Offset: 0x000F1BB4
		public void Notify_BuildingDamaged(Building building, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.BuildingDamaged,
				thing = building,
				dinfo = dinfo
			});
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000F39EC File Offset: 0x000F1BEC
		public void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnDamaged,
				thing = victim,
				dinfo = dinfo
			});
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000F3A24 File Offset: 0x000F1C24
		public void Notify_PawnAttemptArrested(Pawn victim)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnArrestAttempted,
				thing = victim
			});
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000F3A54 File Offset: 0x000F1C54
		public void Notify_Clamor(Thing source, ClamorDef clamorType)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.Clamor,
				thing = source,
				clamorType = clamorType
			});
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x00002681 File Offset: 0x00000881
		public void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x000F3A8A File Offset: 0x000F1C8A
		public void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.curLordToil.Notify_ReachedDutyLocation(pawn);
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000F3A98 File Offset: 0x000F1C98
		public void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.curLordToil.Notify_ConstructionFailed(pawn, frame, newBlueprint);
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000F3AA8 File Offset: 0x000F1CA8
		public void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalLeave)
			{
				if (this.ownedPawns.Any<Pawn>() && this.faction != null)
				{
					Messages.Message("MessagePawnsLeaving".Translate(this.faction.def.pawnsPlural), this.ownedPawns, MessageTypeDefOf.NeutralEvent, true);
				}
				LordToil lordToil = this.Graph.lordToils.Find((LordToil st) => st is LordToil_PanicFlee);
				if (lordToil != null)
				{
					this.GotoToil(lordToil);
					return;
				}
				this.lordManager.RemoveLord(this);
			}
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000F3B64 File Offset: 0x000F1D64
		public void Notify_DormancyWakeup()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.DormancyWakeup
			});
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000F3B8C File Offset: 0x000F1D8C
		public void Notify_MechClusterDefeated()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.MechClusterDefeated
			});
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000F3BB4 File Offset: 0x000F1DB4
		private bool CheckTransitionOnSignal(TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				this.lastPawnHarmTick = Find.TickManager.TicksGame;
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil) && this.graph.transitions[i].CheckSignal(this, signal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000F3C34 File Offset: 0x000F1E34
		private Vector3 DebugCenter()
		{
			Vector3 result = this.Map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			if ((from p in this.ownedPawns
			where p.Spawned
			select p).Any<Pawn>())
			{
				result.x = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.x);
				result.z = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.z);
			}
			return result;
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000F3D38 File Offset: 0x000F1F38
		public void DebugDraw()
		{
			Vector3 a = this.DebugCenter();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid)
			{
				Graphics.DrawMesh(MeshPool.plane14, flagLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Building), Quaternion.identity, Lord.FlagTex, 0);
			}
			GenDraw.DrawLineBetween(a, flagLoc.ToVector3Shifted(), SimpleColor.Red);
			foreach (Pawn pawn in this.ownedPawns)
			{
				SimpleColor color = (!pawn.InMentalState) ? SimpleColor.White : SimpleColor.Yellow;
				GenDraw.DrawLineBetween(a, pawn.DrawPos, color);
			}
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000F3DE8 File Offset: 0x000F1FE8
		public void DebugOnGUI()
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			string label;
			if (this.CurLordToil != null)
			{
				label = string.Concat(new object[]
				{
					"toil ",
					this.graph.lordToils.IndexOf(this.CurLordToil),
					"\n",
					this.CurLordToil.ToString()
				});
			}
			else
			{
				label = "toil=NULL";
			}
			Vector2 vector = this.DebugCenter().MapToUIPosition();
			Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000F3E98 File Offset: 0x000F2098
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Start steal threshold: " + StealAIUtility.StartStealingMarketValueThreshold(this).ToString("F0"));
			stringBuilder.AppendLine("Duties:");
			foreach (Pawn pawn in this.ownedPawns)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					pawn.LabelCap,
					" - ",
					pawn.mindState.duty
				}));
			}
			stringBuilder.AppendLine("Raw save data:");
			stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(this));
			return stringBuilder.ToString();
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000F3F78 File Offset: 0x000F2178
		private bool ShouldDoDebugOutput()
		{
			IntVec3 a = UI.MouseCell();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid && a == flagLoc)
			{
				return true;
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (a == this.ownedPawns[i].Position)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040018CF RID: 6351
		public LordManager lordManager;

		// Token: 0x040018D0 RID: 6352
		private LordToil curLordToil;

		// Token: 0x040018D1 RID: 6353
		private StateGraph graph;

		// Token: 0x040018D2 RID: 6354
		public int loadID = -1;

		// Token: 0x040018D3 RID: 6355
		private LordJob curJob;

		// Token: 0x040018D4 RID: 6356
		public Faction faction;

		// Token: 0x040018D5 RID: 6357
		public List<Pawn> ownedPawns = new List<Pawn>();

		// Token: 0x040018D6 RID: 6358
		public List<Building> ownedBuildings = new List<Building>();

		// Token: 0x040018D7 RID: 6359
		public List<Thing> extraForbiddenThings = new List<Thing>();

		// Token: 0x040018D8 RID: 6360
		public List<string> questTags;

		// Token: 0x040018D9 RID: 6361
		public string inSignalLeave;

		// Token: 0x040018DA RID: 6362
		private bool initialized;

		// Token: 0x040018DB RID: 6363
		public int ticksInToil;

		// Token: 0x040018DC RID: 6364
		public int numPawnsLostViolently;

		// Token: 0x040018DD RID: 6365
		public int numPawnsEverGained;

		// Token: 0x040018DE RID: 6366
		public int initialColonyHealthTotal;

		// Token: 0x040018DF RID: 6367
		public int lastPawnHarmTick = -99999;

		// Token: 0x040018E0 RID: 6368
		private const int AttackTargetCacheInterval = 60;

		// Token: 0x040018E1 RID: 6369
		private static readonly Material FlagTex = MaterialPool.MatFrom("UI/Overlays/SquadFlag");

		// Token: 0x040018E2 RID: 6370
		private int tmpCurLordToilIdx = -1;

		// Token: 0x040018E3 RID: 6371
		private Dictionary<int, LordToilData> tmpLordToilData = new Dictionary<int, LordToilData>();

		// Token: 0x040018E4 RID: 6372
		private Dictionary<int, TriggerData> tmpTriggerData = new Dictionary<int, TriggerData>();
	}
}
