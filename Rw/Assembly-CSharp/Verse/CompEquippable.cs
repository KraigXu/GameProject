using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031E RID: 798
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x000856AC File Offset: 0x000838AC
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x000856B9 File Offset: 0x000838B9
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x000856C6 File Offset: 0x000838C6
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x000856D3 File Offset: 0x000838D3
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x000856DB File Offset: 0x000838DB
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x000856ED File Offset: 0x000838ED
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00019EA1 File Offset: 0x000180A1
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000856FF File Offset: 0x000838FF
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00085706 File Offset: 0x00083906
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0008571A File Offset: 0x0008391A
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00085728 File Offset: 0x00083928
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0008577A File Offset: 0x0008397A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0008579C File Offset: 0x0008399C
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x000857B0 File Offset: 0x000839B0
		public void Notify_EquipmentLost()
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x000857E1 File Offset: 0x000839E1
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x000857F8 File Offset: 0x000839F8
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		// Token: 0x04000EA3 RID: 3747
		public VerbTracker verbTracker;
	}
}
