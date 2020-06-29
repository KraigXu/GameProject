using System;
using Verse;

namespace RimWorld
{
	
	public struct ApparelGraphicRecord
	{
		
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		
		public Graphic graphic;

		
		public Apparel sourceApparel;
	}
}
