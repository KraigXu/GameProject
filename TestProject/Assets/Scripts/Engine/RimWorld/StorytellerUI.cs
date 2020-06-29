using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public static class StorytellerUI
	{
		
		public static void DrawStorytellerSelectionInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, Listing_Standard infoListing)
		{
			GUI.BeginGroup(rect);
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				GUI.DrawTexture(new Rect(390f, rect.height - Storyteller.PortraitSizeLarge.y - 1f, Storyteller.PortraitSizeLarge.x, Storyteller.PortraitSizeLarge.y), chosenStoryteller.portraitLargeTex);
				Widgets.DrawLineHorizontal(0f, rect.height, rect.width);
			}
			Rect outRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x + 16f, rect.height);
			Rect viewRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, (float)DefDatabase<StorytellerDef>.AllDefs.Count<StorytellerDef>() * (Storyteller.PortraitSizeTiny.y + 10f));
			Widgets.BeginScrollView(outRect, ref StorytellerUI.scrollPosition, viewRect, true);
			Rect rect2 = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, Storyteller.PortraitSizeTiny.y);
			foreach (StorytellerDef storytellerDef in from tel in DefDatabase<StorytellerDef>.AllDefs
			orderby tel.listOrder
			select tel)
			{
				if (storytellerDef.listVisible)
				{
					if (Widgets.ButtonImage(rect2, storytellerDef.portraitTinyTex, true))
					{
						TutorSystem.Notify_Event("ChooseStoryteller");
						chosenStoryteller = storytellerDef;
					}
					if (chosenStoryteller == storytellerDef)
					{
						GUI.DrawTexture(rect2, StorytellerUI.StorytellerHighlightTex);
					}
					rect2.y += rect2.height + 8f;
				}
			}
			Widgets.EndScrollView();
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(outRect.xMax + 8f, 0f, 300f, 999f), "HowStorytellersWork".Translate());
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect rect3 = new Rect(outRect.xMax + 8f, outRect.yMin + 160f, 290f, 0f);
				rect3.height = rect.height - rect3.y;
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(rect3.x + 15f, rect3.y - 40f, 9999f, 40f), chosenStoryteller.label);
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				infoListing.Begin(rect3);
				infoListing.Label(chosenStoryteller.description, 160f, null);
				infoListing.Gap(6f);
				foreach (DifficultyDef difficultyDef in DefDatabase<DifficultyDef>.AllDefs)
				{
					if (!difficultyDef.isExtreme || Prefs.ExtremeDifficultyUnlocked)
					{
						GUI.color = difficultyDef.drawColor;
						if (infoListing.RadioButton_NewTemp(difficultyDef.LabelCap, difficulty == difficultyDef, 0f, difficultyDef.description, new float?(0f)))
						{
							difficulty = difficultyDef;
						}
						infoListing.Gap(3f);
					}
				}
				GUI.color = Color.white;
				if (Current.ProgramState == ProgramState.Entry)
				{
					infoListing.Gap(25f);
					bool active = Find.GameInitData.permadeathChosen && Find.GameInitData.permadeath;
					bool active2 = Find.GameInitData.permadeathChosen && !Find.GameInitData.permadeath;
					if (infoListing.RadioButton("ReloadAnytimeMode".Translate(), active2, 0f, "ReloadAnytimeModeInfo".Translate()))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = false;
					}
					infoListing.Gap(3f);
					if (infoListing.RadioButton("CommitmentMode".TranslateWithBackup("PermadeathMode"), active, 0f, "PermadeathModeInfo".Translate()))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = true;
					}
				}
				infoListing.End();
			}
			GUI.EndGroup();
		}

		
		private static Vector2 scrollPosition = default(Vector2);

		
		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight", true);
	}
}
