using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_JoinPlayer : QuestPart
	{
		
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x00135D40 File Offset: 0x00133F40
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
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

		
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x00135D50 File Offset: 0x00133F50
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				if (this.joinPlayer)
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
					}
					return;
				}
				if (this.makePrisoners)
				{
					for (int j = 0; j < this.pawns.Count; j++)
					{
						if (this.pawns[j].RaceProps.Humanlike)
						{
							if (!this.pawns[j].IsPrisonerOfColony)
							{
								this.pawns[j].guest.SetGuestStatus(Faction.OfPlayer, true);
							}
							HealthUtility.TryAnesthetize(this.pawns[j]);
						}
					}
				}
			}
		}

		
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public string inSignal;

		
		public string outSignalResult;

		
		public List<Pawn> pawns = new List<Pawn>();

		
		public bool joinPlayer;

		
		public bool makePrisoners;

		
		public MapParent mapParent;
	}
}
