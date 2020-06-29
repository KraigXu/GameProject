﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Tale_SinglePawn : Tale
	{
		
		
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		
		
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		
		public Tale_SinglePawn()
		{
		}

		
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", Array.Empty<object>());
		}

		
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in this.pawnData.GetRules("ANYPAWN"))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.pawnData.GetRules("PAWN"))
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
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}

		
		public TaleData_Pawn pawnData;
	}
}
