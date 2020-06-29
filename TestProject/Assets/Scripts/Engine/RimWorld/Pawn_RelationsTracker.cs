using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Pawn_RelationsTracker : IExposable
	{
		
		
		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		
		
		public IEnumerable<Pawn> Children
		{
			get
			{
				foreach (Pawn pawn in this.pawnsWithDirectRelationsWithMe)
				{
					List<DirectPawnRelation> list = pawn.relations.directRelations;
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].otherPawn == this.pawn && list[i].def == PawnRelationDefOf.Parent)
						{
							yield return pawn;
							break;
						}
					}
				}
				HashSet<Pawn>.Enumerator enumerator = default(HashSet<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		
		
		public int ChildrenCount
		{
			get
			{
				return this.Children.Count<Pawn>();
			}
		}

		
		
		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any<DirectPawnRelation>() || this.pawnsWithDirectRelationsWithMe.Any<Pawn>();
			}
		}

		
		
		public IEnumerable<Pawn> FamilyByBlood
		{
			get
			{
				if (this.canCacheFamilyByBlood)
				{
					if (!this.familyByBloodIsCached)
					{
						this.cachedFamilyByBlood.Clear();
						foreach (Pawn item in this.FamilyByBlood_Internal)
						{
							this.cachedFamilyByBlood.Add(item);
						}
						this.familyByBloodIsCached = true;
					}
					return this.cachedFamilyByBlood;
				}
				return this.FamilyByBlood_Internal;
			}
		}

		
		
		private IEnumerable<Pawn> FamilyByBlood_Internal
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> familyStack = null;
				List<Pawn> familyChildrenStack = null;
				HashSet<Pawn> familyVisited = null;
				try
				{
					familyStack = SimplePool<List<Pawn>>.Get();
					familyChildrenStack = SimplePool<List<Pawn>>.Get();
					familyVisited = SimplePool<HashSet<Pawn>>.Get();
					familyStack.Add(this.pawn);
					familyVisited.Add(this.pawn);
					while (familyStack.Any<Pawn>())
					{
						Pawn p = familyStack[familyStack.Count - 1];
						familyStack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						Pawn father = p.GetFather();
						if (father != null && !familyVisited.Contains(father))
						{
							familyStack.Add(father);
							familyVisited.Add(father);
						}
						Pawn mother = p.GetMother();
						if (mother != null && !familyVisited.Contains(mother))
						{
							familyStack.Add(mother);
							familyVisited.Add(mother);
						}
						familyChildrenStack.Clear();
						familyChildrenStack.Add(p);
						while (familyChildrenStack.Any<Pawn>())
						{
							Pawn child = familyChildrenStack[familyChildrenStack.Count - 1];
							familyChildrenStack.RemoveLast<Pawn>();
							if (child != p && child != this.pawn)
							{
								yield return child;
							}
							foreach (Pawn item in child.relations.Children)
							{
								if (!familyVisited.Contains(item))
								{
									familyChildrenStack.Add(item);
									familyVisited.Add(item);
								}
							}
							child = null;
						}
						p = null;
					}
				}
				finally
				{
					familyStack.Clear();
					SimplePool<List<Pawn>>.Return(familyStack);
					familyChildrenStack.Clear();
					SimplePool<List<Pawn>>.Return(familyChildrenStack);
					familyVisited.Clear();
					SimplePool<HashSet<Pawn>>.Return(familyVisited);
				}
				yield break;
				yield break;
			}
		}

		
		
		public IEnumerable<Pawn> PotentiallyRelatedPawns
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> stack = null;
				HashSet<Pawn> visited = null;
				try
				{
					stack = SimplePool<List<Pawn>>.Get();
					visited = SimplePool<HashSet<Pawn>>.Get();
					stack.Add(this.pawn);
					visited.Add(this.pawn);
					while (stack.Any<Pawn>())
					{
						Pawn p = stack[stack.Count - 1];
						stack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						for (int i = 0; i < p.relations.directRelations.Count; i++)
						{
							Pawn otherPawn = p.relations.directRelations[i].otherPawn;
							if (!visited.Contains(otherPawn))
							{
								stack.Add(otherPawn);
								visited.Add(otherPawn);
							}
						}
						foreach (Pawn item in p.relations.pawnsWithDirectRelationsWithMe)
						{
							if (!visited.Contains(item))
							{
								stack.Add(item);
								visited.Add(item);
							}
						}
						p = null;
					}
				}
				finally
				{
					stack.Clear();
					SimplePool<List<Pawn>>.Return(stack);
					visited.Clear();
					SimplePool<HashSet<Pawn>>.Return(visited);
				}
				yield break;
				yield break;
			}
		}

		
		
		public IEnumerable<Pawn> RelatedPawns
		{
			get
			{
				this.canCacheFamilyByBlood = true;
				this.familyByBloodIsCached = false;
				this.cachedFamilyByBlood.Clear();
				try
				{
					foreach (Pawn pawn in this.PotentiallyRelatedPawns)
					{
						if ((this.familyByBloodIsCached && this.cachedFamilyByBlood.Contains(pawn)) || this.pawn.GetRelations(pawn).Any<PawnRelationDef>())
						{
							yield return pawn;
						}
					}
					IEnumerator<Pawn> enumerator = null;
				}
				finally
				{
					this.canCacheFamilyByBlood = false;
					this.familyByBloodIsCached = false;
					this.cachedFamilyByBlood.Clear();
				}
				yield break;
				yield break;
			}
		}

		
		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<DirectPawnRelation>(ref this.directRelations, "directRelations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].otherPawn == null)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Pawn ",
							this.pawn,
							" has relation \"",
							this.directRelations[i].def.defName,
							"\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships)."
						}), false);
					}
				}
				this.directRelations.RemoveAll((DirectPawnRelation x) => x.otherPawn == null);
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					this.directRelations[j].otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
				}
			}
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", true, false);
			Scribe_Values.Look<bool>(ref this.canGetRescuedThought, "canGetRescuedThought", true, false);
			Scribe_Values.Look<MarriageNameChange>(ref this.nextMarriageNameChange, "nextMarriageNameChange", MarriageNameChange.NoChange, false);
			Scribe_References.Look<Pawn>(ref this.relativeInvolvedInRescueQuest, "relativeInvolvedInRescueQuest", false);
		}

		
		public void RelationsTrackerTick()
		{
			if (this.pawn.Dead)
			{
				return;
			}
			this.Tick_CheckStartMarriageCeremony();
			this.Tick_CheckDevelopBondRelation();
		}

		
		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				return null;
			}
			return this.directRelations.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == otherPawn);
		}

		
		public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				return null;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		
		public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				return false;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		
		public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to directly add implied pawn relation ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
				return;
			}
			if (otherPawn == this.pawn)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add pawn relation ",
					def,
					" with self, pawn=",
					this.pawn
				}), false);
				return;
			}
			if (this.DirectRelationExists(def, otherPawn))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add the same relation twice: ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
				return;
			}
			int startTicks = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
			def.Worker.OnRelationCreated(this.pawn, otherPawn);
			this.directRelations.Add(new DirectPawnRelation(def, otherPawn, startTicks));
			otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
			if (def.reflexive)
			{
				otherPawn.relations.directRelations.Add(new DirectPawnRelation(def, this.pawn, startTicks));
				this.pawnsWithDirectRelationsWithMe.Add(otherPawn);
			}
			this.GainedOrLostDirectRelation();
			otherPawn.relations.GainedOrLostDirectRelation();
		}

		
		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		
		public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (!this.TryRemoveDirectRelation(def, otherPawn))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not remove relation ",
					def,
					" because it's not here. pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
			}
		}

		
		public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to remove implied pawn relation ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
				return false;
			}



			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == def && this.directRelations[i].otherPawn == otherPawn)
				{
					if (def.reflexive)
					{
						List<DirectPawnRelation> list = otherPawn.relations.directRelations;
						Predicate<DirectPawnRelation> match = (((DirectPawnRelation x) => x.def == def && x.otherPawn == this.pawn));

						DirectPawnRelation item = list.Find(match);
						list.Remove(item);
						Predicate<DirectPawnRelation> match2 = (((DirectPawnRelation x) => x.otherPawn == this.pawn));

						if (list.Find(match2) == null)
						{
							this.pawnsWithDirectRelationsWithMe.Remove(otherPawn);
						}
					}
					this.directRelations.RemoveAt(i);
					List<DirectPawnRelation> list2 = this.directRelations;
					Predicate<DirectPawnRelation> match3 = (((DirectPawnRelation x) => x.otherPawn == otherPawn));

					if (list2.Find(match3) == null)
					{
						otherPawn.relations.pawnsWithDirectRelationsWithMe.Remove(this.pawn);
					}
					this.GainedOrLostDirectRelation();
					otherPawn.relations.GainedOrLostDirectRelation();
					return true;
				}
			}
			return false;
		}

		
		public int OpinionOf(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return 0;
			}
			if (this.pawn.Dead)
			{
				return 0;
			}
			int num = 0;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
			{
				num += pawnRelationDef.opinionOffset;
			}
			if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				num += this.pawn.needs.mood.thoughts.TotalOpinionOffset(other);
			}
			if (num != 0)
			{
				float num2 = 1f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].CurStage != null)
					{
						num2 *= hediffs[i].CurStage.opinionOfOthersFactor;
					}
				}
				num = Mathf.RoundToInt((float)num * num2);
			}
			if (num > 0 && this.pawn.HostileTo(other))
			{
				num = 0;
			}
			return Mathf.Clamp(num, -100, 100);
		}

		
		public string OpinionExplanation(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("OpinionOf".Translate(other.LabelShort, other) + ": " + this.OpinionOf(other).ToStringWithSign());
			string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(other, this.pawn);
			if (!pawnSituationLabel.NullOrEmpty())
			{
				stringBuilder.AppendLine(pawnSituationLabel);
			}
			stringBuilder.AppendLine("--------------");
			bool flag = false;
			if (this.pawn.Dead)
			{
				stringBuilder.AppendLine("IAmDead".Translate());
				flag = true;
			}
			else
			{
				foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
				{
					stringBuilder.AppendLine(pawnRelationDef.GetGenderSpecificLabelCap(other) + ": " + pawnRelationDef.opinionOffset.ToStringWithSign());
					flag = true;
				}
				if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
				{
					ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
					thoughts.GetDistinctSocialThoughtGroups(other, Pawn_RelationsTracker.tmpSocialThoughts);
					for (int i = 0; i < Pawn_RelationsTracker.tmpSocialThoughts.Count; i++)
					{
						ISocialThought socialThought = Pawn_RelationsTracker.tmpSocialThoughts[i];
						int num = 1;
						Thought thought = (Thought)socialThought;
						if (thought.def.IsMemory)
						{
							num = thoughts.memories.NumMemoriesInGroup((Thought_MemorySocial)socialThought);
						}
						stringBuilder.Append(thought.LabelCapSocial);
						if (num != 1)
						{
							stringBuilder.Append(" x" + num);
						}
						stringBuilder.AppendLine(": " + thoughts.OpinionOffsetOfGroup(socialThought, other).ToStringWithSign());
						flag = true;
					}
				}
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int j = 0; j < hediffs.Count; j++)
				{
					HediffStage curStage = hediffs[j].CurStage;
					if (curStage != null && curStage.opinionOfOthersFactor != 1f)
					{
						stringBuilder.Append(hediffs[j].LabelBaseCap);
						if (curStage.opinionOfOthersFactor != 0f)
						{
							stringBuilder.AppendLine(": x" + curStage.opinionOfOthersFactor.ToStringPercent());
						}
						else
						{
							stringBuilder.AppendLine();
						}
						flag = true;
					}
				}
				if (this.pawn.HostileTo(other))
				{
					stringBuilder.AppendLine("Hostile".Translate());
					flag = true;
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLine("NoneBrackets".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		
		public float SecondaryLovinChanceFactor(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			if (this.pawn.story != null && this.pawn.story.traits != null)
			{
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					return 0f;
				}
				if (!this.pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
				{
					if (this.pawn.story.traits.HasTrait(TraitDefOf.Gay))
					{
						if (otherPawn.gender != this.pawn.gender)
						{
							return 0f;
						}
					}
					else if (otherPawn.gender == this.pawn.gender)
					{
						return 0f;
					}
				}
			}
			float ageBiologicalYearsFloat = this.pawn.ageTracker.AgeBiologicalYearsFloat;
			float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
			if (ageBiologicalYearsFloat < 16f || ageBiologicalYearsFloat2 < 16f)
			{
				return 0f;
			}
			float num = 1f;
			if (this.pawn.gender == Gender.Male)
			{
				float min = ageBiologicalYearsFloat - 30f;
				float lower = ageBiologicalYearsFloat - 10f;
				float upper = ageBiologicalYearsFloat + 3f;
				float max = ageBiologicalYearsFloat + 10f;
				num = GenMath.FlatHill(0.2f, min, lower, upper, max, 0.2f, ageBiologicalYearsFloat2);
			}
			else if (this.pawn.gender == Gender.Female)
			{
				float min2 = ageBiologicalYearsFloat - 10f;
				float lower2 = ageBiologicalYearsFloat - 3f;
				float upper2 = ageBiologicalYearsFloat + 10f;
				float max2 = ageBiologicalYearsFloat + 30f;
				num = GenMath.FlatHill(0.2f, min2, lower2, upper2, max2, 0.2f, ageBiologicalYearsFloat2);
			}
			float num2 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat);
			float num3 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat2);
			float num4 = 0f;
			if (otherPawn.RaceProps.Humanlike)
			{
				num4 = otherPawn.GetStatValue(StatDefOf.PawnBeauty, true);
			}
			float num5 = 1f;
			if (num4 < 0f)
			{
				num5 = 0.3f;
			}
			else if (num4 > 0f)
			{
				num5 = 2.3f;
			}
			return num * num2 * num3 * num5;
		}

		
		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(otherPawn))
			{
				num *= pawnRelationDef.romanceChanceFactor;
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num;
		}

		
		public float CompatibilityWith(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			float x = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
			float num = Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x), -0.45f, 0.45f);
			float num2 = this.ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
			return num + num2;
		}

		
		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		
		public void ClearAllRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveDirectRelation(list[i]);
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		
		internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
			if (this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn) && !this.pawn.RaceProps.Animal)
			{
				this.AffectBondedAnimalsOnMyDeath();
			}
			this.Notify_FailedRescueQuest();
		}

		
		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		
		public void Notify_ExitedMap()
		{
			this.CheckRescued();
		}

		
		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.CheckRescued();
			}
		}

		
		public void Notify_PawnSold(Pawn playerNegotiator)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(this.pawn);
					if (mostImportantRelation != null && mostImportantRelation.soldThoughts != null)
					{
						if (mostImportantRelation == PawnRelationDefOf.Bond)
						{
							this.pawn.relations.RemoveDirectRelation(mostImportantRelation, pawn);
						}
						foreach (ThoughtDef def in mostImportantRelation.soldThoughts)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(def, playerNegotiator);
						}
					}
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		
		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		
		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.pawn.needs.mood != null && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer);
				this.canGetRescuedThought = false;
			}
		}

		
		public void Notify_FailedRescueQuest()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		
		private void CheckRescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		
		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		
		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

		
		private void RemoveMySpouseMarriageRelatedThoughts()
		{
			Pawn spouse = this.pawn.GetSpouse();
			if (spouse != null && !spouse.Dead && spouse.needs.mood != null)
			{
				MemoryThoughtHandler memories = spouse.needs.mood.thoughts.memories;
				memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
			}
		}

		
		public void CheckAppendBondedAnimalDiedInfo(ref TaggedString letter, ref TaggedString label)
		{
			if (!this.pawn.RaceProps.Animal || !this.everSeenByPlayer || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				return;
			}
			Predicate<Pawn> isAffected = (Pawn x) => !x.Dead && (!x.RaceProps.Humanlike || !x.story.traits.HasTrait(TraitDefOf.Psychopath));
			int num = 0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[i].otherPawn))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			TaggedString t;
			if (num == 1)
			{
				Pawn firstDirectRelationPawn = this.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
				t = "LetterPartBondedAnimalDied".Translate(this.pawn.LabelDefinite(), firstDirectRelationPawn.LabelShort, this.pawn.Named("ANIMAL"), firstDirectRelationPawn.Named("HUMAN")).CapitalizeFirst();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					if (this.directRelations[j].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[j].otherPawn))
					{
						stringBuilder.AppendLine("  - " + this.directRelations[j].otherPawn.LabelShort);
					}
				}
				t = "LetterPartBondedAnimalDiedMulti".Translate(stringBuilder.ToString().TrimEndNewlines());
			}
			label += " (" + "LetterLabelSuffixBondedAnimalDied".Translate() + ")";
			if (!letter.NullOrEmpty())
			{
				letter += "\n\n";
			}
			letter += t;
		}

		
		private void AffectBondedAnimalsOnMyDeath()
		{
			int num = 0;
			Pawn pawn = null;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && this.directRelations[i].otherPawn.Spawned)
				{
					pawn = this.directRelations[i].otherPawn;
					num++;
					float value = Rand.Value;
					MentalStateDef stateDef;
					if (value < 0.25f)
					{
						stateDef = MentalStateDefOf.Wander_Sad;
					}
					else if (value < 0.5f)
					{
						stateDef = MentalStateDefOf.Wander_Psychotic;
					}
					else if (value < 0.75f)
					{
						stateDef = MentalStateDefOf.Berserk;
					}
					else
					{
						stateDef = MentalStateDefOf.Manhunter;
					}
					this.directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, "MentalStateReason_BondedHumanDeath".Translate(this.pawn), true, false, null, false);
				}
			}
			if (num == 1)
			{
				string str;
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					str = "MessageNamedBondedAnimalMentalBreak".Translate(pawn.KindLabelIndefinite(), pawn.Name.ToStringShort, this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				else
				{
					str = "MessageBondedAnimalMentalBreak".Translate(pawn.LabelIndefinite(), this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				Messages.Message(str.CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall, true);
				return;
			}
			if (num > 1)
			{
				Messages.Message("MessageBondedAnimalsMentalBreak".Translate(num, this.pawn.LabelShort, this.pawn.Named("HUMAN")), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
		}

		
		private void Tick_CheckStartMarriageCeremony()
		{
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(1017))
			{
				int ticksGame = Find.TickManager.TicksGame;
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					float num = (float)(ticksGame - this.directRelations[i].startTicks) / 60000f;
					if (this.directRelations[i].def == PawnRelationDefOf.Fiance && this.pawn.thingIDNumber < this.directRelations[i].otherPawn.thingIDNumber && num > 10f && Rand.MTBEventOccurs(2f, 60000f, 1017f) && this.pawn.Map == this.directRelations[i].otherPawn.Map && this.pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(this.pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.pawn, this.directRelations[i].otherPawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.directRelations[i].otherPawn, this.pawn))
					{
						this.pawn.Map.lordsStarter.TryStartMarriageCeremony(this.pawn, this.directRelations[i].otherPawn);
					}
				}
			}
		}

		
		private void Tick_CheckDevelopBondRelation()
		{
			if (!this.pawn.Spawned || !this.pawn.RaceProps.Animal || this.pawn.Faction != Faction.OfPlayer || this.pawn.playerSettings.RespectedMaster == null)
			{
				return;
			}
			Pawn respectedMaster = this.pawn.playerSettings.RespectedMaster;
			if (this.pawn.IsHashIntervalTick(2500) && this.pawn.Map == respectedMaster.Map && this.pawn.Position.InHorDistOf(respectedMaster.Position, 12f) && GenSight.LineOfSight(this.pawn.Position, respectedMaster.Position, this.pawn.Map, false, null, 0, 0))
			{
				RelationsUtility.TryDevelopBondRelation(respectedMaster, this.pawn, 0.001f);
			}
		}

		
		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		
		private Pawn pawn;

		
		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		
		public bool everSeenByPlayer;

		
		public bool canGetRescuedThought = true;

		
		public Pawn relativeInvolvedInRescueQuest;

		
		public MarriageNameChange nextMarriageNameChange;

		
		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		
		private HashSet<Pawn> cachedFamilyByBlood = new HashSet<Pawn>();

		
		private bool familyByBloodIsCached;

		
		private bool canCacheFamilyByBlood;

		
		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		
		private const float MaxBondRelationCheckDist = 12f;

		
		private const float BondRelationPerIntervalChance = 0.001f;

		
		public const int FriendOpinionThreshold = 20;

		
		public const int RivalOpinionThreshold = -20;

		
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();
	}
}
