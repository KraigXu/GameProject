using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Designator_Dropdown : Designator
	{
		
		// (get) Token: 0x0600577B RID: 22395 RVA: 0x001D12FD File Offset: 0x001CF4FD
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + (this.activeDesignatorSet ? "" : "...");
			}
		}

		
		// (get) Token: 0x0600577C RID: 22396 RVA: 0x001D1323 File Offset: 0x001CF523
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		
		// (get) Token: 0x0600577D RID: 22397 RVA: 0x001D1330 File Offset: 0x001CF530
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		
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

		
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x001D1379 File Offset: 0x001CF579
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		
		// (get) Token: 0x06005780 RID: 22400 RVA: 0x001D1381 File Offset: 0x001CF581
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			return result;
		}

		
		public static void DrawExtraOptionsIcon(Vector2 topLeft, float width)
		{
			GUI.DrawTexture(new Rect(topLeft.x + width - 16f - 1f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
		}

		
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		
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

		
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		
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
						this.n__0(ev);
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

		
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		
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

		
		private List<Designator> elements = new List<Designator>();

		
		private Designator activeDesignator;

		
		private bool activeDesignatorSet;

		
		public static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Widgets/PlusOptions", true);

		
		private const float ButSize = 16f;

		
		private const float ButPadding = 1f;
	}
}
