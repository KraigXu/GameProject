using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038C RID: 908
	public class Command_Action : Command
	{
		// Token: 0x06001AD8 RID: 6872 RVA: 0x000A4F67 File Offset: 0x000A3167
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x04000FE2 RID: 4066
		public Action action;
	}
}
