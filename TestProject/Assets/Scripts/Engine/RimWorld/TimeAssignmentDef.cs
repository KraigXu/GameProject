using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class TimeAssignmentDef : Def
	{
		
		
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}

		
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightNotSelectedTag = "TimeAssignmentButton-" + this.defName + "-NotSelected";
		}

		
		public Color color;

		
		public bool allowRest = true;

		
		public bool allowJoy = true;

		
		[Unsaved(false)]
		public string cachedHighlightNotSelectedTag;

		
		private Texture2D colorTextureInt;
	}
}
