using System;

namespace Verse
{
	
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		
		protected string attribute;
	}
}
