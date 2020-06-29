using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class LetterDef : Def
	{
		
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001BC09 File Offset: 0x00019E09
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		
		public Type letterClass = typeof(StandardLetter);

		
		public Color color = Color.white;

		
		public Color flashColor = Color.white;

		
		public float flashInterval = 90f;

		
		public bool bounce;

		
		public SoundDef arriveSound;

		
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		
		public bool forcedSlowdown;

		
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
