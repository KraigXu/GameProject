using System;

namespace Verse
{
	// Token: 0x0200037A RID: 890
	public class TableDataGetter<T>
	{
		// Token: 0x06001A64 RID: 6756 RVA: 0x000A286C File Offset: 0x000A0A6C
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000A2884 File Offset: 0x000A0A84
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("0.#"));
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x000A28C0 File Offset: 0x000A0AC0
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x000A28FC File Offset: 0x000A0AFC
		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				if (thingDef == null)
				{
					return "";
				}
				return thingDef.defName;
			};
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000A2938 File Offset: 0x000A0B38
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04000F7A RID: 3962
		public string label;

		// Token: 0x04000F7B RID: 3963
		public Func<T, string> getter;
	}
}
