using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001291 RID: 4753
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06006FD1 RID: 28625 RVA: 0x0026F57B File Offset: 0x0026D77B
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06006FD2 RID: 28626 RVA: 0x00250E5E File Offset: 0x0024F05E
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06006FD3 RID: 28627 RVA: 0x0026F587 File Offset: 0x0026D787
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06006FD4 RID: 28628 RVA: 0x0026F599 File Offset: 0x0026D799
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06006FD5 RID: 28629 RVA: 0x0026F5A6 File Offset: 0x0026D7A6
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06006FD6 RID: 28630 RVA: 0x0026F5B7 File Offset: 0x0026D7B7
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs != null && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06006FD7 RID: 28631 RVA: 0x0026F5F0 File Offset: 0x0026D7F0
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x0026F5FD File Offset: 0x0026D7FD
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
