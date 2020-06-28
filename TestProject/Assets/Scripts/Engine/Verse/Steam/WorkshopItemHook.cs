using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000492 RID: 1170
	public class WorkshopItemHook
	{
		// Token: 0x170006D1 RID: 1745
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

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x000D2F5D File Offset: 0x000D115D
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x000D2F6A File Offset: 0x000D116A
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x000D2F77 File Offset: 0x000D1177
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000D2F84 File Offset: 0x000D1184
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000D2F91 File Offset: 0x000D1191
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000D2F9E File Offset: 0x000D119E
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				return this.owner.SupportedVersions;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000D2FAB File Offset: 0x000D11AB
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000D2FE5 File Offset: 0x000D11E5
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000D3017 File Offset: 0x000D1217
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000D3024 File Offset: 0x000D1224
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999u);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000D3066 File Offset: 0x000D1266
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x04001527 RID: 5415
		private WorkshopUploadable owner;

		// Token: 0x04001528 RID: 5416
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x04001529 RID: 5417
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}
