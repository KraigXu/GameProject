using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000456 RID: 1110
	public class KeyBindingData
	{
		// Token: 0x0600211E RID: 8478 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public KeyBindingData()
		{
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000CB290 File Offset: 0x000C9490
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000CB2A8 File Offset: 0x000C94A8
		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != KeyCode.None)
			{
				str += this.keyBindingA.ToString();
			}
			if (this.keyBindingB != KeyCode.None)
			{
				str = str + ", " + this.keyBindingB.ToString();
			}
			return str + "]";
		}

		// Token: 0x0400142F RID: 5167
		public KeyCode keyBindingA;

		// Token: 0x04001430 RID: 5168
		public KeyCode keyBindingB;
	}
}
