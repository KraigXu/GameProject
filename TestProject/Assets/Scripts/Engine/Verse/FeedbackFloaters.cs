using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200038A RID: 906
	public class FeedbackFloaters
	{
		// Token: 0x06001AC3 RID: 6851 RVA: 0x000A4968 File Offset: 0x000A2B68
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000A4978 File Offset: 0x000A2B78
		public void FeedbackUpdate()
		{
			for (int i = this.feeders.Count - 1; i >= 0; i--)
			{
				this.feeders[i].Update();
				if (this.feeders[i].TimeLeft <= 0f)
				{
					this.feeders.Remove(this.feeders[i]);
				}
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000A49E0 File Offset: 0x000A2BE0
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x04000FD2 RID: 4050
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
