using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Choice : QuestPart
	{
		
		
		public override bool PreventsAutoAccept
		{
			get
			{
				return this.choices.Count >= 2;
			}
		}

		
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

		
		public override void PreQuestAccept()
		{
			base.PreQuestAccept();
			if (this.choices.Count >= 2)
			{
				Log.Error("Tried to accept a quest but " + base.GetType().Name + " still has a choice unresolved. Auto-choosing the first option.", false);
				this.Choose(this.choices[0]);
			}
		}

		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalChoiceUsed, "inSignalChoiceUsed", null, false);
			Scribe_Collections.Look<QuestPart_Choice.Choice>(ref this.choices, "choices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.choiceUsed, "choiceUsed", false, false);
		}

		
		public string inSignalChoiceUsed;

		
		public List<QuestPart_Choice.Choice> choices = new List<QuestPart_Choice.Choice>();

		
		public bool choiceUsed;

		
		public class Choice : IExposable
		{
			
			public void ExposeData()
			{
				Scribe_Collections.Look<QuestPart>(ref this.questParts, "questParts", LookMode.Reference, Array.Empty<object>());
				Scribe_Collections.Look<Reward>(ref this.rewards, "rewards", LookMode.Deep, Array.Empty<object>());
			}

			
			public List<QuestPart> questParts = new List<QuestPart>();

			
			public List<Reward> rewards = new List<Reward>();
		}
	}
}
