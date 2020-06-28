using System;

namespace Verse
{
	// Token: 0x02000375 RID: 885
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x06001A48 RID: 6728 RVA: 0x000A1998 File Offset: 0x0009FB98
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x04000F54 RID: 3924
		public string elementToIgnore;
	}
}
