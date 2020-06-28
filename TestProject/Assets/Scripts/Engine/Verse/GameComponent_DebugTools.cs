using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200010F RID: 271
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x000237A4 File Offset: 0x000219A4
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000237B7 File Offset: 0x000219B7
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000237E6 File Offset: 0x000219E6
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x040006E7 RID: 1767
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}
