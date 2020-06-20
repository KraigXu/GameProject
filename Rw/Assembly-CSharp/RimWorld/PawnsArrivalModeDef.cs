using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EC RID: 2284
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060036AE RID: 13998 RVA: 0x00127EF6 File Offset: 0x001260F6
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

		// Token: 0x04001F3B RID: 7995
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x04001F3C RID: 7996
		public SimpleCurve selectionWeightCurve;

		// Token: 0x04001F3D RID: 7997
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04001F3E RID: 7998
		public TechLevel minTechLevel;

		// Token: 0x04001F3F RID: 7999
		public bool forQuickMilitaryAid;

		// Token: 0x04001F40 RID: 8000
		public bool walkIn;

		// Token: 0x04001F41 RID: 8001
		[MustTranslate]
		public string textEnemy;

		// Token: 0x04001F42 RID: 8002
		[MustTranslate]
		public string textFriendly;

		// Token: 0x04001F43 RID: 8003
		[MustTranslate]
		public string textWillArrive;

		// Token: 0x04001F44 RID: 8004
		[Unsaved(false)]
		private PawnsArrivalModeWorker workerInt;
	}
}
