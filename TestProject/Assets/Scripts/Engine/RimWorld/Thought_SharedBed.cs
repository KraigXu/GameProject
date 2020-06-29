using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Thought_SharedBed : Thought_Situational
	{
		
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x0011FF24 File Offset: 0x0011E124
		protected override float BaseMoodOffset
		{
			get
			{
				Pawn mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(this.pawn);
				if (mostDislikedNonPartnerBedOwner == null)
				{
					return 0f;
				}
				return Mathf.Min(0.05f * (float)this.pawn.relations.OpinionOf(mostDislikedNonPartnerBedOwner) - 5f, 0f);
			}
		}
	}
}
