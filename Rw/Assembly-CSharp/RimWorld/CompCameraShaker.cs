using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF9 RID: 3321
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x001B1C5B File Offset: 0x001AFE5B
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x001B1C68 File Offset: 0x001AFE68
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned && this.parent.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.SetMinShake(this.Props.mag);
			}
		}
	}
}
