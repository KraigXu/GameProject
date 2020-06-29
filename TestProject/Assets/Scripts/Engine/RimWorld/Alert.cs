using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		
		
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		
		
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		
		
		public bool Active
		{
			get
			{
				return this.cachedActive;
			}
		}

		
		
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

		
		
		public float Height
		{
			get
			{
				Text.Font = GameFont.Small;
				return Text.CalcHeight(this.Label, 148f);
			}
		}

		
		public abstract AlertReport GetReport();

		
		public virtual TaggedString GetExplanation()
		{
			return this.defaultExplanation;
		}

		
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		
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

		
		public void Recalculate()
		{
			AlertReport report = this.GetReport();
			this.cachedActive = report.active;
			if (report.active)
			{
				this.cachedLabel = this.GetLabel();
			}
		}

		
		public virtual void AlertActiveUpdate()
		{
		}

		
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

		
		public void DrawInfoPane()
		{
			//Alert.c__DisplayClass33_0 c__DisplayClass33_ = new Alert.c__DisplayClass33_0();
			//if (Event.current.type != EventType.Repaint)
			//{
			//	return;
			//}
			//this.Recalculate();
			//if (!this.Active)
			//{
			//	return;
			//}
			//c__DisplayClass33_.expString = this.GetExplanation();
			//if (c__DisplayClass33_.expString.NullOrEmpty())
			//{
			//	return;
			//}
			//Text.Font = GameFont.Small;
			//Text.Anchor = TextAnchor.UpperLeft;
			//if (this.GetReport().AnyCulpritValid)
			//{
			//	c__DisplayClass33_.expString += "\n\n(" + "ClickToJumpToProblem".Translate() + ")";
			//}
			//float num = Text.CalcHeight(c__DisplayClass33_.expString, 310f);
			//num += 20f;
			//c__DisplayClass33_.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			//if (c__DisplayClass33_.infoRect.yMax > (float)UI.screenHeight)
			//{
			//	Alert.c__DisplayClass33_0 c__DisplayClass33_2 = c__DisplayClass33_;
			//	c__DisplayClass33_2.infoRect.y = c__DisplayClass33_2.infoRect.y - ((float)UI.screenHeight - c__DisplayClass33_.infoRect.yMax);
			//}
			//if (c__DisplayClass33_.infoRect.y < 0f)
			//{
			//	c__DisplayClass33_.infoRect.y = 0f;
			//}
			//Find.WindowStack.ImmediateWindow(138956, c__DisplayClass33_.infoRect, WindowLayer.GameUI, delegate
			//{
			//	Text.Font = GameFont.Small;
			//	Rect rect = c__DisplayClass33_.infoRect.AtZero();
			//	Widgets.DrawWindowBackground(rect);
			//	Rect position = rect.ContractedBy(10f);
			//	GUI.BeginGroup(position);
			//	Widgets.Label(new Rect(0f, 0f, position.width, position.height), c__DisplayClass33_.expString);
			//	GUI.EndGroup();
			//}, false, false, 1f);
		}

		
		protected AlertPriority defaultPriority;

		
		protected string defaultLabel;

		
		protected string defaultExplanation;

		
		protected float lastBellTime = -1000f;

		
		private int jumpToTargetCycleIndex;

		
		private bool cachedActive;

		
		private string cachedLabel;

		
		private AlertBounce alertBounce;

		
		public const float Width = 154f;

		
		private const float TextWidth = 148f;

		
		private const float ItemPeekWidth = 30f;

		
		public const float InfoRectWidth = 330f;

		
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
	}
}
