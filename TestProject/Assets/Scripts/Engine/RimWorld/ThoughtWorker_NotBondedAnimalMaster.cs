using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080B RID: 2059
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x06003422 RID: 13346 RVA: 0x0011ED34 File Offset: 0x0011CF34
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p && TrainableUtility.MinimumHandlingSkill(animal) <= p.skills.GetSkill(SkillDefOf.Animals).Level;
		}
	}
}
