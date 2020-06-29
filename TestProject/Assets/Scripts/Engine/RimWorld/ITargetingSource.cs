using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public interface ITargetingSource
	{
		
		bool CanHitTarget(LocalTargetInfo target);

		
		bool ValidateTarget(LocalTargetInfo target);

		
		void DrawHighlight(LocalTargetInfo target);

		
		void OrderForceTarget(LocalTargetInfo target);

		
		void OnGUI(LocalTargetInfo target);

		
		// (get) Token: 0x06005C15 RID: 23573
		bool CasterIsPawn { get; }

		
		// (get) Token: 0x06005C16 RID: 23574
		bool IsMeleeAttack { get; }

		
		// (get) Token: 0x06005C17 RID: 23575
		bool Targetable { get; }

		
		// (get) Token: 0x06005C18 RID: 23576
		bool MultiSelect { get; }

		
		// (get) Token: 0x06005C19 RID: 23577
		Thing Caster { get; }

		
		// (get) Token: 0x06005C1A RID: 23578
		Pawn CasterPawn { get; }

		
		// (get) Token: 0x06005C1B RID: 23579
		Verb GetVerb { get; }

		
		// (get) Token: 0x06005C1C RID: 23580
		Texture2D UIIcon { get; }

		
		// (get) Token: 0x06005C1D RID: 23581
		TargetingParameters targetParams { get; }

		
		// (get) Token: 0x06005C1E RID: 23582
		ITargetingSource DestinationSelector { get; }
	}
}
