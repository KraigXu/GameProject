using System;

namespace RimWorld
{
	// Token: 0x02000C65 RID: 3173
	public interface IStoreSettingsParent
	{
		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06004C09 RID: 19465
		bool StorageTabVisible { get; }

		// Token: 0x06004C0A RID: 19466
		StorageSettings GetStoreSettings();

		// Token: 0x06004C0B RID: 19467
		StorageSettings GetParentStoreSettings();
	}
}
