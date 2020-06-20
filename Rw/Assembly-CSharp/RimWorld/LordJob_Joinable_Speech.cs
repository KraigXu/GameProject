using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000781 RID: 1921
	public class LordJob_Joinable_Speech : LordJob_Joinable_Gathering
	{
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003242 RID: 12866 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool OrganizerIsStartingPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x00117D3D File Offset: 0x00115F3D
		public LordJob_Joinable_Speech()
		{
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x00118060 File Offset: 0x00116260
		public LordJob_Joinable_Speech(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x0011806B File Offset: 0x0011626B
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Speech(spot, gatheringDef, organizer);
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x00118078 File Offset: 0x00116278
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

		// Token: 0x06003247 RID: 12871 RVA: 0x00118187 File Offset: 0x00116387
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportListeningSpeech".Translate(this.organizer.Named("ORGANIZER"));
			}
			return "LordReportGivingSpeech".Translate();
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x001181C4 File Offset: 0x001163C4
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

		// Token: 0x06003249 RID: 12873 RVA: 0x0011840C File Offset: 0x0011660C
		private static bool PositiveOutcome(ThoughtDef outcome)
		{
			return outcome == ThoughtDefOf.EncouragingSpeech || outcome == ThoughtDefOf.InspirationalSpeech;
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x00118420 File Offset: 0x00116620
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

		// Token: 0x04001B48 RID: 6984
		public const float DurationHours = 5f;

		// Token: 0x04001B49 RID: 6985
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

		// Token: 0x04001B4A RID: 6986
		private static List<Tuple<ThoughtDef, float>> outcomeChancesTemp = new List<Tuple<ThoughtDef, float>>();
	}
}
