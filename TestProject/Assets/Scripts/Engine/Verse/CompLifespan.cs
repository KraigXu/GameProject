using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000321 RID: 801
	public class CompLifespan : ThingComp
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x000859C8 File Offset: 0x00083BC8
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x000859D5 File Offset: 0x00083BD5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000859EF File Offset: 0x00083BEF
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00085A1E File Offset: 0x00083C1E
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00085A54 File Offset: 0x00083C54
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod(true, false, true, true);
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x04000EA8 RID: 3752
		public int age = -1;
	}
}
