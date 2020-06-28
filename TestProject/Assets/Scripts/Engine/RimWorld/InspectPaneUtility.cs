using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EA0 RID: 3744
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
		// Token: 0x06005B56 RID: 23382 RVA: 0x001F6D0E File Offset: 0x001F4F0E
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x001F6D1C File Offset: 0x001F4F1C
		public static float PaneWidthFor(IInspectPane pane)
		{
			if (pane == null)
			{
				return 432f;
			}
			InspectPaneUtility.<>c__DisplayClass9_0 <>c__DisplayClass9_;
			<>c__DisplayClass9_.visible = 0;
			if (pane.CurTabs != null)
			{
				IList list = pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0((InspectTabBase)list[i], ref <>c__DisplayClass9_);
					}
				}
				else
				{
					foreach (InspectTabBase tab in pane.CurTabs)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0(tab, ref <>c__DisplayClass9_);
					}
				}
			}
			return 72f * (float)Mathf.Max(6, <>c__DisplayClass9_.visible);
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x001F6DCC File Offset: 0x001F4FCC
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x001F6DE0 File Offset: 0x001F4FE0
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x001F6E24 File Offset: 0x001F5024
		public static string AdjustedLabelFor(List<object> selected, Rect rect)
		{
			Zone zone;
			string text;
			if ((zone = (selected[0] as Zone)) != null)
			{
				if (selected.Count == 1)
				{
					text = zone.label;
				}
				else
				{
					string baseLabel = zone.BaseLabel;
					bool flag = true;
					for (int i = 1; i < selected.Count; i++)
					{
						Zone zone2;
						if ((zone2 = (selected[i] as Zone)) != null && zone2.BaseLabel != baseLabel)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						text = ((!zone.BaseLabel.NullOrEmpty()) ? zone.BaseLabel : "Zone".Translate());
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					text = text + " x" + selected.Count;
				}
			}
			else
			{
				InspectPaneUtility.selectedThings.Clear();
				for (int j = 0; j < selected.Count; j++)
				{
					Thing outerThing;
					if ((outerThing = (selected[j] as Thing)) != null)
					{
						InspectPaneUtility.selectedThings.Add(outerThing.GetInnerIfMinified());
					}
				}
				if (InspectPaneUtility.selectedThings.Count == 1)
				{
					text = InspectPaneUtility.selectedThings[0].LabelCap;
				}
				else if (InspectPaneUtility.selectedThings.Count > 1)
				{
					string label = InspectPaneUtility.selectedThings[0].def.label;
					bool flag2 = true;
					for (int k = 1; k < InspectPaneUtility.selectedThings.Count; k++)
					{
						if (InspectPaneUtility.selectedThings[k].def.label != label)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						text = InspectPaneUtility.selectedThings[0].def.LabelCap;
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					int num = 0;
					for (int l = 0; l < InspectPaneUtility.selectedThings.Count; l++)
					{
						num += InspectPaneUtility.selectedThings[l].stackCount;
					}
					text = text + " x" + num.ToStringCached();
				}
				else
				{
					text = "?";
				}
				InspectPaneUtility.selectedThings.Clear();
			}
			Text.Font = GameFont.Medium;
			return text.Truncate(rect.width, InspectPaneUtility.truncatedLabelsCached);
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x001F7064 File Offset: 0x001F5264
		public static void ExtraOnGUI(IInspectPane pane)
		{
			if (pane.AnythingSelected)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					pane.SelectNextInCell();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					pane.DrawInspectGizmos();
				}
				InspectPaneUtility.DoTabs(pane);
			}
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x001F7094 File Offset: 0x001F5294
		public static void UpdateTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass15_0 <>c__DisplayClass15_;
			<>c__DisplayClass15_.pane = pane;
			<>c__DisplayClass15_.tabUpdated = false;
			if (<>c__DisplayClass15_.pane.CurTabs != null)
			{
				IList list = <>c__DisplayClass15_.pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|15_0((InspectTabBase)list[i], ref <>c__DisplayClass15_);
					}
				}
				else
				{
					foreach (InspectTabBase tab in <>c__DisplayClass15_.pane.CurTabs)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|15_0(tab, ref <>c__DisplayClass15_);
					}
				}
			}
			if (!<>c__DisplayClass15_.tabUpdated)
			{
				<>c__DisplayClass15_.pane.CloseOpenTab();
			}
		}

		// Token: 0x06005B5D RID: 23389 RVA: 0x001F7150 File Offset: 0x001F5350
		public static void InspectPaneOnGUI(Rect inRect, IInspectPane pane)
		{
			pane.RecentHeight = 165f;
			if (pane.AnythingSelected)
			{
				try
				{
					Rect rect = inRect.ContractedBy(12f);
					rect.yMin -= 4f;
					rect.yMax += 6f;
					GUI.BeginGroup(rect);
					float num = 0f;
					if (pane.ShouldShowSelectNextInCellButton)
					{
						Rect rect2 = new Rect(rect.width - 24f, 0f, 24f, 24f);
						MouseoverSounds.DoRegion(rect2);
						if (Widgets.ButtonImage(rect2, TexButton.SelectOverlappingNext, true))
						{
							pane.SelectNextInCell();
						}
						num += 24f;
						TooltipHandler.TipRegionByKey(rect2, "SelectNextInSquareTip", KeyBindingDefOf.SelectNextInCell.MainKeyLabel);
					}
					pane.DoInspectPaneButtons(rect, ref num);
					Rect rect3 = new Rect(0f, 0f, rect.width - num, 50f);
					string label = pane.GetLabel(rect3);
					rect3.width += 300f;
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect3, label);
					if (pane.ShouldShowPaneContents)
					{
						Rect rect4 = rect.AtZero();
						rect4.yMin += 26f;
						pane.DoPaneContents(rect4);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception doing inspect pane: " + ex.ToString(), false);
				}
				finally
				{
					GUI.EndGroup();
				}
			}
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x001F72F4 File Offset: 0x001F54F4
		private static void DoTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass17_0 <>c__DisplayClass17_;
			<>c__DisplayClass17_.pane = pane;
			try
			{
				InspectPaneUtility.<>c__DisplayClass17_1 <>c__DisplayClass17_2;
				<>c__DisplayClass17_2.tabsTopY = <>c__DisplayClass17_.pane.PaneTopY - 30f;
				<>c__DisplayClass17_2.curTabX = InspectPaneUtility.PaneWidthFor(<>c__DisplayClass17_.pane) - 72f;
				<>c__DisplayClass17_2.leftEdge = 0f;
				<>c__DisplayClass17_2.drewOpen = false;
				if (<>c__DisplayClass17_.pane.CurTabs != null)
				{
					IList list = <>c__DisplayClass17_.pane.CurTabs as IList;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							InspectPaneUtility.<DoTabs>g__Do|17_0((InspectTabBase)list[i], ref <>c__DisplayClass17_, ref <>c__DisplayClass17_2);
						}
					}
					else
					{
						foreach (InspectTabBase tab in <>c__DisplayClass17_.pane.CurTabs)
						{
							InspectPaneUtility.<DoTabs>g__Do|17_0(tab, ref <>c__DisplayClass17_, ref <>c__DisplayClass17_2);
						}
					}
				}
				if (<>c__DisplayClass17_2.drewOpen)
				{
					GUI.DrawTexture(new Rect(0f, <>c__DisplayClass17_2.tabsTopY, <>c__DisplayClass17_2.leftEdge, 30f), InspectPaneUtility.InspectTabButtonFillTex);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 742783, false);
			}
		}

		// Token: 0x06005B5F RID: 23391 RVA: 0x001F7430 File Offset: 0x001F5630
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x06005B60 RID: 23392 RVA: 0x001F7444 File Offset: 0x001F5644
		private static void ToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (InspectPaneUtility.IsOpen(tab, pane) || (tab == null && pane.OpenTabType == null))
			{
				pane.OpenTabType = null;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				return;
			}
			tab.OnOpen();
			pane.OpenTabType = tab.GetType();
			SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
		}

		// Token: 0x06005B61 RID: 23393 RVA: 0x001F749C File Offset: 0x001F569C
		public static InspectTabBase OpenTab(Type inspectTabType)
		{
			InspectPaneUtility.<>c__DisplayClass20_0 <>c__DisplayClass20_;
			<>c__DisplayClass20_.inspectTabType = inspectTabType;
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			<>c__DisplayClass20_.tab = null;
			if (mainTabWindow_Inspect.CurTabs != null)
			{
				IList list = mainTabWindow_Inspect.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (InspectPaneUtility.<OpenTab>g__Find|20_0((InspectTabBase)list[i], ref <>c__DisplayClass20_))
						{
							break;
						}
					}
				}
				else
				{
					using (IEnumerator<InspectTabBase> enumerator = mainTabWindow_Inspect.CurTabs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InspectPaneUtility.<OpenTab>g__Find|20_0(enumerator.Current, ref <>c__DisplayClass20_))
							{
								break;
							}
						}
					}
				}
			}
			if (<>c__DisplayClass20_.tab != null)
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, true);
				}
				if (!InspectPaneUtility.IsOpen(<>c__DisplayClass20_.tab, mainTabWindow_Inspect))
				{
					InspectPaneUtility.ToggleTab(<>c__DisplayClass20_.tab, mainTabWindow_Inspect);
				}
			}
			return <>c__DisplayClass20_.tab;
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x001F759C File Offset: 0x001F579C
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (TutorSystem.TutorialMode && !InspectPaneUtility.IsOpen(tab, pane) && !TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				return;
			}
			InspectPaneUtility.ToggleTab(tab, pane);
		}

		// Token: 0x040031D8 RID: 12760
		private static Dictionary<string, string> truncatedLabelsCached = new Dictionary<string, string>();

		// Token: 0x040031D9 RID: 12761
		public const float TabWidth = 72f;

		// Token: 0x040031DA RID: 12762
		public const float TabHeight = 30f;

		// Token: 0x040031DB RID: 12763
		private static readonly Texture2D InspectTabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.07450981f, 0.08627451f, 0.105882354f, 1f));

		// Token: 0x040031DC RID: 12764
		public const float CornerButtonsSize = 24f;

		// Token: 0x040031DD RID: 12765
		public const float PaneInnerMargin = 12f;

		// Token: 0x040031DE RID: 12766
		public const float PaneHeight = 165f;

		// Token: 0x040031DF RID: 12767
		private const int TabMinimum = 6;

		// Token: 0x040031E0 RID: 12768
		private static List<Thing> selectedThings = new List<Thing>();
	}
}
