using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x06003A12 RID: 14866 RVA: 0x001342FB File Offset: 0x001324FB
		public virtual string DescriptionPart { get; }

		
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x00134303 File Offset: 0x00132503
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		
		// (get) Token: 0x06003A14 RID: 14868 RVA: 0x00134316 File Offset: 0x00132516
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield break;
			}
		}

		
		// (get) Token: 0x06003A15 RID: 14869 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x0013431F File Offset: 0x0013251F
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				yield break;
			}
		}

		
		// (get) Token: 0x06003A17 RID: 14871 RVA: 0x00134328 File Offset: 0x00132528
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				yield break;
			}
		}

		
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x00134331 File Offset: 0x00132531
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield break;
			}
		}

		
		// (get) Token: 0x06003A19 RID: 14873 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x0013433A File Offset: 0x0013253A
		public virtual bool PreventsAutoAccept
		{
			get
			{
				return this.RequiresAccepter;
			}
		}

		
		public virtual bool QuestPartReserves(Pawn p)
		{
			return false;
		}

		
		public virtual void Cleanup()
		{
		}

		
		public virtual void AssignDebugData()
		{
		}

		
		public virtual void PreQuestAccept()
		{
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<QuestPart.SignalListenMode>(ref this.signalListenMode, "signalListenMode", QuestPart.SignalListenMode.OngoingOnly, false);
			Scribe_Values.Look<string>(ref this.debugLabel, "debugLabel", null, false);
		}

		
		public virtual void Notify_QuestSignalReceived(Signal signal)
		{
		}

		
		public virtual void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
		}

		
		public virtual void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
		}

		
		public virtual void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
		}

		
		public virtual void Notify_PreCleanup()
		{
		}

		
		public virtual void PostQuestAdded()
		{
		}

		
		public virtual void ReplacePawnReferences(Pawn replace, Pawn with)
		{
		}

		
		public virtual void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
		}

		
		public override string ToString()
		{
			string str = base.GetType().Name + " (index=" + this.Index;
			if (!this.debugLabel.NullOrEmpty())
			{
				str = str + ", debugLabel=" + this.debugLabel;
			}
			return str + ")";
		}

		
		public string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"QuestPart_",
				this.quest.id,
				"_",
				this.Index
			});
		}

		
		public Quest quest;

		
		public QuestPart.SignalListenMode signalListenMode;

		
		public string debugLabel;

		
		public enum SignalListenMode
		{
			
			OngoingOnly,
			
			NotYetAcceptedOnly,
			
			OngoingOrNotYetAccepted,
			
			HistoricalOnly,
			
			Always
		}
	}
}
