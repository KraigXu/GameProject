using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class SpecificApparelRequirement
	{
		
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001C391 File Offset: 0x0001A591
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001C399 File Offset: 0x0001A599
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001C3A1 File Offset: 0x0001A5A1
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001C3A9 File Offset: 0x0001A5A9
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001C3B1 File Offset: 0x0001A5B1
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001C3B9 File Offset: 0x0001A5B9
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		private string requiredTag;

		
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		
		private ThingDef stuff;

		
		private BodyPartGroupDef bodyPartGroup;

		
		private ApparelLayerDef apparelLayer;

		
		private Color color;

		
		public struct TagChance
		{
			
			public string tag;

			
			public float chance;
		}
	}
}
