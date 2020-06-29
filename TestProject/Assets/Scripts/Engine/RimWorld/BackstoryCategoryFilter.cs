using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class BackstoryCategoryFilter
	{
		
		public bool Matches(PawnBio bio)
		{
			return (this.exclude == null || !(from e in this.exclude
			where bio.adulthood.spawnCategories.Contains(e) || bio.childhood.spawnCategories.Contains(e)
			select e).Any<string>()) && (this.categories == null || (from c in this.categories
			where bio.adulthood.spawnCategories.Contains(c) || bio.childhood.spawnCategories.Contains(c)
			select c).Any<string>());
		}

		
		public bool Matches(Backstory backstory)
		{
			return (this.exclude == null || !backstory.spawnCategories.Any((string e) => this.exclude.Contains(e))) && (this.categories == null || backstory.spawnCategories.Any((string c) => this.categories.Contains(c)));
		}

		
		public List<string> categories;

		
		public List<string> exclude;

		
		public float commonality = 1f;
	}
}
