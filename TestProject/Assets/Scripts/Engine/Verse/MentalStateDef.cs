﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	
	public class MentalStateDef : Def
	{
		
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001BD9E File Offset: 0x00019F9E
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001BDDE File Offset: 0x00019FDE
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001BDEC File Offset: 0x00019FEC
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		
		public Type stateClass = typeof(MentalState);

		
		public Type workerClass = typeof(MentalStateWorker);

		
		public MentalStateCategory category;

		
		public bool prisonersCanDo = true;

		
		public bool unspawnedCanDo;

		
		public bool colonistsOnly;

		
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		
		public bool blockNormalThoughts;

		
		public List<InteractionDef> blockInteractionInitiationExcept;

		
		public List<InteractionDef> blockInteractionRecipientExcept;

		
		public bool blockRandomInteraction;

		
		public EffecterDef stateEffecter;

		
		public TaleDef tale;

		
		public bool allowBeatfire;

		
		public DrugCategory drugCategory = DrugCategory.Any;

		
		public bool ignoreDrugPolicy;

		
		public float recoveryMtbDays = 1f;

		
		public int minTicksBeforeRecovery = 500;

		
		public int maxTicksBeforeRecovery = 99999999;

		
		public bool recoverFromSleep;

		
		public bool recoverFromDowned = true;

		
		public bool recoverFromCollapsingExhausted = true;

		
		public ThoughtDef moodRecoveryThought;

		
		[MustTranslate]
		public string beginLetter;

		
		[MustTranslate]
		public string beginLetterLabel;

		
		public LetterDef beginLetterDef;

		
		public Color nameColor = Color.green;

		
		[MustTranslate]
		public string recoveryMessage;

		
		[MustTranslate]
		public string baseInspectLine;

		
		public bool escapingPrisonersIgnore;

		
		private MentalStateWorker workerInt;
	}
}
