using System;

namespace RimWorld
{
	
	public struct FloatMenuAcceptanceReport
	{
		
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x0018C9A9 File Offset: 0x0018ABA9
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		
		// (get) Token: 0x060048EC RID: 18668 RVA: 0x0018C9B1 File Offset: 0x0018ABB1
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		
		// (get) Token: 0x060048ED RID: 18669 RVA: 0x0018C9B9 File Offset: 0x0018ABB9
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		
		// (get) Token: 0x060048EE RID: 18670 RVA: 0x0018C9C4 File Offset: 0x0018ABC4
		public static FloatMenuAcceptanceReport WasAccepted
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = true
				};
			}
		}

		
		// (get) Token: 0x060048EF RID: 18671 RVA: 0x0018C9E4 File Offset: 0x0018ABE4
		public static FloatMenuAcceptanceReport WasRejected
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = false
				};
			}
		}

		
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			if (value)
			{
				return FloatMenuAcceptanceReport.WasAccepted;
			}
			return FloatMenuAcceptanceReport.WasRejected;
		}

		
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		
		private string failMessageInt;

		
		private string failReasonInt;

		
		private bool acceptedInt;
	}
}
