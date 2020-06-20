using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF1 RID: 3057
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x060048B7 RID: 18615 RVA: 0x0018BC11 File Offset: 0x00189E11
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x0018BC19 File Offset: 0x00189E19
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x0018BC34 File Offset: 0x00189E34
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x0018BC8C File Offset: 0x00189E8C
		public void Kidnap(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + pawn, false);
				return;
			}
			if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + pawn, false);
				return;
			}
			pawn.PreKidnapped(kidnapper);
			if (pawn.Spawned)
			{
				pawn.DeSpawn(DestroyMode.Vanish);
			}
			this.kidnappedPawns.Add(pawn);
			if (!Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				if (!Find.WorldPawns.Contains(pawn))
				{
					Log.Error("WorldPawns discarded kidnapped pawn.", false);
					this.kidnappedPawns.Remove(pawn);
				}
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, PawnDiedOrDownedThoughtsKind.Lost);
				BillUtility.Notify_ColonistUnavailable(pawn);
				if (kidnapper != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelPawnsKidnapped".Translate(pawn.Named("PAWN")), "LetterPawnsKidnapped".Translate(pawn.Named("PAWN"), kidnapper.Faction.Named("FACTION")), LetterDefOf.NegativeEvent, null);
				}
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Kidnapped", this.Named("SUBJECT"), kidnapper.Named("KIDNAPPER"));
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x0018BDE3 File Offset: 0x00189FE3
		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (this.kidnappedPawns.Remove(pawn))
			{
				if (pawn.Faction == Faction.OfPlayer)
				{
					PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(pawn);
					return;
				}
			}
			else
			{
				Log.Warning("Tried to remove kidnapped pawn " + pawn + " but he's not here.", false);
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0018BE20 File Offset: 0x0018A020
		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x0018BE90 File Offset: 0x0018A090
		public void KidnappedPawnsTrackerTick()
		{
			for (int i = this.kidnappedPawns.Count - 1; i >= 0; i--)
			{
				if (this.kidnappedPawns[i].DestroyedOrNull())
				{
					this.kidnappedPawns.RemoveAt(i);
				}
			}
			if (Find.TickManager.TicksGame % 15051 == 0)
			{
				for (int j = this.kidnappedPawns.Count - 1; j >= 0; j--)
				{
					if (Rand.MTBEventOccurs(30f, 60000f, 15051f))
					{
						this.kidnappedPawns[j].SetFaction(this.faction, null);
						this.kidnappedPawns.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x040029A1 RID: 10657
		private Faction faction;

		// Token: 0x040029A2 RID: 10658
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x040029A3 RID: 10659
		private const int TryRecruitInterval = 15051;

		// Token: 0x040029A4 RID: 10660
		private const float RecruitMTBDays = 30f;
	}
}
