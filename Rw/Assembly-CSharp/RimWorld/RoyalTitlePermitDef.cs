using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104A RID: 4170
	public class RoyalTitlePermitDef : Def
	{
		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x06006395 RID: 25493 RVA: 0x00228D59 File Offset: 0x00226F59
		public int CooldownTicks
		{
			get
			{
				return (int)(this.cooldownDays * 60000f);
			}
		}

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x06006396 RID: 25494 RVA: 0x00228D68 File Offset: 0x00226F68
		public RoyalTitlePermitWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (RoyalTitlePermitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x00228D9A File Offset: 0x00226F9A
		public override IEnumerable<string> ConfigErrors()
		{
			if (!typeof(RoyalTitlePermitWorker).IsAssignableFrom(this.workerClass))
			{
				yield return string.Format("RoyalTitlePermitDef {0} has worker class {1}, which is not deriving from {2}", this.defName, this.workerClass, typeof(RoyalTitlePermitWorker).FullName);
			}
			if (this.royalAid != null && this.royalAid.pawnKindDef != null && this.royalAid.pawnCount <= 0)
			{
				yield return "pawnCount should be greater than 0, if you specify pawnKindDef";
			}
			yield break;
		}

		// Token: 0x04003C9C RID: 15516
		public Type workerClass = typeof(RoyalTitlePermitWorker);

		// Token: 0x04003C9D RID: 15517
		public RoyalAid royalAid;

		// Token: 0x04003C9E RID: 15518
		public float cooldownDays;

		// Token: 0x04003C9F RID: 15519
		private RoyalTitlePermitWorker worker;
	}
}
