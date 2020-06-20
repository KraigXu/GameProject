using System;

namespace Verse
{
	// Token: 0x0200036A RID: 874
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x06001A3B RID: 6715 RVA: 0x000A187C File Offset: 0x0009FA7C
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x04000F4E RID: 3918
		public float min;

		// Token: 0x04000F4F RID: 3919
		public float max = 1f;
	}
}
