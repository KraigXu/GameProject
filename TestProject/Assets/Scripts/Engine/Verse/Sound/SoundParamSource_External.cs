using System;

namespace Verse.Sound
{
	// Token: 0x020004DD RID: 1245
	public class SoundParamSource_External : SoundParamSource
	{
		// Token: 0x17000728 RID: 1832
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

		// Token: 0x06002452 RID: 9298 RVA: 0x000D8DF0 File Offset: 0x000D6FF0
		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.TryGetValue(this.inParamName, out result))
			{
				return result;
			}
			return this.defaultValue;
		}

		// Token: 0x040015F9 RID: 5625
		[Description("The name of the independent parameter that the game will change to drive this relationship.\n\nThis must exactly match a string that the code will use to modify this sound. If the code doesn't reference this, it will have no effect.\n\nOn the graph, this is the X axis.")]
		public string inParamName = "";

		// Token: 0x040015FA RID: 5626
		[Description("If the code has never set this parameter on a sustainer, it will use this value.")]
		private float defaultValue = 1f;
	}
}
