using System;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_Spawn : CompAbilityEffect
	{
		
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x0015EE20 File Offset: 0x0015D020
		public new CompProperties_AbilitySpawn Props
		{
			get
			{
				return (CompProperties_AbilitySpawn)this.props;
			}
		}

		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenSpawn.Spawn(this.Props.thingDef, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
		}

		
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
