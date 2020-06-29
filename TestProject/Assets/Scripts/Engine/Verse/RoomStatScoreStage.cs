using System;

namespace Verse
{
	
	public class RoomStatScoreStage
	{
		
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		
		public float minScore = float.MinValue;

		
		public string label;

		
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
