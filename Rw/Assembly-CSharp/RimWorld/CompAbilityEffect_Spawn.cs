using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD3 RID: 2771
	public class CompAbilityEffect_Spawn : CompAbilityEffect
	{
		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x0015EE20 File Offset: 0x0015D020
		public new CompProperties_AbilitySpawn Props
		{
			get
			{
				return (CompProperties_AbilitySpawn)this.props;
			}
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x0015EE2D File Offset: 0x0015D02D
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenSpawn.Spawn(this.Props.thingDef, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x0015EE60 File Offset: 0x0015D060
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
