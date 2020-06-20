using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000984 RID: 2436
	public class QuestPart_PawnsKilled : QuestPartActivable
	{
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060039AC RID: 14764 RVA: 0x001329EC File Offset: 0x00130BEC
		public override string DescriptionPart
		{
			get
			{
				return string.Concat(new object[]
				{
					"PawnsKilled".Translate(GenLabel.BestKindLabel(this.race.race.AnyPawnKind, Gender.None, true, -1)).CapitalizeFirst() + ": ",
					this.killed,
					" / ",
					this.count
				});
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x00132A69 File Offset: 0x00130C69
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__0())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.requiredInstigatorFaction != null)
				{
					yield return this.requiredInstigatorFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x00132A79 File Offset: 0x00130C79
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.killed = 0;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x00132A8C File Offset: 0x00130C8C
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (base.State == QuestPartState.Enabled && pawn.def == this.race && (this.requiredInstigatorFaction == null || (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == this.requiredInstigatorFaction))))
			{
				this.killed++;
				Find.SignalManager.SendSignal(new Signal(this.outSignalPawnKilled));
				if (this.killed >= this.count)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00132B30 File Offset: 0x00130D30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<Faction>(ref this.requiredInstigatorFaction, "requiredInstigatorFaction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.killed, "killed", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnKilled, "outSignalPawnKilled", null, false);
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00132BAB File Offset: 0x00130DAB
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.race = ThingDefOf.Muffalo;
			this.requiredInstigatorFaction = Faction.OfPlayer;
			this.count = 10;
		}

		// Token: 0x04002202 RID: 8706
		public ThingDef race;

		// Token: 0x04002203 RID: 8707
		public Faction requiredInstigatorFaction;

		// Token: 0x04002204 RID: 8708
		public int count;

		// Token: 0x04002205 RID: 8709
		public MapParent mapParent;

		// Token: 0x04002206 RID: 8710
		public string outSignalPawnKilled;

		// Token: 0x04002207 RID: 8711
		private int killed;
	}
}
