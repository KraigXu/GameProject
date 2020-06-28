using System;

namespace Verse
{
	// Token: 0x0200017A RID: 378
	public struct ThingRequest
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0003955A File Offset: 0x0003775A
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0003956F File Offset: 0x0003776F
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0003959C File Offset: 0x0003779C
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000395C4 File Offset: 0x000377C4
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000395EC File Offset: 0x000377EC
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00039612 File Offset: 0x00037812
		public bool Accepts(Thing t)
		{
			if (this.singleDef != null)
			{
				return t.def == this.singleDef;
			}
			return this.group == ThingRequestGroup.Everything || this.group.Includes(t.def);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00039648 File Offset: 0x00037848
		public override string ToString()
		{
			string str;
			if (this.singleDef != null)
			{
				str = "singleDef " + this.singleDef.defName;
			}
			else
			{
				str = "group " + this.group.ToString();
			}
			return "ThingRequest(" + str + ")";
		}

		// Token: 0x0400085F RID: 2143
		public ThingDef singleDef;

		// Token: 0x04000860 RID: 2144
		public ThingRequestGroup group;
	}
}
