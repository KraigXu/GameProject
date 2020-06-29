using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_PlantsHarvested : QuestPartActivable
	{
		
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

		
		// (get) Token: 0x060039B5 RID: 14773 RVA: 0x00132C34 File Offset: 0x00130E34
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.n__0())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.plant, -1);
				yield break;
				yield break;
			}
		}

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.harvested = 0;
		}

		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plant, "plant");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.harvested, "harvested", 0, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.plant = ThingDefOf.RawPotatoes;
			this.count = 10;
		}

		
		public ThingDef plant;

		
		public int count;

		
		private int harvested;
	}
}
