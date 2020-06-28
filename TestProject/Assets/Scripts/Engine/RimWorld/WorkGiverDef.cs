using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200104D RID: 4173
	public class WorkGiverDef : Def
	{
		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x060063B2 RID: 25522 RVA: 0x00229152 File Offset: 0x00227352
		public WorkGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060063B3 RID: 25523 RVA: 0x00229184 File Offset: 0x00227384
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.verb.NullOrEmpty())
			{
				yield return this.defName + " lacks a verb.";
			}
			if (this.gerund.NullOrEmpty())
			{
				yield return this.defName + " lacks a gerund.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003CA8 RID: 15528
		public Type giverClass;

		// Token: 0x04003CA9 RID: 15529
		public WorkTypeDef workType;

		// Token: 0x04003CAA RID: 15530
		public WorkTags workTags;

		// Token: 0x04003CAB RID: 15531
		public int priorityInType;

		// Token: 0x04003CAC RID: 15532
		[MustTranslate]
		public string verb;

		// Token: 0x04003CAD RID: 15533
		[MustTranslate]
		public string gerund;

		// Token: 0x04003CAE RID: 15534
		public bool scanThings = true;

		// Token: 0x04003CAF RID: 15535
		public bool scanCells;

		// Token: 0x04003CB0 RID: 15536
		public bool emergency;

		// Token: 0x04003CB1 RID: 15537
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04003CB2 RID: 15538
		public bool directOrderable = true;

		// Token: 0x04003CB3 RID: 15539
		public bool prioritizeSustains;

		// Token: 0x04003CB4 RID: 15540
		public bool nonColonistsCanDo;

		// Token: 0x04003CB5 RID: 15541
		public JobTag tagToGive = JobTag.MiscWork;

		// Token: 0x04003CB6 RID: 15542
		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		// Token: 0x04003CB7 RID: 15543
		public bool canBeDoneWhileDrafted;

		// Token: 0x04003CB8 RID: 15544
		public int autoTakeablePriorityDrafted = -1;

		// Token: 0x04003CB9 RID: 15545
		public ThingDef forceMote;

		// Token: 0x04003CBA RID: 15546
		public List<ThingDef> fixedBillGiverDefs;

		// Token: 0x04003CBB RID: 15547
		public bool billGiversAllHumanlikes;

		// Token: 0x04003CBC RID: 15548
		public bool billGiversAllHumanlikesCorpses;

		// Token: 0x04003CBD RID: 15549
		public bool billGiversAllMechanoids;

		// Token: 0x04003CBE RID: 15550
		public bool billGiversAllMechanoidsCorpses;

		// Token: 0x04003CBF RID: 15551
		public bool billGiversAllAnimals;

		// Token: 0x04003CC0 RID: 15552
		public bool billGiversAllAnimalsCorpses;

		// Token: 0x04003CC1 RID: 15553
		public bool tendToHumanlikesOnly;

		// Token: 0x04003CC2 RID: 15554
		public bool tendToAnimalsOnly;

		// Token: 0x04003CC3 RID: 15555
		public bool feedHumanlikesOnly;

		// Token: 0x04003CC4 RID: 15556
		public bool feedAnimalsOnly;

		// Token: 0x04003CC5 RID: 15557
		public ThingDef scannerDef;

		// Token: 0x04003CC6 RID: 15558
		[Unsaved(false)]
		private WorkGiver workerInt;
	}
}
