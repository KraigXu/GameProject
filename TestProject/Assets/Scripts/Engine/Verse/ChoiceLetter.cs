using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		
		
		public abstract IEnumerable<DiaOption> Choices { get; }

		
		
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

		
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		
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

		
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		
		public string title;

		
		public TaggedString text;

		
		public bool radioMode;

		
		public Quest quest;

		
		public List<ThingDef> hyperlinkThingDefs;

		
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
