using System;
using UnityEngine;

namespace Verse
{
	
	public class ColorGenerator_Single : ColorGenerator
	{
		
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		
		public Color color;
	}
}
