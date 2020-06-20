using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010E6 RID: 4326
	public class SymbolStack
	{
		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x060065B3 RID: 26035 RVA: 0x0023A308 File Offset: 0x00238508
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x060065B4 RID: 26036 RVA: 0x0023A318 File Offset: 0x00238518
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x0023A328 File Offset: 0x00238528
		public void Push(string symbol, ResolveParams resolveParams, string customNameForPath = null)
		{
			string text = BaseGen.CurrentSymbolPath;
			if (!text.NullOrEmpty())
			{
				text += "_";
			}
			text += (customNameForPath ?? symbol);
			SymbolStack.Element item = default(SymbolStack.Element);
			item.symbol = symbol;
			item.resolveParams = resolveParams;
			item.symbolPath = text;
			this.stack.Push(item);
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x0023A388 File Offset: 0x00238588
		public void Push(string symbol, CellRect rect, string customNameForPath = null)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			}, customNameForPath);
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x0023A3B0 File Offset: 0x002385B0
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams, null);
			}
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x0023A3D8 File Offset: 0x002385D8
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect, null);
			}
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x0023A3FE File Offset: 0x002385FE
		public SymbolStack.Element Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x0023A40B File Offset: 0x0023860B
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x04003DDD RID: 15837
		private Stack<SymbolStack.Element> stack = new Stack<SymbolStack.Element>();

		// Token: 0x02001F15 RID: 7957
		public struct Element
		{
			// Token: 0x040074C8 RID: 29896
			public string symbol;

			// Token: 0x040074C9 RID: 29897
			public ResolveParams resolveParams;

			// Token: 0x040074CA RID: 29898
			public string symbolPath;
		}
	}
}
