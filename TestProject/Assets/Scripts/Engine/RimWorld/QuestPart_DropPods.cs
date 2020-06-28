using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099B RID: 2459
	public class QuestPart_DropPods : QuestPart
	{
		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x00134C24 File Offset: 0x00132E24
		// (set) Token: 0x06003A5A RID: 14938 RVA: 0x00134C3C File Offset: 0x00132E3C
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
							Log.Error("Tried to add a destroyed thing to QuestPart_DropPods: " + thing.ToStringSafe<Thing>(), false);
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

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x00134CDC File Offset: 0x00132EDC
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
				if (this.importantLookTarget != null)
				{
					yield return this.importantLookTarget;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x00134CEC File Offset: 0x00132EEC
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00134D08 File Offset: 0x00132F08
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				Thing thing = (from x in this.Things
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing2 = this.Things.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.mapParent != null && this.mapParent.HasMap && this.Things.Any<Thing>())
				{
					Map map = this.mapParent.Map;
					IntVec3 intVec = this.dropSpot.IsValid ? this.dropSpot : this.GetRandomDropSpot();
					if (this.sendStandardLetter)
					{
						TaggedString taggedString;
						TaggedString taggedString2;
						if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
						{
							taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
						}
						else
						{
							taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.Things, "  - "));
							taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, this.quest, null, null);
					}
					if (this.joinPlayer)
					{
						for (int i = 0; i < this.pawns.Count; i++)
						{
							if (this.pawns[i].Faction != Faction.OfPlayer)
							{
								this.pawns[i].SetFaction(Faction.OfPlayer, null);
							}
						}
					}
					else if (this.makePrisoners)
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
					for (int k = 0; k < this.pawns.Count; k++)
					{
						this.pawns[k].needs.SetInitialLevels();
					}
					DropPodUtility.DropThingsNear(intVec, map, this.Things, 110, false, false, !this.useTradeDropSpot, false);
					this.importantLookTarget = this.items.Find((Thing x) => x.GetInnerIfMinified() is MonumentMarker).GetInnerIfMinified();
					this.items.Clear();
				}
				if (!this.outSignalResult.NullOrEmpty())
				{
					if (thing != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing.Named("SUBJECT")));
						return;
					}
					if (thing2 != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing2.Named("SUBJECT")));
						return;
					}
					Find.SignalManager.SendSignal(new Signal(this.outSignalResult));
				}
			}
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x001351AB File Offset: 0x001333AB
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x001351B9 File Offset: 0x001333B9
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x001351CC File Offset: 0x001333CC
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

		// Token: 0x06003A61 RID: 14945 RVA: 0x00135218 File Offset: 0x00133418
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

		// Token: 0x06003A62 RID: 14946 RVA: 0x00135274 File Offset: 0x00133474
		private IntVec3 GetRandomDropSpot()
		{
			Map map = this.mapParent.Map;
			if (this.useTradeDropSpot)
			{
				return DropCellFinder.TradeDropSpot(map);
			}
			IntVec3 result;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map) && map.reachability.CanReachColony(x), map, 1000, out result))
			{
				return result;
			}
			return DropCellFinder.RandomDropSpot(map);
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x001352DC File Offset: 0x001334DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.useTradeDropSpot, "useTradeDropSpot", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			Scribe_Collections.Look<ThingDef>(ref this.thingsToExcludeFromHyperlinks, "thingsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsToExcludeFromHyperlinks == null)
				{
					this.thingsToExcludeFromHyperlinks = new List<ThingDef>();
				}
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x00135480 File Offset: 0x00133680
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				List<Thing> list = ThingSetMakerDefOf.DebugQuestDropPodsContents.root.Generate();
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						pawn.relations.everSeenByPlayer = true;
						if (!pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						}
					}
				}
				this.Things = list;
			}
		}

		// Token: 0x0400225F RID: 8799
		public string inSignal;

		// Token: 0x04002260 RID: 8800
		public string outSignalResult;

		// Token: 0x04002261 RID: 8801
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04002262 RID: 8802
		public bool useTradeDropSpot;

		// Token: 0x04002263 RID: 8803
		public MapParent mapParent;

		// Token: 0x04002264 RID: 8804
		private List<Thing> items = new List<Thing>();

		// Token: 0x04002265 RID: 8805
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002266 RID: 8806
		public List<ThingDef> thingsToExcludeFromHyperlinks = new List<ThingDef>();

		// Token: 0x04002267 RID: 8807
		public bool joinPlayer;

		// Token: 0x04002268 RID: 8808
		public bool makePrisoners;

		// Token: 0x04002269 RID: 8809
		public string customLetterText;

		// Token: 0x0400226A RID: 8810
		public string customLetterLabel;

		// Token: 0x0400226B RID: 8811
		public LetterDef customLetterDef;

		// Token: 0x0400226C RID: 8812
		public bool sendStandardLetter = true;

		// Token: 0x0400226D RID: 8813
		private Thing importantLookTarget;
	}
}
