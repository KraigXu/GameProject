using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	
	public class MentalBreakDef : Def
	{
		
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001BD36 File Offset: 0x00019F36
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			IEnumerator<string> enumerator = null;
			if (this.intensity == MentalBreakIntensity.None)
			{
				yield return "intensity not set";
			}
			yield break;
			yield break;
		}

		
		public Type workerClass = typeof(MentalBreakWorker);

		
		public MentalStateDef mentalState;

		
		public float baseCommonality;

		
		public SimpleCurve commonalityFactorPerPopulationCurve;

		
		public MentalBreakIntensity intensity;

		
		public TraitDef requiredTrait;

		
		private MentalBreakWorker workerInt;
	}
}
