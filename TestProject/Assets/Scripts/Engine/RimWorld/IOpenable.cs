using System;

namespace RimWorld
{
	
	public interface IOpenable
	{
		
		// (get) Token: 0x06004B4F RID: 19279
		bool CanOpen { get; }

		
		void Open();
	}
}
