using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000505 RID: 1285
	public class AudioSourcePool
	{
		// Token: 0x060024E6 RID: 9446 RVA: 0x000DAE2B File Offset: 0x000D902B
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000DAE49 File Offset: 0x000D9049
		public AudioSource GetSource(bool onCamera)
		{
			if (onCamera)
			{
				return this.sourcePoolCamera.GetSourceCamera();
			}
			return this.sourcePoolWorld.GetSourceWorld();
		}

		// Token: 0x04001660 RID: 5728
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x04001661 RID: 5729
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
