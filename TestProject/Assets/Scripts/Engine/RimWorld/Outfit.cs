using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA2 RID: 2978
	public class Outfit : IExposable, ILoadReferenceable
	{
		// Token: 0x060045BD RID: 17853 RVA: 0x00178E3E File Offset: 0x0017703E
		public Outfit()
		{
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x00178E51 File Offset: 0x00177051
		public Outfit(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x00178E72 File Offset: 0x00177072
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x00178EAD File Offset: 0x001770AD
		public string GetUniqueLoadID()
		{
			return "Outfit_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04002822 RID: 10274
		public int uniqueId;

		// Token: 0x04002823 RID: 10275
		public string label;

		// Token: 0x04002824 RID: 10276
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04002825 RID: 10277
		public static readonly Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}
