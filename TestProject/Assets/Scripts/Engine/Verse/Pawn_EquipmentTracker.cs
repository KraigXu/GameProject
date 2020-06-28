using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200029C RID: 668
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x0006C88C File Offset: 0x0006AA8C
		// (set) Token: 0x060012F7 RID: 4855 RVA: 0x0006C8D8 File Offset: 0x0006AAD8
		public ThingWithComps Primary
		{
			get
			{
				for (int i = 0; i < this.equipment.Count; i++)
				{
					if (this.equipment[i].def.equipmentType == EquipmentType.Primary)
					{
						return this.equipment[i];
					}
				}
				return null;
			}
			private set
			{
				if (this.Primary == value)
				{
					return;
				}
				if (value != null && value.def.equipmentType != EquipmentType.Primary)
				{
					Log.Error("Tried to set non-primary equipment as primary.", false);
					return;
				}
				if (this.Primary != null)
				{
					this.equipment.Remove(this.Primary);
				}
				if (value != null)
				{
					this.equipment.TryAdd(value, true);
				}
				if (this.pawn.drafter != null)
				{
					this.pawn.drafter.Notify_PrimaryWeaponChanged();
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0006C954 File Offset: 0x0006AB54
		public CompEquippable PrimaryEq
		{
			get
			{
				if (this.Primary == null)
				{
					return null;
				}
				return this.Primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0006C96B File Offset: 0x0006AB6B
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0006C978 File Offset: 0x0006AB78
		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					ThingWithComps thingWithComps = list[i];
					List<Verb> verbs = thingWithComps.GetComp<CompEquippable>().AllVerbs;
					for (int j = 0; j < verbs.Count; j = num + 1)
					{
						yield return verbs[j];
						num = j;
					}
					verbs = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0006C988 File Offset: 0x0006AB88
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0006C990 File Offset: 0x0006AB90
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0006C9AC File Offset: 0x0006ABAC
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					foreach (Verb verb in allEquipmentListForReading[i].GetComp<CompEquippable>().AllVerbs)
					{
						verb.caster = this.pawn;
					}
				}
			}
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0006CA48 File Offset: 0x0006AC48
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				allEquipmentListForReading[i].GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0006CA83 File Offset: 0x0006AC83
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0006CA90 File Offset: 0x0006AC90
		public void MakeRoomFor(ThingWithComps eq)
		{
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				ThingWithComps thingWithComps;
				if (this.TryDropEquipment(this.Primary, out thingWithComps, this.pawn.Position, true))
				{
					if (thingWithComps != null)
					{
						thingWithComps.SetForbidden(false, true);
						return;
					}
				}
				else
				{
					Log.Error(this.pawn + " couldn't make room for equipment " + eq, false);
				}
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0006CAF2 File Offset: 0x0006ACF2
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0006CB04 File Offset: 0x0006AD04
		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			if (!pos.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" tried to drop ",
					eq,
					" at invalid cell."
				}), false);
				resultingEq = null;
				return false;
			}
			if (this.equipment.TryDrop(eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, null, null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0006CB7C File Offset: 0x0006AD7C
		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				ThingWithComps thingWithComps;
				this.TryDropEquipment(this.equipment[i], out thingWithComps, pos, forbid);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0006CBB8 File Offset: 0x0006ADB8
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0006CBC8 File Offset: 0x0006ADC8
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.", false);
				return;
			}
			this.Remove(eq);
			eq.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0006CBFD File Offset: 0x0006ADFD
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0006CC0B File Offset: 0x0006AE0B
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0006CC19 File Offset: 0x0006AE19
		internal void Notify_PrimaryDestroyed()
		{
			if (this.Primary != null)
			{
				this.Remove(this.Primary);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0006CC4C File Offset: 0x0006AE4C
		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Pawn ",
					this.pawn.LabelCap,
					" got primaryInt equipment ",
					newEq,
					" while already having primaryInt equipment ",
					this.Primary
				}), false);
				return;
			}
			this.equipment.TryAdd(newEq, true);
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0006CCC2 File Offset: 0x0006AEC2
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					ThingWithComps thingWithComps = list[i];
					foreach (Command command in thingWithComps.GetComp<CompEquippable>().GetVerbsCommands())
					{
						switch (i)
						{
						case 0:
							command.hotKey = KeyBindingDefOf.Misc1;
							break;
						case 1:
							command.hotKey = KeyBindingDefOf.Misc2;
							break;
						case 2:
							command.hotKey = KeyBindingDefOf.Misc3;
							break;
						}
						yield return command;
					}
					IEnumerator<Command> enumerator = null;
					num = i;
				}
				list = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0006CCD4 File Offset: 0x0006AED4
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
			eq.Notify_Equipped(this.pawn);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0006CD44 File Offset: 0x0006AF44
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.GetComp<CompEquippable>().Notify_EquipmentLost();
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0006CD54 File Offset: 0x0006AF54
		public void Notify_PawnSpawned()
		{
			if (this.HasAnything() && this.pawn.Downed && this.pawn.GetPosture() != PawnPosture.LayingInBed)
			{
				if (this.pawn.kindDef.destroyGearOnDrop)
				{
					this.DestroyAllEquipment(DestroyMode.Vanish);
					return;
				}
				this.DropAllEquipment(this.pawn.Position, true);
			}
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0006CDB0 File Offset: 0x0006AFB0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0006CDB8 File Offset: 0x0006AFB8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04000CF7 RID: 3319
		public Pawn pawn;

		// Token: 0x04000CF8 RID: 3320
		private ThingOwner<ThingWithComps> equipment;
	}
}
