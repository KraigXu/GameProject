using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class CompEquippable : ThingComp, IVerbOwner
	{
		
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x000856AC File Offset: 0x000838AC
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x000856B9 File Offset: 0x000838B9
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x000856C6 File Offset: 0x000838C6
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x000856D3 File Offset: 0x000838D3
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x000856DB File Offset: 0x000838DB
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x000856ED File Offset: 0x000838ED
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00019EA1 File Offset: 0x000180A1
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000856FF File Offset: 0x000838FF
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		
		public void Notify_EquipmentLost()
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		
		public VerbTracker verbTracker;
	}
}
