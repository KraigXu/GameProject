using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RaidStrategyDef : Def
	{
		
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x0012813F File Offset: 0x0012633F
		public RaidStrategyWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidStrategyWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public Type workerClass;

		
		public SimpleCurve selectionWeightPerPointsCurve;

		
		public float minPawns = 1f;

		
		[MustTranslate]
		public string arrivalTextFriendly;

		
		[MustTranslate]
		public string arrivalTextEnemy;

		
		[MustTranslate]
		public string letterLabelEnemy;

		
		[MustTranslate]
		public string letterLabelFriendly;

		
		public SimpleCurve pointsFactorCurve;

		
		public bool pawnsCanBringFood;

		
		public List<PawnsArrivalModeDef> arriveModes;

		
		private RaidStrategyWorker workerInt;
	}
}
