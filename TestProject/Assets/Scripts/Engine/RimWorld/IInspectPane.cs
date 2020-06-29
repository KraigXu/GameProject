using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public interface IInspectPane
	{
		
		// (get) Token: 0x06005B3B RID: 23355
		// (set) Token: 0x06005B3C RID: 23356
		float RecentHeight { get; set; }

		
		// (get) Token: 0x06005B3D RID: 23357
		// (set) Token: 0x06005B3E RID: 23358
		Type OpenTabType { get; set; }

		
		// (get) Token: 0x06005B3F RID: 23359
		bool AnythingSelected { get; }

		
		// (get) Token: 0x06005B40 RID: 23360
		IEnumerable<InspectTabBase> CurTabs { get; }

		
		// (get) Token: 0x06005B41 RID: 23361
		bool ShouldShowSelectNextInCellButton { get; }

		
		// (get) Token: 0x06005B42 RID: 23362
		bool ShouldShowPaneContents { get; }

		
		// (get) Token: 0x06005B43 RID: 23363
		float PaneTopY { get; }

		
		void DrawInspectGizmos();

		
		string GetLabel(Rect rect);

		
		void DoInspectPaneButtons(Rect rect, ref float lineEndWidth);

		
		void SelectNextInCell();

		
		void DoPaneContents(Rect rect);

		
		void CloseOpenTab();

		
		void Reset();
	}
}
