using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9B RID: 3483
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x060054AB RID: 21675 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x060054AC RID: 21676 RVA: 0x001C397A File Offset: 0x001C1B7A
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x001C3988 File Offset: 0x001C1B88
		public virtual void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap)
			{
				if (this.Props.doCameraShake && usedBy.Spawned)
				{
					Find.CameraDriver.shaker.DoShake(1f);
				}
				if (this.Props.moteOnUsed != null)
				{
					MoteMaker.MakeAttachedOverlay(usedBy, this.Props.moteOnUsed, Vector3.zero, this.Props.moteOnUsedScale, -1f);
				}
			}
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x001C39FF File Offset: 0x001C1BFF
		public virtual TaggedString ConfirmMessage(Pawn p)
		{
			return null;
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001C3A07 File Offset: 0x001C1C07
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}

		// Token: 0x04002E81 RID: 11905
		private const float CameraShakeMag = 1f;
	}
}
