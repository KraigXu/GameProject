using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_AddHediff : QuestPart
	{
		
		
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{

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

		
		
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{


				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				if (this.addToHyperlinks)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.hediffDef, -1);
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
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull() && (!this.checkDiseaseContractChance || Rand.Chance(this.pawns[i].health.immunity.DiseaseContractChanceFactor(this.hediffDef, null))))
					{
						HediffGiverUtility.TryApply(this.pawns[i], this.hediffDef, this.partsToAffect, false, 1, null);
					}
				}
			}
		}

		
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

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.hediffDef = HediffDefOf.Anesthetic;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public List<Pawn> pawns = new List<Pawn>();

		
		public List<BodyPartDef> partsToAffect;

		
		public string inSignal;

		
		public HediffDef hediffDef;

		
		public bool checkDiseaseContractChance;

		
		public bool addToHyperlinks;
	}
}
