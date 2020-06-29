using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class SpecialThingFilterDef : Def
	{
		
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x000CE69C File Offset: 0x000CC89C
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
			foreach (string text in this.n__0())
			{
				yield return text;
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
