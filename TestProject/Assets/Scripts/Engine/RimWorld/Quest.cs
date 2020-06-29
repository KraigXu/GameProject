using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class Quest : IExposable, ILoadReferenceable, ISignalReceiver
	{
		
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x0012C466 File Offset: 0x0012A666
		public List<QuestPart> PartsListForReading
		{
			get
			{
				return this.parts;
			}
		}

		
		// (get) Token: 0x060037FA RID: 14330 RVA: 0x0012C46E File Offset: 0x0012A66E
		public int TicksSinceAppeared
		{
			get
			{
				return Find.TickManager.TicksGame - this.appearanceTick;
			}
		}

		
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x0012C481 File Offset: 0x0012A681
		public int TicksSinceAccepted
		{
			get
			{
				if (this.acceptanceTick >= 0)
				{
					return Find.TickManager.TicksGame - this.acceptanceTick;
				}
				return -1;
			}
		}

		
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x0012C49F File Offset: 0x0012A69F
		public int TicksSinceCleanup
		{
			get
			{
				if (!this.cleanedUp)
				{
					return -1;
				}
				return Find.TickManager.TicksGame - this.cleanupTick;
			}
		}

		
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x0012C4BC File Offset: 0x0012A6BC
		public string AccepterPawnLabelCap
		{
			get
			{
				if (this.accepterPawn == null)
				{
					return this.accepterPawnLabel;
				}
				return this.accepterPawn.LabelCap;
			}
		}

		
		// (get) Token: 0x060037FE RID: 14334 RVA: 0x0012C4D8 File Offset: 0x0012A6D8
		public string InitiateSignal
		{
			get
			{
				return "Quest" + this.id + ".Initiate";
			}
		}

		
		// (get) Token: 0x060037FF RID: 14335 RVA: 0x0012C4F4 File Offset: 0x0012A6F4
		public bool EverAccepted
		{
			get
			{
				return this.initiallyAccepted || this.acceptanceTick >= 0;
			}
		}

		
		// (get) Token: 0x06003800 RID: 14336 RVA: 0x0012C50C File Offset: 0x0012A70C
		public Pawn AccepterPawn
		{
			get
			{
				return this.accepterPawn;
			}
		}

		
		// (get) Token: 0x06003801 RID: 14337 RVA: 0x0012C514 File Offset: 0x0012A714
		public bool RequiresAccepter
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].RequiresAccepter)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x06003802 RID: 14338 RVA: 0x0012C550 File Offset: 0x0012A750
		public QuestState State
		{
			get
			{
				if (this.ticksUntilAcceptanceExpiry == 0)
				{
					return QuestState.EndedOfferExpired;
				}
				if (this.ended)
				{
					if (this.endOutcome == QuestEndOutcome.Success)
					{
						return QuestState.EndedSuccess;
					}
					if (this.endOutcome == QuestEndOutcome.Fail)
					{
						return QuestState.EndedFailed;
					}
					if (this.endOutcome == QuestEndOutcome.InvalidPreAcceptance)
					{
						return QuestState.EndedInvalid;
					}
					return QuestState.EndedUnknownOutcome;
				}
				else
				{
					if (this.acceptanceTick < 0)
					{
						return QuestState.NotYetAccepted;
					}
					return QuestState.Ongoing;
				}
			}
		}

		
		// (get) Token: 0x06003803 RID: 14339 RVA: 0x0012C59E File Offset: 0x0012A79E
		public IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestLookTargets).Distinct<GlobalTargetInfo>();
			}
		}

		
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x0012C5CF File Offset: 0x0012A7CF
		public IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestSelectTargets).Distinct<GlobalTargetInfo>();
			}
		}

		
		// (get) Token: 0x06003805 RID: 14341 RVA: 0x0012C600 File Offset: 0x0012A800
		public IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.InvolvedFactions).Distinct<Faction>();
			}
		}

		
		// (get) Token: 0x06003806 RID: 14342 RVA: 0x0012C631 File Offset: 0x0012A831
		public IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.Hyperlinks).Distinct<Dialog_InfoCard.Hyperlink>();
			}
		}

		
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x0012C662 File Offset: 0x0012A862
		public bool Historical
		{
			get
			{
				return this.State != QuestState.NotYetAccepted && this.State != QuestState.Ongoing;
			}
		}

		
		// (get) Token: 0x06003808 RID: 14344 RVA: 0x0012C67C File Offset: 0x0012A87C
		public bool IncreasesPopulation
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].IncreasesPopulation)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		public static Quest MakeRaw()
		{
			return new Quest
			{
				id = Find.UniqueIDsManager.GetNextQuestID(),
				appearanceTick = Find.TickManager.TicksGame,
				name = "Unnamed quest"
			};
		}

		
		public void QuestTick()
		{
			if (this.Historical)
			{
				if (!this.cleanedUp)
				{
					this.CleanupQuestParts();
				}
				if (this.TicksSinceCleanup >= 1800000)
				{
					this.parts.Clear();
				}
				return;
			}
			if (this.ticksUntilAcceptanceExpiry > 0 && this.State == QuestState.NotYetAccepted)
			{
				this.ticksUntilAcceptanceExpiry--;
				if (this.ticksUntilAcceptanceExpiry == 0 && !this.cleanedUp)
				{
					this.CleanupQuestParts();
				}
			}
			if (!this.Historical)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.parts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						try
						{
							questPartActivable.QuestPartTick();
						}
						catch (Exception arg)
						{
							Log.Error("Exception ticking QuestPart: " + arg, false);
						}
						if (this.Historical)
						{
							break;
						}
					}
				}
			}
		}

		
		public void AddPart(QuestPart part)
		{
			if (this.parts.Contains(part))
			{
				Log.Error("Tried to add the same QuestPart twice: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>(), false);
				return;
			}
			part.quest = this;
			this.parts.Add(part);
		}

		
		public void RemovePart(QuestPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Tried to remove QuestPart which doesn't exist: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>(), false);
				return;
			}
			part.quest = null;
			this.parts.Remove(part);
		}

		
		public void Accept(Pawn by)
		{
			if (this.State != QuestState.NotYetAccepted)
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].PreQuestAccept();
			}
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.accepterPawn = by;
			this.dismissed = false;
			this.Initiate();
		}

		
		public void End(QuestEndOutcome outcome, bool sendLetter = true)
		{
			if (this.Historical)
			{
				Log.Error("Tried to resolve a historical quest. id=" + this.id, false);
				return;
			}
			this.ended = true;
			this.endOutcome = outcome;
			this.CleanupQuestParts();
			if (!this.EverAccepted && this.State == QuestState.EndedOfferExpired)
			{
				return;
			}
			if (sendLetter)
			{
				string key = null;
				string key2 = null;
				LetterDef textLetterDef = null;
				switch (this.State)
				{
				case QuestState.EndedUnknownOutcome:
					key2 = "LetterQuestConcludedLabel";
					key = "LetterQuestCompletedConcluded";
					textLetterDef = LetterDefOf.NeutralEvent;
					SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
					break;
				case QuestState.EndedSuccess:
					key2 = "LetterQuestCompletedLabel";
					key = "LetterQuestCompletedSuccess";
					textLetterDef = LetterDefOf.PositiveEvent;
					SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
					break;
				case QuestState.EndedFailed:
					key2 = "LetterQuestFailedLabel";
					key = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
					break;
				}
				Find.LetterStack.ReceiveLetter(key2.Translate(), key.Translate(this.name.CapitalizeFirst()), textLetterDef, null, null, this, null, null);
			}
		}

		
		public bool QuestReserves(Pawn p)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		
		public void SetInitiallyAccepted()
		{
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.ticksUntilAcceptanceExpiry = -1;
			this.initiallyAccepted = true;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.appearanceTick, "appearanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.acceptanceTick, "acceptanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.ticksUntilAcceptanceExpiry, "ticksUntilAcceptanceExpiry", -1, false);
			Scribe_References.Look<Pawn>(ref this.accepterPawn, "acceptedBy", false);
			Scribe_Values.Look<string>(ref this.accepterPawnLabel, "acceptedByLabel", null, false);
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<QuestEndOutcome>(ref this.endOutcome, "endOutcome", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<bool>(ref this.cleanedUp, "cleanedUp", false, false);
			Scribe_Values.Look<int>(ref this.cleanupTick, "cleanupTick", -1, false);
			Scribe_Values.Look<bool>(ref this.initiallyAccepted, "initiallyAccepted", false, false);
			Scribe_Values.Look<bool>(ref this.dismissed, "dismissed", false, false);
			Scribe_Values.Look<bool>(ref this.hiddenInUI, "hiddenInUI", false, false);
			Scribe_Values.Look<int>(ref this.challengeRating, "challengeRating", 0, false);
			Scribe_Values.Look<TaggedString>(ref this.description, "description", default(TaggedString), false);
			Scribe_Values.Look<string>(ref this.lastSlateStateDebug, "lastSlateStateDebug", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.root, "root");
			Scribe_Collections.Look<string>(ref this.signalsReceivedDebug, "signalsReceivedDebug", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<QuestPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.parts.RemoveAll((QuestPart x) => x == null) != 0)
				{
					Log.Error("Some quest parts were null after loading.", false);
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].quest = this;
				}
			}
		}

		
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			if (this.accepterPawn == pawn)
			{
				this.accepterPawn = null;
				this.accepterPawnLabel = pawn.LabelCap;
			}
		}

		
		public void Notify_SignalReceived(Signal signal)
		{
			if (!signal.tag.StartsWith("Quest" + this.id + "."))
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					bool flag;
					switch (this.parts[i].signalListenMode)
					{
					case QuestPart.SignalListenMode.OngoingOnly:
						flag = (this.State == QuestState.Ongoing);
						break;
					case QuestPart.SignalListenMode.NotYetAcceptedOnly:
						flag = (this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.OngoingOrNotYetAccepted:
						flag = (this.State == QuestState.Ongoing || this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.HistoricalOnly:
						flag = this.Historical;
						break;
					case QuestPart.SignalListenMode.Always:
						flag = true;
						break;
					default:
						flag = false;
						break;
					}
					if (flag)
					{
						this.parts[i].Notify_QuestSignalReceived(signal);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error while processing a quest signal: " + arg, false);
				}
			}
		}

		
		public void Initiate()
		{
			Find.SignalManager.SendSignal(new Signal(this.InitiateSignal));
		}

		
		public void CleanupQuestParts()
		{
			if (this.cleanedUp)
			{
				return;
			}
			this.cleanupTick = Find.TickManager.TicksGame;
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					this.parts[i].Notify_PreCleanup();
				}
				catch (Exception arg)
				{
					Log.Error("Error in QuestPart Notify_PreCleanup: " + arg, false);
				}
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				try
				{
					this.parts[j].Cleanup();
				}
				catch (Exception arg2)
				{
					Log.Error("Error in QuestPart cleanup: " + arg2, false);
				}
			}
			this.cleanedUp = true;
		}

		
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_ThingsProduced(worker, things);
			}
		}

		
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PlantHarvested(worker, harvested);
			}
		}

		
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnKilled(pawn, dinfo);
			}
		}

		
		public string GetUniqueLoadID()
		{
			return "Quest_" + this.id;
		}

		
		public int id;

		
		private List<QuestPart> parts = new List<QuestPart>();

		
		public string name;

		
		public TaggedString description;

		
		public float points;

		
		public int challengeRating = -1;

		
		public List<string> tags = new List<string>();

		
		public string lastSlateStateDebug;

		
		public QuestScriptDef root;

		
		public int appearanceTick = -1;

		
		public int acceptanceTick = -1;

		
		public bool initiallyAccepted;

		
		public bool dismissed;

		
		public bool hiddenInUI;

		
		public int ticksUntilAcceptanceExpiry = -1;

		
		private Pawn accepterPawn;

		
		private string accepterPawnLabel;

		
		public List<string> signalsReceivedDebug;

		
		private bool ended;

		
		private QuestEndOutcome endOutcome;

		
		private bool cleanedUp;

		
		public int cleanupTick = -1;

		
		public const int MaxSignalsReceivedDebugCount = 25;

		
		private const int RemoveAllQuestPartsAfterTicksSinceCleanup = 1800000;
	}
}
