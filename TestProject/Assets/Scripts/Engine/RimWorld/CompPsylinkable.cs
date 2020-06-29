using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompPsylinkable : ThingComp
	{
		
		
		public CompProperties_Psylinkable Props
		{
			get
			{
				return (CompProperties_Psylinkable)this.props;
			}
		}

		
		
		public CompSpawnSubplant CompSubplant
		{
			get
			{
				return this.parent.TryGetComp<CompSpawnSubplant>();
			}
		}

		
		private IEnumerable<Pawn> GetPawnsThatCanPsylink(int level = -1)
		{
			return from p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists
			where this.Props.requiredFocus.CanPawnUse(p) && this.GetRequiredPlantCount(p) <= this.CompSubplant.SubplantsForReading.Count && (level == -1 || p.GetPsylinkLevel() == level)
			select p;
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CompSubplant.onGrassGrown = new Action(this.OnGrassGrown);
		}

		
		private void OnGrassGrown()
		{
			bool flag = false;
			foreach (Pawn item in this.GetPawnsThatCanPsylink(-1))
			{
				if (!this.pawnsThatCanPsylinkLastGrassGrow.Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
				string text = "";
				for (int i = 0; i < this.Props.requiredSubplantCountPerPsylinkLevel.Count; i++)
				{
					IEnumerable<string> enumerable = from p in this.GetPawnsThatCanPsylink(i)
					select p.LabelShort;
					if (enumerable.Count<string>() > 0)
					{
						text = string.Concat(new object[]
						{
							text,
							"- " + "Level".Translate() + " ",
							i + 1,
							": ",
							this.Props.requiredSubplantCountPerPsylinkLevel[i],
							" ",
							compSpawnSubplant.Props.subplant.label,
							" (",
							enumerable.ToCommaList(false),
							")\n"
						});
					}
				}
				Find.LetterStack.ReceiveLetter(this.Props.enoughPlantsLetterLabel, this.Props.enoughPlantsLetterText.Formatted(compSpawnSubplant.SubplantsForReading.Count, text.TrimEndNewlines()), LetterDefOf.NeutralEvent, new LookTargets(this.GetPawnsThatCanPsylink(-1)), null, null, null, null);
			}
			this.pawnsThatCanPsylinkLastGrassGrow.Clear();
			this.pawnsThatCanPsylinkLastGrassGrow.AddRange(this.GetPawnsThatCanPsylink(-1));
		}

		
		private int GetRequiredPlantCount(Pawn pawn)
		{
			int psylinkLevel = pawn.GetPsylinkLevel();
			if (this.parent.TryGetComp<CompSpawnSubplant>() == null)
			{
				Log.Warning("CompPsylinkable with requiredSubplantCountPerPsylinkLevel set on a Thing without CompSpawnSubplant!", false);
				return -1;
			}
			int result;
			if (this.Props.requiredSubplantCountPerPsylinkLevel.Count <= psylinkLevel)
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel.Last<int>();
			}
			else
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel[psylinkLevel];
			}
			return result;
		}

		
		public AcceptanceReport CanPsylink(Pawn pawn, LocalTargetInfo? knownSpot = null)
		{
			if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!this.Props.requiredFocus.CanPawnUse(pawn))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedFocus".Translate(this.Props.requiredFocus.label));
			}
			if (pawn.GetPsylinkLevel() >= pawn.GetMaxPsylinkLevel())
			{
				return new AcceptanceReport("InstallImplantAlreadyMaxLevel".Translate());
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, this.parent, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(this.parent, pawn);
				return new AcceptanceReport((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2));
			}
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				return new AcceptanceReport("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()));
			}
			LocalTargetInfo localTargetInfo;
			if (knownSpot != null)
			{
				if (!this.CanUseSpot(pawn, knownSpot.Value))
				{
					return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
				}
			}
			else if (!this.TryFindLinkSpot(pawn, out localTargetInfo))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
			}
			return AcceptanceReport.WasAccepted;
		}

		
		public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
		{
			spot = MeditationUtility.FindMeditationSpot(pawn).spot;
			if (this.CanUseSpot(pawn, spot))
			{
				return true;
			}
			int num = GenRadial.NumCellsInRadius(2.9f);
			int num2 = GenRadial.NumCellsInRadius(3.9f);
			for (int i = num; i < num2; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.CanUseSpot(pawn, c))
				{
					spot = c;
					return true;
				}
			}
			spot = IntVec3.Zero;
			return false;
		}

		
		private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
		{
			IntVec3 cell = spot.Cell;
			return cell.DistanceTo(this.parent.Position) <= 3.9f && cell.Standable(this.parent.Map) && GenSight.LineOfSight(cell, this.parent.Position, this.parent.Map, false, null, 0, 0) && pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
		}

		
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (pawn.Dead || pawn.Drafted)
			{
				yield break;
			}
			string text = "BeginLinkingRitualFloatMenu".Translate();
			AcceptanceReport acceptanceReport = this.CanPsylink(pawn, null);
			if (!acceptanceReport.Accepted && !string.IsNullOrWhiteSpace(acceptanceReport.Reason))
			{
				text = text + ": " + acceptanceReport.Reason;
			}
			yield return new FloatMenuOption(text, delegate
			{
				TaggedString psylinkAffectedByTraitsNegativelyWarning = RoyalTitleUtility.GetPsylinkAffectedByTraitsNegativelyWarning(pawn);
				if (psylinkAffectedByTraitsNegativelyWarning != null)
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text2 = psylinkAffectedByTraitsNegativelyWarning;
					string buttonAText = "Confirm".Translate();
					Action buttonAAction = delegate
					{
						this.BeginLinkingRitual(pawn);
					};
					windowStack.Add(new Dialog_MessageBox(text2, buttonAText, buttonAAction, "GoBack".Translate(), null, null, false, null, null));
					return;
				}
				this.BeginLinkingRitual(pawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null)
			{
				Disabled = !acceptanceReport.Accepted
			};
			yield break;
		}

		
		private void BeginLinkingRitual(Pawn pawn)
		{
			LocalTargetInfo localTargetInfo;
			if (!this.TryFindLinkSpot(pawn, out localTargetInfo) || !this.CanPsylink(pawn, new LocalTargetInfo?(localTargetInfo)).Accepted)
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LinkPsylinkable, this.parent, localTargetInfo);
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		
		public void FinishLinkingRitual(Pawn pawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psylinkables are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 5464564, false);
				return;
			}
			MoteMaker.MakeStaticMote(this.parent.Position, pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, 10f);
			SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(this.parent));
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			List<Thing> list = (from p in compSpawnSubplant.SubplantsForReading
			orderby p.Position.DistanceTo(this.parent.Position) descending
			select p).ToList<Thing>();
			int num = 0;
			while (num < requiredPlantCount && num < list.Count)
			{
				list[num].Destroy(DestroyMode.Vanish);
				num++;
			}
			compSpawnSubplant.Cleanup();
			pawn.ChangePsylinkLevel(1);
			string str = "LetterTextLinkingRitualCompleted".Translate(pawn.Named("PAWN"), this.parent.Named("LINKABLE"));
			Find.LetterStack.ReceiveLetter("LetterLabelLinkingRitualCompleted".Translate(), str, LetterDefOf.PositiveEvent, new LookTargets(new TargetInfo[]
			{
				pawn,
				this.parent
			}), null, null, null, null);
		}

		
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsThatCanPsylinkLastGrassGrow, "pawnsThatCanPsylinkLastGrassGrow", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnsThatCanPsylinkLastGrassGrow.RemoveAll((Pawn x) => x == null);
			}
		}

		
		private List<Pawn> pawnsThatCanPsylinkLastGrassGrow = new List<Pawn>();

		
		public const float MaxDistance = 3.9f;
	}
}
