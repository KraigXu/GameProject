using System;
using System.Collections.Generic;

namespace Verse
{
	
	public abstract class Name : IExposable
	{
		
		// (get) Token: 0x0600118C RID: 4492
		public abstract string ToStringFull { get; }

		
		// (get) Token: 0x0600118D RID: 4493
		public abstract string ToStringShort { get; }

		
		// (get) Token: 0x0600118E RID: 4494
		public abstract bool IsValid { get; }

		
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

		
		public abstract bool ConfusinglySimilarTo(Name other);

		
		public abstract void ExposeData();

		
		// (get) Token: 0x06001192 RID: 4498
		public abstract bool Numerical { get; }
	}
}
