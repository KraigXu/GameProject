using System;
using UnityEngine;

namespace Verse
{
	
	public class Command_Action : Command
	{
		
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		
		public Action action;
	}
}
