using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000393 RID: 915
	public struct GizmoResult
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x000A5895 File Offset: 0x000A3A95
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x000A589D File Offset: 0x000A3A9D
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000A58A5 File Offset: 0x000A3AA5
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000A58B5 File Offset: 0x000A3AB5
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x04000FF6 RID: 4086
		private GizmoState stateInt;

		// Token: 0x04000FF7 RID: 4087
		private Event interactEventInt;
	}
}
