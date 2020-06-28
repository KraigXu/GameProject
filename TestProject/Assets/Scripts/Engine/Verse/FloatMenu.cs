using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200037D RID: 893
	public class FloatMenu : Window
	{
		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001A6B RID: 6763 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x000A2BC0 File Offset: 0x000A0DC0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x000A2BD3 File Offset: 0x000A0DD3
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x000A2BE1 File Offset: 0x000A0DE1
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x000A2BFC File Offset: 0x000A0DFC
		private float MaxViewHeight
		{
			get
			{
				if (this.UsingScrollbar)
				{
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (requiredHeight > num)
						{
							num = requiredHeight;
						}
						num2 += requiredHeight + -1f;
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					return num2 / (float)columnCount;
				}
				return this.MaxWindowHeight;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x000A2C74 File Offset: 0x000A0E74
		private float TotalViewHeight
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				float maxViewHeight = this.MaxViewHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x000A2CE8 File Offset: 0x000A0EE8
		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num += 16f;
				}
				return num;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x000A2D18 File Offset: 0x000A0F18
		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300f)
					{
						return 300f;
					}
					if (requiredWidth > num)
					{
						num = requiredWidth;
					}
				}
				return Mathf.Round(num);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x000A2D6D File Offset: 0x000A0F6D
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x000A2D87 File Offset: 0x000A0F87
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x000A2D97 File Offset: 0x000A0F97
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A76 RID: 6774 RVA: 0x000A2DAC File Offset: 0x000A0FAC
		private int ColumnCountIfNoScrollbar
		{
			get
			{
				if (this.options == null)
				{
					return 1;
				}
				Text.Font = GameFont.Small;
				int num = 1;
				float num2 = 0f;
				float maxWindowHeight = this.MaxWindowHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxWindowHeight)
					{
						num2 = requiredHeight;
						num++;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return num;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x000A2E23 File Offset: 0x000A1023
		public FloatMenuSizeMode SizeMode
		{
			get
			{
				if (this.options.Count > 60)
				{
					return FloatMenuSizeMode.Tiny;
				}
				return FloatMenuSizeMode.Normal;
			}
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000A2E38 File Offset: 0x000A1038
		public FloatMenu(List<FloatMenuOption> options)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.", false);
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending
			select op).ToList<FloatMenuOption>();
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000A2EF8 File Offset: 0x000A10F8
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000A2F10 File Offset: 0x000A1110
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x000A2FC0 File Offset: 0x000A11C0
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(this.windowRect.x, this.windowRect.y);
				Text.Font = GameFont.Small;
				float width = Mathf.Max(150f, 15f + Text.CalcSize(this.title).x);
				Rect titleRect = new Rect(vector.x + FloatMenu.TitleOffset.x, vector.y + FloatMenu.TitleOffset.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, delegate
				{
					GUI.color = this.baseColor;
					Text.Font = GameFont.Small;
					Rect position = titleRect.AtZero();
					position.width = 150f;
					GUI.DrawTexture(position, TexUI.TextBGBlack);
					Rect rect = titleRect.AtZero();
					rect.x += 15f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, this.title);
					Text.Anchor = TextAnchor.UpperLeft;
				}, false, false, 0f);
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000A3090 File Offset: 0x000A1290
		public override void DoWindowContents(Rect rect)
		{
			if (this.needSelection && Find.Selector.SingleSelectedThing == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			this.UpdateBaseColor();
			bool usingScrollbar = this.UsingScrollbar;
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Vector2 zero = Vector2.zero;
			float maxViewHeight = this.MaxViewHeight;
			float columnWidth = this.ColumnWidth;
			if (usingScrollbar)
			{
				rect.width -= 10f;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, this.TotalWidth - 16f, this.TotalViewHeight), true);
			}
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuOption floatMenuOption = this.options[i];
				float requiredHeight = floatMenuOption.RequiredHeight;
				if (zero.y + requiredHeight + -1f > maxViewHeight)
				{
					zero.y = 0f;
					zero.x += columnWidth + -1f;
				}
				Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
				zero.y += requiredHeight + -1f;
				if (floatMenuOption.DoGUI(rect2, this.givesColonistOrders, this))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
			}
			if (usingScrollbar)
			{
				Widgets.EndScrollView();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
				this.Close(true);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000A3211 File Offset: 0x000A1411
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000A322C File Offset: 0x000A142C
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreOptionChosen(FloatMenuOption opt)
		{
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000A3248 File Offset: 0x000A1448
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
					if (num > 95f)
					{
						this.Close(false);
						this.Cancel();
						return;
					}
				}
			}
		}

		// Token: 0x04000F80 RID: 3968
		public bool givesColonistOrders;

		// Token: 0x04000F81 RID: 3969
		public bool vanishIfMouseDistant = true;

		// Token: 0x04000F82 RID: 3970
		public Action onCloseCallback;

		// Token: 0x04000F83 RID: 3971
		protected List<FloatMenuOption> options;

		// Token: 0x04000F84 RID: 3972
		private string title;

		// Token: 0x04000F85 RID: 3973
		private bool needSelection;

		// Token: 0x04000F86 RID: 3974
		private Color baseColor = Color.white;

		// Token: 0x04000F87 RID: 3975
		private Vector2 scrollPosition;

		// Token: 0x04000F88 RID: 3976
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x04000F89 RID: 3977
		private const float OptionSpacing = -1f;

		// Token: 0x04000F8A RID: 3978
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x04000F8B RID: 3979
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04000F8C RID: 3980
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04000F8D RID: 3981
		private const float FadeStartMouseDist = 5f;

		// Token: 0x04000F8E RID: 3982
		private const float FadeFinishMouseDist = 100f;
	}
}
