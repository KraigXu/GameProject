using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000928 RID: 2344
	public class ExternalHistory : IExposable
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060037B6 RID: 14262 RVA: 0x0012AEB4 File Offset: 0x001290B4
		public string AllInformation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("storyteller: ");
				stringBuilder.Append(this.storytellerName);
				stringBuilder.Append("   userName: ");
				stringBuilder.Append(this.userName);
				stringBuilder.Append("   realWorldDate(UTC): ");
				stringBuilder.Append(this.realWorldDate);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0012AF18 File Offset: 0x00129118
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.gameVersion, "gameVersion", null, false);
			Scribe_Values.Look<string>(ref this.gameplayID, "gameplayID", null, false);
			Scribe_Values.Look<string>(ref this.userName, "userName", null, false);
			Scribe_Values.Look<string>(ref this.storytellerName, "storytellerName", null, false);
			Scribe_Values.Look<string>(ref this.realWorldDate, "realWorldDate", null, false);
			Scribe_Values.Look<string>(ref this.firstUploadDate, "firstUploadDate", null, false);
			Scribe_Values.Look<int>(ref this.firstUploadTime, "firstUploadTime", 0, false);
			Scribe_Values.Look<bool>(ref this.devMode, "devMode", false, false);
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
		}

		// Token: 0x040020F8 RID: 8440
		public string gameVersion = "?";

		// Token: 0x040020F9 RID: 8441
		public string gameplayID = "?";

		// Token: 0x040020FA RID: 8442
		public string userName = "?";

		// Token: 0x040020FB RID: 8443
		public string storytellerName = "?";

		// Token: 0x040020FC RID: 8444
		public string realWorldDate = "?";

		// Token: 0x040020FD RID: 8445
		public string firstUploadDate = "?";

		// Token: 0x040020FE RID: 8446
		public int firstUploadTime;

		// Token: 0x040020FF RID: 8447
		public bool devMode;

		// Token: 0x04002100 RID: 8448
		public History history = new History();

		// Token: 0x04002101 RID: 8449
		public static string defaultUserName = "Anonymous";
	}
}
