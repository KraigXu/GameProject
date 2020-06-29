using System;
using UnityEngine;

namespace RimWorld
{
	
	public class Thought_NotBondedAnimalMaster : Thought_Situational
	{
		
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x0011ED6E File Offset: 0x0011CF6E
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_NotBondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		
		private const int MaxAnimals = 3;
	}
}
