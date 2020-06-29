using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_GiveToCaravan : QuestPart
	{
		
		// (get) Token: 0x06003A7A RID: 14970 RVA: 0x001359BD File Offset: 0x00133BBD
		// (set) Token: 0x06003A7B RID: 14971 RVA: 0x001359D8 File Offset: 0x00133BD8
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

		
		// (get) Token: 0x06003A7C RID: 14972 RVA: 0x00135A58 File Offset: 0x00133C58
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.caravan != null)
				{
					yield return this.caravan;
				}
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x00135A68 File Offset: 0x00133C68
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, true, false);
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				Caravan caravan = this.caravan;
				if (caravan == null)
				{
					signal.args.TryGetArg<Caravan>("CARAVAN", out caravan);
				}
				if (caravan != null && this.Things.Any<Thing>())
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
						caravan.AddPawn(this.pawns[i], true);
					}
					for (int j = 0; j < this.items.Count; j++)
					{
						CaravanInventoryUtility.GiveThing(caravan, this.items[j]);
					}
					this.items.Clear();
				}
			}
		}

		
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}

		
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].Destroy(DestroyMode.Vanish);
			}
			this.items.Clear();
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public string inSignal;

		
		public Caravan caravan;

		
		private List<Thing> items = new List<Thing>();

		
		private List<Pawn> pawns = new List<Pawn>();
	}
}
