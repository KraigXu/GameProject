using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Infestation : QuestPart
	{
		
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x0013695A File Offset: 0x00134B5A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x0013696A File Offset: 0x00134B6A
		public override string QuestSelectTargetsLabel
		{
			get
			{
				return "SelectHiveTargets".Translate();
			}
		}

		
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x0013697B File Offset: 0x00134B7B
		public override IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__1())
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

		
		public string inSignal;

		
		public int hivesCount;

		
		public MapParent mapParent;

		
		public string tag;

		
		public string customLetterText;

		
		public string customLetterLabel;

		
		public LetterDef customLetterDef;

		
		public bool sendStandardLetter = true;

		
		private IntVec3 loc = IntVec3.Invalid;
	}
}
