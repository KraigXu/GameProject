using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000033 RID: 51
	public class LogMessageQueue
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000F5D4 File Offset: 0x0000D7D4
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000F5DC File Offset: 0x0000D7DC
		public void Enqueue(LogMessage msg)
		{
			if (this.lastMessage != null && msg.CanCombineWith(this.lastMessage))
			{
				this.lastMessage.repeats++;
				return;
			}
			this.lastMessage = msg;
			this.messages.Enqueue(msg);
			if (this.messages.Count > this.maxMessages)
			{
				EditWindow_Log.Notify_MessageDequeued(this.messages.Dequeue());
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000F649 File Offset: 0x0000D849
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x040000A5 RID: 165
		public int maxMessages = 200;

		// Token: 0x040000A6 RID: 166
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x040000A7 RID: 167
		private LogMessage lastMessage;
	}
}
