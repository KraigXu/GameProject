using System;
using UnityEngine;

namespace Verse
{
	
	public class ColorForStuff
	{
		
		
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		
		
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		private ThingDef stuff;

		
		private Color color = Color.white;
	}
}
