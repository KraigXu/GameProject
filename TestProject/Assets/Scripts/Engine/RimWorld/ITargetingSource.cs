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

		
		
		bool CasterIsPawn { get; }

		
		
		bool IsMeleeAttack { get; }

		
		
		bool Targetable { get; }

		
		
		bool MultiSelect { get; }

		
		
		Thing Caster { get; }

		
		
		Pawn CasterPawn { get; }

		
		
		Verb GetVerb { get; }

		
		
		Texture2D UIIcon { get; }

		
		
		TargetingParameters targetParams { get; }

		
		
		ITargetingSource DestinationSelector { get; }
	}
}
