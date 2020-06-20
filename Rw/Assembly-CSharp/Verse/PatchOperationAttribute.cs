using System;

namespace Verse
{
	// Token: 0x02000209 RID: 521
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x06000ED3 RID: 3795 RVA: 0x000545DB File Offset: 0x000527DB
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04000AF9 RID: 2809
		protected string attribute;
	}
}
