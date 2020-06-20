using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000514 RID: 1300
	public class Job : IExposable, ILoadReferenceable
	{
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x000DC621 File Offset: 0x000DA821
		public RecipeDef RecipeDef
		{
			get
			{
				if (this.bill == null)
				{
					return null;
				}
				return this.bill.recipe;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x000DC638 File Offset: 0x000DA838
		public JobDriver GetCachedDriverDirect
		{
			get
			{
				return this.cachedDriver;
			}
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000DC640 File Offset: 0x000DA840
		public void Clear()
		{
			this.def = null;
			this.targetA = LocalTargetInfo.Invalid;
			this.targetB = LocalTargetInfo.Invalid;
			this.targetC = LocalTargetInfo.Invalid;
			this.targetQueueA = null;
			this.targetQueueB = null;
			this.count = -1;
			this.countQueue = null;
			this.loadID = -1;
			this.startTick = -1;
			this.expiryInterval = -1;
			this.checkOverrideOnExpire = false;
			this.playerForced = false;
			this.placedThings = null;
			this.maxNumMeleeAttacks = int.MaxValue;
			this.maxNumStaticAttacks = int.MaxValue;
			this.locomotionUrgency = LocomotionUrgency.Jog;
			this.haulMode = HaulMode.Undefined;
			this.bill = null;
			this.commTarget = null;
			this.plantDefToSow = null;
			this.verbToUse = null;
			this.haulOpportunisticDuplicates = false;
			this.exitMapOnArrival = false;
			this.failIfCantJoinOrCreateCaravan = false;
			this.killIncappedTarget = false;
			this.ignoreForbidden = false;
			this.ignoreDesignations = false;
			this.canBash = false;
			this.canUseRangedWeapon = true;
			this.haulDroppedApparel = false;
			this.restUntilHealed = false;
			this.ignoreJoyTimeAssignment = false;
			this.doUntilGatheringEnded = false;
			this.overeat = false;
			this.attackDoorIfTargetLost = false;
			this.takeExtraIngestibles = 0;
			this.expireRequiresEnemiesNearby = false;
			this.lord = null;
			this.collideWithPawns = false;
			this.forceSleep = false;
			this.interaction = null;
			this.endIfCantShootTargetFromCurPos = false;
			this.endIfCantShootInMelee = false;
			this.checkEncumbrance = false;
			this.followRadius = 0f;
			this.endAfterTendedOnce = false;
			this.quest = null;
			this.mote = null;
			this.jobGiverThinkTree = null;
			this.jobGiver = null;
			this.workGiverDef = null;
			if (this.cachedDriver != null)
			{
				this.cachedDriver.job = null;
			}
			this.cachedDriver = null;
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000DC7EC File Offset: 0x000DA9EC
		public Job()
		{
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000DC867 File Offset: 0x000DAA67
		public Job(JobDef def) : this(def, null)
		{
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000DC876 File Offset: 0x000DAA76
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000DC888 File Offset: 0x000DAA88
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000DC928 File Offset: 0x000DAB28
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000DC9D0 File Offset: 0x000DABD0
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000DCA78 File Offset: 0x000DAC78
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000DCB18 File Offset: 0x000DAD18
		public LocalTargetInfo GetTarget(TargetIndex ind)
		{
			switch (ind)
			{
			case TargetIndex.A:
				return this.targetA;
			case TargetIndex.B:
				return this.targetB;
			case TargetIndex.C:
				return this.targetC;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000DCB4C File Offset: 0x000DAD4C
		public List<LocalTargetInfo> GetTargetQueue(TargetIndex ind)
		{
			if (ind == TargetIndex.A)
			{
				if (this.targetQueueA == null)
				{
					this.targetQueueA = new List<LocalTargetInfo>();
				}
				return this.targetQueueA;
			}
			if (ind != TargetIndex.B)
			{
				throw new ArgumentException();
			}
			if (this.targetQueueB == null)
			{
				this.targetQueueB = new List<LocalTargetInfo>();
			}
			return this.targetQueueB;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000DCB9C File Offset: 0x000DAD9C
		public void SetTarget(TargetIndex ind, LocalTargetInfo pack)
		{
			switch (ind)
			{
			case TargetIndex.A:
				this.targetA = pack;
				return;
			case TargetIndex.B:
				this.targetB = pack;
				return;
			case TargetIndex.C:
				this.targetC = pack;
				return;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000DCBD1 File Offset: 0x000DADD1
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000DCBE0 File Offset: 0x000DADE0
		public void ExposeData()
		{
			ILoadReferenceable loadReferenceable = (ILoadReferenceable)this.commTarget;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "commTarget", false);
			this.commTarget = (ICommunicable)loadReferenceable;
			Scribe_References.Look<Verb>(ref this.verbToUse, "verbToUse", false);
			Scribe_References.Look<Bill>(ref this.bill, "bill", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Defs.Look<JobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_TargetInfo.Look(ref this.targetA, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, "targetC");
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueA, "targetQueueA", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueB, "targetQueueB", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.count, "count", -1, false);
			Scribe_Collections.Look<int>(ref this.countQueue, "countQueue", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_Values.Look<int>(ref this.expiryInterval, "expiryInterval", -1, false);
			Scribe_Values.Look<bool>(ref this.checkOverrideOnExpire, "checkOverrideOnExpire", false, false);
			Scribe_Values.Look<bool>(ref this.playerForced, "playerForced", false, false);
			Scribe_Collections.Look<ThingCountClass>(ref this.placedThings, "placedThings", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.maxNumMeleeAttacks, "maxNumMeleeAttacks", int.MaxValue, false);
			Scribe_Values.Look<int>(ref this.maxNumStaticAttacks, "maxNumStaticAttacks", int.MaxValue, false);
			Scribe_Values.Look<bool>(ref this.exitMapOnArrival, "exitMapOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.failIfCantJoinOrCreateCaravan, "failIfCantJoinOrCreateCaravan", false, false);
			Scribe_Values.Look<bool>(ref this.killIncappedTarget, "killIncappedTarget", false, false);
			Scribe_Values.Look<bool>(ref this.haulOpportunisticDuplicates, "haulOpportunisticDuplicates", false, false);
			Scribe_Values.Look<HaulMode>(ref this.haulMode, "haulMode", HaulMode.Undefined, false);
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToSow, "plantDefToSow");
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
			Scribe_Values.Look<bool>(ref this.canBash, "canBash", false, false);
			Scribe_Values.Look<bool>(ref this.canUseRangedWeapon, "canUseRangedWeapon", true, false);
			Scribe_Values.Look<bool>(ref this.haulDroppedApparel, "haulDroppedApparel", false, false);
			Scribe_Values.Look<bool>(ref this.restUntilHealed, "restUntilHealed", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreJoyTimeAssignment, "ignoreJoyTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.overeat, "overeat", false, false);
			Scribe_Values.Look<bool>(ref this.attackDoorIfTargetLost, "attackDoorIfTargetLost", false, false);
			Scribe_Values.Look<int>(ref this.takeExtraIngestibles, "takeExtraIngestibles", 0, false);
			Scribe_Values.Look<bool>(ref this.expireRequiresEnemiesNearby, "expireRequiresEnemiesNearby", false, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_Values.Look<bool>(ref this.forceSleep, "forceSleep", false, false);
			Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
			Scribe_Values.Look<bool>(ref this.endIfCantShootTargetFromCurPos, "endIfCantShootTargetFromCurPos", false, false);
			Scribe_Values.Look<bool>(ref this.endIfCantShootInMelee, "endIfCantShootInMelee", false, false);
			Scribe_Values.Look<bool>(ref this.checkEncumbrance, "checkEncumbrance", false, false);
			Scribe_Values.Look<float>(ref this.followRadius, "followRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.endAfterTendedOnce, "endAfterTendedOnce", false, false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.workGiverDef, "workGiverDef");
			Scribe_Defs.Look<ThinkTreeDef>(ref this.jobGiverThinkTree, "jobGiverThinkTree");
			Scribe_Values.Look<bool>(ref this.doUntilGatheringEnded, "doUntilGatheringEnded", false, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.jobGiverKey = ((this.jobGiver != null) ? this.jobGiver.UniqueSaveKey : -1);
			}
			Scribe_Values.Look<int>(ref this.jobGiverKey, "lastJobGiverKey", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.jobGiverKey != -1 && !this.jobGiverThinkTree.TryGetThinkNodeWithSaveKey(this.jobGiverKey, out this.jobGiver))
			{
				Log.Warning("Could not find think node with key " + this.jobGiverKey, false);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbToUse != null && this.verbToUse.BuggedAfterLoading)
			{
				this.verbToUse = null;
				Log.Warning(base.GetType() + " had a bugged verbToUse after loading.", false);
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000DD02F File Offset: 0x000DB22F
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			return jobDriver;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000DD054 File Offset: 0x000DB254
		public JobDriver GetCachedDriver(Pawn driverPawn)
		{
			if (this.cachedDriver == null)
			{
				this.cachedDriver = this.MakeDriver(driverPawn);
			}
			if (this.cachedDriver.pawn != driverPawn)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to use the same driver for 2 pawns: ",
					this.cachedDriver.ToStringSafe<JobDriver>(),
					", first pawn= ",
					this.cachedDriver.pawn.ToStringSafe<Pawn>(),
					", second pawn=",
					driverPawn.ToStringSafe<Pawn>()
				}), false);
			}
			return this.cachedDriver;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000DD0DD File Offset: 0x000DB2DD
		public bool TryMakePreToilReservations(Pawn driverPawn, bool errorOnFailed)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations(errorOnFailed);
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000DD0EC File Offset: 0x000DB2EC
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000DD0FA File Offset: 0x000DB2FA
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000DD102 File Offset: 0x000DB302
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000DD120 File Offset: 0x000DB320
		public bool JobIsSameAs(Job other)
		{
			return other != null && (this == other || (this.def == other.def && !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && this.verbToUse == other.verbToUse && !(this.targetC != other.targetC) && this.commTarget == other.commTarget && this.bill == other.bill));
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000DD1AC File Offset: 0x000DB3AC
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000DD21C File Offset: 0x000DB41C
		public bool AnyTargetOutsideArea(Area zone)
		{
			if ((this.targetA.IsValid && !zone[this.targetA.Cell]) || (this.targetB.IsValid && !zone[this.targetB.Cell]) || (this.targetC.IsValid && !zone[this.targetC.Cell]))
			{
				return true;
			}
			if (this.targetQueueA != null)
			{
				foreach (LocalTargetInfo localTargetInfo in this.targetQueueA)
				{
					if (localTargetInfo.IsValid && !zone[localTargetInfo.Cell])
					{
						return true;
					}
				}
			}
			if (this.targetQueueB != null)
			{
				foreach (LocalTargetInfo localTargetInfo2 in this.targetQueueB)
				{
					if (localTargetInfo2.IsValid && !zone[localTargetInfo2.Cell])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000DD350 File Offset: 0x000DB550
		public override string ToString()
		{
			string text = this.def.ToString() + " (" + this.GetUniqueLoadID() + ")";
			if (this.targetA.IsValid)
			{
				text = text + " A=" + this.targetA.ToString();
			}
			if (this.targetB.IsValid)
			{
				text = text + " B=" + this.targetB.ToString();
			}
			if (this.targetC.IsValid)
			{
				text = text + " C=" + this.targetC.ToString();
			}
			return text;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000DD3FD File Offset: 0x000DB5FD
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID;
		}

		// Token: 0x04001698 RID: 5784
		public JobDef def;

		// Token: 0x04001699 RID: 5785
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		// Token: 0x0400169A RID: 5786
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		// Token: 0x0400169B RID: 5787
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		// Token: 0x0400169C RID: 5788
		public List<LocalTargetInfo> targetQueueA;

		// Token: 0x0400169D RID: 5789
		public List<LocalTargetInfo> targetQueueB;

		// Token: 0x0400169E RID: 5790
		public int count = -1;

		// Token: 0x0400169F RID: 5791
		public List<int> countQueue;

		// Token: 0x040016A0 RID: 5792
		public int loadID = -1;

		// Token: 0x040016A1 RID: 5793
		public int startTick = -1;

		// Token: 0x040016A2 RID: 5794
		public int expiryInterval = -1;

		// Token: 0x040016A3 RID: 5795
		public bool checkOverrideOnExpire;

		// Token: 0x040016A4 RID: 5796
		public bool playerForced;

		// Token: 0x040016A5 RID: 5797
		public List<ThingCountClass> placedThings;

		// Token: 0x040016A6 RID: 5798
		public int maxNumMeleeAttacks = int.MaxValue;

		// Token: 0x040016A7 RID: 5799
		public int maxNumStaticAttacks = int.MaxValue;

		// Token: 0x040016A8 RID: 5800
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		// Token: 0x040016A9 RID: 5801
		public HaulMode haulMode;

		// Token: 0x040016AA RID: 5802
		public Bill bill;

		// Token: 0x040016AB RID: 5803
		public ICommunicable commTarget;

		// Token: 0x040016AC RID: 5804
		public ThingDef plantDefToSow;

		// Token: 0x040016AD RID: 5805
		public Verb verbToUse;

		// Token: 0x040016AE RID: 5806
		public bool haulOpportunisticDuplicates;

		// Token: 0x040016AF RID: 5807
		public bool exitMapOnArrival;

		// Token: 0x040016B0 RID: 5808
		public bool failIfCantJoinOrCreateCaravan;

		// Token: 0x040016B1 RID: 5809
		public bool killIncappedTarget;

		// Token: 0x040016B2 RID: 5810
		public bool ignoreForbidden;

		// Token: 0x040016B3 RID: 5811
		public bool ignoreDesignations;

		// Token: 0x040016B4 RID: 5812
		public bool canBash;

		// Token: 0x040016B5 RID: 5813
		public bool canUseRangedWeapon = true;

		// Token: 0x040016B6 RID: 5814
		public bool haulDroppedApparel;

		// Token: 0x040016B7 RID: 5815
		public bool restUntilHealed;

		// Token: 0x040016B8 RID: 5816
		public bool ignoreJoyTimeAssignment;

		// Token: 0x040016B9 RID: 5817
		public bool doUntilGatheringEnded;

		// Token: 0x040016BA RID: 5818
		public bool overeat;

		// Token: 0x040016BB RID: 5819
		public bool attackDoorIfTargetLost;

		// Token: 0x040016BC RID: 5820
		public int takeExtraIngestibles;

		// Token: 0x040016BD RID: 5821
		public bool expireRequiresEnemiesNearby;

		// Token: 0x040016BE RID: 5822
		public Lord lord;

		// Token: 0x040016BF RID: 5823
		public bool collideWithPawns;

		// Token: 0x040016C0 RID: 5824
		public bool forceSleep;

		// Token: 0x040016C1 RID: 5825
		public InteractionDef interaction;

		// Token: 0x040016C2 RID: 5826
		public bool endIfCantShootTargetFromCurPos;

		// Token: 0x040016C3 RID: 5827
		public bool endIfCantShootInMelee;

		// Token: 0x040016C4 RID: 5828
		public bool checkEncumbrance;

		// Token: 0x040016C5 RID: 5829
		public float followRadius;

		// Token: 0x040016C6 RID: 5830
		public bool endAfterTendedOnce;

		// Token: 0x040016C7 RID: 5831
		public Quest quest;

		// Token: 0x040016C8 RID: 5832
		public Mote mote;

		// Token: 0x040016C9 RID: 5833
		public ThinkTreeDef jobGiverThinkTree;

		// Token: 0x040016CA RID: 5834
		public ThinkNode jobGiver;

		// Token: 0x040016CB RID: 5835
		public WorkGiverDef workGiverDef;

		// Token: 0x040016CC RID: 5836
		private JobDriver cachedDriver;

		// Token: 0x040016CD RID: 5837
		private int jobGiverKey = -1;
	}
}
