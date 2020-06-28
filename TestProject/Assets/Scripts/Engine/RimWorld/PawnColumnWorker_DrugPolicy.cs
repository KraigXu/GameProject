using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EE5 RID: 3813
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x06005D6D RID: 23917 RVA: 0x00204D20 File Offset: 0x00202F20
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x00204DB1 File Offset: 0x00202FB1
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x00204DC3 File Offset: 0x00202FC3
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x00204DDB File Offset: 0x00202FDB
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x00204AFD File Offset: 0x00202CFD
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x00204DFC File Offset: 0x00202FFC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x00204E1F File Offset: 0x0020301F
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			{
				return pawn.drugs.CurrentPolicy.uniqueId;
			}
			return int.MinValue;
		}

		// Token: 0x040032C6 RID: 12998
		private const int TopAreaHeight = 65;

		// Token: 0x040032C7 RID: 12999
		public const int ManageDrugPoliciesButtonHeight = 32;
	}
}
