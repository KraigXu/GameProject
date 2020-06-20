using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A1 RID: 2465
	public class QuestPart_PawnsArrive : QuestPart
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x00135F6C File Offset: 0x0013416C
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

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x00135F7C File Offset: 0x0013417C
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, false);
			}
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x00135F90 File Offset: 0x00134190
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				if (this.mapParent != null && this.mapParent.HasMap && this.pawns.Any<Pawn>())
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.joinPlayer && this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
					}
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = this.mapParent.Map;
					incidentParms.spawnCenter = this.spawnNear;
					PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode ?? PawnsArrivalModeDefOf.EdgeWalkIn;
					pawnsArrivalModeDef.Worker.TryResolveRaidSpawnCenter(incidentParms);
					pawnsArrivalModeDef.Worker.Arrive(this.pawns, incidentParms);
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
							if (this.joinPlayer)
							{
								taggedString = "LetterPawnsArriveAndJoin".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
								taggedString2 = "LetterLabelPawnsArriveAndJoin".Translate();
							}
							else
							{
								taggedString = "LetterPawnsArrive".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
								taggedString2 = "LetterLabelPawnsArrive".Translate();
							}
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, this.pawns[0], null, this.quest, null, null);
					}
				}
			}
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00136274 File Offset: 0x00134474
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.spawnNear, "spawnNear", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00136378 File Offset: 0x00134578
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
				pawn.relations.everSeenByPlayer = true;
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.pawns.Add(pawn);
				this.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.joinPlayer = true;
			}
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x00136462 File Offset: 0x00134662
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x00136470 File Offset: 0x00134670
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400228A RID: 8842
		public string inSignal;

		// Token: 0x0400228B RID: 8843
		public MapParent mapParent;

		// Token: 0x0400228C RID: 8844
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400228D RID: 8845
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x0400228E RID: 8846
		public IntVec3 spawnNear = IntVec3.Invalid;

		// Token: 0x0400228F RID: 8847
		public bool joinPlayer;

		// Token: 0x04002290 RID: 8848
		public string customLetterText;

		// Token: 0x04002291 RID: 8849
		public string customLetterLabel;

		// Token: 0x04002292 RID: 8850
		public LetterDef customLetterDef;

		// Token: 0x04002293 RID: 8851
		public bool sendStandardLetter = true;
	}
}
