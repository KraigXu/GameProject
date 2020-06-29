using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class LogMessageQueue
	{
		
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000F5D4 File Offset: 0x0000D7D4
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		
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

		
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		
		public int maxMessages = 200;

		
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		
		private LogMessage lastMessage;
	}
}
