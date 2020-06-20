using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C3B RID: 3131
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x00194343 File Offset: 0x00192543
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06004AB6 RID: 19126 RVA: 0x00194350 File Offset: 0x00192550
		public override string ShortSummary
		{
			get
			{
				string text = this.def.LabelCap + ": " + this.firstPawnData.name;
				if (this.secondPawnData != null)
				{
					text = text + ", " + this.secondPawnData.name;
				}
				return text;
			}
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x001943A8 File Offset: 0x001925A8
		public Tale_DoublePawn()
		{
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x001943B0 File Offset: 0x001925B0
		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x001943FD File Offset: 0x001925FD
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x00194430 File Offset: 0x00192630
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", Array.Empty<object>());
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", Array.Empty<object>());
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x00194462 File Offset: 0x00192662
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set", false);
			}
			foreach (Rule rule in this.firstPawnData.GetRules("ANYPAWN"))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.firstPawnData.GetRules(this.def.firstPawnSymbol))
			{
				yield return rule2;
			}
			enumerator = null;
			if (this.secondPawnData != null)
			{
				foreach (Rule rule3 in this.firstPawnData.GetRules("ANYPAWN"))
				{
					yield return rule3;
				}
				enumerator = null;
				foreach (Rule rule4 in this.secondPawnData.GetRules(this.def.secondPawnSymbol))
				{
					yield return rule4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x00194472 File Offset: 0x00192672
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x04002A69 RID: 10857
		public TaleData_Pawn firstPawnData;

		// Token: 0x04002A6A RID: 10858
		public TaleData_Pawn secondPawnData;
	}
}
