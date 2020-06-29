using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_ExtraFaction : QuestPartActivable
	{
		
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x001314F2 File Offset: 0x0012F6F2
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__0())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.extraFaction != null && this.extraFaction.faction != null)
				{
					yield return this.extraFaction.faction;
				}
				yield break;
				yield break;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.affectedPawns, "affectedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.areHelpers, "areHelpers", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.affectedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.affectedPawns.Replace(replace, with);
		}

		
		public override void Cleanup()
		{
			base.Cleanup();
			this.SetRelationsGainTickForPawns();
		}

		
		protected override void Disable()
		{
			base.Disable();
			this.SetRelationsGainTickForPawns();
		}

		
		private void SetRelationsGainTickForPawns()
		{
			foreach (Pawn pawn in this.affectedPawns)
			{
				if (pawn.mindState != null)
				{
					pawn.mindState.SetNoAidRelationsGainUntilTick(Find.TickManager.TicksGame + 1800000);
				}
			}
		}

		
		public ExtraFaction extraFaction;

		
		public List<Pawn> affectedPawns = new List<Pawn>();

		
		public bool areHelpers;

		
		private const int RelationsGainAvailableInTicks = 1800000;
	}
}
