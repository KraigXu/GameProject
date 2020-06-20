using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000996 RID: 2454
	public abstract class QuestPart : IExposable, ILoadReferenceable
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003A12 RID: 14866 RVA: 0x001342FB File Offset: 0x001324FB
		public virtual string DescriptionPart { get; }

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x00134303 File Offset: 0x00132503
		public int Index
		{
			get
			{
				return this.quest.PartsListForReading.IndexOf(this);
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003A14 RID: 14868 RVA: 0x00134316 File Offset: 0x00132516
		public virtual IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003A15 RID: 14869 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string QuestSelectTargetsLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x0013431F File Offset: 0x0013251F
		public virtual IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06003A17 RID: 14871 RVA: 0x00134328 File Offset: 0x00132528
		public virtual IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x00134331 File Offset: 0x00132531
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06003A19 RID: 14873 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IncreasesPopulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool RequiresAccepter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x0013433A File Offset: 0x0013253A
		public virtual bool PreventsAutoAccept
		{
			get
			{
				return this.RequiresAccepter;
			}
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool QuestPartReserves(Pawn p)
		{
			return false;
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Cleanup()
		{
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void AssignDebugData()
		{
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreQuestAccept()
		{
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00134342 File Offset: 0x00132542
		public virtual void ExposeData()
		{
			Scribe_Values.Look<QuestPart.SignalListenMode>(ref this.signalListenMode, "signalListenMode", QuestPart.SignalListenMode.OngoingOnly, false);
			Scribe_Values.Look<string>(ref this.debugLabel, "debugLabel", null, false);
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_QuestSignalReceived(Signal signal)
		{
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PreCleanup()
		{
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostQuestAdded()
		{
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ReplacePawnReferences(Pawn replace, Pawn with)
		{
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x00134368 File Offset: 0x00132568
		public override string ToString()
		{
			string str = base.GetType().Name + " (index=" + this.Index;
			if (!this.debugLabel.NullOrEmpty())
			{
				str = str + ", debugLabel=" + this.debugLabel;
			}
			return str + ")";
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x001343C2 File Offset: 0x001325C2
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

		// Token: 0x04002242 RID: 8770
		public Quest quest;

		// Token: 0x04002243 RID: 8771
		public QuestPart.SignalListenMode signalListenMode;

		// Token: 0x04002244 RID: 8772
		public string debugLabel;

		// Token: 0x0200199E RID: 6558
		public enum SignalListenMode
		{
			// Token: 0x040061A1 RID: 24993
			OngoingOnly,
			// Token: 0x040061A2 RID: 24994
			NotYetAcceptedOnly,
			// Token: 0x040061A3 RID: 24995
			OngoingOrNotYetAccepted,
			// Token: 0x040061A4 RID: 24996
			HistoricalOnly,
			// Token: 0x040061A5 RID: 24997
			Always
		}
	}
}
