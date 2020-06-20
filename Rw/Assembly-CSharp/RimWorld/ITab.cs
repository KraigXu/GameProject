using System;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EA1 RID: 3745
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x001F7756 File Offset: 0x001F5956
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06005B69 RID: 23401 RVA: 0x001F7762 File Offset: 0x001F5962
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06005B6A RID: 23402 RVA: 0x001F776E File Offset: 0x001F596E
		protected Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06005B6B RID: 23403 RVA: 0x001F777B File Offset: 0x001F597B
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06005B6C RID: 23404 RVA: 0x001F778C File Offset: 0x001F598C
		protected override bool StillValid
		{
			get
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					return false;
				}
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)Find.MainTabsRoot.OpenTab.TabWindow;
				return mainTabWindow_Inspect.CurTabs != null && mainTabWindow_Inspect.CurTabs.Contains(this);
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06005B6D RID: 23405 RVA: 0x001F77D7 File Offset: 0x001F59D7
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x001F77E4 File Offset: 0x001F59E4
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}
	}
}
