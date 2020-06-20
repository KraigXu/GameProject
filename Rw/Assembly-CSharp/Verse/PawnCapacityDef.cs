using System;

namespace Verse
{
	// Token: 0x020000CC RID: 204
	public class PawnCapacityDef : Def
	{
		// Token: 0x17000103 RID: 259
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

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001BF12 File Offset: 0x0001A112
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001BF30 File Offset: 0x0001A130
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

		// Token: 0x04000476 RID: 1142
		public int listOrder;

		// Token: 0x04000477 RID: 1143
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x04000478 RID: 1144
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x04000479 RID: 1145
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x0400047A RID: 1146
		public bool showOnHumanlikes = true;

		// Token: 0x0400047B RID: 1147
		public bool showOnAnimals = true;

		// Token: 0x0400047C RID: 1148
		public bool showOnMechanoids = true;

		// Token: 0x0400047D RID: 1149
		public bool lethalFlesh;

		// Token: 0x0400047E RID: 1150
		public bool lethalMechanoids;

		// Token: 0x0400047F RID: 1151
		public float minForCapable;

		// Token: 0x04000480 RID: 1152
		public float minValue;

		// Token: 0x04000481 RID: 1153
		public bool zeroIfCannotBeAwake;

		// Token: 0x04000482 RID: 1154
		public bool showOnCaravanHealthTab;

		// Token: 0x04000483 RID: 1155
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
