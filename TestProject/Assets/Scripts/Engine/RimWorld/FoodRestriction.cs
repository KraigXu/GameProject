using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7C RID: 2940
	public class FoodRestriction : IExposable, ILoadReferenceable
	{
		// Token: 0x060044DD RID: 17629 RVA: 0x00173FE7 File Offset: 0x001721E7
		public FoodRestriction(int id, string label)
		{
			this.id = id;
			this.label = label;
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x00174008 File Offset: 0x00172208
		public FoodRestriction()
		{
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x0017401B File Offset: 0x0017221B
		public bool Allows(ThingDef def)
		{
			return this.filter.Allows(def);
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00174029 File Offset: 0x00172229
		public bool Allows(Thing thing)
		{
			return this.filter.Allows(thing);
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00174037 File Offset: 0x00172237
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00174072 File Offset: 0x00172272
		public string GetUniqueLoadID()
		{
			return "FoodRestriction_" + this.label + this.id;
		}

		// Token: 0x04002754 RID: 10068
		public int id;

		// Token: 0x04002755 RID: 10069
		public string label;

		// Token: 0x04002756 RID: 10070
		public ThingFilter filter = new ThingFilter();
	}
}
