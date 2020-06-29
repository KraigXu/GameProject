using System;
using UnityEngine;

namespace Verse
{
	
	public class LogMessage
	{
		
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

		
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		
		public override string ToString()
		{
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + this.text;
			}
			return this.text;
		}

		
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		
		public string text;

		
		public LogMessageType type;

		
		public int repeats = 1;

		
		private string stackTrace;
	}
}
