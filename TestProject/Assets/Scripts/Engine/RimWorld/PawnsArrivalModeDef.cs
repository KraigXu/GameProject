using System;
using Verse;

namespace RimWorld
{
	
	public class PawnsArrivalModeDef : Def
	{
		
		
		public PawnsArrivalModeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnsArrivalModeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		
		public SimpleCurve selectionWeightCurve;

		
		public SimpleCurve pointsFactorCurve;

		
		public TechLevel minTechLevel;

		
		public bool forQuickMilitaryAid;

		
		public bool walkIn;

		
		[MustTranslate]
		public string textEnemy;

		
		[MustTranslate]
		public string textFriendly;

		
		[MustTranslate]
		public string textWillArrive;

		
		[Unsaved(false)]
		private PawnsArrivalModeWorker workerInt;
	}
}
