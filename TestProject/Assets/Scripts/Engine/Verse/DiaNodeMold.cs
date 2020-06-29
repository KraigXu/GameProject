﻿using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class DiaNodeMold
	{
		
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}

		
		public string name = "Unnamed";

		
		public bool unique;

		
		public List<string> texts = new List<string>();

		
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		
		[Unsaved(false)]
		public bool isRoot = true;

		
		[Unsaved(false)]
		public bool used;

		
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
