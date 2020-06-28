using System;

namespace Verse
{
	// Token: 0x02000237 RID: 567
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0005B922 File Offset: 0x00059B22
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.", false);
				return;
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0005B950 File Offset: 0x00059B50
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}
	}
}
