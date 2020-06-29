using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Page_SelectStorytellerInGame : Page
	{
		
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x001EF0BE File Offset: 0x001ED2BE
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref storyteller.def, ref storyteller.difficulty, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
