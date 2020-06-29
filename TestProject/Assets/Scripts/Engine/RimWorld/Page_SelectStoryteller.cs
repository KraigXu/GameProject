using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Page_SelectStoryteller : Page
	{
		
		
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		
		public override void PreOpen()
		{
			base.PreOpen();
			if (this.storyteller == null)
			{
				this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
				where d.listVisible
				orderby d.listOrder
				select d).First<StorytellerDef>();
			}
		}

		
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			StorytellerUI.DrawStorytellerSelectionInterface(base.GetMainRect(rect, 0f, false), ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			string midLabel = null;
			Action midAct = null;
			if (!Prefs.ExtremeDifficultyUnlocked)
			{
				midLabel = "UnlockExtremeDifficulty".Translate();
				midAct = delegate
				{
					this.OpenDifficultyUnlockConfirmation();
				};
			}
			base.DoBottomButtons(rect, null, midLabel, midAct, true, true);
			Rect rect2 = new Rect(rect.xMax - Page.BottomButSize.x - 200f - 6f, rect.yMax - Page.BottomButSize.y, 200f, Page.BottomButSize.y);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "CanChangeStorytellerSettingsDuringPlay".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		
		private void OpenDifficultyUnlockConfirmation()
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnlockExtremeDifficulty".Translate(), delegate
			{
				Prefs.ExtremeDifficultyUnlocked = true;
				Prefs.Save();
			}, true, null));
		}

		
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.difficulty == null)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				this.difficulty = DifficultyDefOf.Rough;
			}
			if (!Find.GameInitData.permadeathChosen)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChoosePermadeath".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Reload anytime mode has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				Find.GameInitData.permadeathChosen = true;
				Find.GameInitData.permadeath = false;
			}
			Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty);
			return true;
		}

		
		private StorytellerDef storyteller;

		
		private DifficultyDef difficulty;

		
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
