using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B1 RID: 2481
	public class SignalManager
	{
		// Token: 0x06003B0F RID: 15119 RVA: 0x00138E30 File Offset: 0x00137030
		public void RegisterReceiver(ISignalReceiver receiver)
		{
			if (receiver == null)
			{
				Log.Error("Tried to register a null reciever.", false);
				return;
			}
			if (this.receivers.Contains(receiver))
			{
				Log.Error("Tried to register the same receiver twice: " + receiver.ToStringSafe<ISignalReceiver>(), false);
				return;
			}
			this.receivers.Add(receiver);
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x00138E7D File Offset: 0x0013707D
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x00138E8C File Offset: 0x0013708C
		public void SendSignal(Signal signal)
		{
			if (this.signalsThisFrame >= 3000)
			{
				if (this.signalsThisFrame == 3000)
				{
					Log.Error("Reached max signals per frame (" + 3000 + "). Ignoring further signals.", false);
				}
				this.signalsThisFrame++;
				return;
			}
			this.signalsThisFrame++;
			if (DebugViewSettings.logSignals)
			{
				Log.Message("Signal: tag=" + signal.tag.ToStringSafe<string>() + " args=" + signal.args.Args.ToStringSafeEnumerable(), false);
			}
			for (int i = 0; i < this.receivers.Count; i++)
			{
				try
				{
					this.receivers[i].Notify_SignalReceived(signal);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while sending signal to ",
						this.receivers[i].ToStringSafe<ISignalReceiver>(),
						": ",
						ex
					}), false);
				}
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x00138FA0 File Offset: 0x001371A0
		public void SignalManagerUpdate()
		{
			this.signalsThisFrame = 0;
		}

		// Token: 0x040022F7 RID: 8951
		private int signalsThisFrame;

		// Token: 0x040022F8 RID: 8952
		private const int MaxSignalsPerFrame = 3000;

		// Token: 0x040022F9 RID: 8953
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();
	}
}
