using System;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public abstract class ITab : InspectTabBase
	{
		
		
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		
		
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		
		
		protected Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		
		
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		
		
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
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}
	}
}
