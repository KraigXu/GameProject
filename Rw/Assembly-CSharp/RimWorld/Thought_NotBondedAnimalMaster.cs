using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200080C RID: 2060
	public class Thought_NotBondedAnimalMaster : Thought_Situational
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x0011ED6E File Offset: 0x0011CF6E
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_NotBondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x04001BB4 RID: 7092
		private const int MaxAnimals = 3;
	}
}
