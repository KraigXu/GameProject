using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000C9 RID: 201
	public class MentalStateDef : Def
	{
		// Token: 0x17000100 RID: 256
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001BDDE File Offset: 0x00019FDE
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x17000102 RID: 258
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

		// Token: 0x060005AF RID: 1455 RVA: 0x0001BE31 File Offset: 0x0001A031
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x04000455 RID: 1109
		public Type stateClass = typeof(MentalState);

		// Token: 0x04000456 RID: 1110
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x04000457 RID: 1111
		public MentalStateCategory category;

		// Token: 0x04000458 RID: 1112
		public bool prisonersCanDo = true;

		// Token: 0x04000459 RID: 1113
		public bool unspawnedCanDo;

		// Token: 0x0400045A RID: 1114
		public bool colonistsOnly;

		// Token: 0x0400045B RID: 1115
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x0400045C RID: 1116
		public bool blockNormalThoughts;

		// Token: 0x0400045D RID: 1117
		public List<InteractionDef> blockInteractionInitiationExcept;

		// Token: 0x0400045E RID: 1118
		public List<InteractionDef> blockInteractionRecipientExcept;

		// Token: 0x0400045F RID: 1119
		public bool blockRandomInteraction;

		// Token: 0x04000460 RID: 1120
		public EffecterDef stateEffecter;

		// Token: 0x04000461 RID: 1121
		public TaleDef tale;

		// Token: 0x04000462 RID: 1122
		public bool allowBeatfire;

		// Token: 0x04000463 RID: 1123
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x04000464 RID: 1124
		public bool ignoreDrugPolicy;

		// Token: 0x04000465 RID: 1125
		public float recoveryMtbDays = 1f;

		// Token: 0x04000466 RID: 1126
		public int minTicksBeforeRecovery = 500;

		// Token: 0x04000467 RID: 1127
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x04000468 RID: 1128
		public bool recoverFromSleep;

		// Token: 0x04000469 RID: 1129
		public bool recoverFromDowned = true;

		// Token: 0x0400046A RID: 1130
		public bool recoverFromCollapsingExhausted = true;

		// Token: 0x0400046B RID: 1131
		public ThoughtDef moodRecoveryThought;

		// Token: 0x0400046C RID: 1132
		[MustTranslate]
		public string beginLetter;

		// Token: 0x0400046D RID: 1133
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x0400046E RID: 1134
		public LetterDef beginLetterDef;

		// Token: 0x0400046F RID: 1135
		public Color nameColor = Color.green;

		// Token: 0x04000470 RID: 1136
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x04000471 RID: 1137
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04000472 RID: 1138
		public bool escapingPrisonersIgnore;

		// Token: 0x04000473 RID: 1139
		private MentalStateWorker workerInt;
	}
}
