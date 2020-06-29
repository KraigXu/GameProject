using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Drug : CompProperties
	{
		
		// (get) Token: 0x0600350A RID: 13578 RVA: 0x001227CC File Offset: 0x001209CC
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x001227DB File Offset: 0x001209DB
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
			foreach (string text in this.n__0(parentDef))
			{
				yield return text;
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
