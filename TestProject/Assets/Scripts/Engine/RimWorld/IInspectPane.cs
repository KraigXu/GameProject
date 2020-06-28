using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9D RID: 3741
	public interface IInspectPane
	{
		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06005B3B RID: 23355
		// (set) Token: 0x06005B3C RID: 23356
		float RecentHeight { get; set; }

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06005B3D RID: 23357
		// (set) Token: 0x06005B3E RID: 23358
		Type OpenTabType { get; set; }

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06005B3F RID: 23359
		bool AnythingSelected { get; }

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06005B40 RID: 23360
		IEnumerable<InspectTabBase> CurTabs { get; }

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06005B41 RID: 23361
		bool ShouldShowSelectNextInCellButton { get; }

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06005B42 RID: 23362
		bool ShouldShowPaneContents { get; }

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06005B43 RID: 23363
		float PaneTopY { get; }

		// Token: 0x06005B44 RID: 23364
		void DrawInspectGizmos();

		// Token: 0x06005B45 RID: 23365
		string GetLabel(Rect rect);

		// Token: 0x06005B46 RID: 23366
		void DoInspectPaneButtons(Rect rect, ref float lineEndWidth);

		// Token: 0x06005B47 RID: 23367
		void SelectNextInCell();

		// Token: 0x06005B48 RID: 23368
		void DoPaneContents(Rect rect);

		// Token: 0x06005B49 RID: 23369
		void CloseOpenTab();

		// Token: 0x06005B4A RID: 23370
		void Reset();
	}
}
