using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000994 RID: 2452
	public class QuestPart_ThingsProduced : QuestPartActivable
	{
		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06003A05 RID: 14853 RVA: 0x00133FE4 File Offset: 0x001321E4
		public override string DescriptionPart
		{
			get
			{
				return string.Concat(new object[]
				{
					"ThingsProduced".Translate().CapitalizeFirst() + ": ",
					this.produced,
					" / ",
					this.count
				});
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003A06 RID: 14854 RVA: 0x00134044 File Offset: 0x00132244
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.<>n__0())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.def, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00134054 File Offset: 0x00132254
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.produced = 0;
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x00134064 File Offset: 0x00132264
		public override void Notify_ThingsProduced(Pawn actor, List<Thing> things)
		{
			base.Notify_ThingsProduced(actor, things);
			if (base.State == QuestPartState.Enabled)
			{
				for (int i = 0; i < things.Count; i++)
				{
					Thing innerIfMinified = things[i].GetInnerIfMinified();
					if (innerIfMinified.def == this.def && innerIfMinified.Stuff == this.stuff)
					{
						this.produced += things[i].stackCount;
					}
				}
				if (this.produced >= this.count)
				{
					this.produced = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x001340F8 File Offset: 0x001322F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.produced, "produced", 0, false);
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x0013414F File Offset: 0x0013234F
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThingDefOf.MealSimple;
			this.count = 10;
		}

		// Token: 0x04002237 RID: 8759
		public ThingDef def;

		// Token: 0x04002238 RID: 8760
		public ThingDef stuff;

		// Token: 0x04002239 RID: 8761
		public int count;

		// Token: 0x0400223A RID: 8762
		private int produced;
	}
}
