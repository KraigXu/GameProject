using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200015C RID: 348
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x060009BC RID: 2492 RVA: 0x00034D3E File Offset: 0x00032F3E
		public DesignationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00034D58 File Offset: 0x00032F58
		public void ExposeData()
		{
			Scribe_Collections.Look<Designation>(ref this.allDesignations, "allDesignations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.", false);
				}
				if (this.allDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.", false);
				}
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					this.allDesignations[i].designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.allDesignations.Count - 1; j >= 0; j--)
				{
					TargetType targetType = this.allDesignations[j].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType == TargetType.Cell)
						{
							if (!this.allDesignations[j].target.Cell.IsValid)
							{
								Log.Error("Cell-needing designation " + this.allDesignations[j] + " had no cell target. Removing...", false);
								this.allDesignations.RemoveAt(j);
							}
						}
					}
					else if (!this.allDesignations[j].target.HasThing)
					{
						Log.Error("Thing-needing designation " + this.allDesignations[j] + " had no thing target. Removing...", false);
						this.allDesignations.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00034EFC File Offset: 0x000330FC
		public void DrawDesignations()
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				if (!this.allDesignations[i].target.HasThing || this.allDesignations[i].target.Thing.Map == this.map)
				{
					this.allDesignations[i].DesignationDraw();
				}
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00034F6C File Offset: 0x0003316C
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target, false);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target, false);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing)
			{
				newDes.target.Thing.SetForbidden(false, false);
			}
			this.allDesignations.Add(newDes);
			newDes.designationManager = this;
			newDes.Notify_Added();
			Map map = newDes.target.HasThing ? newDes.target.Thing.Map : this.map;
			if (map != null)
			{
				MoteMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00035070 File Offset: 0x00033270
		public Designation DesignationOn(Thing t)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000350B4 File Offset: 0x000332B4
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.", false);
				return null;
			}
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t && designation.def == def)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00035124 File Offset: 0x00033324
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.", false);
				return null;
			}
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x000351BD File Offset: 0x000333BD
		public IEnumerable<Designation> AllDesignationsOn(Thing t)
		{
			int count = this.allDesignations.Count;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				if (this.allDesignations[i].target.Thing == t)
				{
					yield return this.allDesignations[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000351D4 File Offset: 0x000333D4
		public IEnumerable<Designation> AllDesignationsAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Designation designation = this.allDesignations[i];
				if ((!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
				{
					yield return designation;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000351EC File Offset: 0x000333EC
		public bool HasMapDesignationAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (!designation.target.HasThing && designation.target.Cell == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00035241 File Offset: 0x00033441
		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					yield return designation;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00035258 File Offset: 0x00033458
		public bool AnySpawnedDesignationOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000352BB File Offset: 0x000334BB
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000352D0 File Offset: 0x000334D0
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x000352F0 File Offset: 0x000334F0
		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if ((!standardCanceling || designation.def.designateCancelable) && designation.target.Thing == t)
				{
					designation.Notify_Removing();
				}
			}
			this.allDesignations.RemoveAll((Designation d) => (!standardCanceling || d.def.designateCancelable) && d.target.Thing == t);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003537C File Offset: 0x0003357C
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0003539C File Offset: 0x0003359C
		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				if (this.allDesignations[i].def == def)
				{
					this.allDesignations[i].Notify_Removing();
					this.allDesignations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x000353F4 File Offset: 0x000335F4
		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				Designation designation = this.allDesignations[i];
				if (cellRect.Contains(designation.target.Cell) && designation.def.removeIfBuildingDespawned)
				{
					this.RemoveDesignation(designation);
				}
			}
		}

		// Token: 0x040007FB RID: 2043
		public Map map;

		// Token: 0x040007FC RID: 2044
		public List<Designation> allDesignations = new List<Designation>();
	}
}
