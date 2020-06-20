using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200006E RID: 110
	public class Editable
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00016C6A File Offset: 0x00014E6A
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
