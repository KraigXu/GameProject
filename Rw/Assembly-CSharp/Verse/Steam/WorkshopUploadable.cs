using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000491 RID: 1169
	public interface WorkshopUploadable
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060022A4 RID: 8868
		IEnumerable<System.Version> SupportedVersions { get; }

		// Token: 0x060022A5 RID: 8869
		bool CanToUploadToWorkshop();

		// Token: 0x060022A6 RID: 8870
		void PrepareForWorkshopUpload();

		// Token: 0x060022A7 RID: 8871
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x060022A8 RID: 8872
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x060022A9 RID: 8873
		string GetWorkshopName();

		// Token: 0x060022AA RID: 8874
		string GetWorkshopDescription();

		// Token: 0x060022AB RID: 8875
		string GetWorkshopPreviewImagePath();

		// Token: 0x060022AC RID: 8876
		IList<string> GetWorkshopTags();

		// Token: 0x060022AD RID: 8877
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x060022AE RID: 8878
		WorkshopItemHook GetWorkshopItemHook();
	}
}
