using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class InteractionDef : Def
	{
		
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

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		
		private Type workerClass = typeof(InteractionWorker);

		
		public ThingDef interactionMote;

		
		public float socialFightBaseChance;

		
		public ThoughtDef initiatorThought;

		
		public SkillDef initiatorXpGainSkill;

		
		public int initiatorXpGainAmount;

		
		public ThoughtDef recipientThought;

		
		public SkillDef recipientXpGainSkill;

		
		public int recipientXpGainAmount;

		
		[NoTranslate]
		private string symbol;

		
		public RulePack logRulesInitiator;

		
		public RulePack logRulesRecipient;

		
		[Unsaved(false)]
		private InteractionWorker workerInt;

		
		[Unsaved(false)]
		private Texture2D symbolTex;
	}
}
