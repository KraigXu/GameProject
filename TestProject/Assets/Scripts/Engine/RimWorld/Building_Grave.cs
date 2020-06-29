using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Building_Grave : Building_Casket, IStoreSettingsParent, IHaulDestination
	{
		
		// (get) Token: 0x06004CD3 RID: 19667 RVA: 0x0019C303 File Offset: 0x0019A503
		public Pawn AssignedPawn
		{
			get
			{
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		
		// (get) Token: 0x06004CD4 RID: 19668 RVA: 0x0019C332 File Offset: 0x0019A532
		public CompAssignableToPawn_Grave CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Grave>();
			}
		}

		
		// (get) Token: 0x06004CD5 RID: 19669 RVA: 0x0019C33C File Offset: 0x0019A53C
		public override Graphic Graphic
		{
			get
			{
				if (!this.HasCorpse)
				{
					return base.Graphic;
				}
				if (this.def.building.fullGraveGraphicData == null)
				{
					return base.Graphic;
				}
				if (this.cachedGraphicFull == null)
				{
					this.cachedGraphicFull = this.def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return this.cachedGraphicFull;
			}
		}

		
		// (get) Token: 0x06004CD6 RID: 19670 RVA: 0x0019C39B File Offset: 0x0019A59B
		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		
		// (get) Token: 0x06004CD7 RID: 19671 RVA: 0x0019C3A8 File Offset: 0x0019A5A8
		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		
		// (get) Token: 0x06004CD8 RID: 19672 RVA: 0x0019C3E3 File Offset: 0x0019A5E3
		public bool StorageTabVisible
		{
			get
			{
				return this.AssignedPawn == null && !this.HasCorpse;
			}
		}

		
		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		
		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		
		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		
		public virtual void Notify_CorpseBuried(Pawn worker)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active)
			{
				comp.JustCreatedBy(worker);
				comp.InitializeArt(this.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
			worker.records.Increment(RecordDefOf.CorpsesBuried);
			TaleRecorder.RecordTale(TaleDefOf.BuriedCorpse, new object[]
			{
				worker,
				(this.Corpse != null) ? this.Corpse.InnerPawn : null
			});
		}

		
		public override bool Accepts(Thing thing)
		{
			if (!base.Accepts(thing))
			{
				return false;
			}
			if (this.HasCorpse)
			{
				return false;
			}
			if (this.AssignedPawn != null)
			{
				Corpse corpse = thing as Corpse;
				if (corpse == null)
				{
					return false;
				}
				if (corpse.InnerPawn != this.AssignedPawn)
				{
					return false;
				}
			}
			else if (!this.storageSettings.AllowedToAccept(thing))
			{
				return false;
			}
			return true;
		}

		
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != this)
				{
					corpse.InnerPawn.ownership.UnclaimGrave();
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				return true;
			}
			return false;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			if (this.StorageTabVisible)
			{
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.storageSettings))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.HasCorpse)
			{
				if (base.Tile != -1)
				{
					string value = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
					stringBuilder.AppendLine();
					stringBuilder.Append("DiedOn".Translate(value));
				}
			}
			else if (this.AssignedPawn != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AssignedColonist".Translate());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.AssignedPawn.LabelCap);
			}
			return stringBuilder.ToString();
		}

		
		private StorageSettings storageSettings;

		
		private Graphic cachedGraphicFull;
	}
}
