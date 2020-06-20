using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096B RID: 2411
	public class QuestPart_Choice : QuestPart
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x001303B0 File Offset: 0x0012E5B0
		public override bool PreventsAutoAccept
		{
			get
			{
				return this.choices.Count >= 2;
			}
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x001303C4 File Offset: 0x0012E5C4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalChoiceUsed)
			{
				this.choiceUsed = true;
				for (int i = 0; i < this.choices.Count; i++)
				{
					for (int j = 0; j < this.choices[i].rewards.Count; j++)
					{
						this.choices[i].rewards[j].Notify_Used();
					}
				}
			}
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x00130448 File Offset: 0x0012E648
		public override void Notify_PreCleanup()
		{
			base.Notify_PreCleanup();
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					this.choices[i].rewards[j].Notify_PreCleanup();
				}
			}
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x001304B0 File Offset: 0x0012E6B0
		public void Choose(QuestPart_Choice.Choice choice)
		{
			for (int i = this.choices.Count - 1; i >= 0; i--)
			{
				if (this.choices[i] != choice)
				{
					for (int j = 0; j < this.choices[i].questParts.Count; j++)
					{
						if (!choice.questParts.Contains(this.choices[i].questParts[j]))
						{
							this.choices[i].questParts[j].Notify_PreCleanup();
							this.choices[i].questParts[j].Cleanup();
							this.quest.RemovePart(this.choices[i].questParts[j]);
						}
					}
					this.choices.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x001305A0 File Offset: 0x0012E7A0
		public override void PreQuestAccept()
		{
			base.PreQuestAccept();
			if (this.choices.Count >= 2)
			{
				Log.Error("Tried to accept a quest but " + base.GetType().Name + " still has a choice unresolved. Auto-choosing the first option.", false);
				this.Choose(this.choices[0]);
			}
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x001305F4 File Offset: 0x0012E7F4
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					Reward_Items reward_Items;
					if ((reward_Items = (this.choices[i].rewards[j] as Reward_Items)) != null)
					{
						for (int k = 0; k < reward_Items.items.Count; k++)
						{
							if (reward_Items.items[k].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x0013069C File Offset: 0x0012E89C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalChoiceUsed, "inSignalChoiceUsed", null, false);
			Scribe_Collections.Look<QuestPart_Choice.Choice>(ref this.choices, "choices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.choiceUsed, "choiceUsed", false, false);
		}

		// Token: 0x040021AD RID: 8621
		public string inSignalChoiceUsed;

		// Token: 0x040021AE RID: 8622
		public List<QuestPart_Choice.Choice> choices = new List<QuestPart_Choice.Choice>();

		// Token: 0x040021AF RID: 8623
		public bool choiceUsed;

		// Token: 0x02001969 RID: 6505
		public class Choice : IExposable
		{
			// Token: 0x06009290 RID: 37520 RVA: 0x002DEFF3 File Offset: 0x002DD1F3
			public void ExposeData()
			{
				Scribe_Collections.Look<QuestPart>(ref this.questParts, "questParts", LookMode.Reference, Array.Empty<object>());
				Scribe_Collections.Look<Reward>(ref this.rewards, "rewards", LookMode.Deep, Array.Empty<object>());
			}

			// Token: 0x040060CE RID: 24782
			public List<QuestPart> questParts = new List<QuestPart>();

			// Token: 0x040060CF RID: 24783
			public List<Reward> rewards = new List<Reward>();
		}
	}
}
