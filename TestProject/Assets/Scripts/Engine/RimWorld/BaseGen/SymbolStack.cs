using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolStack
	{
		
		
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		
		
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		
		public void Push(string symbol, ResolveParams resolveParams, string customNameForPath = null)
		{
			string text = BaseGenCore.CurrentSymbolPath;
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

		
		public void Push(string symbol, CellRect rect, string customNameForPath = null)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			}, customNameForPath);
		}

		
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams, null);
			}
		}

		
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect, null);
			}
		}

		
		public SymbolStack.Element Pop()
		{
			return this.stack.Pop();
		}

		
		public void Clear()
		{
			this.stack.Clear();
		}

		
		private Stack<SymbolStack.Element> stack = new Stack<SymbolStack.Element>();

		
		public struct Element
		{
			
			public string symbol;

			
			public ResolveParams resolveParams;

			
			public string symbolPath;
		}
	}
}
