using System;

namespace Verse
{
	
	public abstract class Listing_Lines : Listing
	{
		
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		
		public float lineHeight = 20f;
	}
}
