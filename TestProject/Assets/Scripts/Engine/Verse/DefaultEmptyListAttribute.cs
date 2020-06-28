using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200036E RID: 878
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06001A40 RID: 6720 RVA: 0x000A18F4 File Offset: 0x0009FAF4
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000A1900 File Offset: 0x0009FB00
		public override bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			Type[] genericArguments = obj.GetType().GetGenericArguments();
			return genericArguments.Length == 1 && !(genericArguments[0] != (Type)this.value) && (int)obj.GetType().GetProperty("Count").GetValue(obj, null) == 0;
		}
	}
}
