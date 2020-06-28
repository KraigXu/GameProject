using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD3 RID: 4051
	public abstract class Reward : IExposable
	{
		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x0600613F RID: 24895 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x06006140 RID: 24896 RVA: 0x0021C995 File Offset: 0x0021AB95
		public virtual IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x06006141 RID: 24897 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float TotalMarketValue
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06006142 RID: 24898
		public abstract void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed);

		// Token: 0x06006143 RID: 24899
		public abstract IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules);

		// Token: 0x06006144 RID: 24900
		public abstract string GetDescription(RewardsGeneratorParams parms);

		// Token: 0x06006145 RID: 24901 RVA: 0x0021C99E File Offset: 0x0021AB9E
		public virtual void Notify_Used()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x0021C99E File Offset: 0x0021AB9E
		public virtual void Notify_PreCleanup()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x0021C9A7 File Offset: 0x0021ABA7
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.usedOrCleanedUp, "usedOrCleanedUp", false, false);
		}

		// Token: 0x04003B3A RID: 15162
		protected bool usedOrCleanedUp;
	}
}
