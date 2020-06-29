using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_PawnsKilled : QuestPartActivable
	{
		
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

		
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x00132A69 File Offset: 0x00130C69
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__0())
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

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.killed = 0;
		}

		
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

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.race = ThingDefOf.Muffalo;
			this.requiredInstigatorFaction = Faction.OfPlayer;
			this.count = 10;
		}

		
		public ThingDef race;

		
		public Faction requiredInstigatorFaction;

		
		public int count;

		
		public MapParent mapParent;

		
		public string outSignalPawnKilled;

		
		private int killed;
	}
}
