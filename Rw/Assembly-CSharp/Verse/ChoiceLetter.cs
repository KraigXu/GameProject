using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200039A RID: 922
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001B10 RID: 6928
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x000A63EE File Offset: 0x000A45EE
		protected DiaOption Option_Close
		{
			get
			{
				return new DiaOption("Close".Translate())
				{
					action = delegate
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x000A6420 File Offset: 0x000A4620
		protected DiaOption Option_JumpToLocation
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate
				{
					CameraJumper.TryJumpAndSelect(target);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x000A6490 File Offset: 0x000A4690
		protected DiaOption Option_JumpToLocationAndPostpone
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate
				{
					CameraJumper.TryJumpAndSelect(target);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000A64F7 File Offset: 0x000A46F7
		protected DiaOption Option_Reject
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x000A6528 File Offset: 0x000A4728
		protected DiaOption Option_Postpone
		{
			get
			{
				DiaOption diaOption = new DiaOption("PostponeLetter".Translate());
				diaOption.resolveTree = true;
				if (base.TimeoutActive && this.disappearAtTick <= Find.TickManager.TicksGame + 1)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000A6578 File Offset: 0x000A4778
		protected DiaOption Option_ViewInQuestsTab(string labelKey = "ViewRelatedQuest", bool postpone = false)
		{
			TaggedString taggedString = labelKey.Translate();
			if (this.title != this.quest.name)
			{
				taggedString += ": " + this.quest.name;
			}
			DiaOption diaOption = new DiaOption(taggedString);
			diaOption.action = delegate
			{
				if (this.quest != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					if (!postpone)
					{
						Find.LetterStack.RemoveLetter(this);
					}
				}
			};
			diaOption.resolveTree = true;
			if (this.quest == null)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000A660C File Offset: 0x000A480C
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000A6668 File Offset: 0x000A4868
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Collections.Look<ThingDef>(ref this.hyperlinkThingDefs, "hyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.hyperlinkHediffDefs, "hyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000A66F6 File Offset: 0x000A48F6
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000A6704 File Offset: 0x000A4904
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		// Token: 0x04001018 RID: 4120
		public string title;

		// Token: 0x04001019 RID: 4121
		public TaggedString text;

		// Token: 0x0400101A RID: 4122
		public bool radioMode;

		// Token: 0x0400101B RID: 4123
		public Quest quest;

		// Token: 0x0400101C RID: 4124
		public List<ThingDef> hyperlinkThingDefs;

		// Token: 0x0400101D RID: 4125
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
