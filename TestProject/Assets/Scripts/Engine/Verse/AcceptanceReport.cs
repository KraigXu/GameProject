using System;

namespace Verse
{
	
	public struct AcceptanceReport
	{
		
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x000BE42D File Offset: 0x000BC62D
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x000BE435 File Offset: 0x000BC635
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		
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

		
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		
		private string reasonTextInt;

		
		private bool acceptedInt;
	}
}
