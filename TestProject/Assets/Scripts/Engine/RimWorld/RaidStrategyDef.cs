using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F1 RID: 2289
	public class RaidStrategyDef : Def
	{
		// Token: 0x170009CC RID: 2508
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

		// Token: 0x04001F5B RID: 8027
		public Type workerClass;

		// Token: 0x04001F5C RID: 8028
		public SimpleCurve selectionWeightPerPointsCurve;

		// Token: 0x04001F5D RID: 8029
		public float minPawns = 1f;

		// Token: 0x04001F5E RID: 8030
		[MustTranslate]
		public string arrivalTextFriendly;

		// Token: 0x04001F5F RID: 8031
		[MustTranslate]
		public string arrivalTextEnemy;

		// Token: 0x04001F60 RID: 8032
		[MustTranslate]
		public string letterLabelEnemy;

		// Token: 0x04001F61 RID: 8033
		[MustTranslate]
		public string letterLabelFriendly;

		// Token: 0x04001F62 RID: 8034
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04001F63 RID: 8035
		public bool pawnsCanBringFood;

		// Token: 0x04001F64 RID: 8036
		public List<PawnsArrivalModeDef> arriveModes;

		// Token: 0x04001F65 RID: 8037
		private RaidStrategyWorker workerInt;
	}
}
