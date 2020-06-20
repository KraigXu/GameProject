using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E72 RID: 3698
	public static class NeedsCardUtility
	{
		// Token: 0x0600599B RID: 22939 RVA: 0x001E39EC File Offset: 0x001E1BEC
		public static Vector2 GetSize(Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			if (pawn.needs.mood != null)
			{
				return NeedsCardUtility.FullSize;
			}
			return new Vector2(225f, (float)NeedsCardUtility.displayNeeds.Count * Mathf.Min(70f, NeedsCardUtility.FullSize.y / (float)NeedsCardUtility.displayNeeds.Count));
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x001E3A48 File Offset: 0x001E1C48
		public static void DoNeedsMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			Rect rect2 = new Rect(rect.x, rect.y, 225f, rect.height);
			NeedsCardUtility.DoNeeds(rect2, pawn);
			if (pawn.needs.mood != null)
			{
				NeedsCardUtility.DoMoodAndThoughts(new Rect(rect2.xMax, rect.y, rect.width - rect2.width, rect.height), pawn, ref thoughtScrollPosition);
			}
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x001E3ABC File Offset: 0x001E1CBC
		public static void DoNeeds(Rect rect, Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.displayNeeds.Count; i++)
			{
				Need need = NeedsCardUtility.displayNeeds[i];
				Rect rect2 = new Rect(rect.x, rect.y + num, rect.width, Mathf.Min(70f, rect.height / (float)NeedsCardUtility.displayNeeds.Count));
				if (!need.def.major)
				{
					if (i > 0 && NeedsCardUtility.displayNeeds[i - 1].def.major)
					{
						rect2.y += 10f;
					}
					rect2.width *= 0.73f;
					rect2.height = Mathf.Max(rect2.height * 0.666f, 30f);
				}
				need.DrawOnGUI(rect2, int.MaxValue, -1f, true, true);
				num = rect2.yMax;
			}
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x001E3BC0 File Offset: 0x001E1DC0
		private static void DoMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width * 0.8f, 70f);
			pawn.needs.mood.DrawOnGUI(rect2, int.MaxValue, -1f, true, true);
			NeedsCardUtility.DrawThoughtListing(new Rect(0f, 80f, rect.width, rect.height - 70f - 10f).ContractedBy(10f), pawn, ref thoughtScrollPosition);
			GUI.EndGroup();
		}

		// Token: 0x0600599F RID: 22943 RVA: 0x001E3C54 File Offset: 0x001E1E54
		private static void UpdateDisplayNeeds(Pawn pawn)
		{
			NeedsCardUtility.displayNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (allNeeds[i].ShowOnNeedList)
				{
					NeedsCardUtility.displayNeeds.Add(allNeeds[i]);
				}
			}
			PawnNeedsUIUtility.SortInDisplayOrder(NeedsCardUtility.displayNeeds);
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x001E3CB4 File Offset: 0x001E1EB4
		private static void DrawThoughtListing(Rect listingRect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			Text.Font = GameFont.Small;
			PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(pawn.needs.mood, NeedsCardUtility.thoughtGroupsPresent);
			float height = (float)NeedsCardUtility.thoughtGroupsPresent.Count * 24f;
			Widgets.BeginScrollView(listingRect, ref thoughtScrollPosition, new Rect(0f, 0f, listingRect.width - 16f, height), true);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.thoughtGroupsPresent.Count; i++)
			{
				if (NeedsCardUtility.DrawThoughtGroup(new Rect(0f, num, listingRect.width - 16f, 20f), NeedsCardUtility.thoughtGroupsPresent[i], pawn))
				{
					num += 24f;
				}
			}
			Widgets.EndScrollView();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x001E3D88 File Offset: 0x001E1F88
		private static bool DrawThoughtGroup(Rect rect, Thought group, Pawn pawn)
		{
			try
			{
				pawn.needs.mood.thoughts.GetMoodThoughts(group, NeedsCardUtility.thoughtGroup);
				Thought leadingThoughtInGroup = PawnNeedsUIUtility.GetLeadingThoughtInGroup(NeedsCardUtility.thoughtGroup);
				if (!leadingThoughtInGroup.VisibleInNeedsTab)
				{
					NeedsCardUtility.thoughtGroup.Clear();
					return false;
				}
				if (leadingThoughtInGroup != NeedsCardUtility.thoughtGroup[0])
				{
					NeedsCardUtility.thoughtGroup.Remove(leadingThoughtInGroup);
					NeedsCardUtility.thoughtGroup.Insert(0, leadingThoughtInGroup);
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (Mouse.IsOver(rect))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(leadingThoughtInGroup.Description);
					if (group.def.DurationTicks > 5)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						Thought_Memory thought_Memory = leadingThoughtInGroup as Thought_Memory;
						if (thought_Memory != null)
						{
							if (NeedsCardUtility.thoughtGroup.Count == 1)
							{
								stringBuilder.Append("ThoughtExpiresIn".Translate((group.def.DurationTicks - thought_Memory.age).ToStringTicksToPeriod(true, false, true, true)));
							}
							else
							{
								int num = int.MaxValue;
								int num2 = int.MinValue;
								foreach (Thought thought in NeedsCardUtility.thoughtGroup)
								{
									Thought_Memory thought_Memory2 = (Thought_Memory)thought;
									num = Mathf.Min(num, thought_Memory2.age);
									num2 = Mathf.Max(num2, thought_Memory2.age);
								}
								stringBuilder.Append("ThoughtStartsExpiringIn".Translate((group.def.DurationTicks - num2).ToStringTicksToPeriod(true, false, true, true)));
								stringBuilder.AppendLine();
								stringBuilder.Append("ThoughtFinishesExpiringIn".Translate((group.def.DurationTicks - num).ToStringTicksToPeriod(true, false, true, true)));
							}
						}
					}
					if (NeedsCardUtility.thoughtGroup.Count > 1)
					{
						bool flag = false;
						for (int i = 1; i < NeedsCardUtility.thoughtGroup.Count; i++)
						{
							bool flag2 = false;
							for (int j = 0; j < i; j++)
							{
								if (NeedsCardUtility.thoughtGroup[i].LabelCap == NeedsCardUtility.thoughtGroup[j].LabelCap)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								if (!flag)
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLine();
									flag = true;
								}
								stringBuilder.AppendLine("+ " + NeedsCardUtility.thoughtGroup[i].LabelCap);
							}
						}
					}
					TooltipHandler.TipRegion(rect, new TipSignal(stringBuilder.ToString(), 7291));
				}
				Text.WordWrap = false;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect2 = new Rect(rect.x + 10f, rect.y, 225f, rect.height);
				rect2.yMin -= 3f;
				rect2.yMax += 3f;
				string text = leadingThoughtInGroup.LabelCap;
				if (NeedsCardUtility.thoughtGroup.Count > 1)
				{
					text = text + " x" + NeedsCardUtility.thoughtGroup.Count;
				}
				Widgets.Label(rect2, text);
				Text.Anchor = TextAnchor.MiddleCenter;
				float num3 = pawn.needs.mood.thoughts.MoodOffsetOfGroup(group);
				if (num3 == 0f)
				{
					GUI.color = NeedsCardUtility.NoEffectColor;
				}
				else if (num3 > 0f)
				{
					GUI.color = NeedsCardUtility.MoodColor;
				}
				else
				{
					GUI.color = NeedsCardUtility.MoodColorNegative;
				}
				Widgets.Label(new Rect(rect.x + 235f, rect.y, 32f, rect.height), num3.ToString("##0"));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.WordWrap = true;
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Exception in DrawThoughtGroup for ",
					group.def,
					" on ",
					pawn,
					": ",
					ex.ToString()
				}), 3452698, false);
			}
			return true;
		}

		// Token: 0x040030A6 RID: 12454
		private static List<Need> displayNeeds = new List<Need>();

		// Token: 0x040030A7 RID: 12455
		private static readonly Color MoodColor = new Color(0.1f, 1f, 0.1f);

		// Token: 0x040030A8 RID: 12456
		private static readonly Color MoodColorNegative = new Color(0.8f, 0.4f, 0.4f);

		// Token: 0x040030A9 RID: 12457
		private static readonly Color NoEffectColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x040030AA RID: 12458
		private const float ThoughtHeight = 20f;

		// Token: 0x040030AB RID: 12459
		private const float ThoughtSpacing = 4f;

		// Token: 0x040030AC RID: 12460
		private const float ThoughtIntervalY = 24f;

		// Token: 0x040030AD RID: 12461
		private const float MoodX = 235f;

		// Token: 0x040030AE RID: 12462
		private const float MoodNumberWidth = 32f;

		// Token: 0x040030AF RID: 12463
		private const float NeedsColumnWidth = 225f;

		// Token: 0x040030B0 RID: 12464
		public static readonly Vector2 FullSize = new Vector2(580f, 520f);

		// Token: 0x040030B1 RID: 12465
		private static List<Thought> thoughtGroupsPresent = new List<Thought>();

		// Token: 0x040030B2 RID: 12466
		private static List<Thought> thoughtGroup = new List<Thought>();
	}
}
