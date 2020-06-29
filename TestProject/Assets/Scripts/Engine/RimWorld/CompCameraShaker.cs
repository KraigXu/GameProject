using System;
using Verse;

namespace RimWorld
{
	
	public class CompCameraShaker : ThingComp
	{
		
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x001B1C5B File Offset: 0x001AFE5B
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		
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
