using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class TraitSet : IExposable
	{
		
		// (get) Token: 0x060047FA RID: 18426 RVA: 0x00186260 File Offset: 0x00184460
		public float HungerRateFactor
		{
			get
			{
				float num = 1f;
				foreach (Trait trait in this.allTraits)
				{
					num *= trait.CurrentData.hungerRateFactor;
				}
				return num;
			}
		}

		
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, Array.Empty<object>());
		}

		
		public void GainTrait(Trait trait)
		{
			if (this.HasTrait(trait.def))
			{
				Log.Warning(this.pawn + " already has trait " + trait.def, false);
				return;
			}
			this.allTraits.Add(trait);
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		
		public bool HasTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return true;
				}
			}
			return false;
		}

		
		// (get) Token: 0x060047FF RID: 18431 RVA: 0x001863FA File Offset: 0x001845FA
		public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
		{
			get
			{
				int num;
				for (int i = 0; i < this.allTraits.Count; i = num + 1)
				{
					Trait trait = this.allTraits[i];
					if (trait.CurrentData.theOnlyAllowedMentalBreaks != null)
					{
						for (int j = 0; j < trait.CurrentData.theOnlyAllowedMentalBreaks.Count; j = num + 1)
						{
							yield return trait.CurrentData.theOnlyAllowedMentalBreaks[j];
							num = j;
						}
					}
					trait = null;
					num = i;
				}
				yield break;
			}
		}

		
		public Trait GetTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		
		public int DegreeOfTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return this.allTraits[i].Degree;
				}
			}
			return 0;
		}

		
		protected Pawn pawn;

		
		public List<Trait> allTraits = new List<Trait>();
	}
}
