using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A34 RID: 2612
	public class DamageWatcher : IExposable
	{
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x00145F24 File Offset: 0x00144124
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00145F2C File Offset: 0x0014412C
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.everDamage += amount;
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x00145F4A File Offset: 0x0014414A
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
		}

		// Token: 0x04002408 RID: 9224
		private float everDamage;
	}
}
