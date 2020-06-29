using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	
	public class Job : IExposable, ILoadReferenceable
	{
		
		
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

		
		
		public JobDriver GetCachedDriverDirect
		{
			get
			{
				return this.cachedDriver;
			}
		}

		
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

		
		public Job()
		{
		}

		
		public Job(JobDef def) : this(def, null)
		{
		}

		
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		
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

		
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		
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

		
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			return jobDriver;
		}

		
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

		
		public bool TryMakePreToilReservations(Pawn driverPawn, bool errorOnFailed)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations(errorOnFailed);
		}

		
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		
		public bool JobIsSameAs(Job other)
		{
			return other != null && (this == other || (this.def == other.def && !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && this.verbToUse == other.verbToUse && !(this.targetC != other.targetC) && this.commTarget == other.commTarget && this.bill == other.bill));
		}

		
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		
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

		
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID;
		}

		
		public JobDef def;

		
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		
		public List<LocalTargetInfo> targetQueueA;

		
		public List<LocalTargetInfo> targetQueueB;

		
		public int count = -1;

		
		public List<int> countQueue;

		
		public int loadID = -1;

		
		public int startTick = -1;

		
		public int expiryInterval = -1;

		
		public bool checkOverrideOnExpire;

		
		public bool playerForced;

		
		public List<ThingCountClass> placedThings;

		
		public int maxNumMeleeAttacks = int.MaxValue;

		
		public int maxNumStaticAttacks = int.MaxValue;

		
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		
		public HaulMode haulMode;

		
		public Bill bill;

		
		public ICommunicable commTarget;

		
		public ThingDef plantDefToSow;

		
		public Verb verbToUse;

		
		public bool haulOpportunisticDuplicates;

		
		public bool exitMapOnArrival;

		
		public bool failIfCantJoinOrCreateCaravan;

		
		public bool killIncappedTarget;

		
		public bool ignoreForbidden;

		
		public bool ignoreDesignations;

		
		public bool canBash;

		
		public bool canUseRangedWeapon = true;

		
		public bool haulDroppedApparel;

		
		public bool restUntilHealed;

		
		public bool ignoreJoyTimeAssignment;

		
		public bool doUntilGatheringEnded;

		
		public bool overeat;

		
		public bool attackDoorIfTargetLost;

		
		public int takeExtraIngestibles;

		
		public bool expireRequiresEnemiesNearby;

		
		public Lord lord;

		
		public bool collideWithPawns;

		
		public bool forceSleep;

		
		public InteractionDef interaction;

		
		public bool endIfCantShootTargetFromCurPos;

		
		public bool endIfCantShootInMelee;

		
		public bool checkEncumbrance;

		
		public float followRadius;

		
		public bool endAfterTendedOnce;

		
		public Quest quest;

		
		public Mote mote;

		
		public ThinkTreeDef jobGiverThinkTree;

		
		public ThinkNode jobGiver;

		
		public WorkGiverDef workGiverDef;

		
		private JobDriver cachedDriver;

		
		private int jobGiverKey = -1;
	}
}
