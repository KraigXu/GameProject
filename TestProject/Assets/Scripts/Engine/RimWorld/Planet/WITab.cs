using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class WITab : InspectTabBase
	{
		
		// (get) Token: 0x06006FD1 RID: 28625 RVA: 0x0026F57B File Offset: 0x0026D77B
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		
		// (get) Token: 0x06006FD2 RID: 28626 RVA: 0x00250E5E File Offset: 0x0024F05E
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		
		// (get) Token: 0x06006FD3 RID: 28627 RVA: 0x0026F587 File Offset: 0x0026D787
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		
		// (get) Token: 0x06006FD4 RID: 28628 RVA: 0x0026F599 File Offset: 0x0026D799
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		
		// (get) Token: 0x06006FD5 RID: 28629 RVA: 0x0026F5A6 File Offset: 0x0026D7A6
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		
		// (get) Token: 0x06006FD6 RID: 28630 RVA: 0x0026F5B7 File Offset: 0x0026D7B7
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs != null && this.InspectPane.CurTabs.Contains(this);
			}
		}

		
		// (get) Token: 0x06006FD7 RID: 28631 RVA: 0x0026F5F0 File Offset: 0x0026D7F0
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
