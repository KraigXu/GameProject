using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000322 RID: 802
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x00085AD2 File Offset: 0x00083CD2
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00085ADF File Offset: 0x00083CDF
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00085AF9 File Offset: 0x00083CF9
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00085B04 File Offset: 0x00083D04
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}
