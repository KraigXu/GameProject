using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000964 RID: 2404
	public class QuestPart_AddHediff : QuestPart
	{
		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060038F0 RID: 14576 RVA: 0x0012FAEC File Offset: 0x0012DCEC
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060038F1 RID: 14577 RVA: 0x0012FAFC File Offset: 0x0012DCFC
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.<>n__1())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				if (this.addToHyperlinks)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.hediffDef, -1);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0012FB0C File Offset: 0x0012DD0C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull() && (!this.checkDiseaseContractChance || Rand.Chance(this.pawns[i].health.immunity.DiseaseContractChanceFactor(this.hediffDef, null))))
					{
						HediffGiverUtility.TryApply(this.pawns[i], this.hediffDef, this.partsToAffect, false, 1, null);
					}
				}
			}
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x0012FBB0 File Offset: 0x0012DDB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<BodyPartDef>(ref this.partsToAffect, "partsToAffect", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Values.Look<bool>(ref this.checkDiseaseContractChance, "checkDiseaseContractChance", false, false);
			Scribe_Values.Look<bool>(ref this.addToHyperlinks, "addToHyperlinks", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0012FC68 File Offset: 0x0012DE68
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.hediffDef = HediffDefOf.Anesthetic;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0012FCB5 File Offset: 0x0012DEB5
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400218F RID: 8591
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002190 RID: 8592
		public List<BodyPartDef> partsToAffect;

		// Token: 0x04002191 RID: 8593
		public string inSignal;

		// Token: 0x04002192 RID: 8594
		public HediffDef hediffDef;

		// Token: 0x04002193 RID: 8595
		public bool checkDiseaseContractChance;

		// Token: 0x04002194 RID: 8596
		public bool addToHyperlinks;
	}
}
