using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class CompUseEffect : ThingComp
	{
		
		
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		
		
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		
		public virtual void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap)
			{
				if (this.Props.doCameraShake && usedBy.Spawned)
				{
					Find.CameraDriver.shaker.DoShake(1f);
				}
				if (this.Props.moteOnUsed != null)
				{
					MoteMaker.MakeAttachedOverlay(usedBy, this.Props.moteOnUsed, Vector3.zero, this.Props.moteOnUsedScale, -1f);
				}
			}
		}

		
		public virtual TaggedString ConfirmMessage(Pawn p)
		{
			return null;
		}

		
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}

		
		private const float CameraShakeMag = 1f;
	}
}
