using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class DiaNode
	{
		
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				List<DiaOptionMold>.Enumerator enumerator = this.def.optionList.GetEnumerator();
				{
					while (enumerator.MoveNext())
					{
						DiaOptionMold diaOptionMold = enumerator.Current;
						this.options.Add(new DiaOption(diaOptionMold));
					}
					return;
				}
			}
			this.options.Add(new DiaOption("OK".Translate()));
		}

		
		public void PreClose()
		{
		}

		
		public TaggedString text;

		
		public List<DiaOption> options = new List<DiaOption>();

		
		protected DiaNodeMold def;
	}
}
