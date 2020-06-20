using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F04 RID: 3844
	public class TransferableComparer_Category : TransferableComparer
	{
		// Token: 0x06005E40 RID: 24128 RVA: 0x0020A5FC File Offset: 0x002087FC
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x06005E41 RID: 24129 RVA: 0x0020A610 File Offset: 0x00208810
		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			if (lhsTh.category != rhsTh.category)
			{
				return lhsTh.category.CompareTo(rhsTh.category);
			}
			float num = TransferableUIUtility.DefaultListOrderPriority(lhsTh);
			float num2 = TransferableUIUtility.DefaultListOrderPriority(rhsTh);
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			int num3 = 0;
			if (!lhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				num3 = (int)lhsTh.thingCategories[0].index;
			}
			int value = 0;
			if (!rhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				value = (int)rhsTh.thingCategories[0].index;
			}
			return num3.CompareTo(value);
		}
	}
}
