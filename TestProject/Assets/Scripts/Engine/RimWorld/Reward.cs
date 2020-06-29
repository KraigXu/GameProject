using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public abstract class Reward : IExposable
	{
		
		// (get) Token: 0x0600613F RID: 24895 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06006140 RID: 24896 RVA: 0x0021C995 File Offset: 0x0021AB95
		public virtual IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield break;
			}
		}

		
		// (get) Token: 0x06006141 RID: 24897 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float TotalMarketValue
		{
			get
			{
				return 0f;
			}
		}

		
		public abstract void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed);

		
		public abstract IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules);

		
		public abstract string GetDescription(RewardsGeneratorParams parms);

		
		public virtual void Notify_Used()
		{
			this.usedOrCleanedUp = true;
		}

		
		public virtual void Notify_PreCleanup()
		{
			this.usedOrCleanedUp = true;
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.usedOrCleanedUp, "usedOrCleanedUp", false, false);
		}

		
		protected bool usedOrCleanedUp;
	}
}
