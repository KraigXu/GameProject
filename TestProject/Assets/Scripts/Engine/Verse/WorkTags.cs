using System;

namespace Verse
{
	
	[Flags]
	public enum WorkTags
	{
		
		None = 0,
		
		ManualDumb = 2,
		
		ManualSkilled = 4,
		
		Violent = 8,
		
		Caring = 16,
		
		Social = 32,
		
		Commoner = 64,
		
		Intellectual = 128,
		
		Animals = 256,
		
		Artistic = 512,
		
		Crafting = 1024,
		
		Cooking = 2048,
		
		Firefighting = 4096,
		
		Cleaning = 8192,
		
		Hauling = 16384,
		
		PlantWork = 32768,
		
		Mining = 65536,
		
		Hunting = 131072,
		
		AllWork = 262144
	}
}
