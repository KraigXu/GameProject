using System;
using UnityEngine;

namespace Verse
{
	
	public struct GizmoResult
	{
		
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x000A5895 File Offset: 0x000A3A95
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x000A589D File Offset: 0x000A3A9D
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		
		private GizmoState stateInt;

		
		private Event interactEventInt;
	}
}
