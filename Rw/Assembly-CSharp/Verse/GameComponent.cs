using System;

namespace Verse
{
	// Token: 0x0200010E RID: 270
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void LoadedGame()
		{
		}
	}
}
