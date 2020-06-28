using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000290 RID: 656
	public abstract class Name : IExposable
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600118C RID: 4492
		public abstract string ToStringFull { get; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600118D RID: 4493
		public abstract string ToStringShort { get; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600118E RID: 4494
		public abstract bool IsValid { get; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x000635B4 File Offset: 0x000617B4
		public bool UsedThisGame
		{
			get
			{
				using (IEnumerator<Name> enumerator = NameUseChecker.AllPawnsNamesEverUsed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ConfusinglySimilarTo(this))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06001190 RID: 4496
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x06001191 RID: 4497
		public abstract void ExposeData();

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001192 RID: 4498
		public abstract bool Numerical { get; }
	}
}
