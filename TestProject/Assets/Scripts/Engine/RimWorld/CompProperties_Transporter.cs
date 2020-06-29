using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Transporter : CompProperties
	{
		
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		
		public float massCapacity = 150f;

		
		public float restEffectiveness;

		
		public bool max1PerGroup;

		
		public bool canChangeAssignedThingsAfterStarting;

		
		public bool showOverallStats = true;

		
		public SoundDef pawnLoadedSound;

		
		public SoundDef pawnExitSound;
	}
}
