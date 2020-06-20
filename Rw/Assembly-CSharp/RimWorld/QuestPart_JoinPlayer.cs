using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A0 RID: 2464
	public class QuestPart_JoinPlayer : QuestPart
	{
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x00135D40 File Offset: 0x00133F40
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
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

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x00135D50 File Offset: 0x00133F50
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x00135D6C File Offset: 0x00133F6C
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

		// Token: 0x06003A8A RID: 14986 RVA: 0x00135E86 File Offset: 0x00134086
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x00135E94 File Offset: 0x00134094
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00135EA4 File Offset: 0x001340A4
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

		// Token: 0x04002284 RID: 8836
		public string inSignal;

		// Token: 0x04002285 RID: 8837
		public string outSignalResult;

		// Token: 0x04002286 RID: 8838
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002287 RID: 8839
		public bool joinPlayer;

		// Token: 0x04002288 RID: 8840
		public bool makePrisoners;

		// Token: 0x04002289 RID: 8841
		public MapParent mapParent;
	}
}
