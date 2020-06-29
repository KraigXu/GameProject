using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0005F939 File Offset: 0x0005DB39
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0005F946 File Offset: 0x0005DB46
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0005F94E File Offset: 0x0005DB4E
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0005F95B File Offset: 0x0005DB5B
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x0005F968 File Offset: 0x0005DB68
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0005F970 File Offset: 0x0005DB70
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Hediff;
			}
		}

		
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbTracker == null)
			{
				this.verbTracker = new VerbTracker(this);
			}
		}

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this);
		}

		
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		
		public VerbTracker verbTracker;
	}
}
