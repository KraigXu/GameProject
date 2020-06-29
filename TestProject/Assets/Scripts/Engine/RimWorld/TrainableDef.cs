using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class TrainableDef : Def
	{
		
		
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		
		public bool MatchesTag(string tag)
		{
			if (tag == this.defName)
			{
				return true;
			}
			for (int i = 0; i < this.tags.Count; i++)
			{
				if (this.tags[i] == tag)
				{
					return true;
				}
			}
			return false;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.difficulty < 0f)
			{
				yield return "difficulty not set";
			}
			yield break;
			yield break;
		}

		
		public float difficulty = -1f;

		
		public float minBodySize;

		
		public List<TrainableDef> prerequisites;

		
		[NoTranslate]
		public List<string> tags = new List<string>();

		
		public bool defaultTrainable;

		
		public TrainabilityDef requiredTrainability;

		
		public int steps = 1;

		
		public float listPriority;

		
		[NoTranslate]
		public string icon;

		
		[Unsaved(false)]
		public int indent;

		
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
