using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000377 RID: 887
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x000A1B0B File Offset: 0x0009FD0B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x000A1B2A File Offset: 0x0009FD2A
		// (set) Token: 0x06001A50 RID: 6736 RVA: 0x000A1B31 File Offset: 0x0009FD31
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage == value)
				{
					return;
				}
				EditWindow_Log.selectedMessage = value;
				if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
				{
					UI.UnfocusCurrentControl();
				}
			}
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000A1B5F File Offset: 0x0009FD5F
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000A1B72 File Offset: 0x0009FD72
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000A1B81 File Offset: 0x0009FD81
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000A1B93 File Offset: 0x0009FD93
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000A1BD1 File Offset: 0x0009FDD1
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000A1BE2 File Offset: 0x0009FDE2
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000A1BF0 File Offset: 0x0009FDF0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Clear", "Clear all log messages.", true, true))
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			}
			if (widgetRow.ButtonText("Trace big", "Set the stack trace to be large on screen.", true, true))
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			}
			if (widgetRow.ButtonText("Trace medium", "Set the stack trace to be medium-sized on screen.", true, true))
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			}
			if (widgetRow.ButtonText("Trace small", "Set the stack trace to be small on screen.", true, true))
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			}
			if (EditWindow_Log.canAutoOpen)
			{
				if (widgetRow.ButtonText("Auto-open is ON", "", true, true))
				{
					EditWindow_Log.canAutoOpen = false;
				}
			}
			else if (widgetRow.ButtonText("Auto-open is OFF", "", true, true))
			{
				EditWindow_Log.canAutoOpen = true;
			}
			if (widgetRow.ButtonText("Copy to clipboard", "Copy all messages to the clipboard.", true, true))
			{
				this.CopyAllMessagesToClipboard();
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x000A1DB0 File Offset: 0x0009FFB0
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x000A1DC0 File Offset: 0x0009FFC0
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			Widgets.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				string text = logMessage.text;
				if (text.Length > 1000)
				{
					text = text.Substring(0, 1000);
				}
				float num2 = Math.Min(30f, Text.CalcHeight(text, width));
				GUI.color = new Color(1f, 1f, 1f, 0.7f);
				Widgets.Label(new Rect(4f, num, 28f, num2), logMessage.repeats.ToStringCached());
				Rect rect = new Rect(28f, num, width, num2);
				if (EditWindow_Log.selectedMessage == logMessage)
				{
					GUI.DrawTexture(rect, EditWindow_Log.SelectedMessageTex);
				}
				else if (flag)
				{
					GUI.DrawTexture(rect, EditWindow_Log.AltMessageTex);
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					EditWindow_Log.ClearSelectedMessage();
					EditWindow_Log.SelectedMessage = logMessage;
				}
				GUI.color = logMessage.Color;
				Widgets.Label(rect, text);
				num += num2;
				flag = !flag;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.listingViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000A1F7C File Offset: 0x000A017C
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage == null)
			{
				return;
			}
			Rect rect = detailsRect;
			rect.height = 7f;
			Rect rect2 = detailsRect;
			rect2.yMin = rect.yMax;
			GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.borderDragging = true;
				Event.current.Use();
			}
			if (this.borderDragging)
			{
				EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				this.borderDragging = false;
			}
			GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
			string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
			GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
			if (text.Length > 15000)
			{
				Widgets.LabelScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, false, true, true);
				return;
			}
			Widgets.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x000A2090 File Offset: 0x000A0290
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04000F5B RID: 3931
		private static LogMessage selectedMessage = null;

		// Token: 0x04000F5C RID: 3932
		private static Vector2 messagesScrollPosition;

		// Token: 0x04000F5D RID: 3933
		private static Vector2 detailsScrollPosition;

		// Token: 0x04000F5E RID: 3934
		private static float detailsPaneHeight = 100f;

		// Token: 0x04000F5F RID: 3935
		private static bool canAutoOpen = true;

		// Token: 0x04000F60 RID: 3936
		public static bool wantsToOpen = false;

		// Token: 0x04000F61 RID: 3937
		private float listingViewHeight;

		// Token: 0x04000F62 RID: 3938
		private bool borderDragging;

		// Token: 0x04000F63 RID: 3939
		private const float CountWidth = 28f;

		// Token: 0x04000F64 RID: 3940
		private const float Yinc = 25f;

		// Token: 0x04000F65 RID: 3941
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x04000F66 RID: 3942
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x04000F67 RID: 3943
		private const float ListingMinHeight = 80f;

		// Token: 0x04000F68 RID: 3944
		private const float TopAreaHeight = 26f;

		// Token: 0x04000F69 RID: 3945
		private const float MessageMaxHeight = 30f;

		// Token: 0x04000F6A RID: 3946
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x04000F6B RID: 3947
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x04000F6C RID: 3948
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x04000F6D RID: 3949
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x04000F6E RID: 3950
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}
