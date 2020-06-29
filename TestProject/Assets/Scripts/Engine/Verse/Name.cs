using System;
using System.Collections.Generic;

namespace Verse
{
	
	public abstract class Name : IExposable
	{
		
		
		public abstract string ToStringFull { get; }

		
		
		public abstract string ToStringShort { get; }

		
		
		public abstract bool IsValid { get; }

		
		
		public bool UsedThisGame
		{
			get
			{
				IEnumerator<Name> enumerator = NameUseChecker.AllPawnsNamesEverUsed.GetEnumerator();
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

		
		
		public abstract bool Numerical { get; }
	}
}
