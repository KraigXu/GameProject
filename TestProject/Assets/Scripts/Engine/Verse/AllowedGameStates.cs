﻿using System;

namespace Verse
{
	
	[Flags]
	public enum AllowedGameStates
	{
		
		Invalid = 0,
		
		Entry = 1,
		
		Playing = 2,
		
		WorldRenderedNow = 4,
		
		IsCurrentlyOnMap = 8,
		
		HasGameCondition = 16,
		
		PlayingOnMap = 10,
		
		PlayingOnWorld = 6
	}
}
