using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000DD1 RID: 3537
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x060055DA RID: 21978 RVA: 0x001C7BC6 File Offset: 0x001C5DC6
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060055DB RID: 21979 RVA: 0x001C7BCE File Offset: 0x001C5DCE
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060055DC RID: 21980 RVA: 0x001C7BD5 File Offset: 0x001C5DD5
		public bool Active
		{
			get
			{
				return this.cachedActive;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060055DD RID: 21981 RVA: 0x001C7BDD File Offset: 0x001C5DDD
		public string Label
		{
			get
			{
				if (!this.Active)
				{
					return "";
				}
				return this.cachedLabel;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060055DE RID: 21982 RVA: 0x001C7BF3 File Offset: 0x001C5DF3
		public float Height
		{
			get
			{
				Text.Font = GameFont.Small;
				return Text.CalcHeight(this.Label, 148f);
			}
		}

		// Token: 0x060055DF RID: 21983
		public abstract AlertReport GetReport();

		// Token: 0x060055E0 RID: 21984 RVA: 0x001C7C0B File Offset: 0x001C5E0B
		public virtual TaggedString GetExplanation()
		{
			return this.defaultExplanation;
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x001C7C18 File Offset: 0x001C5E18
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x001C7C20 File Offset: 0x001C5E20
		public void Notify_Started()
		{
			if (this.Priority >= AlertPriority.High)
			{
				if (this.alertBounce == null)
				{
					this.alertBounce = new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if (Time.timeSinceLevelLoad > 1f && Time.realtimeSinceStartup > this.lastBellTime + 0.5f)
				{
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime = Time.realtimeSinceStartup;
				}
			}
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x001C7C8C File Offset: 0x001C5E8C
		public void Recalculate()
		{
			AlertReport report = this.GetReport();
			this.cachedActive = report.active;
			if (report.active)
			{
				this.cachedLabel = this.GetLabel();
			}
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void AlertActiveUpdate()
		{
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x001C7CC0 File Offset: 0x001C5EC0
		public virtual Rect DrawAt(float topY, bool minimized)
		{
			Rect rect = new Rect((float)UI.screenWidth - 154f, topY, 154f, this.Height);
			if (this.alertBounce != null)
			{
				rect.x -= this.alertBounce.CalculateHorizontalOffset();
			}
			GUI.color = this.BGColor;
			GUI.DrawTexture(rect, Alert.AlertBGTex);
			GUI.color = Color.white;
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, this.Height), this.Label);
			GUI.EndGroup();
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, Alert.AlertBGTexHighlight);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.OnClick();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x001C7D8C File Offset: 0x001C5F8C
		protected virtual void OnClick()
		{
			IEnumerable<GlobalTargetInfo> allCulprits = this.GetReport().AllCulprits;
			if (allCulprits != null)
			{
				Alert.tmpTargets.Clear();
				foreach (GlobalTargetInfo item in allCulprits)
				{
					if (item.IsValid)
					{
						Alert.tmpTargets.Add(item);
					}
				}
				if (Alert.tmpTargets.Any<GlobalTargetInfo>())
				{
					if (Event.current.button == 1)
					{
						this.jumpToTargetCycleIndex--;
					}
					else
					{
						this.jumpToTargetCycleIndex++;
					}
					CameraJumper.TryJumpAndSelect(Alert.tmpTargets[GenMath.PositiveMod(this.jumpToTargetCycleIndex, Alert.tmpTargets.Count)]);
					Alert.tmpTargets.Clear();
				}
			}
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x001C7E68 File Offset: 0x001C6068
		public void DrawInfoPane()
		{
			//Alert.<>c__DisplayClass33_0 <>c__DisplayClass33_ = new Alert.<>c__DisplayClass33_0();
			//if (Event.current.type != EventType.Repaint)
			//{
			//	return;
			//}
			//this.Recalculate();
			//if (!this.Active)
			//{
			//	return;
			//}
			//<>c__DisplayClass33_.expString = this.GetExplanation();
			//if (<>c__DisplayClass33_.expString.NullOrEmpty())
			//{
			//	return;
			//}
			//Text.Font = GameFont.Small;
			//Text.Anchor = TextAnchor.UpperLeft;
			//if (this.GetReport().AnyCulpritValid)
			//{
			//	<>c__DisplayClass33_.expString += "\n\n(" + "ClickToJumpToProblem".Translate() + ")";
			//}
			//float num = Text.CalcHeight(<>c__DisplayClass33_.expString, 310f);
			//num += 20f;
			//<>c__DisplayClass33_.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			//if (<>c__DisplayClass33_.infoRect.yMax > (float)UI.screenHeight)
			//{
			//	Alert.<>c__DisplayClass33_0 <>c__DisplayClass33_2 = <>c__DisplayClass33_;
			//	<>c__DisplayClass33_2.infoRect.y = <>c__DisplayClass33_2.infoRect.y - ((float)UI.screenHeight - <>c__DisplayClass33_.infoRect.yMax);
			//}
			//if (<>c__DisplayClass33_.infoRect.y < 0f)
			//{
			//	<>c__DisplayClass33_.infoRect.y = 0f;
			//}
			//Find.WindowStack.ImmediateWindow(138956, <>c__DisplayClass33_.infoRect, WindowLayer.GameUI, delegate
			//{
			//	Text.Font = GameFont.Small;
			//	Rect rect = <>c__DisplayClass33_.infoRect.AtZero();
			//	Widgets.DrawWindowBackground(rect);
			//	Rect position = rect.ContractedBy(10f);
			//	GUI.BeginGroup(position);
			//	Widgets.Label(new Rect(0f, 0f, position.width, position.height), <>c__DisplayClass33_.expString);
			//	GUI.EndGroup();
			//}, false, false, 1f);
		}

		// Token: 0x04002EEC RID: 12012
		protected AlertPriority defaultPriority;

		// Token: 0x04002EED RID: 12013
		protected string defaultLabel;

		// Token: 0x04002EEE RID: 12014
		protected string defaultExplanation;

		// Token: 0x04002EEF RID: 12015
		protected float lastBellTime = -1000f;

		// Token: 0x04002EF0 RID: 12016
		private int jumpToTargetCycleIndex;

		// Token: 0x04002EF1 RID: 12017
		private bool cachedActive;

		// Token: 0x04002EF2 RID: 12018
		private string cachedLabel;

		// Token: 0x04002EF3 RID: 12019
		private AlertBounce alertBounce;

		// Token: 0x04002EF4 RID: 12020
		public const float Width = 154f;

		// Token: 0x04002EF5 RID: 12021
		private const float TextWidth = 148f;

		// Token: 0x04002EF6 RID: 12022
		private const float ItemPeekWidth = 30f;

		// Token: 0x04002EF7 RID: 12023
		public const float InfoRectWidth = 330f;

		// Token: 0x04002EF8 RID: 12024
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04002EF9 RID: 12025
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		// Token: 0x04002EFA RID: 12026
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
	}
}
