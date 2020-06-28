using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6D RID: 2925
	public class BackstoryCategoryFilter
	{
		// Token: 0x0600446B RID: 17515 RVA: 0x00171F74 File Offset: 0x00170174
		public bool Matches(PawnBio bio)
		{
			return (this.exclude == null || !(from e in this.exclude
			where bio.adulthood.spawnCategories.Contains(e) || bio.childhood.spawnCategories.Contains(e)
			select e).Any<string>()) && (this.categories == null || (from c in this.categories
			where bio.adulthood.spawnCategories.Contains(c) || bio.childhood.spawnCategories.Contains(c)
			select c).Any<string>());
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x00171FDC File Offset: 0x001701DC
		public bool Matches(Backstory backstory)
		{
			return (this.exclude == null || !backstory.spawnCategories.Any((string e) => this.exclude.Contains(e))) && (this.categories == null || backstory.spawnCategories.Any((string c) => this.categories.Contains(c)));
		}

		// Token: 0x040026F4 RID: 9972
		public List<string> categories;

		// Token: 0x040026F5 RID: 9973
		public List<string> exclude;

		// Token: 0x040026F6 RID: 9974
		public float commonality = 1f;
	}
}
