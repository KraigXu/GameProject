using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008DF RID: 2271
	public abstract class MainButtonWorker
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x00126E24 File Offset: 0x00125024
		public virtual bool Disabled
		{
			get
			{
				return (Find.CurrentMap == null && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.WorldRoutePlanner.Active && Find.WorldRoutePlanner.FormingCaravan && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World));
			}
		}

		// Token: 0x0600366E RID: 13934
		public abstract void Activate();

		// Token: 0x0600366F RID: 13935 RVA: 0x00126E8C File Offset: 0x0012508C
		public virtual void InterfaceTryActivate()
		{
			if (TutorSystem.TutorialMode && this.def.canBeTutorDenied && Find.MainTabsRoot.OpenTab != this.def && !TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				return;
			}
			this.Activate();
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00126EEC File Offset: 0x001250EC
		public virtual void DoButton(Rect rect)
		{
			Text.Font = GameFont.Small;
			string text = this.def.LabelCap;
			float num = this.def.LabelCapWidth;
			if (num > rect.width - 2f)
			{
				text = this.def.ShortenedLabelCap;
				num = this.def.ShortenedLabelCapWidth;
			}
			if (this.Disabled)
			{
				Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
				{
					Event.current.Use();
					return;
				}
			}
			else
			{
				bool flag = num > 0.85f * rect.width - 1f;
				Rect rect2 = rect;
				string label = (this.def.Icon == null) ? text : "";
				float textLeftMargin = flag ? 2f : -1f;
				if (Widgets.ButtonTextSubtle(rect2, label, this.ButtonBarPercent, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2)))
				{
					this.InterfaceTryActivate();
				}
				if (this.def.Icon != null)
				{
					Vector2 vector = rect.center;
					float num2 = 16f;
					if (Mouse.IsOver(rect))
					{
						vector += new Vector2(2f, -2f);
					}
					GUI.DrawTexture(new Rect(vector.x - num2, vector.y - num2, 32f, 32f), this.def.Icon);
				}
				if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
				{
					UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
				}
				if (Mouse.IsOver(rect) && !this.def.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, this.def.LabelCap + "\n\n" + this.def.description);
				}
			}
		}

		// Token: 0x04001EED RID: 7917
		public MainButtonDef def;

		// Token: 0x04001EEE RID: 7918
		private const float CompactModeMargin = 2f;

		// Token: 0x04001EEF RID: 7919
		private const float IconSize = 32f;
	}
}
