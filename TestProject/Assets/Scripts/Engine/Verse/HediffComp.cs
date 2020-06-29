using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp
	{
		
		
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		
		
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		
		
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		
		
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		
		
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		
		
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		
		public virtual void CompPostMake()
		{
		}

		
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		
		public virtual void CompExposeData()
		{
		}

		
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		
		public virtual void CompPostPostRemoved()
		{
		}

		
		public virtual void CompPostMerged(Hediff other)
		{
		}

		
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		
		public virtual void Notify_PawnDied()
		{
		}

		
		public virtual void Notify_PawnKilled()
		{
		}

		
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		
		public virtual string CompDebugString()
		{
			return null;
		}

		
		public HediffWithComps parent;

		
		public HediffCompProperties props;
	}
}
