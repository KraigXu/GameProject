using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_EmptyStateGraphic : CompProperties
	{
		
		public CompProperties_EmptyStateGraphic()
		{
			this.compClass = typeof(CompEmptyStateGraphic);
		}

		
		public GraphicData graphicData;
	}
}
