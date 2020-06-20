using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x0200020D RID: 525
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06000EDB RID: 3803 RVA: 0x000547E8 File Offset: 0x000529E8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0005484C File Offset: 0x00052A4C
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0005485C File Offset: 0x00052A5C
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + this.lastFailedOperation;
			}
			return text + ")";
		}

		// Token: 0x04000AFC RID: 2812
		private List<PatchOperation> operations;

		// Token: 0x04000AFD RID: 2813
		private PatchOperation lastFailedOperation;
	}
}
