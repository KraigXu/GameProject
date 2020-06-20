using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000301 RID: 769
	public class RealtimeMoteList
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x0007E33F File Offset: 0x0007C53F
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0007E34C File Offset: 0x0007C54C
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0007E35A File Offset: 0x0007C55A
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0007E36C File Offset: 0x0007C56C
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x04000E29 RID: 3625
		public List<Mote> allMotes = new List<Mote>();
	}
}
