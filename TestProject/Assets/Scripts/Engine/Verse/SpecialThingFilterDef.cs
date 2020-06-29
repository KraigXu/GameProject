using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class SpecialThingFilterDef : Def
	{
		
		
		public SpecialThingFilterWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "SpecialThingFilterDef " + this.defName + " has no worker class.";
			}
			yield break;
			yield break;
		}

		
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		
		public ThingCategoryDef parentCategory;

		
		public string saveKey;

		
		public bool allowedByDefault;

		
		public bool configurable = true;

		
		public Type workerClass;

		
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
