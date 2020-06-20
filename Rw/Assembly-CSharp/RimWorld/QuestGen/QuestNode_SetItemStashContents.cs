using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200118C RID: 4492
	public class QuestNode_SetItemStashContents : QuestNode
	{
		// Token: 0x06006827 RID: 26663 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x002465C4 File Offset: 0x002447C4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<IEnumerable<ThingDef>>("itemStashThings", this.GetContents(slate), false);
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x002465EA File Offset: 0x002447EA
		private IEnumerable<ThingDef> GetContents(Slate slate)
		{
			IEnumerable<ThingDef> value = this.items.GetValue(slate);
			if (value != null)
			{
				foreach (ThingDef thingDef in value)
				{
					yield return thingDef;
				}
				IEnumerator<ThingDef> enumerator = null;
			}
			List<QuestNode_SetItemStashContents.ThingCategoryCount> value2 = this.categories.GetValue(slate);
			if (value2 != null)
			{
				using (List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = value2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						QuestNode_SetItemStashContents.<>c__DisplayClass6_0 <>c__DisplayClass6_ = new QuestNode_SetItemStashContents.<>c__DisplayClass6_0();
						<>c__DisplayClass6_.c = enumerator2.Current;
						try
						{
							int amt = Mathf.Max(<>c__DisplayClass6_.c.amount.RandomInRange, 1);
							int num;
							for (int i = 0; i < amt; i = num + 1)
							{
								IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
								Func<ThingDef, bool> predicate;
								if ((predicate = <>c__DisplayClass6_.<>9__0) == null)
								{
									predicate = (<>c__DisplayClass6_.<>9__0 = ((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(<>c__DisplayClass6_.c.category) && (<>c__DisplayClass6_.c.allowDuplicates || !QuestNode_SetItemStashContents.tmpItems.Contains(x))));
								}
								ThingDef thingDef2;
								if (allDefs.Where(predicate).TryRandomElement(out thingDef2))
								{
									QuestNode_SetItemStashContents.tmpItems.Add(thingDef2);
									yield return thingDef2;
								}
								num = i;
							}
						}
						finally
						{
							QuestNode_SetItemStashContents.tmpItems.Clear();
						}
						<>c__DisplayClass6_ = null;
					}
				}
				List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = default(List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04004072 RID: 16498
		public SlateRef<IEnumerable<ThingDef>> items;

		// Token: 0x04004073 RID: 16499
		public SlateRef<List<QuestNode_SetItemStashContents.ThingCategoryCount>> categories;

		// Token: 0x04004074 RID: 16500
		private static List<ThingDef> tmpItems = new List<ThingDef>();

		// Token: 0x02001F56 RID: 8022
		public class ThingCategoryCount
		{
			// Token: 0x04007562 RID: 30050
			public ThingCategoryDef category;

			// Token: 0x04007563 RID: 30051
			public IntRange amount;

			// Token: 0x04007564 RID: 30052
			public bool allowDuplicates = true;
		}
	}
}
