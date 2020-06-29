using System;
using RimWorld;

namespace Verse
{
	
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		
		public ChemicalDef chemical;
	}
}
