using System;

namespace Verse
{
	// Token: 0x020000E4 RID: 228
	public class RoomStatScoreStage
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x0001DBB8 File Offset: 0x0001BDB8
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04000555 RID: 1365
		public float minScore = float.MinValue;

		// Token: 0x04000556 RID: 1366
		public string label;

		// Token: 0x04000557 RID: 1367
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
