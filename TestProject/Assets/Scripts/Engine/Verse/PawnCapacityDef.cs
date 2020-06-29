using System;

namespace Verse
{
	
	public class PawnCapacityDef : Def
	{
		
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001BEEC File Offset: 0x0001A0EC
		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		
		public string GetLabelFor(bool isFlesh, bool isHumanlike)
		{
			if (isHumanlike)
			{
				return this.label;
			}
			if (isFlesh)
			{
				if (!this.labelAnimals.NullOrEmpty())
				{
					return this.labelAnimals;
				}
				return this.label;
			}
			else
			{
				if (!this.labelMechanoids.NullOrEmpty())
				{
					return this.labelMechanoids;
				}
				return this.label;
			}
		}

		
		public int listOrder;

		
		public Type workerClass = typeof(PawnCapacityWorker);

		
		[MustTranslate]
		public string labelMechanoids = "";

		
		[MustTranslate]
		public string labelAnimals = "";

		
		public bool showOnHumanlikes = true;

		
		public bool showOnAnimals = true;

		
		public bool showOnMechanoids = true;

		
		public bool lethalFlesh;

		
		public bool lethalMechanoids;

		
		public float minForCapable;

		
		public float minValue;

		
		public bool zeroIfCannotBeAwake;

		
		public bool showOnCaravanHealthTab;

		
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
