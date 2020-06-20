using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC9 RID: 4041
	public class ShortcutKeys
	{
		// Token: 0x06006111 RID: 24849 RVA: 0x0021B0C7 File Offset: 0x002192C7
		public void ShortcutKeysOnGUI()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (KeyBindingDefOf.NextColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectNextColonist();
					Event.current.Use();
				}
				if (KeyBindingDefOf.PreviousColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectPreviousColonist();
					Event.current.Use();
				}
			}
		}
	}
}
