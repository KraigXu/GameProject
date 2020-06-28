using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8D RID: 3725
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x001EF0BE File Offset: 0x001ED2BE
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x001EF341 File Offset: 0x001ED541
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x06005AD7 RID: 23255 RVA: 0x001EF364 File Offset: 0x001ED564
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

		// Token: 0x04003184 RID: 12676
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
