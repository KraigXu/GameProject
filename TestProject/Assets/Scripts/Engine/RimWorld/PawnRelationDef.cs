using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class PawnRelationDef : Def
	{
		
		
		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		
		public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.lostThoughtFemale;
			}
			return this.lostThought;
		}

		
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.implied && this.reflexive)
			{
				yield return this.defName + ": implied relations can't use the \"reflexive\" option.";
				this.reflexive = false;
			}
			yield break;
			yield break;
		}

		
		public Type workerClass = typeof(PawnRelationWorker);

		
		[MustTranslate]
		public string labelFemale;

		
		public float importance;

		
		public bool implied;

		
		public bool reflexive;

		
		public int opinionOffset;

		
		public float generationChanceFactor;

		
		public float romanceChanceFactor = 1f;

		
		public float incestOpinionOffset;

		
		public bool familyByBloodRelation;

		
		public ThoughtDef diedThought;

		
		public ThoughtDef diedThoughtFemale;

		
		public ThoughtDef lostThought;

		
		public ThoughtDef lostThoughtFemale;

		
		public List<ThoughtDef> soldThoughts;

		
		public ThoughtDef killedThought;

		
		public ThoughtDef killedThoughtFemale;

		
		[Unsaved(false)]
		private PawnRelationWorker workerInt;
	}
}
