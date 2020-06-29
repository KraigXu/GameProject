using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class SpecificApparelRequirement
	{
		
		
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		
		
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		
		
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		
		
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		
		
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		
		
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
