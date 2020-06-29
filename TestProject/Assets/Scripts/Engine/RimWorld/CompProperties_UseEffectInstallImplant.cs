using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_UseEffectInstallImplant : CompProperties_Usable
	{
		
		public CompProperties_UseEffectInstallImplant()
		{
			this.compClass = typeof(CompUseEffect_InstallImplant);
		}

		
		public HediffDef hediffDef;

		
		public BodyPartDef bodyPart;

		
		public bool canUpgrade;

		
		public bool allowNonColonists;
	}
}
