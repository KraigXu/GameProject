using System;

namespace Verse
{
	
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		
		public bool sendLetterWhenDiscovered;

		
		public string discoverLetterLabel;

		
		public string discoverLetterText;

		
		public MessageTypeDef messageType;

		
		public LetterDef letterType;
	}
}
