using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	
	public class DeathLetter : ChoiceLetter
	{
		
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x000A6768 File Offset: 0x000A4968
		protected DiaOption Option_ReadMore
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("ReadMore".Translate());
				diaOption.action = delegate
				{
					CameraJumper.TryJumpAndSelect(target);
					Find.LetterStack.RemoveLetter(this);
					InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
				};
				diaOption.resolveTree = true;
				if (!target.IsValid)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x000A67D6 File Offset: 0x000A49D6
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
				if (this.lookTargets.IsValid())
				{
					yield return this.Option_ReadMore;
				}
				if (this.quest != null)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", false);
				}
				yield break;
			}
		}

		
		public override void OpenLetter()
		{
			Pawn targetPawn = this.lookTargets.TryGetPrimaryTarget().Thing as Pawn;
			TaggedString taggedString = this.text;
			
			string text = (from entry in (from entry in (from battle in Find.BattleLog.Battles
			where battle.Concerns(targetPawn)
			select battle).SelectMany(delegate(Battle battle)
			{
				IEnumerable<LogEntry> entries = battle.Entries;
				Func<LogEntry, bool> predicate;
				if ((predicate ) == null)
				{
					predicate = (9__4 = ((LogEntry entry) => entry.Concerns(targetPawn) && entry.ShowInCompactView()));
				}
				return entries.Where(predicate);
			})
			orderby entry.Age
			select entry).Take(5).Reverse<LogEntry>()
			select "  " + entry.ToGameStringFromPOV(null, false)).ToLineList(null, false);
			if (text.Length > 0)
			{
				taggedString = string.Format("{0}\n\n{1}\n{2}", taggedString, "LastEventsInLife".Translate(targetPawn.LabelDefinite(), targetPawn.Named("PAWN")).Resolve() + ":", text);
			}
			DiaNode diaNode = new DiaNode(taggedString);
			diaNode.options.AddRange(this.Choices);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title));
		}
	}
}
