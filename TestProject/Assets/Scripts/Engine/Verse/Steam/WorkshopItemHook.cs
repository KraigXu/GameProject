using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	
	public class WorkshopItemHook
	{
		
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x000D2F42 File Offset: 0x000D1142
		// (set) Token: 0x060022B0 RID: 8880 RVA: 0x000D2F4F File Offset: 0x000D114F
		public PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.owner.GetPublishedFileId();
			}
			set
			{
				this.owner.SetPublishedFileId(value);
			}
		}

		
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x000D2F5D File Offset: 0x000D115D
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x000D2F6A File Offset: 0x000D116A
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x000D2F77 File Offset: 0x000D1177
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000D2F84 File Offset: 0x000D1184
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000D2F91 File Offset: 0x000D1191
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000D2F9E File Offset: 0x000D119E
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				return this.owner.SupportedVersions;
			}
		}

		
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000D2FAB File Offset: 0x000D11AB
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999u);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		
		private WorkshopUploadable owner;

		
		private CSteamID steamAuthor = CSteamID.Nil;

		
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}
