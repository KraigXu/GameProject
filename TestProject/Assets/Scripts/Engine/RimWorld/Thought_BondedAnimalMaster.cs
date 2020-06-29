using System;
using UnityEngine;

namespace RimWorld
{
	
	public class Thought_BondedAnimalMaster : Thought_Situational
	{
		
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x0011ECD8 File Offset: 0x0011CED8
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		
		private const int MaxAnimals = 3;
	}
}
