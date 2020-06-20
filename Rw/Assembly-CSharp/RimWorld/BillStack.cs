using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000616 RID: 1558
	public class BillStack : IExposable
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000F7B6C File Offset: 0x000F5D6C
		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000F7B74 File Offset: 0x000F5D74
		public IEnumerator<Bill> GetEnumerator()
		{
			return this.bills.GetEnumerator();
		}

		// Token: 0x17000809 RID: 2057
		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000F7B94 File Offset: 0x000F5D94
		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000F7BA4 File Offset: 0x000F5DA4
		public Bill FirstShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return this.bills[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000F7BE4 File Offset: 0x000F5DE4
		public bool AnyShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000F7C18 File Offset: 0x000F5E18
		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000F7C32 File Offset: 0x000F5E32
		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000F7C47 File Offset: 0x000F5E47
		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000F7C5D File Offset: 0x000F5E5D
		public void Clear()
		{
			this.bills.Clear();
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000F7C6C File Offset: 0x000F5E6C
		public void Reorder(Bill bill, int offset)
		{
			int num = this.bills.IndexOf(bill);
			num += offset;
			if (num >= 0)
			{
				this.bills.Remove(bill);
				this.bills.Insert(num, bill);
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000F7CA8 File Offset: 0x000F5EA8
		public void RemoveIncompletableBills()
		{
			for (int i = this.bills.Count - 1; i >= 0; i--)
			{
				if (!this.bills[i].CompletableEver)
				{
					this.bills.Remove(this.bills[i]);
				}
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000F7CF8 File Offset: 0x000F5EF8
		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000F7D08 File Offset: 0x000F5F08
		public void ExposeData()
		{
			Scribe_Collections.Look<Bill>(ref this.bills, "bills", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.bills.RemoveAll((Bill x) => x == null) != 0)
				{
					Log.Error("Some bills were null after loading.", false);
				}
				if (this.bills.RemoveAll((Bill x) => x.recipe == null) != 0)
				{
					Log.Error("Some bills had null recipe after loading.", false);
				}
				for (int i = 0; i < this.bills.Count; i++)
				{
					this.bills[i].billStack = this;
				}
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000F7DCC File Offset: 0x000F5FCC
		public Bill DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			if (this.Count < 15)
			{
				Rect rect2 = new Rect(0f, 0f, 150f, 29f);
				if (Widgets.ButtonText(rect2, "AddBill".Translate(), true, true, true))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker()));
				}
				UIHighlighter.HighlightOpportunity(rect2, "AddBill");
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.Count; i++)
			{
				Bill bill = this.bills[i];
				Rect rect3 = bill.DoInterface(0f, num, viewRect.width, i);
				if (!bill.DeletedOrDereferenced && Mouse.IsOver(rect3))
				{
					result = bill;
				}
				num += rect3.height + 6f;
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num + 60f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			return result;
		}

		// Token: 0x04001960 RID: 6496
		[Unsaved(false)]
		public IBillGiver billGiver;

		// Token: 0x04001961 RID: 6497
		private List<Bill> bills = new List<Bill>();

		// Token: 0x04001962 RID: 6498
		public const int MaxCount = 15;

		// Token: 0x04001963 RID: 6499
		private const float TopAreaHeight = 35f;

		// Token: 0x04001964 RID: 6500
		private const float BillInterfaceSpacing = 6f;

		// Token: 0x04001965 RID: 6501
		private const float ExtraViewHeight = 60f;
	}
}
