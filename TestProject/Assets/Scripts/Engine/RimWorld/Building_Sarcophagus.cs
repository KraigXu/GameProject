using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C84 RID: 3204
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x06004D1D RID: 19741 RVA: 0x0019D3BE File Offset: 0x0019B5BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0019D3D8 File Offset: 0x0019B5D8
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				this.thisIsFirstBodyEver = !this.everNonEmpty;
				this.everNonEmpty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x0019D400 File Offset: 0x0019B600
		public override void Notify_CorpseBuried(Pawn worker)
		{
			base.Notify_CorpseBuried(worker);
			if (this.thisIsFirstBodyEver && worker.IsColonist && base.Corpse.InnerPawn.def.race.Humanlike && !base.Corpse.everBuriedInSarcophagus)
			{
				base.Corpse.everBuriedInSarcophagus = true;
				foreach (Pawn pawn in base.Map.mapPawns.FreeColonists)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null);
					}
				}
			}
		}

		// Token: 0x04002B28 RID: 11048
		private bool everNonEmpty;

		// Token: 0x04002B29 RID: 11049
		private bool thisIsFirstBodyEver;
	}
}
