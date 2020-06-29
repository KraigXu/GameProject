using System;

namespace RimWorld
{
	
	public interface IStoreSettingsParent
	{
		
		// (get) Token: 0x06004C09 RID: 19465
		bool StorageTabVisible { get; }

		
		StorageSettings GetStoreSettings();

		
		StorageSettings GetParentStoreSettings();
	}
}
