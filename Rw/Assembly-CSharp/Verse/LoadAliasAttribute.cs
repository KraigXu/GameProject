using System;

namespace Verse
{
	// Token: 0x02000372 RID: 882
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06001A45 RID: 6725 RVA: 0x000A197A File Offset: 0x0009FB7A
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x04000F52 RID: 3922
		public string alias;
	}
}
