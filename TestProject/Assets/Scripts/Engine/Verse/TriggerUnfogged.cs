using System;
using RimWorld;

namespace Verse
{
	
	public class TriggerUnfogged : Thing
	{
		
		public override void Tick()
		{
			if (base.Spawned)
			{
				if (base.Position.Fogged(base.Map))
				{
					this.everFogged = true;
					return;
				}
				if (this.everFogged)
				{
					this.Activated();
					return;
				}
				this.Destroy(DestroyMode.Vanish);
			}
		}

		
		public void Activated()
		{
			//Find.SignalManager.SendSignal(new Signal(this.signalTag));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.everFogged, "everFogged", false, false);
		}

		
		public string signalTag;

		
		private bool everFogged;
	}
}
