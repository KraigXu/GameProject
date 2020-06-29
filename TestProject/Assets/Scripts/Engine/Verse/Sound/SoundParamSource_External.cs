using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_External : SoundParamSource
	{
		
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x000D8DCD File Offset: 0x000D6FCD
		public override string Label
		{
			get
			{
				if (this.inParamName == "")
				{
					return "Undefined external";
				}
				return this.inParamName;
			}
		}

		
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out result))
			{
				return result;
			}
			return this.defaultValue;
		}

		
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}
