using System;

namespace Verse
{
	
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		
		public bool allowLoading;
	}
}
