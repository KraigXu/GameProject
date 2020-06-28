using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E26 RID: 3622
	[StaticConstructorOnStartup]
	public class Designator_Dropdown : Designator
	{
		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x0600577B RID: 22395 RVA: 0x001D12FD File Offset: 0x001CF4FD
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + (this.activeDesignatorSet ? "" : "...");
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x0600577C RID: 22396 RVA: 0x001D1323 File Offset: 0x001CF523
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x0600577D RID: 22397 RVA: 0x001D1330 File Offset: 0x001CF530
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x0600577E RID: 22398 RVA: 0x001D1340 File Offset: 0x001CF540
		public override bool Visible
		{
			get
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x001D1379 File Offset: 0x001CF579
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06005780 RID: 22400 RVA: 0x001D1381 File Offset: 0x001CF581
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x001D13A1 File Offset: 0x001CF5A1
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			return result;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x001D13B8 File Offset: 0x001CF5B8
		public static void DrawExtraOptionsIcon(Vector2 topLeft, float width)
		{
			GUI.DrawTexture(new Rect(topLeft.x + width - 16f - 1f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x001D13F3 File Offset: 0x001CF5F3
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x001D1414 File Offset: 0x001CF614
		public void SetActiveDesignator(Designator des, bool explicitySet = true)
		{
			this.activeDesignator = des;
			this.icon = des.icon;
			this.iconDrawScale = des.iconDrawScale;
			this.iconProportions = des.iconProportions;
			this.iconTexCoords = des.iconTexCoords;
			this.iconAngle = des.iconAngle;
			this.iconOffset = des.iconOffset;
			if (explicitySet)
			{
				this.activeDesignatorSet = true;
			}
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x001D147A File Offset: 0x001CF67A
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x001D1488 File Offset: 0x001CF688
		public override void ProcessInput(Event ev)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator des = this.elements[i];
				if (des.Visible)
				{
					Action action = delegate
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(des);
						this.SetActiveDesignator(des, true);
					};
					ThingDef designatorCost = this.GetDesignatorCost(des);
					if (designatorCost != null)
					{
						list.Add(new FloatMenuOption(des.LabelCap, action, designatorCost, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else
					{
						list.Add(new FloatMenuOption(des.LabelCap, action, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.vanishIfMouseDistant = true;
			floatMenu.onCloseCallback = delegate
			{
				this.activeDesignatorSet = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this.activeDesignator);
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x001D15A2 File Offset: 0x001CF7A2
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x001D15B0 File Offset: 0x001CF7B0
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x001D15BD File Offset: 0x001CF7BD
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x001D15CC File Offset: 0x001CF7CC
		private ThingDef GetDesignatorCost(Designator des)
		{
			Designator_Place designator_Place = des as Designator_Place;
			if (designator_Place != null)
			{
				BuildableDef placingDef = designator_Place.PlacingDef;
				if (placingDef.costList.Count > 0)
				{
					return placingDef.costList.MaxBy((ThingDefCountClass c) => c.thingDef.BaseMarketValue * (float)c.count).thingDef;
				}
			}
			return null;
		}

		// Token: 0x04002F9C RID: 12188
		private List<Designator> elements = new List<Designator>();

		// Token: 0x04002F9D RID: 12189
		private Designator activeDesignator;

		// Token: 0x04002F9E RID: 12190
		private bool activeDesignatorSet;

		// Token: 0x04002F9F RID: 12191
		public static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Widgets/PlusOptions", true);

		// Token: 0x04002FA0 RID: 12192
		private const float ButSize = 16f;

		// Token: 0x04002FA1 RID: 12193
		private const float ButPadding = 1f;
	}
}
