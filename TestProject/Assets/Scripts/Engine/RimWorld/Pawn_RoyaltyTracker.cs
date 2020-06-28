﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB3 RID: 2995
	[StaticConstructorOnStartup]
	public class Pawn_RoyaltyTracker : IExposable
	{
		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004692 RID: 18066 RVA: 0x0017D080 File Offset: 0x0017B280
		public List<RoyalTitle> AllTitlesForReading
		{
			get
			{
				return this.titles;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004693 RID: 18067 RVA: 0x0017D088 File Offset: 0x0017B288
		public List<RoyalTitle> AllTitlesInEffectForReading
		{
			get
			{
				if (!this.pawn.IsWildMan())
				{
					return this.titles;
				}
				return Pawn_RoyaltyTracker.EmptyTitles;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004694 RID: 18068 RVA: 0x0017D0A4 File Offset: 0x0017B2A4
		public RoyalTitle MostSeniorTitle
		{
			get
			{
				List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
				int num = -1;
				RoyalTitle royalTitle = null;
				for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
				{
					if (allTitlesInEffectForReading[i].def.seniority > num)
					{
						num = allTitlesInEffectForReading[i].def.seniority;
						royalTitle = allTitlesInEffectForReading[i];
					}
				}
				return royalTitle ?? null;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004695 RID: 18069 RVA: 0x0017D101 File Offset: 0x0017B301
		public IEnumerable<QuestScriptDef> PossibleDecreeQuests
		{
			get
			{
				this.tmpDecreeTags.Clear();
				List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
				for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
				{
					if (allTitlesInEffectForReading[i].def.decreeTags != null)
					{
						this.tmpDecreeTags.AddRange(allTitlesInEffectForReading[i].def.decreeTags);
					}
				}
				foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
				{
					if (questScriptDef.decreeTags != null && questScriptDef.decreeTags.Any((string x) => this.tmpDecreeTags.Contains(x)))
					{
						Slate slate = new Slate();
						Slate slate2 = slate;
						string name = "points";
						IIncidentTarget mapHeld = this.pawn.MapHeld;
						slate2.Set<float>(name, StorytellerUtility.DefaultThreatPointsNow(mapHeld ?? Find.World), false);
						slate.Set<Pawn>("asker", this.pawn, false);
						if (questScriptDef.CanRun(slate))
						{
							yield return questScriptDef;
						}
					}
				}
				IEnumerator<QuestScriptDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x0017D114 File Offset: 0x0017B314
		public Pawn_RoyaltyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x0017D194 File Offset: 0x0017B394
		private int FindFactionTitleIndex(Faction faction, bool createIfNotExisting = false)
		{
			for (int i = 0; i < this.titles.Count; i++)
			{
				if (this.titles[i].faction == faction)
				{
					return i;
				}
			}
			if (createIfNotExisting)
			{
				this.titles.Add(new RoyalTitle
				{
					faction = faction,
					receivedTick = GenTicks.TicksGame,
					conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(this.pawn)
				});
				return this.titles.Count - 1;
			}
			return -1;
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x0017D214 File Offset: 0x0017B414
		public bool HasAnyTitleIn(Faction faction)
		{
			using (List<RoyalTitle>.Enumerator enumerator = this.titles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.faction == faction)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x0017D270 File Offset: 0x0017B470
		public bool HasTitle(RoyalTitleDef title)
		{
			using (List<RoyalTitle>.Enumerator enumerator = this.titles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == title)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0017D2CC File Offset: 0x0017B4CC
		public bool HasPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			return currentTitle != null && currentTitle.permits != null && currentTitle.permits.Contains(permit);
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x0600469B RID: 18075 RVA: 0x0017D2FC File Offset: 0x0017B4FC
		public bool HasAidPermit
		{
			get
			{
				foreach (RoyalTitle royalTitle in this.AllTitlesInEffectForReading)
				{
					if (!royalTitle.def.permits.NullOrEmpty<RoyalTitlePermitDef>())
					{
						using (List<RoyalTitlePermitDef>.Enumerator enumerator2 = royalTitle.def.permits.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.workerClass == typeof(RoyalTitlePermitWorker_CallAid))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x0017D3B8 File Offset: 0x0017B5B8
		public int GetPermitLastUsedTick(RoyalTitlePermitDef permitDef)
		{
			if (!this.permitLastUsedTick.ContainsKey(permitDef))
			{
				return -1;
			}
			return this.permitLastUsedTick[permitDef];
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x0017D3D8 File Offset: 0x0017B5D8
		public bool PermitOnCooldown(RoyalTitlePermitDef permitDef)
		{
			int num = this.GetPermitLastUsedTick(permitDef);
			return num != -1 && GenTicks.TicksGame < num + permitDef.CooldownTicks;
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x0017D402 File Offset: 0x0017B602
		public void Notify_PermitUsed(RoyalTitlePermitDef permitDef)
		{
			if (!this.permitLastUsedTick.ContainsKey(permitDef))
			{
				this.permitLastUsedTick.Add(permitDef, GenTicks.TicksGame);
			}
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0017D424 File Offset: 0x0017B624
		public RoyalTitleDef MainTitle()
		{
			if (this.titles.Count == 0)
			{
				return null;
			}
			RoyalTitleDef royalTitleDef = null;
			foreach (RoyalTitle royalTitle in this.titles)
			{
				if (royalTitleDef == null || royalTitle.def.seniority > royalTitleDef.seniority)
				{
					royalTitleDef = royalTitle.def;
				}
			}
			return royalTitleDef;
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x0017D4A0 File Offset: 0x0017B6A0
		public int GetFavor(Faction faction)
		{
			int result;
			if (!this.favor.TryGetValue(faction, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x0017D4C0 File Offset: 0x0017B6C0
		public void GainFavor(Faction faction, int amount)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Royal favor is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 63699999, false);
				return;
			}
			int num;
			if (!this.favor.TryGetValue(faction, out num))
			{
				num = 0;
				this.favor.Add(faction, 0);
			}
			num += amount;
			this.favor[faction] = num;
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.UpdateRoyalTitle(faction);
			RoyalTitleDef currentTitle2 = this.GetCurrentTitle(faction);
			if (currentTitle2 != currentTitle)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, currentTitle2, false);
				this.OnPostTitleChanged(faction, currentTitle2);
			}
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x0017D544 File Offset: 0x0017B744
		public bool TryRemoveFavor(Faction faction, int amount)
		{
			int num = this.GetFavor(faction);
			if (num < amount)
			{
				return false;
			}
			this.SetFavor(faction, num - amount);
			return true;
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x0017D56C File Offset: 0x0017B76C
		public void SetFavor(Faction faction, int amount)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Royal favor is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 7641236, false);
				return;
			}
			if (amount == 0 && this.favor.ContainsKey(faction) && this.FindFactionTitleIndex(faction, false) == -1)
			{
				this.favor.Remove(faction);
				return;
			}
			this.favor.SetOrAdd(faction, amount);
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x0017D5C8 File Offset: 0x0017B7C8
		public RoyalTitleDef GetCurrentTitle(Faction faction)
		{
			RoyalTitle currentTitleInFaction = this.GetCurrentTitleInFaction(faction);
			if (currentTitleInFaction == null)
			{
				return null;
			}
			return currentTitleInFaction.def;
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x0017D5DC File Offset: 0x0017B7DC
		public RoyalTitle GetCurrentTitleInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Cannot get current title for null faction.", false);
			}
			int num = this.FindFactionTitleIndex(faction, false);
			if (num == -1)
			{
				return null;
			}
			return this.titles[num];
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x0017D614 File Offset: 0x0017B814
		public void SetTitle(Faction faction, RoyalTitleDef title, bool grantRewards, bool rewardsOnlyForNewestTitle = false, bool sendLetter = true)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Royal favor is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 7445532, false);
				return;
			}
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.OnPreTitleChanged(faction, currentTitle, title, sendLetter);
			if (grantRewards)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, title, rewardsOnlyForNewestTitle);
			}
			int index = this.FindFactionTitleIndex(faction, true);
			if (title != null)
			{
				this.titles[index].def = title;
				this.titles[index].receivedTick = GenTicks.TicksGame;
			}
			else
			{
				this.titles.RemoveAt(index);
			}
			this.SetFavor(faction, 0);
			this.OnPostTitleChanged(faction, title);
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x0017D6B0 File Offset: 0x0017B8B0
		public void ReduceTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			if (currentTitle == null || !currentTitle.Awardable)
			{
				return;
			}
			RoyalTitleDef previousTitle = currentTitle.GetPreviousTitle(faction);
			this.OnPreTitleChanged(faction, currentTitle, previousTitle, true);
			this.CleanupThoughts(currentTitle);
			this.CleanupThoughts(previousTitle);
			if (currentTitle.awardThought != null && this.pawn.needs.mood != null)
			{
				Thought_MemoryRoyalTitle thought_MemoryRoyalTitle = (Thought_MemoryRoyalTitle)ThoughtMaker.MakeThought(currentTitle.lostThought);
				thought_MemoryRoyalTitle.titleDef = currentTitle;
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryRoyalTitle, null);
			}
			int index = this.FindFactionTitleIndex(faction, false);
			if (previousTitle == null)
			{
				this.titles.RemoveAt(index);
			}
			else
			{
				this.titles[index].def = previousTitle;
			}
			this.SetFavor(faction, 0);
			this.OnPostTitleChanged(faction, previousTitle);
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x0017D780 File Offset: 0x0017B980
		public Pawn GetHeir(Faction faction)
		{
			if (this.heirs != null && this.heirs.ContainsKey(faction))
			{
				return this.heirs[faction];
			}
			return null;
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x0017D7A6 File Offset: 0x0017B9A6
		public void SetHeir(Pawn heir, Faction faction)
		{
			if (this.heirs != null)
			{
				this.heirs[faction] = heir;
			}
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x0017D7C0 File Offset: 0x0017B9C0
		public void AssignHeirIfNone(RoyalTitleDef t, Faction faction)
		{
			if (!this.heirs.ContainsKey(faction) && t.Awardable && this.pawn.FactionOrExtraHomeFaction != Faction.Empire)
			{
				this.SetHeir(t.GetInheritanceWorker(faction).FindHeir(faction, this.pawn, t), faction);
			}
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x0017D810 File Offset: 0x0017BA10
		public void RoyaltyTrackerTick()
		{
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				allTitlesInEffectForReading[i].RoyalTitleTick(this.pawn);
			}
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			this.factionHeirsToClearTmp.Clear();
			foreach (KeyValuePair<Faction, Pawn> keyValuePair in this.heirs)
			{
				RoyalTitleDef currentTitle = this.GetCurrentTitle(keyValuePair.Key);
				if (currentTitle != null && currentTitle.canBeInherited)
				{
					Pawn value = keyValuePair.Value;
					if (value != null && value.Dead)
					{
						Find.LetterStack.ReceiveLetter("LetterTitleHeirLostLabel".Translate(), "LetterTitleHeirLost".Translate(this.pawn.Named("HOLDER"), value.Named("HEIR"), keyValuePair.Key.Named("FACTION")), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null);
						this.factionHeirsToClearTmp.Add(keyValuePair.Key);
					}
				}
			}
			foreach (Faction key in this.factionHeirsToClearTmp)
			{
				this.heirs[key] = null;
			}
			for (int j = this.permitLastUsedTick.Count - 1; j >= 0; j--)
			{
				KeyValuePair<RoyalTitlePermitDef, int> keyValuePair2 = this.permitLastUsedTick.ElementAt(j);
				if (!this.PermitOnCooldown(keyValuePair2.Key))
				{
					Messages.Message("MessagePermitCooldownFinished".Translate(this.pawn, keyValuePair2.Key.LabelCap), this.pawn, MessageTypeDefOf.PositiveEvent, true);
					this.permitLastUsedTick.Remove(keyValuePair2.Key);
				}
			}
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x0017DA3C File Offset: 0x0017BC3C
		public void IssueDecree(bool causedByMentalBreak, string mentalBreakReason = null)
		{
			Pawn_RoyaltyTracker.<>c__DisplayClass43_0 <>c__DisplayClass43_ = new Pawn_RoyaltyTracker.<>c__DisplayClass43_0();
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Decrees are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 281653, false);
				return;
			}
			Pawn_RoyaltyTracker.<>c__DisplayClass43_0 <>c__DisplayClass43_2 = <>c__DisplayClass43_;
			IIncidentTarget mapHeld = this.pawn.MapHeld;
			<>c__DisplayClass43_2.target = (mapHeld ?? Find.World);
			QuestScriptDef questScriptDef;
			if (this.PossibleDecreeQuests.TryRandomElementByWeight((QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalDecreeSelectionWeight(x, <>c__DisplayClass43_.target.StoryState), out questScriptDef))
			{
				this.lastDecreeTicks = Find.TickManager.TicksGame;
				Slate slate = new Slate();
				slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(<>c__DisplayClass43_.target), false);
				slate.Set<Pawn>("asker", this.pawn, false);
				Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
				<>c__DisplayClass43_.target.StoryState.RecordDecreeFired(questScriptDef);
				string str;
				if (causedByMentalBreak)
				{
					str = "WildDecree".Translate() + ": " + this.pawn.LabelShortCap;
				}
				else
				{
					str = "LetterLabelRandomDecree".Translate(this.pawn);
				}
				string text;
				if (causedByMentalBreak)
				{
					text = "LetterDecreeMentalBreak".Translate(this.pawn);
				}
				else
				{
					text = "LetterRandomDecree".Translate(this.pawn);
				}
				if (mentalBreakReason != null)
				{
					text = text + "\n\n" + mentalBreakReason;
				}
				text += "\n\n" + "LetterDecree_Quest".Translate(quest.name);
				ChoiceLetter let = LetterMaker.MakeLetter(str, text, IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
				Find.LetterStack.ReceiveLetter(let, null);
			}
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x0017DBF8 File Offset: 0x0017BDF8
		private void CleanupThoughts(RoyalTitleDef title)
		{
			if (title == null)
			{
				return;
			}
			if (title.awardThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(title.awardThought);
			}
			if (title.lostThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(title.lostThought);
			}
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x0017DCA4 File Offset: 0x0017BEA4
		private void OnPreTitleChanged(Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, bool sendLetter = true)
		{
			this.AssignHeirIfNone(newTitle, faction);
			if (this.pawn.IsColonist && sendLetter)
			{
				TaggedString taggedString = null;
				TaggedString label = null;
				if (currentTitle == null || faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) < faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle))
				{
					taggedString = "LetterGainedRoyalTitle".Translate(this.pawn.Named("PAWN"), faction.Named("FACTION"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
					label = "LetterLabelGainedRoyalTitle".Translate(this.pawn.Named("PAWN"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
				}
				else
				{
					taggedString = "LetterLostRoyalTitle".Translate(this.pawn.Named("PAWN"), faction.Named("FACTION"), currentTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
					label = "LetterLabelLostRoyalTitle".Translate(this.pawn.Named("PAWN"), currentTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
				}
				string text = RoyalTitleUtility.BuildDifferenceExplanationText(currentTitle, newTitle, faction, this.pawn);
				if (text.Length > 0)
				{
					taggedString += "\n\n" + text;
				}
				taggedString = taggedString.Resolve().TrimEndNewlines();
				Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
			}
			if (currentTitle != null)
			{
				for (int i = 0; i < currentTitle.grantedAbilities.Count; i++)
				{
					this.pawn.abilities.RemoveAbility(currentTitle.grantedAbilities[i]);
				}
			}
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x0017DE74 File Offset: 0x0017C074
		private void OnPostTitleChanged(Faction faction, RoyalTitleDef newTitle)
		{
			this.pawn.Notify_DisabledWorkTypesChanged();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (newTitle != null)
			{
				if (newTitle.disabledJoyKinds != null && this.pawn.jobs != null && RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(this.pawn))
				{
					foreach (JoyKindDef joyKind in newTitle.disabledJoyKinds)
					{
						this.pawn.jobs.Notify_JoyKindDisabled(joyKind);
					}
				}
				for (int i = 0; i < newTitle.grantedAbilities.Count; i++)
				{
					this.pawn.abilities.GainAbility(newTitle.grantedAbilities[i]);
				}
				this.UpdateHighestTitleAchieved(faction, newTitle);
			}
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "TitleChanged", this.pawn.Named("SUBJECT"));
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x0017DF84 File Offset: 0x0017C184
		private void UpdateRoyalTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			if (currentTitle != null && !currentTitle.Awardable)
			{
				return;
			}
			RoyalTitleDef nextTitle = currentTitle.GetNextTitle(faction);
			if (nextTitle == null)
			{
				return;
			}
			int num = this.GetFavor(faction);
			if (num >= nextTitle.favorCost)
			{
				this.OnPreTitleChanged(faction, currentTitle, nextTitle, true);
				this.SetFavor(faction, num - nextTitle.favorCost);
				int index = this.FindFactionTitleIndex(faction, true);
				this.titles[index].def = nextTitle;
				this.CleanupThoughts(currentTitle);
				this.CleanupThoughts(nextTitle);
				if (nextTitle.awardThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					Thought_MemoryRoyalTitle thought_MemoryRoyalTitle = (Thought_MemoryRoyalTitle)ThoughtMaker.MakeThought(nextTitle.awardThought);
					thought_MemoryRoyalTitle.titleDef = nextTitle;
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryRoyalTitle, null);
				}
				this.UpdateRoyalTitle(faction);
			}
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x0017E074 File Offset: 0x0017C274
		public List<Thing> ApplyRewardsForTitle(Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, bool onlyForNewestTitle = false)
		{
			List<Thing> list = new List<Thing>();
			List<ThingCount> list2 = new List<ThingCount>();
			if (newTitle != null && newTitle.Awardable && this.pawn.IsColonist && this.NewHighestTitle(faction, newTitle))
			{
				int num = ((currentTitle != null) ? faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) : 0) + 1;
				int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle);
				if (onlyForNewestTitle)
				{
					num = num2;
				}
				IntVec3 dropCenter = IntVec3.Invalid;
				Map mapHeld = this.pawn.MapHeld;
				if (mapHeld != null)
				{
					if (mapHeld.IsPlayerHome)
					{
						dropCenter = DropCellFinder.TradeDropSpot(mapHeld);
					}
					else if (!DropCellFinder.TryFindDropSpotNear(this.pawn.Position, mapHeld, out dropCenter, false, false, true, null))
					{
						dropCenter = DropCellFinder.RandomDropSpot(mapHeld);
					}
				}
				for (int i = num; i <= num2; i++)
				{
					RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[i];
					if (royalTitleDef.rewards != null)
					{
						List<Thing> list3 = royalTitleDef.rewards.Select(delegate(ThingDefCountClass r)
						{
							Thing thing = ThingMaker.MakeThing(r.thingDef, null);
							thing.stackCount = r.count;
							return thing;
						}).ToList<Thing>();
						for (int j = 0; j < list3.Count; j++)
						{
							if (list3[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								break;
							}
						}
						if (this.pawn.Spawned)
						{
							DropPodUtility.DropThingsNear(dropCenter, mapHeld, list3, 110, false, false, false, false);
						}
						else
						{
							foreach (Thing item in list3)
							{
								this.pawn.inventory.TryAddItemNotForSale(item);
							}
						}
						for (int k = 0; k < list3.Count; k++)
						{
							list2.Add(new ThingCount(list3[k], list3[k].stackCount));
						}
						list.AddRange(list3);
					}
				}
				if (list.Count > 0)
				{
					TaggedString text = "LetterRewardsForNewTitle".Translate(this.pawn.Named("PAWN"), faction.Named("FACTION"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE")) + "\n\n" + GenLabel.ThingsLabel(list2, "  - ", true) + "\n\n" + (this.pawn.Spawned ? "LetterRewardsForNewTitleDeliveryBase" : "LetterRewardsForNewTitleDeliveryDirect").Translate(this.pawn.Named("PAWN"));
					Find.LetterStack.ReceiveLetter("LetterLabelRewardsForNewTitle".Translate(), text, LetterDefOf.PositiveEvent, list, null, null, null, null);
				}
			}
			return list;
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x0017E35C File Offset: 0x0017C55C
		private void UpdateHighestTitleAchieved(Faction faction, RoyalTitleDef title)
		{
			if (!this.highestTitles.ContainsKey(faction))
			{
				this.highestTitles.Add(faction, title);
				return;
			}
			if (this.NewHighestTitle(faction, title))
			{
				this.highestTitles[faction] = title;
			}
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x0017E394 File Offset: 0x0017C594
		public bool NewHighestTitle(Faction faction, RoyalTitleDef newTitle)
		{
			if (this.highestTitles == null)
			{
				this.highestTitles = new Dictionary<Faction, RoyalTitleDef>();
			}
			if (!this.highestTitles.ContainsKey(faction))
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.highestTitles[faction]);
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle) > num;
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x0017E3F8 File Offset: 0x0017C5F8
		public void Notify_PawnKilled()
		{
			if (PawnGenerator.IsBeingGenerated(this.pawn) || this.AllTitlesForReading.Count == 0)
			{
				return;
			}
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				stringBuilder.AppendLine("LetterTitleInheritance_Base".Translate(this.pawn.Named("PAWN")));
				stringBuilder.AppendLine();
				foreach (RoyalTitle royalTitle in this.AllTitlesForReading)
				{
					if (royalTitle.def.canBeInherited)
					{
						if (this.pawn.IsFreeColonist && !this.pawn.IsQuestLodger())
						{
							flag = true;
						}
						RoyalTitleInheritanceOutcome outcome;
						if (royalTitle.def.TryInherit(this.pawn, royalTitle.faction, out outcome))
						{
							if (outcome.HeirHasTitle && !outcome.heirTitleHigher)
							{
								stringBuilder.AppendLine("LetterTitleInheritance_AsReplacement".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), outcome.heirCurrentTitle.GetLabelFor(outcome.heir).Named("REPLACEDTITLE")).CapitalizeFirst().Resolve());
								stringBuilder.AppendLine();
							}
							else if (outcome.heirTitleHigher)
							{
								stringBuilder.AppendLine("LetterTitleInheritance_NoEffectHigherTitle".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), outcome.heirCurrentTitle.GetLabelFor(outcome.heir).Named("HIGHERTITLE")).CapitalizeFirst().Resolve());
								stringBuilder.AppendLine();
							}
							else
							{
								stringBuilder.AppendLine("LetterTitleInheritance_WasInherited".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE")).CapitalizeFirst().Resolve());
								stringBuilder.AppendLine();
							}
							if (!outcome.heirTitleHigher)
							{
								RoyalTitle titleLocal = royalTitle;
								Pawn_RoyaltyTracker.tmpInheritedTitles.Add(delegate
								{
									outcome.heir.royalty.SetTitle(titleLocal.faction, titleLocal.def, true, true, true);
									titleLocal.wasInherited = true;
								});
								if (outcome.heir.IsFreeColonist && !outcome.heir.IsQuestLodger())
								{
									flag = true;
								}
							}
						}
						else
						{
							stringBuilder.AppendLine("LetterTitleInheritance_NoHeirFound".Translate(this.pawn.Named("PAWN"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).CapitalizeFirst().Resolve());
						}
					}
				}
				if (stringBuilder.Length > 0 && flag)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleInheritance".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null);
				}
				foreach (Action action in Pawn_RoyaltyTracker.tmpInheritedTitles)
				{
					action();
				}
			}
			finally
			{
				Pawn_RoyaltyTracker.tmpInheritedTitles.Clear();
			}
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x0017E86C File Offset: 0x0017CA6C
		public void Notify_Resurrected()
		{
			foreach (Faction faction in (from t in this.titles
			select t.faction).Distinct<Faction>().ToList<Faction>())
			{
				int index = this.FindFactionTitleIndex(faction, false);
				if (this.titles[index].wasInherited)
				{
					this.SetTitle(faction, null, false, false, false);
				}
			}
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0017E910 File Offset: 0x0017CB10
		public Gizmo RoyalAidGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandCallRoyalAid".Translate();
			command_Action.defaultDesc = "CommandCallRoyalAidDesc".Translate();
			command_Action.icon = Pawn_RoyaltyTracker.CommandTex;
			if (this.pawn.Downed)
			{
				command_Action.Disable("CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn));
			}
			if (this.pawn.IsQuestLodger())
			{
				command_Action.Disable("CommandCallRoyalAidLodger".Translate());
			}
			command_Action.action = delegate
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RoyalTitle royalTitle in from t in this.AllTitlesInEffectForReading
				where !t.def.permits.NullOrEmpty<RoyalTitlePermitDef>()
				select t)
				{
					foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
					{
						IEnumerable<FloatMenuOption> royalAidOptions = royalTitlePermitDef.Worker.GetRoyalAidOptions(this.pawn.MapHeld, this.pawn, royalTitle.faction);
						if (royalAidOptions != null)
						{
							list.AddRange(royalAidOptions);
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			};
			return command_Action;
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x0017E9C9 File Offset: 0x0017CBC9
		public bool CanRequireThroneroom()
		{
			return this.pawn.IsFreeColonist && this.allowRoomRequirements && !this.pawn.IsQuestLodger();
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x0017E9F0 File Offset: 0x0017CBF0
		public RoyalTitle HighestTitleWithThroneRoomRequirements()
		{
			if (!this.CanRequireThroneroom())
			{
				return null;
			}
			RoyalTitle royalTitle = null;
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				if (!allTitlesInEffectForReading[i].def.throneRoomRequirements.EnumerableNullOrEmpty<RoomRequirement>() && (royalTitle == null || allTitlesInEffectForReading[i].def.seniority > royalTitle.def.seniority))
				{
					royalTitle = allTitlesInEffectForReading[i];
				}
			}
			return royalTitle;
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x0017EA63 File Offset: 0x0017CC63
		public IEnumerable<string> GetUnmetThroneroomRequirements(bool includeOnGracePeriod = true, bool onlyOnGracePeriod = false)
		{
			if (this.pawn.ownership.AssignedThrone == null)
			{
				yield break;
			}
			RoyalTitle highestTitle = this.HighestTitleWithThroneRoomRequirements();
			if (highestTitle == null)
			{
				yield break;
			}
			Room throneRoom = this.pawn.ownership.AssignedThrone.GetRoom(RegionType.Set_Passable);
			if (throneRoom == null)
			{
				yield break;
			}
			RoyalTitleDef prevTitle = highestTitle.def.GetPreviousTitle(highestTitle.faction);
			foreach (RoomRequirement roomRequirement in highestTitle.def.throneRoomRequirements)
			{
				if (!roomRequirement.Met(throneRoom, this.pawn))
				{
					bool flag = highestTitle.RoomRequirementGracePeriodActive(this.pawn);
					bool flag2 = prevTitle != null && !prevTitle.HasSameThroneroomRequirement(roomRequirement);
					if ((!onlyOnGracePeriod || (flag2 && flag)) && (!flag || !flag2 || includeOnGracePeriod))
					{
						yield return roomRequirement.LabelCap(throneRoom);
					}
				}
			}
			List<RoomRequirement>.Enumerator enumerator = default(List<RoomRequirement>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x0017EA81 File Offset: 0x0017CC81
		public bool CanRequireBedroom()
		{
			return this.allowRoomRequirements && !this.pawn.IsPrisoner;
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x0017EA9C File Offset: 0x0017CC9C
		public RoyalTitle HighestTitleWithBedroomRequirements()
		{
			if (!this.CanRequireBedroom())
			{
				return null;
			}
			RoyalTitle royalTitle = null;
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				if (!allTitlesInEffectForReading[i].def.GetBedroomRequirements(this.pawn).EnumerableNullOrEmpty<RoomRequirement>() && (royalTitle == null || allTitlesInEffectForReading[i].def.seniority > royalTitle.def.seniority))
				{
					royalTitle = allTitlesInEffectForReading[i];
				}
			}
			return royalTitle;
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x0017EB15 File Offset: 0x0017CD15
		public IEnumerable<string> GetUnmetBedroomRequirements(bool includeOnGracePeriod = true, bool onlyOnGracePeriod = false)
		{
			RoyalTitle royalTitle = this.HighestTitleWithBedroomRequirements();
			if (royalTitle == null)
			{
				yield break;
			}
			bool gracePeriodActive = royalTitle.RoomRequirementGracePeriodActive(this.pawn);
			RoyalTitleDef prevTitle = royalTitle.def.GetPreviousTitle(royalTitle.faction);
			if (!this.HasPersonalBedroom())
			{
				yield break;
			}
			Room bedroom = this.pawn.ownership.OwnedRoom;
			foreach (RoomRequirement roomRequirement in royalTitle.def.GetBedroomRequirements(this.pawn))
			{
				if (!roomRequirement.Met(bedroom, null))
				{
					bool flag = prevTitle != null && !prevTitle.HasSameBedroomRequirement(roomRequirement);
					if ((!onlyOnGracePeriod || (flag && gracePeriodActive)) && (!gracePeriodActive || !flag || includeOnGracePeriod))
					{
						yield return roomRequirement.LabelCap(bedroom);
					}
				}
			}
			IEnumerator<RoomRequirement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x0017EB34 File Offset: 0x0017CD34
		public bool HasPersonalBedroom()
		{
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return false;
			}
			Room ownedRoom = this.pawn.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			foreach (Building_Bed building_Bed in ownedRoom.ContainedBeds)
			{
				if (building_Bed != ownedBed && building_Bed.OwnersForReading.Any((Pawn p) => p != this.pawn && !p.RaceProps.Animal && !LovePartnerRelationUtility.LovePartnerRelationExists(p, this.pawn)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x0017EBCC File Offset: 0x0017CDCC
		public void ExposeData()
		{
			Scribe_Collections.Look<RoyalTitle>(ref this.titles, "titles", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction, int>(ref this.favor, "favor", LookMode.Reference, LookMode.Value, ref this.tmpFavorFactions, ref this.tmpAmounts);
			Scribe_Values.Look<int>(ref this.lastDecreeTicks, "lastDecreeTicks", -999999, false);
			Scribe_Collections.Look<Faction, RoyalTitleDef>(ref this.highestTitles, "highestTitles", LookMode.Reference, LookMode.Def, ref this.tmpHighestTitleFactions, ref this.tmpTitleDefs);
			Scribe_Collections.Look<Faction, Pawn>(ref this.heirs, "heirs", LookMode.Reference, LookMode.Reference, ref this.tmpHeirFactions, ref this.tmpPawns);
			Scribe_Collections.Look<RoyalTitlePermitDef, int>(ref this.permitLastUsedTick, "permitLastUsed", LookMode.Def, LookMode.Value, ref this.tmpPermitDefs, ref this.tmlPermitLastUsed);
			Scribe_Values.Look<bool>(ref this.allowRoomRequirements, "allowRoomRequirements", true, false);
			Scribe_Values.Look<bool>(ref this.allowApparelRequirements, "allowApparelRequirements", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.titles.RemoveAll((RoyalTitle x) => x.def == null) != 0)
				{
					Log.Error("Some RoyalTitles had null defs after loading.", false);
				}
				if (this.heirs == null)
				{
					this.heirs = new Dictionary<Faction, Pawn>();
				}
				if (this.permitLastUsedTick == null)
				{
					this.permitLastUsedTick = new Dictionary<RoyalTitlePermitDef, int>();
				}
				foreach (RoyalTitle royalTitle in this.titles)
				{
					this.AssignHeirIfNone(royalTitle.def, royalTitle.faction);
				}
				for (int i = 0; i < this.AllTitlesInEffectForReading.Count; i++)
				{
					RoyalTitle royalTitle2 = this.AllTitlesInEffectForReading[i];
					for (int j = 0; j < royalTitle2.def.grantedAbilities.Count; j++)
					{
						this.pawn.abilities.GainAbility(royalTitle2.def.grantedAbilities[j]);
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0400288B RID: 10379
		public Pawn pawn;

		// Token: 0x0400288C RID: 10380
		private List<RoyalTitle> titles = new List<RoyalTitle>();

		// Token: 0x0400288D RID: 10381
		private Dictionary<Faction, int> favor = new Dictionary<Faction, int>();

		// Token: 0x0400288E RID: 10382
		private Dictionary<Faction, RoyalTitleDef> highestTitles = new Dictionary<Faction, RoyalTitleDef>();

		// Token: 0x0400288F RID: 10383
		private Dictionary<Faction, Pawn> heirs = new Dictionary<Faction, Pawn>();

		// Token: 0x04002890 RID: 10384
		private Dictionary<RoyalTitlePermitDef, int> permitLastUsedTick = new Dictionary<RoyalTitlePermitDef, int>();

		// Token: 0x04002891 RID: 10385
		public int lastDecreeTicks = -999999;

		// Token: 0x04002892 RID: 10386
		public bool allowRoomRequirements = true;

		// Token: 0x04002893 RID: 10387
		public bool allowApparelRequirements = true;

		// Token: 0x04002894 RID: 10388
		private static List<RoyalTitle> EmptyTitles = new List<RoyalTitle>();

		// Token: 0x04002895 RID: 10389
		private List<string> tmpDecreeTags = new List<string>();

		// Token: 0x04002896 RID: 10390
		private List<Faction> factionHeirsToClearTmp = new List<Faction>();

		// Token: 0x04002897 RID: 10391
		private static List<Action> tmpInheritedTitles = new List<Action>();

		// Token: 0x04002898 RID: 10392
		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);

		// Token: 0x04002899 RID: 10393
		private List<Faction> tmpFavorFactions;

		// Token: 0x0400289A RID: 10394
		private List<Faction> tmpHighestTitleFactions;

		// Token: 0x0400289B RID: 10395
		private List<Faction> tmpHeirFactions;

		// Token: 0x0400289C RID: 10396
		private List<int> tmpAmounts;

		// Token: 0x0400289D RID: 10397
		private List<int> tmlPermitLastUsed;

		// Token: 0x0400289E RID: 10398
		private List<Pawn> tmpPawns;

		// Token: 0x0400289F RID: 10399
		private List<RoyalTitleDef> tmpTitleDefs;

		// Token: 0x040028A0 RID: 10400
		private List<RoyalTitlePermitDef> tmpPermitDefs;
	}
}
