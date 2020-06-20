using System;

namespace Verse
{
	// Token: 0x02000202 RID: 514
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x06000EC5 RID: 3781 RVA: 0x000540D3 File Offset: 0x000522D3
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x04000AF1 RID: 2801
		protected string xpath;
	}
}
