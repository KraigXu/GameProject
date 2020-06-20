using System;

namespace Verse
{
	// Token: 0x02000409 RID: 1033
	public struct AcceptanceReport
	{
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x000BE42D File Offset: 0x000BC62D
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x000BE435 File Offset: 0x000BC635
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x000BE440 File Offset: 0x000BC640
		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000BE464 File Offset: 0x000BC664
		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000BE486 File Offset: 0x000BC686
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000BE496 File Offset: 0x000BC696
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000BE4A6 File Offset: 0x000BC6A6
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000BE4AE File Offset: 0x000BC6AE
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x040012D4 RID: 4820
		private string reasonTextInt;

		// Token: 0x040012D5 RID: 4821
		private bool acceptedInt;
	}
}
