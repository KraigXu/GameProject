using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B83 RID: 2947
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x0600451A RID: 17690 RVA: 0x00175A95 File Offset: 0x00173C95
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x0600451B RID: 17691 RVA: 0x00175AA7 File Offset: 0x00173CA7
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x0600451C RID: 17692 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x0600451D RID: 17693 RVA: 0x0004E475 File Offset: 0x0004C675
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600451E RID: 17694 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600451F RID: 17695 RVA: 0x00175AB6 File Offset: 0x00173CB6
		// (set) Token: 0x06004520 RID: 17696 RVA: 0x00175ABE File Offset: 0x00173CBE
		public virtual float CurLevel
		{
			get
			{
				return this.curLevelInt;
			}
			set
			{
				this.curLevelInt = Mathf.Clamp(value, 0f, this.MaxLevel);
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004521 RID: 17697 RVA: 0x00175AD7 File Offset: 0x00173CD7
		// (set) Token: 0x06004522 RID: 17698 RVA: 0x00175AE6 File Offset: 0x00173CE6
		public float CurLevelPercentage
		{
			get
			{
				return this.CurLevel / this.MaxLevel;
			}
			set
			{
				this.CurLevel = value * this.MaxLevel;
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004523 RID: 17699 RVA: 0x00175AF8 File Offset: 0x00173CF8
		protected virtual bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || (this.def.freezeInMentalState && this.pawn.InMentalState) || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004524 RID: 17700 RVA: 0x00175B55 File Offset: 0x00173D55
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06004525 RID: 17701 RVA: 0x00175B85 File Offset: 0x00173D85
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public Need()
		{
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00175B92 File Offset: 0x00173D92
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00175BA7 File Offset: 0x00173DA7
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x06004529 RID: 17705
		public abstract void NeedInterval();

		// Token: 0x0600452A RID: 17706 RVA: 0x00175BD0 File Offset: 0x00173DD0
		public virtual string GetTipString()
		{
			return string.Concat(new string[]
			{
				this.LabelCap,
				": ",
				this.CurLevelPercentage.ToStringPercent(),
				"\n",
				this.def.description
			});
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00175C1D File Offset: 0x00173E1D
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00175C2C File Offset: 0x00173E2C
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (doTooltip && Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => this.GetTipString(), rect.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (customMargin >= 0f) ? customMargin : (num2 + 15f);
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			Text.Font = ((rect.height > 55f) ? GameFont.Small : GameFont.Tiny);
			Text.Anchor = TextAnchor.LowerLeft;
			Widgets.Label(new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f), this.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
			rect2 = new Rect(rect2.x + num3, rect2.y, rect2.width - num3 * 2f, rect2.height - num2);
			Rect rect3 = rect2;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect3.width *= num4;
			Rect barRect = Widgets.FillableBar(rect3, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect3, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect, this.threshPercents[i] * num4);
				}
			}
			if (this.def.scaleBar)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect2, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00175EE8 File Offset: 0x001740E8
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				Log.ErrorOnce(this.def + " drawing bar percent > 1 : " + pct, 6932178, false);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), Need.BarInstantMarkerTex);
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00175F88 File Offset: 0x00174188
		private void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((barRect.width > 60f) ? 2 : 1);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
			Texture2D image;
			if (threshPct < this.CurLevelPercentage)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00176050 File Offset: 0x00174250
		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = position.yMin + 4f;
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = position2.yMax - 4f;
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0f)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400277D RID: 10109
		public NeedDef def;

		// Token: 0x0400277E RID: 10110
		protected Pawn pawn;

		// Token: 0x0400277F RID: 10111
		protected float curLevelInt;

		// Token: 0x04002780 RID: 10112
		protected List<float> threshPercents;

		// Token: 0x04002781 RID: 10113
		public const float MaxDrawHeight = 70f;

		// Token: 0x04002782 RID: 10114
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x04002783 RID: 10115
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x04002784 RID: 10116
		private const float BarInstantMarkerSize = 12f;
	}
}
