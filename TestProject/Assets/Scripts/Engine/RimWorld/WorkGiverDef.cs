using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiverDef : Def
	{
		
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

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public Type giverClass;

		
		public WorkTypeDef workType;

		
		public WorkTags workTags;

		
		public int priorityInType;

		
		[MustTranslate]
		public string verb;

		
		[MustTranslate]
		public string gerund;

		
		public bool scanThings = true;

		
		public bool scanCells;

		
		public bool emergency;

		
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		
		public bool directOrderable = true;

		
		public bool prioritizeSustains;

		
		public bool nonColonistsCanDo;

		
		public JobTag tagToGive = JobTag.MiscWork;

		
		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		
		public bool canBeDoneWhileDrafted;

		
		public int autoTakeablePriorityDrafted = -1;

		
		public ThingDef forceMote;

		
		public List<ThingDef> fixedBillGiverDefs;

		
		public bool billGiversAllHumanlikes;

		
		public bool billGiversAllHumanlikesCorpses;

		
		public bool billGiversAllMechanoids;

		
		public bool billGiversAllMechanoidsCorpses;

		
		public bool billGiversAllAnimals;

		
		public bool billGiversAllAnimalsCorpses;

		
		public bool tendToHumanlikesOnly;

		
		public bool tendToAnimalsOnly;

		
		public bool feedHumanlikesOnly;

		
		public bool feedAnimalsOnly;

		
		public ThingDef scannerDef;

		
		[Unsaved(false)]
		private WorkGiver workerInt;
	}
}
