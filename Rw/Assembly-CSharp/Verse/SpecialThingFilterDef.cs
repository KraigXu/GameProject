using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200047C RID: 1148
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x1700069D RID: 1693
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

		// Token: 0x060021E0 RID: 8672 RVA: 0x000CE6C2 File Offset: 0x000CC8C2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
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

		// Token: 0x060021E1 RID: 8673 RVA: 0x000CE6D2 File Offset: 0x000CC8D2
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x040014D3 RID: 5331
		public ThingCategoryDef parentCategory;

		// Token: 0x040014D4 RID: 5332
		public string saveKey;

		// Token: 0x040014D5 RID: 5333
		public bool allowedByDefault;

		// Token: 0x040014D6 RID: 5334
		public bool configurable = true;

		// Token: 0x040014D7 RID: 5335
		public Type workerClass;

		// Token: 0x040014D8 RID: 5336
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
