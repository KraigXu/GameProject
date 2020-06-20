using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000809 RID: 2057
	public class Thought_BondedAnimalMaster : Thought_Situational
	{
		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x0011ECD8 File Offset: 0x0011CED8
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x04001BB3 RID: 7091
		private const int MaxAnimals = 3;
	}
}
