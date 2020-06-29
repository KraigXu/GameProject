using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoyalTitlePermitDef : Def
	{
		
		// (get) Token: 0x06006395 RID: 25493 RVA: 0x00228D59 File Offset: 0x00226F59
		public int CooldownTicks
		{
			get
			{
				return (int)(this.cooldownDays * 60000f);
			}
		}

		
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

		
		public Type workerClass = typeof(RoyalTitlePermitWorker);

		
		public RoyalAid royalAid;

		
		public float cooldownDays;

		
		private RoyalTitlePermitWorker worker;
	}
}
