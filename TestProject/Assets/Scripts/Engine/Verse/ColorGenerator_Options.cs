﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class ColorGenerator_Options : ColorGenerator
	{
		
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00017AFC File Offset: 0x00015CFC
		public override Color ExemplaryColor
		{
			get
			{
				ColorOption colorOption = null;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (colorOption == null || this.options[i].weight > colorOption.weight)
					{
						colorOption = this.options[i];
					}
				}
				if (colorOption == null)
				{
					return Color.white;
				}
				if (colorOption.only.a >= 0f)
				{
					return colorOption.only;
				}
				return new Color((colorOption.min.r + colorOption.max.r) / 2f, (colorOption.min.g + colorOption.max.g) / 2f, (colorOption.min.b + colorOption.max.b) / 2f, (colorOption.min.a + colorOption.max.a) / 2f);
			}
		}

		
		public override Color NewRandomizedColor()
		{
			return this.options.RandomElementByWeight((ColorOption pi) => pi.weight).RandomizedColor();
		}

		
		public List<ColorOption> options = new List<ColorOption>();
	}
}
