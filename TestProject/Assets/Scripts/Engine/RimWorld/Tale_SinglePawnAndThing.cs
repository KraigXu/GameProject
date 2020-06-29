using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		
		public Tale_SinglePawnAndThing()
		{
		}

		
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", Array.Empty<object>());
		}

		
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{

			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.thingData.GetRules("THING"))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		
		public TaleData_Thing thingData;
	}
}
