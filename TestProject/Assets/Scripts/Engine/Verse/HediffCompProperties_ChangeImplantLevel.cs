using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implant == null)
			{
				yield return "implant is null";
			}
			else if (!typeof(Hediff_ImplantWithLevel).IsAssignableFrom(this.implant.hediffClass))
			{
				yield return "implant is not Hediff_ImplantWithLevel";
			}
			if (this.levelOffset == 0)
			{
				yield return "levelOffset is 0";
			}
			if (this.probabilityPerStage == null)
			{
				yield return "probabilityPerStage is not defined";
			}
			else if (this.probabilityPerStage.Count != parentDef.stages.Count)
			{
				yield return "probabilityPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		
		public HediffDef implant;

		
		public int levelOffset;

		
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
