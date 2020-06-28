using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000201 RID: 513
	public class PatchOperation
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x00053FF4 File Offset: 0x000521F4
		public bool Apply(XmlDocument xml)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(base.GetType().FullName + " Worker");
			}
			bool flag = this.ApplyWorker(xml);
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
			if (this.success == PatchOperation.Success.Always)
			{
				flag = true;
			}
			else if (this.success == PatchOperation.Success.Never)
			{
				flag = false;
			}
			else if (this.success == PatchOperation.Success.Invert)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.neverSucceeded = false;
			}
			return flag;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0005406D File Offset: 0x0005226D
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail", false);
			return false;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0005407C File Offset: 0x0005227C
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text, false);
			}
		}

		// Token: 0x04000AEE RID: 2798
		public string sourceFile;

		// Token: 0x04000AEF RID: 2799
		private bool neverSucceeded = true;

		// Token: 0x04000AF0 RID: 2800
		private PatchOperation.Success success;

		// Token: 0x0200141A RID: 5146
		private enum Success
		{
			// Token: 0x04004C68 RID: 19560
			Normal,
			// Token: 0x04004C69 RID: 19561
			Invert,
			// Token: 0x04004C6A RID: 19562
			Always,
			// Token: 0x04004C6B RID: 19563
			Never
		}
	}
}
