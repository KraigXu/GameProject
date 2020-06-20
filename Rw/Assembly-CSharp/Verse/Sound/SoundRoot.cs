using System;

namespace Verse.Sound
{
	// Token: 0x020004FF RID: 1279
	public class SoundRoot
	{
		// Token: 0x060024D8 RID: 9432 RVA: 0x000DA9E0 File Offset: 0x000D8BE0
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000DAA09 File Offset: 0x000D8C09
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x04001659 RID: 5721
		public AudioSourcePool sourcePool;

		// Token: 0x0400165A RID: 5722
		public SampleOneShotManager oneShotManager;

		// Token: 0x0400165B RID: 5723
		public SustainerManager sustainerManager;
	}
}
