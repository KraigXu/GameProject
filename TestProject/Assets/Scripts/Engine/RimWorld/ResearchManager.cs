using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public sealed class ResearchManager : IExposable
	{
		
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x00137CE9 File Offset: 0x00135EE9
		public bool AnyProjectIsAvailable
		{
			get
			{
				return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find((ResearchProjectDef x) => x.CanStartNow) != null;
			}
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ResearchProjectDef>(ref this.currentProj, "currentProj");
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.progress, "progress", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchProjectDef, int>(ref this.techprints, "techprints", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.ResearchManagerPostLoadInit();
			}
			if (Scribe.mode != LoadSaveMode.Saving && this.techprints == null)
			{
				this.techprints = new Dictionary<ResearchProjectDef, int>();
			}
		}

		
		public float GetProgress(ResearchProjectDef proj)
		{
			float result;
			if (this.progress.TryGetValue(proj, out result))
			{
				return result;
			}
			this.progress.Add(proj, 0f);
			return 0f;
		}

		
		public int GetTechprints(ResearchProjectDef proj)
		{
			int result;
			if (!this.techprints.TryGetValue(proj, out result))
			{
				result = 0;
			}
			return result;
		}

		
		public void ApplyTechprint(ResearchProjectDef proj, Pawn applyingPawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Techprints are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657212, false);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("LetterTechprintAppliedPartIntro".Translate(proj.Named("PROJECT")));
			stringBuilder.AppendLine();
			if (proj.techprintCount > this.GetTechprints(proj))
			{
				this.AddTechprints(proj, 1);
				if (proj.techprintCount == this.GetTechprints(proj))
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartJustUnlocked".Translate(proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartNotUnlockedYet".Translate(this.GetTechprints(proj), proj.techprintCount.ToString(), proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
			}
			else if (proj.IsFinished)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyResearched".Translate(proj.Named("PROJECT")));
				stringBuilder.AppendLine();
			}
			else if (!proj.IsFinished)
			{
				float num = (proj.baseCost - this.GetProgress(proj)) * 0.5f;
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyUnlocked".Translate(num, proj.Named("PROJECT")));
				stringBuilder.AppendLine();
				float num2;
				if (!this.progress.TryGetValue(proj, out num2))
				{
					this.progress.Add(proj, Mathf.Min(num, proj.baseCost));
				}
				else
				{
					this.progress[proj] = Mathf.Min(num2 + num, proj.baseCost);
				}
			}
			if (applyingPawn != null)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartExpAwarded".Translate(2000.ToString(), SkillDefOf.Intellectual.label, applyingPawn.Named("PAWN")));
				applyingPawn.skills.Learn(SkillDefOf.Intellectual, 2000f, false);
			}
			if (stringBuilder.Length > 0)
			{
				Find.LetterStack.ReceiveLetter("LetterTechprintAppliedLabel".Translate(proj.Named("PROJECT")), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null);
			}
		}

		
		public void AddTechprints(ResearchProjectDef proj, int amount)
		{
			int num;
			if (this.techprints.TryGetValue(proj, out num))
			{
				num += amount;
				if (num > proj.techprintCount)
				{
					num = proj.techprintCount;
				}
				this.techprints[proj] = num;
				return;
			}
			this.techprints.Add(proj, amount);
		}

		
		public void ResearchPerformed(float amount, Pawn researcher)
		{
			if (this.currentProj == null)
			{
				Log.Error("Researched without having an active project.", false);
				return;
			}
			amount *= this.ResearchPointsPerWorkTick;
			amount *= Find.Storyteller.difficulty.researchSpeedFactor;
			if (researcher != null && researcher.Faction != null)
			{
				amount /= this.currentProj.CostFactor(researcher.Faction.def.techLevel);
			}
			if (DebugSettings.fastResearch)
			{
				amount *= 500f;
			}
			if (researcher != null)
			{
				researcher.records.AddTo(RecordDefOf.ResearchPointsResearched, amount);
			}
			float num = this.GetProgress(this.currentProj);
			num += amount;
			this.progress[this.currentProj] = num;
			if (this.currentProj.IsFinished)
			{
				this.FinishProject(this.currentProj, true, researcher);
			}
		}

		
		public void ReapplyAllMods()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.IsFinished)
				{
					researchProjectDef.ReapplyAllMods();
				}
			}
		}

		
		public void FinishProject(ResearchProjectDef proj, bool doCompletionDialog = false, Pawn researcher = null)
		{
			if (proj.prerequisites != null)
			{
				for (int i = 0; i < proj.prerequisites.Count; i++)
				{
					if (!proj.prerequisites[i].IsFinished)
					{
						this.FinishProject(proj.prerequisites[i], false, null);
					}
				}
			}
			int num = this.GetTechprints(proj);
			if (num < proj.techprintCount)
			{
				this.AddTechprints(proj, proj.techprintCount - num);
			}
			this.progress[proj] = proj.baseCost;
			if (researcher != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.FinishedResearchProject, new object[]
				{
					researcher,
					this.currentProj
				});
			}
			this.ReapplyAllMods();
			if (doCompletionDialog)
			{
				DiaNode diaNode = new DiaNode("ResearchFinished".Translate(this.currentProj.LabelCap) + "\n\n" + this.currentProj.description);
				diaNode.options.Add(DiaOption.DefaultOK);
				DiaOption diaOption = new DiaOption("ResearchScreen".Translate());
				diaOption.resolveTree = true;
				diaOption.action = delegate
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
				};
				diaNode.options.Add(diaOption);
				Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
			}
			if (!proj.discoveredLetterTitle.NullOrEmpty() && Find.Storyteller.difficulty.difficulty >= proj.discoveredLetterMinDifficulty)
			{
				Find.LetterStack.ReceiveLetter(proj.discoveredLetterTitle, proj.discoveredLetterText, LetterDefOf.NeutralEvent, null);
			}
			if (proj.unlockExtremeDifficulty && Find.Storyteller.difficulty.difficulty >= DifficultyDefOf.Rough.difficulty)
			{
				Prefs.ExtremeDifficultyUnlocked = true;
				Prefs.Save();
			}
			if (this.currentProj == proj)
			{
				this.currentProj = null;
			}
		}

		
		public void DebugSetAllProjectsFinished()
		{
			this.progress.Clear();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				this.progress.Add(researchProjectDef, researchProjectDef.baseCost);
			}
			this.ReapplyAllMods();
		}

		
		public ResearchProjectDef currentProj;

		
		private Dictionary<ResearchProjectDef, float> progress = new Dictionary<ResearchProjectDef, float>();

		
		private Dictionary<ResearchProjectDef, int> techprints = new Dictionary<ResearchProjectDef, int>();

		
		private float ResearchPointsPerWorkTick = 0.00825f;

		
		public const int IntellectualExpPerTechprint = 2000;
	}
}
