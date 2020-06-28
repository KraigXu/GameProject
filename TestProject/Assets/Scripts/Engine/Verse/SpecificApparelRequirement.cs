using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D2 RID: 210
	public class SpecificApparelRequirement
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001C391 File Offset: 0x0001A591
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001C399 File Offset: 0x0001A599
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001C3A1 File Offset: 0x0001A5A1
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001C3A9 File Offset: 0x0001A5A9
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001C3B1 File Offset: 0x0001A5B1
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001C3B9 File Offset: 0x0001A5B9
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x040004DF RID: 1247
		private string requiredTag;

		// Token: 0x040004E0 RID: 1248
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		// Token: 0x040004E1 RID: 1249
		private ThingDef stuff;

		// Token: 0x040004E2 RID: 1250
		private BodyPartGroupDef bodyPartGroup;

		// Token: 0x040004E3 RID: 1251
		private ApparelLayerDef apparelLayer;

		// Token: 0x040004E4 RID: 1252
		private Color color;

		// Token: 0x0200134F RID: 4943
		public struct TagChance
		{
			// Token: 0x04004934 RID: 18740
			public string tag;

			// Token: 0x04004935 RID: 18741
			public float chance;
		}
	}
}
