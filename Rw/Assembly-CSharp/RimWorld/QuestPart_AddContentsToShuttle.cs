using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000963 RID: 2403
	public class QuestPart_AddContentsToShuttle : QuestPart
	{
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060038E3 RID: 14563 RVA: 0x0012F78C File Offset: 0x0012D98C
		// (set) Token: 0x060038E4 RID: 14564 RVA: 0x0012F7A4 File Offset: 0x0012D9A4
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.items.Concat(this.pawns.Cast<Thing>());
			}
			set
			{
				this.items.Clear();
				this.pawns.Clear();
				if (value != null)
				{
					foreach (Thing thing in value)
					{
						if (thing.Destroyed)
						{
							Log.Error("Tried to add a destroyed thing to QuestPart_AddContentsToShuttle: " + thing.ToStringSafe<Thing>(), false);
						}
						else
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								this.pawns.Add(pawn);
							}
							else
							{
								this.items.Add(thing);
							}
						}
					}
				}
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x060038E5 RID: 14565 RVA: 0x0012F844 File Offset: 0x0012DA44
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.<>n__0())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				foreach (Thing outerThing in this.items)
				{
					ThingDef def = outerThing.GetInnerIfMinified().def;
					yield return new Dialog_InfoCard.Hyperlink(def, -1);
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060038E6 RID: 14566 RVA: 0x0012F854 File Offset: 0x0012DA54
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__1())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x0012F864 File Offset: 0x0012DA64
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(this.pawns[i]);
					}
				}
				CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.pawns, true, false);
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.items, true, false);
				this.items.Clear();
			}
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x0012F969 File Offset: 0x0012DB69
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0012F977 File Offset: 0x0012DB77
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x0012F988 File Offset: 0x0012DB88
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!this.items[i].Destroyed)
				{
					this.items[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.items.Clear();
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x0012F9E4 File Offset: 0x0012DBE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x0012FAA4 File Offset: 0x0012DCA4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x0400218B RID: 8587
		public string inSignal;

		// Token: 0x0400218C RID: 8588
		public Thing shuttle;

		// Token: 0x0400218D RID: 8589
		private List<Thing> items = new List<Thing>();

		// Token: 0x0400218E RID: 8590
		private List<Pawn> pawns = new List<Pawn>();
	}
}
