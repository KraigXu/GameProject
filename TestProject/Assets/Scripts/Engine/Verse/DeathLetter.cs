using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200039B RID: 923
	public class DeathLetter : ChoiceLetter
	{
		// Token: 0x17000532 RID: 1330
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

		// Token: 0x17000533 RID: 1331
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

		// Token: 0x06001B20 RID: 6944 RVA: 0x000A67E8 File Offset: 0x000A49E8
		public override void OpenLetter()
		{
			Pawn targetPawn = this.lookTargets.TryGetPrimaryTarget().Thing as Pawn;
			TaggedString taggedString = this.text;
			Func<LogEntry, bool> <>9__4;
			string text = (from entry in (from entry in (from battle in Find.BattleLog.Battles
			where battle.Concerns(targetPawn)
			select battle).SelectMany(delegate(Battle battle)
			{
				IEnumerable<LogEntry> entries = battle.Entries;
				Func<LogEntry, bool> predicate;
				if ((predicate = <>9__4) == null)
				{
					predicate = (<>9__4 = ((LogEntry entry) => entry.Concerns(targetPawn) && entry.ShowInCompactView()));
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
