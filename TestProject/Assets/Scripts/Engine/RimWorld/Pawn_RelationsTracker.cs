using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC6 RID: 3014
	public class Pawn_RelationsTracker : IExposable
	{
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004742 RID: 18242 RVA: 0x00181DF9 File Offset: 0x0017FFF9
		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004743 RID: 18243 RVA: 0x00181E01 File Offset: 0x00180001
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

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004744 RID: 18244 RVA: 0x00181E11 File Offset: 0x00180011
		public int ChildrenCount
		{
			get
			{
				return this.Children.Count<Pawn>();
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004745 RID: 18245 RVA: 0x00181E1E File Offset: 0x0018001E
		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any<DirectPawnRelation>() || this.pawnsWithDirectRelationsWithMe.Any<Pawn>();
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004746 RID: 18246 RVA: 0x00181E3C File Offset: 0x0018003C
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

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004747 RID: 18247 RVA: 0x00181EC0 File Offset: 0x001800C0
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

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004748 RID: 18248 RVA: 0x00181ED0 File Offset: 0x001800D0
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

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004749 RID: 18249 RVA: 0x00181EE0 File Offset: 0x001800E0
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

		// Token: 0x0600474A RID: 18250 RVA: 0x00181EF0 File Offset: 0x001800F0
		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x00181F28 File Offset: 0x00180128
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

		// Token: 0x0600474C RID: 18252 RVA: 0x00182079 File Offset: 0x00180279
		public void RelationsTrackerTick()
		{
			if (this.pawn.Dead)
			{
				return;
			}
			this.Tick_CheckStartMarriageCeremony();
			this.Tick_CheckDevelopBondRelation();
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x00182098 File Offset: 0x00180298
		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				return null;
			}
			return this.directRelations.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == otherPawn);
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x001820F8 File Offset: 0x001802F8
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

		// Token: 0x0600474F RID: 18255 RVA: 0x00182168 File Offset: 0x00180368
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

		// Token: 0x06004750 RID: 18256 RVA: 0x001821C8 File Offset: 0x001803C8
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

		// Token: 0x06004751 RID: 18257 RVA: 0x0018232A File Offset: 0x0018052A
		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x00182340 File Offset: 0x00180540
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

		// Token: 0x06004753 RID: 18259 RVA: 0x00182394 File Offset: 0x00180594
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
			Predicate<DirectPawnRelation> <>9__1;
			Predicate<DirectPawnRelation> <>9__2;
			Predicate<DirectPawnRelation> <>9__0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == def && this.directRelations[i].otherPawn == otherPawn)
				{
					if (def.reflexive)
					{
						List<DirectPawnRelation> list = otherPawn.relations.directRelations;
						Predicate<DirectPawnRelation> match;
						if ((match = <>9__1) == null)
						{
							match = (<>9__1 = ((DirectPawnRelation x) => x.def == def && x.otherPawn == this.pawn));
						}
						DirectPawnRelation item = list.Find(match);
						list.Remove(item);
						Predicate<DirectPawnRelation> match2;
						if ((match2 = <>9__2) == null)
						{
							match2 = (<>9__2 = ((DirectPawnRelation x) => x.otherPawn == this.pawn));
						}
						if (list.Find(match2) == null)
						{
							this.pawnsWithDirectRelationsWithMe.Remove(otherPawn);
						}
					}
					this.directRelations.RemoveAt(i);
					List<DirectPawnRelation> list2 = this.directRelations;
					Predicate<DirectPawnRelation> match3;
					if ((match3 = <>9__0) == null)
					{
						match3 = (<>9__0 = ((DirectPawnRelation x) => x.otherPawn == otherPawn));
					}
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

		// Token: 0x06004754 RID: 18260 RVA: 0x00182554 File Offset: 0x00180754
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

		// Token: 0x06004755 RID: 18261 RVA: 0x00182698 File Offset: 0x00180898
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

		// Token: 0x06004756 RID: 18262 RVA: 0x001829A4 File Offset: 0x00180BA4
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

		// Token: 0x06004757 RID: 18263 RVA: 0x00182BCC File Offset: 0x00180DCC
		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(otherPawn))
			{
				num *= pawnRelationDef.romanceChanceFactor;
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num;
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x00182C30 File Offset: 0x00180E30
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

		// Token: 0x06004759 RID: 18265 RVA: 0x00182CB9 File Offset: 0x00180EB9
		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x00182CF0 File Offset: 0x00180EF0
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

		// Token: 0x0600475B RID: 18267 RVA: 0x00182DA4 File Offset: 0x00180FA4
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

		// Token: 0x0600475C RID: 18268 RVA: 0x00182E54 File Offset: 0x00181054
		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00182E6A File Offset: 0x0018106A
		public void Notify_ExitedMap()
		{
			this.CheckRescued();
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x00182E72 File Offset: 0x00181072
		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.CheckRescued();
			}
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00182E8C File Offset: 0x0018108C
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

		// Token: 0x06004760 RID: 18272 RVA: 0x00182F84 File Offset: 0x00181184
		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x00182F8C File Offset: 0x0018118C
		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.pawn.needs.mood != null && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer);
				this.canGetRescuedThought = false;
			}
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x00182FEC File Offset: 0x001811EC
		public void Notify_FailedRescueQuest()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x001830C0 File Offset: 0x001812C0
		private void CheckRescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x00183193 File Offset: 0x00181393
		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x001831B5 File Offset: 0x001813B5
		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x001831D8 File Offset: 0x001813D8
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

		// Token: 0x06004767 RID: 18279 RVA: 0x00183234 File Offset: 0x00181434
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

		// Token: 0x06004768 RID: 18280 RVA: 0x00183458 File Offset: 0x00181658
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

		// Token: 0x06004769 RID: 18281 RVA: 0x00183670 File Offset: 0x00181870
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

		// Token: 0x0600476A RID: 18282 RVA: 0x0018380C File Offset: 0x00181A0C
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

		// Token: 0x0600476B RID: 18283 RVA: 0x001838EC File Offset: 0x00181AEC
		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x0400290B RID: 10507
		private Pawn pawn;

		// Token: 0x0400290C RID: 10508
		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		// Token: 0x0400290D RID: 10509
		public bool everSeenByPlayer;

		// Token: 0x0400290E RID: 10510
		public bool canGetRescuedThought = true;

		// Token: 0x0400290F RID: 10511
		public Pawn relativeInvolvedInRescueQuest;

		// Token: 0x04002910 RID: 10512
		public MarriageNameChange nextMarriageNameChange;

		// Token: 0x04002911 RID: 10513
		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		// Token: 0x04002912 RID: 10514
		private HashSet<Pawn> cachedFamilyByBlood = new HashSet<Pawn>();

		// Token: 0x04002913 RID: 10515
		private bool familyByBloodIsCached;

		// Token: 0x04002914 RID: 10516
		private bool canCacheFamilyByBlood;

		// Token: 0x04002915 RID: 10517
		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		// Token: 0x04002916 RID: 10518
		private const float MaxBondRelationCheckDist = 12f;

		// Token: 0x04002917 RID: 10519
		private const float BondRelationPerIntervalChance = 0.001f;

		// Token: 0x04002918 RID: 10520
		public const int FriendOpinionThreshold = 20;

		// Token: 0x04002919 RID: 10521
		public const int RivalOpinionThreshold = -20;

		// Token: 0x0400291A RID: 10522
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();
	}
}
