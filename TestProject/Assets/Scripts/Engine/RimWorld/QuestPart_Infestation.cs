using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A6 RID: 2470
	public class QuestPart_Infestation : QuestPart
	{
		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x0013695A File Offset: 0x00134B5A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				if (this.mapParent != null && this.mapParent.HasMap && this.loc.IsValid)
				{
					yield return new TargetInfo(this.loc, this.mapParent.Map, false);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x0013696A File Offset: 0x00134B6A
		public override string QuestSelectTargetsLabel
		{
			get
			{
				return "SelectHiveTargets".Translate();
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x0013697B File Offset: 0x00134B7B
		public override IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__1())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					List<Thing> hives = this.mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.Hive);
					int num;
					for (int i = 0; i < hives.Count; i = num + 1)
					{
						Hive hive;
						if ((hive = (hives[i] as Hive)) != null && !hive.questTags.NullOrEmpty<string>() && hive.questTags.Contains(this.tag))
						{
							yield return hive;
						}
						num = i;
					}
					hives = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x0013698C File Offset: 0x00134B8C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.loc = IntVec3.Invalid;
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, this.mapParent.Map, true, true, this.tag);
					if (thing != null)
					{
						this.loc = thing.Position;
						if (this.sendStandardLetter)
						{
							TaggedString label = this.customLetterLabel.NullOrEmpty() ? IncidentDefOf.Infestation.letterLabel : this.customLetterLabel.Formatted(IncidentDefOf.Infestation.letterLabel.Named("BASELABEL"));
							TaggedString text = this.customLetterText.NullOrEmpty() ? IncidentDefOf.Infestation.letterText : this.customLetterText.Formatted(IncidentDefOf.Infestation.letterText.Named("BASETEXT"));
							Find.LetterStack.ReceiveLetter(label, text, this.customLetterDef ?? IncidentDefOf.Infestation.letterDef, new TargetInfo(this.loc, this.mapParent.Map, false), null, this.quest, null, null);
						}
					}
				}
			}
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00136ADC File Offset: 0x00134CDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00136B96 File Offset: 0x00134D96
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.hivesCount = 5;
			}
		}

		// Token: 0x0400229D RID: 8861
		public string inSignal;

		// Token: 0x0400229E RID: 8862
		public int hivesCount;

		// Token: 0x0400229F RID: 8863
		public MapParent mapParent;

		// Token: 0x040022A0 RID: 8864
		public string tag;

		// Token: 0x040022A1 RID: 8865
		public string customLetterText;

		// Token: 0x040022A2 RID: 8866
		public string customLetterLabel;

		// Token: 0x040022A3 RID: 8867
		public LetterDef customLetterDef;

		// Token: 0x040022A4 RID: 8868
		public bool sendStandardLetter = true;

		// Token: 0x040022A5 RID: 8869
		private IntVec3 loc = IntVec3.Invalid;
	}
}
