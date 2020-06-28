using System;

namespace Verse
{
	// Token: 0x0200036C RID: 876
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x06001A3D RID: 6717 RVA: 0x000A18AC File Offset: 0x0009FAAC
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x000A18BB File Offset: 0x0009FABB
		public virtual bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return this.value == null;
			}
			return this.value != null && this.value.Equals(obj);
		}

		// Token: 0x04000F51 RID: 3921
		public object value;
	}
}
