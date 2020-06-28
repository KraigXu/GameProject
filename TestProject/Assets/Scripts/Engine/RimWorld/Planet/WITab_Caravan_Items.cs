using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001294 RID: 4756
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x06007001 RID: 28673 RVA: 0x00270FF6 File Offset: 0x0026F1F6
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x06007002 RID: 28674 RVA: 0x00271014 File Offset: 0x0026F214
		protected override void FillTab()
		{
			this.CheckCreateSorters();
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			if (Widgets.ButtonText(new Rect(rect.x + 10f, rect.y + 10f, 200f, 27f), "AssignDrugPolicies".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_AssignCaravanDrugPolicies(base.SelCaravan));
			}
			rect.yMin += 37f;
			GUI.BeginGroup(rect.ContractedBy(10f));
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheItems();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheItems();
			});
			GUI.EndGroup();
			rect.yMin += 25f;
			GUI.BeginGroup(rect);
			this.CheckCacheItems();
			CaravanItemsTabUtility.DoRows(rect.size, this.cachedItems, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight);
			GUI.EndGroup();
		}

		// Token: 0x06007003 RID: 28675 RVA: 0x0027113E File Offset: 0x0026F33E
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x06007004 RID: 28676 RVA: 0x00271164 File Offset: 0x0026F364
		private void CheckCacheItems()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			if (list.Count != this.cachedItemsCount)
			{
				this.CacheItems();
				return;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num = Gen.HashCombineInt(num, list[i].GetHashCode());
			}
			if (num != this.cachedItemsHash)
			{
				this.CacheItems();
			}
		}

		// Token: 0x06007005 RID: 28677 RVA: 0x002711C8 File Offset: 0x0026F3C8
		private void CacheItems()
		{
			this.CheckCreateSorters();
			this.cachedItems.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			int seed = 0;
			for (int i = 0; i < list.Count; i++)
			{
				TransferableImmutable transferableImmutable = TransferableUtility.TransferableMatching<TransferableImmutable>(list[i], this.cachedItems, TransferAsOneMode.Normal);
				if (transferableImmutable == null)
				{
					transferableImmutable = new TransferableImmutable();
					this.cachedItems.Add(transferableImmutable);
				}
				transferableImmutable.things.Add(list[i]);
				seed = Gen.HashCombineInt(seed, list[i].GetHashCode());
			}
			this.cachedItems = this.cachedItems.OrderBy((TransferableImmutable tr) => tr, this.sorter1.Comparer).ThenBy((TransferableImmutable tr) => tr, this.sorter2.Comparer).ThenBy((TransferableImmutable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableImmutable>();
			this.cachedItemsCount = list.Count;
			this.cachedItemsHash = seed;
		}

		// Token: 0x06007006 RID: 28678 RVA: 0x002712F9 File Offset: 0x0026F4F9
		private void CheckCreateSorters()
		{
			if (this.sorter1 == null)
			{
				this.sorter1 = TransferableSorterDefOf.Category;
			}
			if (this.sorter2 == null)
			{
				this.sorter2 = TransferableSorterDefOf.MarketValue;
			}
		}

		// Token: 0x040044EA RID: 17642
		private Vector2 scrollPosition;

		// Token: 0x040044EB RID: 17643
		private float scrollViewHeight;

		// Token: 0x040044EC RID: 17644
		private TransferableSorterDef sorter1;

		// Token: 0x040044ED RID: 17645
		private TransferableSorterDef sorter2;

		// Token: 0x040044EE RID: 17646
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x040044EF RID: 17647
		private int cachedItemsHash;

		// Token: 0x040044F0 RID: 17648
		private int cachedItemsCount;

		// Token: 0x040044F1 RID: 17649
		private const float SortersSpace = 25f;

		// Token: 0x040044F2 RID: 17650
		private const float AssignDrugPoliciesButtonHeight = 27f;
	}
}
