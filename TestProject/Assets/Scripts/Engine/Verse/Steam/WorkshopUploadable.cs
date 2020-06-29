using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	
	public interface WorkshopUploadable
	{
		
		// (get) Token: 0x060022A4 RID: 8868
		IEnumerable<System.Version> SupportedVersions { get; }

		
		bool CanToUploadToWorkshop();

		
		void PrepareForWorkshopUpload();

		
		PublishedFileId_t GetPublishedFileId();

		
		void SetPublishedFileId(PublishedFileId_t pfid);

		
		string GetWorkshopName();

		
		string GetWorkshopDescription();

		
		string GetWorkshopPreviewImagePath();

		
		IList<string> GetWorkshopTags();

		
		DirectoryInfo GetWorkshopUploadDirectory();

		
		WorkshopItemHook GetWorkshopItemHook();
	}
}
