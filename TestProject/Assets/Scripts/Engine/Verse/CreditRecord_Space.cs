using System;
using UnityEngine;

namespace Verse
{
	
	public class CreditRecord_Space : CreditsEntry
	{
		
		public CreditRecord_Space()
		{
		}

		
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		
		public override void Draw(Rect rect)
		{
		}

		
		private float height = 10f;
	}
}
