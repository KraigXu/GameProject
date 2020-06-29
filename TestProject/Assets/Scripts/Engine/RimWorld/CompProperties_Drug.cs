using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Drug : CompProperties
	{
		
		
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		
		
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
			}
			
			IEnumerator<string> enumerator = null;
			if (this.Addictive && this.chemical == null)
			{
				yield return "addictive but chemical is null";
			}
			yield break;
			yield break;
		}

		
		public ChemicalDef chemical;

		
		public float addictiveness;

		
		public float minToleranceToAddict;

		
		public float existingAddictionSeverityOffset = 0.1f;

		
		public float needLevelOffset = 1f;

		
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		
		public float largeOverdoseChance;

		
		public bool isCombatEnhancingDrug;

		
		public float listOrder;
	}
}
