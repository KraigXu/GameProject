using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020004A4 RID: 1188
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000D438C File Offset: 0x000D258C
		public int SourceModuleCount
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules.Length;
				}
				return 0;
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000D43A0 File Offset: 0x000D25A0
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x170006EE RID: 1774
		public virtual ModuleBase this[int index]
		{
			get
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (this.modules[index] == null)
				{
					throw new ArgumentNullException("Desired element is null");
				}
				return this.modules[index];
			}
			set
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (value == null)
				{
					throw new ArgumentNullException("Value should not be null");
				}
				this.modules[index] = value;
			}
		}

		// Token: 0x06002318 RID: 8984
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06002319 RID: 8985 RVA: 0x000D4424 File Offset: 0x000D2624
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000D4444 File Offset: 0x000D2644
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000D4462 File Offset: 0x000D2662
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000D4480 File Offset: 0x000D2680
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000D4488 File Offset: 0x000D2688
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000D44A4 File Offset: 0x000D26A4
		protected virtual bool Disposing()
		{
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					this.modules[i].Dispose();
					this.modules[i] = null;
				}
				this.modules = null;
			}
			return true;
		}

		// Token: 0x04001558 RID: 5464
		protected ModuleBase[] modules;

		// Token: 0x04001559 RID: 5465
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
