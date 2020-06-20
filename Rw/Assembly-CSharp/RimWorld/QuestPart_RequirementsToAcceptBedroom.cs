using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098A RID: 2442
	public class QuestPart_RequirementsToAcceptBedroom : QuestPart_RequirementsToAccept
	{
		// Token: 0x060039CE RID: 14798 RVA: 0x00133128 File Offset: 0x00131328
		public override AcceptanceReport CanAccept()
		{
			int num = this.CulpritsAre().Count<Pawn>();
			if (num > 0)
			{
				return ((num > 1) ? "QuestBedroomRequirementsUnsatisfied" : "QuestBedroomRequirementsUnsatisfiedSingle").Translate() + " " + (from p in this.CulpritsAre()
				select p.royalty.MainTitle().GetLabelFor(p).CapitalizeFirst() + " " + p.LabelShort).ToCommaList(true) + ".";
			}
			return true;
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x001331B0 File Offset: 0x001313B0
		private List<Pawn> CulpritsAre()
		{
			this.culpritsResult.Clear();
			if (this.targetPawns.Any<Pawn>())
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
				{
					if (pawn.royalty != null && pawn.royalty.HighestTitleWithBedroomRequirements() != null && (!pawn.royalty.HasPersonalBedroom() || pawn.royalty.GetUnmetBedroomRequirements(true, false).Any<string>()))
					{
						this.culpritsResult.Add(pawn);
					}
				}
			}
			this.tmpOccupiedBeds.Clear();
			List<Thing> list = this.mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Bed);
			foreach (Pawn pawn2 in this.targetPawns)
			{
				RoyalTitle royalTitle = pawn2.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null)
				{
					Thing thing = null;
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						if (thing2.Faction == Faction.OfPlayer && thing2.GetRoom(RegionType.Set_Passable) != null && !this.tmpOccupiedBeds.Contains(thing2))
						{
							CompAssignableToPawn compAssignableToPawn = thing2.TryGetComp<CompAssignableToPawn>();
							if (compAssignableToPawn != null && compAssignableToPawn.AssignedPawnsForReading.Count <= 0 && RoyalTitleUtility.BedroomSatisfiesRequirements(thing2.GetRoom(RegionType.Set_Passable), royalTitle))
							{
								thing = thing2;
								break;
							}
						}
					}
					if (thing != null)
					{
						this.tmpOccupiedBeds.Add(thing);
					}
					else
					{
						this.culpritsResult.Add(pawn2);
					}
				}
			}
			this.tmpOccupiedBeds.Clear();
			return this.culpritsResult;
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x0013337C File Offset: 0x0013157C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.targetPawns, "targetPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.targetPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060039D1 RID: 14801 RVA: 0x001333E9 File Offset: 0x001315E9
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return this.CulpritsAre().Select(delegate(Pawn p)
				{
					RoyalTitle royalTitle = p.royalty.HighestTitleWithBedroomRequirements();
					return new Dialog_InfoCard.Hyperlink(royalTitle.def, royalTitle.faction, -1);
				});
			}
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x00133415 File Offset: 0x00131615
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.targetPawns.Replace(replace, with);
		}

		// Token: 0x04002214 RID: 8724
		public List<Pawn> targetPawns = new List<Pawn>();

		// Token: 0x04002215 RID: 8725
		public MapParent mapParent;

		// Token: 0x04002216 RID: 8726
		private List<Thing> tmpOccupiedBeds = new List<Thing>();

		// Token: 0x04002217 RID: 8727
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
