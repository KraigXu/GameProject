using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C6 RID: 966
	public static class Mouse
	{
		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x000AD9E4 File Offset: 0x000ABBE4
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000ADA27 File Offset: 0x000ABC27
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
