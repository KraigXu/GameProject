using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000979 RID: 2425
	public class QuestPart_ExtraFaction : QuestPartActivable
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x001314F2 File Offset: 0x0012F6F2
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__0())
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

		// Token: 0x06003964 RID: 14692 RVA: 0x00131504 File Offset: 0x0012F704
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

		// Token: 0x06003965 RID: 14693 RVA: 0x00131587 File Offset: 0x0012F787
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.affectedPawns.Replace(replace, with);
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x00131597 File Offset: 0x0012F797
		public override void Cleanup()
		{
			base.Cleanup();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x001315A5 File Offset: 0x0012F7A5
		protected override void Disable()
		{
			base.Disable();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x001315B4 File Offset: 0x0012F7B4
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

		// Token: 0x040021D4 RID: 8660
		public ExtraFaction extraFaction;

		// Token: 0x040021D5 RID: 8661
		public List<Pawn> affectedPawns = new List<Pawn>();

		// Token: 0x040021D6 RID: 8662
		public bool areHelpers;

		// Token: 0x040021D7 RID: 8663
		private const int RelationsGainAvailableInTicks = 1800000;
	}
}
