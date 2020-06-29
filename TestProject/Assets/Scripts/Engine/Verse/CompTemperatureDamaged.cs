using System;
using RimWorld;

namespace Verse
{
	
	public class CompTemperatureDamaged : ThingComp
	{
		
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x00085AD2 File Offset: 0x00083CD2
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}
