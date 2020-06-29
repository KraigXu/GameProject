using System;

namespace Verse
{
	
	public struct DebugMenuOption
	{
		
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		
		public DebugMenuOptionMode mode;

		
		public string label;

		
		public Action method;
	}
}
