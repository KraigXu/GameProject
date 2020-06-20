using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C3F RID: 3135
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06004ACC RID: 19148 RVA: 0x001945C8 File Offset: 0x001927C8
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06004ACD RID: 19149 RVA: 0x001945D5 File Offset: 0x001927D5
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x001943A8 File Offset: 0x001925A8
		public Tale_SinglePawn()
		{
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x00194601 File Offset: 0x00192801
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00194634 File Offset: 0x00192834
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0019464F File Offset: 0x0019284F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", Array.Empty<object>());
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x0019466C File Offset: 0x0019286C
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

		// Token: 0x06004AD3 RID: 19155 RVA: 0x0019467C File Offset: 0x0019287C
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x04002A6D RID: 10861
		public TaleData_Pawn pawnData;
	}
}
