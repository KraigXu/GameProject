using System;

namespace RimWorld
{
	// Token: 0x02000BF7 RID: 3063
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x0018C9A9 File Offset: 0x0018ABA9
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x060048EC RID: 18668 RVA: 0x0018C9B1 File Offset: 0x0018ABB1
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x060048ED RID: 18669 RVA: 0x0018C9B9 File Offset: 0x0018ABB9
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x17000CFB RID: 3323
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

		// Token: 0x17000CFC RID: 3324
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

		// Token: 0x060048F0 RID: 18672 RVA: 0x0018CA02 File Offset: 0x0018AC02
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			if (value)
			{
				return FloatMenuAcceptanceReport.WasAccepted;
			}
			return FloatMenuAcceptanceReport.WasRejected;
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0018CA12 File Offset: 0x0018AC12
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0018CA1C File Offset: 0x0018AC1C
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x0018CA44 File Offset: 0x0018AC44
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x040029C3 RID: 10691
		private string failMessageInt;

		// Token: 0x040029C4 RID: 10692
		private string failReasonInt;

		// Token: 0x040029C5 RID: 10693
		private bool acceptedInt;
	}
}
