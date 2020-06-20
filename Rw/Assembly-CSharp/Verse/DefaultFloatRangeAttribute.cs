using System;

namespace Verse
{
	// Token: 0x0200036D RID: 877
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06001A3F RID: 6719 RVA: 0x000A18E0 File Offset: 0x0009FAE0
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
