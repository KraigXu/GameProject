using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class SongDef : Def
	{
		
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
			}
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		
		[NoTranslate]
		public string clipPath;

		
		public float volume = 1f;

		
		public bool playOnMap = true;

		
		public float commonality = 1f;

		
		public bool tense;

		
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		
		public List<Season> allowedSeasons;

		
		public RoyalTitleDef minRoyalTitle;

		
		[Unsaved(false)]
		public AudioClip clip;
	}
}
