using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000032 RID: 50
	public class LogMessage
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000F4E0 File Offset: 0x0000D6E0
		public Color Color
		{
			get
			{
				switch (this.type)
				{
				case LogMessageType.Message:
					return Color.white;
				case LogMessageType.Warning:
					return Color.yellow;
				case LogMessageType.Error:
					return Color.red;
				default:
					return Color.white;
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000F51F File Offset: 0x0000D71F
		public string StackTrace
		{
			get
			{
				if (this.stackTrace != null)
				{
					return this.stackTrace;
				}
				return "No stack trace.";
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F535 File Offset: 0x0000D735
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000F559 File Offset: 0x0000D759
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F57D File Offset: 0x0000D77D
		public override string ToString()
		{
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + this.text;
			}
			return this.text;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x040000A1 RID: 161
		public string text;

		// Token: 0x040000A2 RID: 162
		public LogMessageType type;

		// Token: 0x040000A3 RID: 163
		public int repeats = 1;

		// Token: 0x040000A4 RID: 164
		private string stackTrace;
	}
}
