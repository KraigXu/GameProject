using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E50 RID: 3664
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06005886 RID: 22662 RVA: 0x001D644C File Offset: 0x001D464C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x06005887 RID: 22663 RVA: 0x001D645D File Offset: 0x001D465D
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x001D6474 File Offset: 0x001D4674
		public override void DoWindowContents(Rect rect)
		{
			rect.height -= this.CloseButSize.y;
			float num = 0f;
			if (Widgets.ButtonText(new Rect(rect.width - 354f - 16f, num, 354f, 32f), "ManageDrugPolicies".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			num += 42f;
			Rect outRect = new Rect(0f, num, rect.width, rect.height - num);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.lastHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float num2 = 0f;
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				if (this.caravan.pawns[i].drugs != null)
				{
					if (num2 + 30f >= this.scrollPos.y && num2 <= this.scrollPos.y + outRect.height)
					{
						this.DoRow(new Rect(0f, num2, viewRect.width, 30f), this.caravan.pawns[i]);
					}
					num2 += 30f;
				}
			}
			this.lastHeight = num2;
			Widgets.EndScrollView();
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x001D65F4 File Offset: 0x001D47F4
		private void DoRow(Rect rect, Pawn pawn)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 354f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect2, pawn.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(new Rect(rect.x + rect.width - 354f, rect.y, 354f, 30f), pawn);
		}

		// Token: 0x04002FC2 RID: 12226
		private Caravan caravan;

		// Token: 0x04002FC3 RID: 12227
		private Vector2 scrollPos;

		// Token: 0x04002FC4 RID: 12228
		private float lastHeight;

		// Token: 0x04002FC5 RID: 12229
		private const float RowHeight = 30f;

		// Token: 0x04002FC6 RID: 12230
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x04002FC7 RID: 12231
		private const int ManageDrugPoliciesButtonHeight = 32;
	}
}
