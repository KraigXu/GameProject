using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		
		
		public virtual string DescriptionPart { get; }

		
		
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		
		
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield break;
			}
		}

		
		
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		
		
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				yield break;
			}
		}

		
		
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				yield break;
			}
		}

		
		
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield break;
			}
		}

		
		
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		
		
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		
		
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
