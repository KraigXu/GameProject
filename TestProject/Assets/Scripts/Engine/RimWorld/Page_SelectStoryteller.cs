using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8C RID: 3724
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06005ACE RID: 23246 RVA: 0x001EF0BE File Offset: 0x001ED2BE
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x001EF0D0 File Offset: 0x001ED2D0
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

		// Token: 0x06005AD0 RID: 23248 RVA: 0x001EF144 File Offset: 0x001ED344
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

		// Token: 0x06005AD1 RID: 23249 RVA: 0x001EF215 File Offset: 0x001ED415
		private void OpenDifficultyUnlockConfirmation()
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnlockExtremeDifficulty".Translate(), delegate
			{
				Prefs.ExtremeDifficultyUnlocked = true;
				Prefs.Save();
			}, true, null));
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x001EF254 File Offset: 0x001ED454
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

		// Token: 0x04003181 RID: 12673
		private StorytellerDef storyteller;

		// Token: 0x04003182 RID: 12674
		private DifficultyDef difficulty;

		// Token: 0x04003183 RID: 12675
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
