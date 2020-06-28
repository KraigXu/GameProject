using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000F02 RID: 3842
	public static class PriceTypeUtlity
	{
		// Token: 0x06005E3A RID: 24122 RVA: 0x00209FB4 File Offset: 0x002081B4
		public static float PriceMultiplier(this PriceType pType)
		{
			switch (pType)
			{
			case PriceType.VeryCheap:
				return 0.4f;
			case PriceType.Cheap:
				return 0.7f;
			case PriceType.Normal:
				return 1f;
			case PriceType.Expensive:
				return 2f;
			case PriceType.Exorbitant:
				return 5f;
			default:
				return -1f;
			}
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x0020A004 File Offset: 0x00208204
		public static PriceType ClosestPriceType(float priceFactor)
		{
			float num = 99999f;
			PriceType priceType = PriceType.Undefined;
			foreach (object obj in Enum.GetValues(typeof(PriceType)))
			{
				PriceType priceType2 = (PriceType)obj;
				float num2 = Mathf.Abs(priceFactor - priceType2.PriceMultiplier());
				if (num2 < num)
				{
					num = num2;
					priceType = priceType2;
				}
			}
			if (priceType == PriceType.Undefined)
			{
				priceType = PriceType.VeryCheap;
			}
			return priceType;
		}
	}
}
