using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SetItemStashContents : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<IEnumerable<ThingDef>>("itemStashThings", this.GetContents(slate), false);
		}

		
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
				//List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = value2.GetEnumerator();
				//{
				//	while (enumerator2.MoveNext())
				//	{
				//		QuestNode_SetItemStashContents.c__DisplayClass6_0 c__DisplayClass6_ = new QuestNode_SetItemStashContents.c__DisplayClass6_0();
				//		c__DisplayClass6_.c = enumerator2.Current;
				//		try
				//		{
				//			int amt = Mathf.Max(c__DisplayClass6_.c.amount.RandomInRange, 1);
				//			int num;
				//			for (int i = 0; i < amt; i = num + 1)
				//			{
				//				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				//				Func<ThingDef, bool> predicate;
				//				if ((predicate = c__DisplayClass6_.9__0) == null)
				//				{
				//					predicate = (c__DisplayClass6_. ((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(c__DisplayClass6_.c.category) && (c__DisplayClass6_.c.allowDuplicates || !QuestNode_SetItemStashContents.tmpItems.Contains(x))));
				//				}
				//				ThingDef thingDef2;
				//				if (allDefs.Where(predicate).TryRandomElement(out thingDef2))
				//				{
				//					QuestNode_SetItemStashContents.tmpItems.Add(thingDef2);
				//					yield return thingDef2;
				//				}
				//				num = i;
				//			}
				//		}
				//		finally
				//		{
				//			QuestNode_SetItemStashContents.tmpItems.Clear();
				//		}
				//		c__DisplayClass6_ = null;
				//	}
				//}
				List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = default(List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator);
			}
			yield break;
			yield break;
		}

		
		public SlateRef<IEnumerable<ThingDef>> items;

		
		public SlateRef<List<QuestNode_SetItemStashContents.ThingCategoryCount>> categories;

		
		private static List<ThingDef> tmpItems = new List<ThingDef>();

		
		public class ThingCategoryCount
		{
			
			public ThingCategoryDef category;

			
			public IntRange amount;

			
			public bool allowDuplicates = true;
		}
	}
}
