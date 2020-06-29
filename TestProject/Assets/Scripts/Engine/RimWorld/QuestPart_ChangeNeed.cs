using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_ChangeNeed : QuestPart
	{
		
		// (get) Token: 0x06003916 RID: 14614 RVA: 0x00130289 File Offset: 0x0012E489
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				Need need = this.pawn.needs.TryGetNeed(this.need);
				if (need != null)
				{
					need.CurLevel += this.offset;
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<float>(ref this.offset, "offset", 0f, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.need = NeedDefOf.Food;
			this.offset = 0.5f;
			if (Find.AnyPlayerHomeMap != null)
			{
				Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		
		public string inSignal;

		
		public Pawn pawn;

		
		public NeedDef need;

		
		public float offset;
	}
}
