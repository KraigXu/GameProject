using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class GatheringDef : Def
	{
		
		// (get) Token: 0x0600360C RID: 13836 RVA: 0x0012581B File Offset: 0x00123A1B
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x0012582A File Offset: 0x00123A2A
		public GatheringWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (GatheringWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		
		public bool CanExecute(Map map, Pawn organizer = null, bool ignoreGameConditions = false)
		{
			return (ignoreGameConditions || GatheringsUtility.AcceptableGameConditionsToStartGathering(map, this)) && this.Worker.CanExecute(map, organizer);
		}

		
		public Type workerClass = typeof(GatheringWorker);

		
		public DutyDef duty;

		
		public float randomSelectionWeight;

		
		public bool respectTimetable = true;

		
		public List<ThingDef> gatherSpotDefs;

		
		[MustTranslate]
		public string letterTitle;

		
		[MustTranslate]
		public string letterText;

		
		[MustTranslate]
		public string calledOffMessage;

		
		[MustTranslate]
		public string finishedMessage;

		
		public List<RoyalTitleDef> requiredTitleAny = new List<RoyalTitleDef>();

		
		private GatheringWorker worker;
	}
}
