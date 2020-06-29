using System;
using UnityEngine;

namespace Verse
{
	
	public static class Mouse
	{
		
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x000AD9E4 File Offset: 0x000ABBE4
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
