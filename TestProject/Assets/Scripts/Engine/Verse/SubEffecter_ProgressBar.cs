﻿using System;
using RimWorld;

namespace Verse
{
	
	public class SubEffecter_ProgressBar : SubEffecter
	{
		
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		
		public MoteProgressBar mote;

		
		private const float Width = 0.68f;

		
		private const float Height = 0.12f;
	}
}
