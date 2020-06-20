using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000274 RID: 628
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0005F939 File Offset: 0x0005DB39
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0005F946 File Offset: 0x0005DB46
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0005F94E File Offset: 0x0005DB4E
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0005F95B File Offset: 0x0005DB5B
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x0005F968 File Offset: 0x0005DB68
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0005F970 File Offset: 0x0005DB70
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Hediff;
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0005F977 File Offset: 0x0005DB77
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0005F98B File Offset: 0x0005DB8B
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

		// Token: 0x060010DE RID: 4318 RVA: 0x0005F9C9 File Offset: 0x0005DBC9
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0005F9DD File Offset: 0x0005DBDD
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0005FA0A File Offset: 0x0005DC0A
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		// Token: 0x04000C3F RID: 3135
		public VerbTracker verbTracker;
	}
}
