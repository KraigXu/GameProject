using System;

namespace Verse
{
	// Token: 0x0200032A RID: 810
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x060017B5 RID: 6069 RVA: 0x000877BC File Offset: 0x000859BC
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04000EE2 RID: 3810
		public string category;

		// Token: 0x04000EE3 RID: 3811
		public float min;

		// Token: 0x04000EE4 RID: 3812
		public float max;
	}
}
