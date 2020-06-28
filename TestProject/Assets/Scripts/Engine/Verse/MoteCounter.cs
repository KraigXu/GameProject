using System;

namespace Verse
{
	// Token: 0x02000048 RID: 72
	public class MoteCounter
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00012D20 File Offset: 0x00010F20
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00012D28 File Offset: 0x00010F28
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00012D37 File Offset: 0x00010F37
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00012D46 File Offset: 0x00010F46
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00012D55 File Offset: 0x00010F55
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00012D65 File Offset: 0x00010F65
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x040000FE RID: 254
		private int moteCount;

		// Token: 0x040000FF RID: 255
		private const int SaturatedCount = 250;
	}
}
