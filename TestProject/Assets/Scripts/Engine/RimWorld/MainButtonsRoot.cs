using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBB RID: 3771
	public class MainButtonsRoot
	{
		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06005C2F RID: 23599 RVA: 0x001FD60C File Offset: 0x001FB80C
		private int VisibleButtonsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.allButtonsInOrder.Count; i++)
				{
					if (this.allButtonsInOrder[i].buttonVisible)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x001FD64C File Offset: 0x001FB84C
		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x001FD6A0 File Offset: 0x001FB8A0
		public void MainButtonsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.DoButtons();
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (!this.allButtonsInOrder[i].Worker.Disabled && this.allButtonsInOrder[i].hotKey != null && this.allButtonsInOrder[i].hotKey.KeyDownEvent)
				{
					Event.current.Use();
					this.allButtonsInOrder[i].Worker.InterfaceTryActivate();
					return;
				}
			}
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x001FD73C File Offset: 0x001FB93C
		public void HandleLowPriorityShortcuts()
		{
			this.tabs.HandleLowPriorityShortcuts();
			if (WorldRendererUtility.WorldRenderedNow && Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null && KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x001FD790 File Offset: 0x001FB990
		private void DoButtons()
		{
			float num = 0f;
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (this.allButtonsInOrder[i].buttonVisible)
				{
					num += (this.allButtonsInOrder[i].minimized ? 0.5f : 1f);
				}
			}
			GUI.color = Color.white;
			int num2 = (int)((float)UI.screenWidth / num);
			int num3 = num2 / 2;
			int num4 = this.allButtonsInOrder.FindLastIndex((MainButtonDef x) => x.buttonVisible);
			int num5 = 0;
			for (int j = 0; j < this.allButtonsInOrder.Count; j++)
			{
				if (this.allButtonsInOrder[j].buttonVisible)
				{
					int num6 = this.allButtonsInOrder[j].minimized ? num3 : num2;
					if (j == num4)
					{
						num6 = UI.screenWidth - num5;
					}
					Rect rect = new Rect((float)num5, (float)(UI.screenHeight - 35), (float)num6, 35f);
					this.allButtonsInOrder[j].Worker.DoButton(rect);
					num5 += num6;
				}
			}
		}

		// Token: 0x04003248 RID: 12872
		public MainTabsRoot tabs = new MainTabsRoot();

		// Token: 0x04003249 RID: 12873
		private List<MainButtonDef> allButtonsInOrder;
	}
}
