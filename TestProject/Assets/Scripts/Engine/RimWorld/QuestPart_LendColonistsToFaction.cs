using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_LendColonistsToFaction : QuestPartActivable
	{
		
		// (get) Token: 0x06003990 RID: 14736 RVA: 0x00131FD8 File Offset: 0x001301D8
		public List<Thing> LentColonistsListForReading
		{
			get
			{
				return this.lentColonists;
			}
		}

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
			if (this.lendColonistsToFaction != null && compTransporter != null)
			{
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)compTransporter.innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn pawn;
						if ((pawn = (enumerator.Current as Pawn)) != null && pawn.IsFreeColonist)
						{
							this.lentColonists.Add(pawn);
						}
					}
				}
				this.returnColonistsOnTick = GenTicks.TicksGame + this.returnLentColonistsInTicks;
			}
		}

		
		// (get) Token: 0x06003992 RID: 14738 RVA: 0x00132074 File Offset: 0x00130274
		public override string DescriptionPart
		{
			get
			{
				if (base.State == QuestPartState.Disabled || this.lentColonists.Count == 0)
				{
					return null;
				}
				return "PawnsLent".Translate((from t in this.lentColonists
				select t.LabelShort).ToCommaList(true), Mathf.Max(this.returnColonistsOnTick - GenTicks.TicksGame, 0).ToStringTicksToDays("0.0"));
			}
		}

		
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.returnLentColonistsInTicks)
			{
				base.Complete();
			}
		}

		
		protected override void Complete(SignalArgs signalArgs)
		{
			Map map = (this.returnMap == null) ? Find.AnyPlayerHomeMap : this.returnMap.Map;
			if (map == null)
			{
				return;
			}
			base.Complete(new SignalArgs(new LookTargets(this.lentColonists).Named("SUBJECT"), (from c in this.lentColonists
			select c.LabelShort).ToCommaList(true).Named("PAWNS")));
			if (this.lendColonistsToFaction == Faction.Empire)
			{
				SkyfallerUtility.MakeDropoffShuttle(map, this.lentColonists, Faction.Empire);
				return;
			}
			DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(map), map, this.lentColonists, 110, false, false, true, false);
		}

		
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.lentColonists.Contains(pawn))
			{
				Building_Grave assignedGrave = null;
				if (pawn.ownership != null)
				{
					assignedGrave = pawn.ownership.AssignedGrave;
				}
				Corpse val = pawn.MakeCorpse(assignedGrave, false, 0f);
				this.lentColonists.Remove(pawn);
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(anyPlayerHomeMap), anyPlayerHomeMap, Gen.YieldSingle<Corpse>(val), 110, false, false, true, false);
				}
			}
		}

		
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.lendColonistsToFaction, "lendColonistsToFaction", false);
			Scribe_Values.Look<int>(ref this.returnLentColonistsInTicks, "returnLentColonistsInTicks", 0, false);
			Scribe_Values.Look<int>(ref this.returnColonistsOnTick, "colonistsReturnOnTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.lentColonists, "lentPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.returnMap, "returnMap", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lentColonists.RemoveAll((Thing x) => x == null);
			}
		}

		
		public Thing shuttle;

		
		public Faction lendColonistsToFaction;

		
		public int returnLentColonistsInTicks = -1;

		
		public MapParent returnMap;

		
		private int returnColonistsOnTick;

		
		private List<Thing> lentColonists = new List<Thing>();
	}
}
