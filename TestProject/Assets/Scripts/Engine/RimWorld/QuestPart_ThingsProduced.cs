﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_ThingsProduced : QuestPartActivable
	{
		
		
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

		
		
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{


				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.def, -1);
				yield break;
				yield break;
			}
		}

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.produced = 0;
		}

		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.produced, "produced", 0, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThingDefOf.MealSimple;
			this.count = 10;
		}

		
		public ThingDef def;

		
		public ThingDef stuff;

		
		public int count;

		
		private int produced;
	}
}
