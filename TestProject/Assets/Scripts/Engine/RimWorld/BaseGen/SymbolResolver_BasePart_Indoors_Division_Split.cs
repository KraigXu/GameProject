﻿using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Indoors_Division_Split : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.rect.Width >= 9 || rp.rect.Height >= 9);
		}

		
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width < 9 && rp.rect.Height < 9)
			{
				Log.Warning("Too small rect. params=" + rp, false);
				return;
			}
			if ((Rand.Bool && rp.rect.Height >= 9) || rp.rect.Width < 9)
			{
				int num = Rand.RangeInclusive(4, rp.rect.Height - 5);
				ResolveParams resolveParams = rp;
				resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num + 1);
				BaseGenCore.symbolStack.Push("basePart_indoors", resolveParams, null);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num, rp.rect.Width, rp.rect.Height - num);
				BaseGenCore.symbolStack.Push("basePart_indoors", resolveParams2, null);
				return;
			}
			int num2 = Rand.RangeInclusive(4, rp.rect.Width - 5);
			ResolveParams resolveParams3 = rp;
			resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num2 + 1, rp.rect.Height);
			BaseGenCore.symbolStack.Push("basePart_indoors", resolveParams3, null);
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = new CellRect(rp.rect.minX + num2, rp.rect.minZ, rp.rect.Width - num2, rp.rect.Height);
			BaseGenCore.symbolStack.Push("basePart_indoors", resolveParams4, null);
		}

		
		private const int MinLengthAfterSplit = 5;

		
		private const int MinWidthOrHeight = 9;
	}
}
