using System;

namespace Verse
{
	
	public class MoteCounter
	{
		
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00012D20 File Offset: 0x00010F20
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00012D28 File Offset: 0x00010F28
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00012D37 File Offset: 0x00010F37
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00012D46 File Offset: 0x00010F46
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		
		private int moteCount;

		
		private const int SaturatedCount = 250;
	}
}
