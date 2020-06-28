using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD7 RID: 3031
	public class TraitSet : IExposable
	{
		// Token: 0x17000CD6 RID: 3286
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

		// Token: 0x060047FB RID: 18427 RVA: 0x001862C4 File Offset: 0x001844C4
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x001862DE File Offset: 0x001844DE
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x001862F8 File Offset: 0x001844F8
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

		// Token: 0x060047FE RID: 18430 RVA: 0x001863C0 File Offset: 0x001845C0
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

		// Token: 0x17000CD7 RID: 3287
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

		// Token: 0x06004800 RID: 18432 RVA: 0x0018640C File Offset: 0x0018460C
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

		// Token: 0x06004801 RID: 18433 RVA: 0x00186454 File Offset: 0x00184654
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

		// Token: 0x04002941 RID: 10561
		protected Pawn pawn;

		// Token: 0x04002942 RID: 10562
		public List<Trait> allTraits = new List<Trait>();
	}
}
