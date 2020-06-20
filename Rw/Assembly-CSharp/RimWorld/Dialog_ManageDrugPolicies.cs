using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E58 RID: 3672
	[StaticConstructorOnStartup]
	public class Dialog_ManageDrugPolicies : Window
	{
		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06005900 RID: 22784 RVA: 0x001DAED0 File Offset: 0x001D90D0
		// (set) Token: 0x06005901 RID: 22785 RVA: 0x001DAED8 File Offset: 0x001D90D8
		private DrugPolicy SelectedPolicy
		{
			get
			{
				return this.selPolicy;
			}
			set
			{
				this.CheckSelectedPolicyHasName();
				this.selPolicy = value;
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06005902 RID: 22786 RVA: 0x001DAEE7 File Offset: 0x001D90E7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06005903 RID: 22787 RVA: 0x001DAEF8 File Offset: 0x001D90F8
		private void CheckSelectedPolicyHasName()
		{
			if (this.SelectedPolicy != null && this.SelectedPolicy.label.NullOrEmpty())
			{
				this.SelectedPolicy.label = "Unnamed";
			}
		}

		// Token: 0x06005904 RID: 22788 RVA: 0x001DAF24 File Offset: 0x001D9124
		public Dialog_ManageDrugPolicies(DrugPolicy selectedAssignedDrugs)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.SelectedPolicy = selectedAssignedDrugs;
		}

		// Token: 0x06005905 RID: 22789 RVA: 0x001DAF58 File Offset: 0x001D9158
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectDrugPolicy".Translate(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (DrugPolicy localAssignedDrugs3 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = localAssignedDrugs3;
					list.Add(new FloatMenuOption(localAssignedDrugs.label, delegate
					{
						this.SelectedPolicy = localAssignedDrugs;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewDrugPolicy".Translate(), true, true, true))
			{
				this.SelectedPolicy = Current.Game.drugPolicyDatabase.MakeNewDrugPolicy();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteDrugPolicy".Translate(), true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (DrugPolicy localAssignedDrugs2 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = localAssignedDrugs2;
					list2.Add(new FloatMenuOption(localAssignedDrugs.label, delegate
					{
						AcceptanceReport acceptanceReport = Current.Game.drugPolicyDatabase.TryDelete(localAssignedDrugs);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
							return;
						}
						if (localAssignedDrugs == this.SelectedPolicy)
						{
							this.SelectedPolicy = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y).ContractedBy(10f);
			if (this.SelectedPolicy == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoDrugPolicySelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			GUI.BeginGroup(rect4);
			Dialog_ManageDrugPolicies.DoNameInputRect(new Rect(0f, 0f, 200f, 30f), ref this.SelectedPolicy.label);
			Rect rect5 = new Rect(0f, 40f, rect4.width, rect4.height - 45f - 10f);
			this.DoPolicyConfigArea(rect5);
			GUI.EndGroup();
		}

		// Token: 0x06005906 RID: 22790 RVA: 0x001DB260 File Offset: 0x001D9460
		public override void PostOpen()
		{
			base.PostOpen();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x001DB273 File Offset: 0x001D9473
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedPolicyHasName();
		}

		// Token: 0x06005908 RID: 22792 RVA: 0x001DB281 File Offset: 0x001D9481
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Dialog_ManageDrugPolicies.ValidNameRegex);
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x001DB294 File Offset: 0x001D9494
		private void DoPolicyConfigArea(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 54f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax;
			rect3.height -= 50f;
			Rect rect4 = rect;
			rect4.yMin = rect4.yMax - 50f;
			this.DoColumnLabels(rect2);
			Widgets.DrawMenuSection(rect3);
			if (this.SelectedPolicy.Count == 0)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "NoDrugs".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			float height = (float)this.SelectedPolicy.Count * 35f;
			Rect viewRect = new Rect(0f, 0f, rect3.width - 16f, height);
			Widgets.BeginScrollView(rect3, ref this.scrollPosition, viewRect, true);
			DrugPolicy selectedPolicy = this.SelectedPolicy;
			for (int i = 0; i < selectedPolicy.Count; i++)
			{
				Rect rect5 = new Rect(0f, (float)i * 35f, viewRect.width, 35f);
				this.DoEntryRow(rect5, selectedPolicy[i]);
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x001DB3CC File Offset: 0x001D95CC
		private void CalculateColumnsWidths(Rect rect, out float addictionWidth, out float allowJoyWidth, out float scheduledWidth, out float drugNameWidth, out float frequencyWidth, out float moodThresholdWidth, out float joyThresholdWidth, out float takeToInventoryWidth)
		{
			float num = rect.width - 108f;
			drugNameWidth = num * 0.2f;
			addictionWidth = 36f;
			allowJoyWidth = 36f;
			scheduledWidth = 36f;
			frequencyWidth = num * 0.35f;
			moodThresholdWidth = num * 0.15f;
			joyThresholdWidth = num * 0.15f;
			takeToInventoryWidth = num * 0.15f;
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x001DB430 File Offset: 0x001D9630
		private void DoColumnLabels(Rect rect)
		{
			rect.width -= 16f;
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			float num9 = rect.x;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(num9 + 4f, rect.y, num4, rect.height);
			Widgets.Label(rect2, "DrugColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect2, "DrugNameColumnDesc");
			num9 += num4;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = new Rect(num9, rect.y, num2 + num2, rect.height / 2f);
			Widgets.Label(rect3, "DrugUsageColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect3, "DrugUsageColumnDesc");
			Rect rect4 = new Rect(num9, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect4, Dialog_ManageDrugPolicies.IconForAddiction);
			TooltipHandler.TipRegionByKey(rect4, "DrugUsageTipForAddiction");
			num9 += num;
			Rect rect5 = new Rect(num9, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect5, Dialog_ManageDrugPolicies.IconForJoy);
			TooltipHandler.TipRegionByKey(rect5, "DrugUsageTipForJoy");
			num9 += num2;
			Rect rect6 = new Rect(num9, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect6, Dialog_ManageDrugPolicies.IconScheduled);
			TooltipHandler.TipRegionByKey(rect6, "DrugUsageTipScheduled");
			num9 += num3;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect7 = new Rect(num9, rect.y, num5, rect.height);
			Widgets.Label(rect7, "FrequencyColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect7, "FrequencyColumnDesc");
			num9 += num5;
			Rect rect8 = new Rect(num9, rect.y, num6, rect.height);
			Widgets.Label(rect8, "MoodThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect8, "MoodThresholdColumnDesc");
			num9 += num6;
			Rect rect9 = new Rect(num9, rect.y, num7, rect.height);
			Widgets.Label(rect9, "JoyThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect9, "JoyThresholdColumnDesc");
			num9 += num7;
			Rect rect10 = new Rect(num9, rect.y, num8, rect.height);
			Widgets.Label(rect10, "TakeToInventoryColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect10, "TakeToInventoryColumnDesc");
			num9 += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600590C RID: 22796 RVA: 0x001DB688 File Offset: 0x001D9888
		private void DoEntryRow(Rect rect, DrugPolicyEntry entry)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num9 = rect.x;
			Widgets.Label(new Rect(num9, rect.y, num4, rect.height).ContractedBy(4f), entry.drug.LabelCap);
			Widgets.InfoCardButton(num9 + Text.CalcSize(entry.drug.LabelCap).x + 5f, rect.y + (rect.height - 24f) / 2f, entry.drug);
			num9 += num4;
			if (entry.drug.IsAddictiveDrug)
			{
				Widgets.Checkbox(num9, rect.y, ref entry.allowedForAddiction, 24f, false, true, null, null);
			}
			num9 += num;
			if (entry.drug.IsPleasureDrug)
			{
				Widgets.Checkbox(num9, rect.y, ref entry.allowedForJoy, 24f, false, true, null, null);
			}
			num9 += num2;
			Widgets.Checkbox(num9, rect.y, ref entry.allowScheduled, 24f, false, true, null, null);
			num9 += num3;
			if (entry.allowScheduled)
			{
				entry.daysFrequency = Widgets.FrequencyHorizontalSlider(new Rect(num9, rect.y, num5, rect.height).ContractedBy(4f), entry.daysFrequency, 0.1f, 25f, true);
				num9 += num5;
				string label;
				if (entry.onlyIfMoodBelow < 1f)
				{
					label = entry.onlyIfMoodBelow.ToStringPercent();
				}
				else
				{
					label = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfMoodBelow = Widgets.HorizontalSlider(new Rect(num9, rect.y, num6, rect.height).ContractedBy(4f), entry.onlyIfMoodBelow, 0.01f, 1f, true, label, null, null, -1f);
				num9 += num6;
				string label2;
				if (entry.onlyIfJoyBelow < 1f)
				{
					label2 = entry.onlyIfJoyBelow.ToStringPercent();
				}
				else
				{
					label2 = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfJoyBelow = Widgets.HorizontalSlider(new Rect(num9, rect.y, num7, rect.height).ContractedBy(4f), entry.onlyIfJoyBelow, 0.01f, 1f, true, label2, null, null, -1f);
				num9 += num7;
			}
			else
			{
				num9 += num5 + num6 + num7;
			}
			Widgets.TextFieldNumeric<int>(new Rect(num9, rect.y, num8, rect.height).ContractedBy(4f), ref entry.takeToInventory, ref entry.takeToInventoryTempBuffer, 0f, 15f);
			num9 += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04003027 RID: 12327
		private Vector2 scrollPosition;

		// Token: 0x04003028 RID: 12328
		private DrugPolicy selPolicy;

		// Token: 0x04003029 RID: 12329
		private const float TopAreaHeight = 40f;

		// Token: 0x0400302A RID: 12330
		private const float TopButtonHeight = 35f;

		// Token: 0x0400302B RID: 12331
		private const float TopButtonWidth = 150f;

		// Token: 0x0400302C RID: 12332
		private const float DrugEntryRowHeight = 35f;

		// Token: 0x0400302D RID: 12333
		private const float BottomButtonsAreaHeight = 50f;

		// Token: 0x0400302E RID: 12334
		private const float AddEntryButtonHeight = 35f;

		// Token: 0x0400302F RID: 12335
		private const float AddEntryButtonWidth = 150f;

		// Token: 0x04003030 RID: 12336
		private const float CellsPadding = 4f;

		// Token: 0x04003031 RID: 12337
		private static readonly Texture2D IconForAddiction = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForAddiction", true);

		// Token: 0x04003032 RID: 12338
		private static readonly Texture2D IconForJoy = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForJoy", true);

		// Token: 0x04003033 RID: 12339
		private static readonly Texture2D IconScheduled = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Scheduled", true);

		// Token: 0x04003034 RID: 12340
		private static readonly Regex ValidNameRegex = Outfit.ValidNameRegex;

		// Token: 0x04003035 RID: 12341
		private const float UsageSpacing = 12f;
	}
}
