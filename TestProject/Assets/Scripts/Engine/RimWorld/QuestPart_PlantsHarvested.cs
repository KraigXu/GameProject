using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000985 RID: 2437
	public class QuestPart_PlantsHarvested : QuestPartActivable
	{
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x00132BD4 File Offset: 0x00130DD4
		public override string DescriptionPart
		{
			get
			{
				return string.Concat(new object[]
				{
					"PlantsHarvested".Translate().CapitalizeFirst() + ": ",
					this.harvested,
					" / ",
					this.count
				});
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060039B5 RID: 14773 RVA: 0x00132C34 File Offset: 0x00130E34
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.<>n__0())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.plant, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00132C44 File Offset: 0x00130E44
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.harvested = 0;
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x00132C54 File Offset: 0x00130E54
		public override void Notify_PlantHarvested(Pawn actor, Thing harvested)
		{
			base.Notify_PlantHarvested(actor, harvested);
			if (base.State == QuestPartState.Enabled && harvested.def == this.plant)
			{
				this.harvested += harvested.stackCount;
				if (this.harvested >= this.count)
				{
					this.harvested = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x00132CB3 File Offset: 0x00130EB3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plant, "plant");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.harvested, "harvested", 0, false);
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00132CEF File Offset: 0x00130EEF
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.plant = ThingDefOf.RawPotatoes;
			this.count = 10;
		}

		// Token: 0x04002208 RID: 8712
		public ThingDef plant;

		// Token: 0x04002209 RID: 8713
		public int count;

		// Token: 0x0400220A RID: 8714
		private int harvested;
	}
}
