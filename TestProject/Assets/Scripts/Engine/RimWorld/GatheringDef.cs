using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class GatheringDef : Def
	{
		
		
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		
		
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
