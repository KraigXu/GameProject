using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096D RID: 2413
	public class QuestPart_DescriptionPart : QuestPartActivable
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x0600392C RID: 14636 RVA: 0x00130855 File Offset: 0x0012EA55
		public override string DescriptionPart
		{
			get
			{
				return this.resolvedDescriptionPart;
			}
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0013085D File Offset: 0x0012EA5D
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedDescriptionPart = receivedArgs.GetFormattedText(this.descriptionPart);
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x00130883 File Offset: 0x0012EA83
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.descriptionPart, "descriptionPart", null, false);
			Scribe_Values.Look<string>(ref this.resolvedDescriptionPart, "resolvedDescriptionPart", null, false);
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x001308AF File Offset: 0x0012EAAF
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.descriptionPart = "Debug description part.";
		}

		// Token: 0x040021B3 RID: 8627
		public string descriptionPart;

		// Token: 0x040021B4 RID: 8628
		private string resolvedDescriptionPart;
	}
}
