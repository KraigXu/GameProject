using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020008D9 RID: 2265
	public class InteractionDef : Def
	{
		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003651 RID: 13905 RVA: 0x00126979 File Offset: 0x00124B79
		public InteractionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InteractionWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.interaction = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003652 RID: 13906 RVA: 0x001269AB File Offset: 0x00124BAB
		public Texture2D Symbol
		{
			get
			{
				if (this.symbolTex == null)
				{
					this.symbolTex = ContentFinder<Texture2D>.Get(this.symbol, true);
				}
				return this.symbolTex;
			}
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x001269D3 File Offset: 0x00124BD3
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x04001EAD RID: 7853
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x04001EAE RID: 7854
		public ThingDef interactionMote;

		// Token: 0x04001EAF RID: 7855
		public float socialFightBaseChance;

		// Token: 0x04001EB0 RID: 7856
		public ThoughtDef initiatorThought;

		// Token: 0x04001EB1 RID: 7857
		public SkillDef initiatorXpGainSkill;

		// Token: 0x04001EB2 RID: 7858
		public int initiatorXpGainAmount;

		// Token: 0x04001EB3 RID: 7859
		public ThoughtDef recipientThought;

		// Token: 0x04001EB4 RID: 7860
		public SkillDef recipientXpGainSkill;

		// Token: 0x04001EB5 RID: 7861
		public int recipientXpGainAmount;

		// Token: 0x04001EB6 RID: 7862
		[NoTranslate]
		private string symbol;

		// Token: 0x04001EB7 RID: 7863
		public RulePack logRulesInitiator;

		// Token: 0x04001EB8 RID: 7864
		public RulePack logRulesRecipient;

		// Token: 0x04001EB9 RID: 7865
		[Unsaved(false)]
		private InteractionWorker workerInt;

		// Token: 0x04001EBA RID: 7866
		[Unsaved(false)]
		private Texture2D symbolTex;
	}
}
