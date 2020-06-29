using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Letter : QuestPart
	{
		
		// (get) Token: 0x06003999 RID: 14745 RVA: 0x00132389 File Offset: 0x00130589
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.letter.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x00132399 File Offset: 0x00130599
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__1())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.letter.relatedFaction != null)
				{
					yield return this.letter.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Letter letter = Gen.MemberwiseClone<Letter>(this.letter);
				letter.ID = Find.UniqueIDsManager.GetNextLetterID();
				ChoiceLetter choiceLetter = letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.quest = this.quest;
				}
				ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn = letter as ChoiceLetter_ChoosePawn;
				if (choiceLetter_ChoosePawn != null)
				{
					if (this.useColonistsOnMap != null && this.useColonistsOnMap.HasMap)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.useColonistsOnMap.Map.mapPawns.FreeColonists);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					Caravan caravan;
					if (this.useColonistsFromCaravanArg && signal.args.TryGetArg<Caravan>("CARAVAN", out caravan) && caravan != null)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(from x in caravan.PawnsListForReading
						where x.IsFreeColonist
						select x);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
				}
				LookTargets lookTargets;
				if (this.getLookTargetsFromSignal && !letter.lookTargets.IsValid() && SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
				{
					letter.lookTargets = lookTargets;
				}
				letter.label = signal.args.GetFormattedText(letter.label);
				ChoiceLetter choiceLetter2 = letter as ChoiceLetter;
				bool flag = true;
				if (choiceLetter2 != null)
				{
					choiceLetter2.title = signal.args.GetFormattedText(choiceLetter2.title);
					choiceLetter2.text = signal.args.GetFormattedText(choiceLetter2.text);
					if (choiceLetter2.text.NullOrEmpty())
					{
						flag = false;
					}
				}
				if (this.filterDeadPawnsFromLookTargets)
				{
					for (int i = letter.lookTargets.targets.Count - 1; i >= 0; i--)
					{
						Thing thing = letter.lookTargets.targets[i].Thing;
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.Dead)
						{
							letter.lookTargets.targets.Remove(thing);
						}
					}
				}
				if (flag)
				{
					Find.LetterStack.ReceiveLetter(letter, null);
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_References.Look<MapParent>(ref this.useColonistsOnMap, "useColonistsOnMap", false);
			Scribe_Values.Look<bool>(ref this.useColonistsFromCaravanArg, "useColonistsFromCaravanArg", false, false);
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			Scribe_Values.Look<bool>(ref this.filterDeadPawnsFromLookTargets, "filterDeadPawnsFromLookTargets", false, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.letter = LetterMaker.MakeLetter("Dev: Test", "Test text", LetterDefOf.PositiveEvent, null, null);
		}

		
		public string inSignal;

		
		public Letter letter;

		
		public bool getLookTargetsFromSignal = true;

		
		public MapParent useColonistsOnMap;

		
		public bool useColonistsFromCaravanArg;

		
		public string chosenPawnSignal;

		
		public bool filterDeadPawnsFromLookTargets;
	}
}
