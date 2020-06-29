using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_Joinable_Speech : LordJob_Joinable_Gathering
	{
		
		
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		
		
		public override bool OrganizerIsStartingPawn
		{
			get
			{
				return true;
			}
		}

		
		public LordJob_Joinable_Speech()
		{
		}

		
		public LordJob_Joinable_Speech(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Speech(spot, gatheringDef, organizer);
		}

		
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = this.CreateGatheringToil(this.spot, this.organizer, this.gatheringDef);
			stateGraph.AddToil(lordToil);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			float speechDuration = 12500f;
			Transition transition = new Transition(lordToil, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1));
			transition.AddTrigger(new Trigger_PawnKilled());
			transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
			transition.AddPreAction(new TransitionAction_Custom(delegate
			{
				this.ApplyOutcome((float)this.lord.ticksInToil / speechDuration);
			}));
			stateGraph.AddTransition(transition, false);
			this.timeoutTrigger = new Trigger_TicksPassedAfterConditionMet((int)speechDuration, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
			Transition transition2 = new Transition(lordToil, lordToil_End, false, true);
			transition2.AddTrigger(this.timeoutTrigger);
			transition2.AddPreAction(new TransitionAction_Custom(delegate
			{
				this.ApplyOutcome(1f);
			}));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportListeningSpeech".Translate(this.organizer.Named("ORGANIZER"));
			}
			return "LordReportGivingSpeech".Translate();
		}

		
		protected virtual void ApplyOutcome(float progress)
		{
			if (progress < 0.5f)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelSpeechCancelled".Translate(), "LetterSpeechCancelled".Translate(this.organizer.Named("ORGANIZER")).CapitalizeFirst(), LetterDefOf.NegativeEvent, this.organizer, null, null, null, null);
				return;
			}
			ThoughtDef key = LordJob_Joinable_Speech.OutcomeThoughtChances.RandomElementByWeight(delegate(KeyValuePair<ThoughtDef, float> t)
			{
				if (!LordJob_Joinable_Speech.PositiveOutcome(t.Key))
				{
					return LordJob_Joinable_Speech.OutcomeThoughtChances[t.Key];
				}
				return LordJob_Joinable_Speech.OutcomeThoughtChances[t.Key] * this.organizer.GetStatValue(StatDefOf.SocialImpact, true) * progress;
			}).Key;
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				if (pawn != this.organizer && this.organizer.Position.InHorDistOf(pawn.Position, 18f))
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(key, this.organizer);
				}
			}
			TaggedString taggedString = "LetterFinishedSpeech".Translate(this.organizer.Named("ORGANIZER")).CapitalizeFirst() + " " + ("Letter" + key.defName).Translate();
			if (progress < 1f)
			{
				taggedString += "\n\n" + "LetterSpeechInterrupted".Translate(progress.ToStringPercent(), this.organizer.Named("ORGANIZER"));
			}
			Find.LetterStack.ReceiveLetter(key.stages[0].LabelCap, taggedString, LordJob_Joinable_Speech.PositiveOutcome(key) ? LetterDefOf.PositiveEvent : LetterDefOf.NegativeEvent, this.organizer, null, null, null, null);
			Ability ability = this.organizer.abilities.GetAbility(AbilityDefOf.Speech);
			RoyalTitle mostSeniorTitle = this.organizer.royalty.MostSeniorTitle;
			if (ability != null && mostSeniorTitle != null)
			{
				ability.StartCooldown(mostSeniorTitle.def.speechCooldown.RandomInRange);
			}
		}

		
		private static bool PositiveOutcome(ThoughtDef outcome)
		{
			return outcome == ThoughtDefOf.EncouragingSpeech || outcome == ThoughtDefOf.InspirationalSpeech;
		}

		
		public static IEnumerable<Tuple<ThoughtDef, float>> OutcomeChancesForPawn(Pawn p)
		{
			LordJob_Joinable_Speech.outcomeChancesTemp.Clear();
			float num = 1f / LordJob_Joinable_Speech.OutcomeThoughtChances.Sum(delegate(KeyValuePair<ThoughtDef, float> c)
			{
				if (!LordJob_Joinable_Speech.PositiveOutcome(c.Key))
				{
					return c.Value;
				}
				return c.Value * p.GetStatValue(StatDefOf.SocialImpact, true);
			});
			foreach (KeyValuePair<ThoughtDef, float> keyValuePair in LordJob_Joinable_Speech.OutcomeThoughtChances)
			{
				LordJob_Joinable_Speech.outcomeChancesTemp.Add(new Tuple<ThoughtDef, float>(keyValuePair.Key, (LordJob_Joinable_Speech.PositiveOutcome(keyValuePair.Key) ? (keyValuePair.Value * p.GetStatValue(StatDefOf.SocialImpact, true)) : keyValuePair.Value) * num));
			}
			return LordJob_Joinable_Speech.outcomeChancesTemp;
		}

		
		public const float DurationHours = 5f;

		
		public static readonly Dictionary<ThoughtDef, float> OutcomeThoughtChances = new Dictionary<ThoughtDef, float>
		{
			{
				ThoughtDefOf.TerribleSpeech,
				0.05f
			},
			{
				ThoughtDefOf.UninspiringSpeech,
				0.15f
			},
			{
				ThoughtDefOf.EncouragingSpeech,
				0.6f
			},
			{
				ThoughtDefOf.InspirationalSpeech,
				0.2f
			}
		};

		
		private static List<Tuple<ThoughtDef, float>> outcomeChancesTemp = new List<Tuple<ThoughtDef, float>>();
	}
}
