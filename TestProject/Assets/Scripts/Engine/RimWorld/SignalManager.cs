using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class SignalManager
	{
		
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

		
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		
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

		
		public void SignalManagerUpdate()
		{
			this.signalsThisFrame = 0;
		}

		
		private int signalsThisFrame;

		
		private const int MaxSignalsPerFrame = 3000;

		
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();
	}
}
