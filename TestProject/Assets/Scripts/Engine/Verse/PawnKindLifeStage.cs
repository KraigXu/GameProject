using System;

namespace Verse
{
	// Token: 0x020000D1 RID: 209
	public class PawnKindLifeStage
	{
		// Token: 0x060005CA RID: 1482 RVA: 0x0001C29B File Offset: 0x0001A49B
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001C2C4 File Offset: 0x0001A4C4
		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleDessicatedBodyGraphicData != null && this.femaleDessicatedBodyGraphicData.graphicClass == null)
			{
				this.femaleDessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}

		// Token: 0x040004D1 RID: 1233
		[MustTranslate]
		public string label;

		// Token: 0x040004D2 RID: 1234
		[MustTranslate]
		public string labelPlural;

		// Token: 0x040004D3 RID: 1235
		[MustTranslate]
		public string labelMale;

		// Token: 0x040004D4 RID: 1236
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x040004D5 RID: 1237
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040004D6 RID: 1238
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x040004D7 RID: 1239
		[Unsaved(false)]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel;

		// Token: 0x040004D8 RID: 1240
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale;

		// Token: 0x040004D9 RID: 1241
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelFemale;

		// Token: 0x040004DA RID: 1242
		public GraphicData bodyGraphicData;

		// Token: 0x040004DB RID: 1243
		public GraphicData femaleGraphicData;

		// Token: 0x040004DC RID: 1244
		public GraphicData dessicatedBodyGraphicData;

		// Token: 0x040004DD RID: 1245
		public GraphicData femaleDessicatedBodyGraphicData;

		// Token: 0x040004DE RID: 1246
		public BodyPartToDrop butcherBodyPart;
	}
}
