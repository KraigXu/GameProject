using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040D RID: 1037
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000BEBAE File Offset: 0x000BCDAE
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000BEBC5 File Offset: 0x000BCDC5
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000BEBD0 File Offset: 0x000BCDD0
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000BEBD8 File Offset: 0x000BCDD8
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x000BEBE5 File Offset: 0x000BCDE5
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000BEBFC File Offset: 0x000BCDFC
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x000BEC08 File Offset: 0x000BCE08
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000BEC2D File Offset: 0x000BCE2D
		public IntVec3 Cell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.PositionHeld;
				}
				return this.cellInt;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x000BEC4C File Offset: 0x000BCE4C
		public Vector3 CenterVector3
		{
			get
			{
				if (this.thingInt != null)
				{
					if (this.thingInt.Spawned)
					{
						return this.thingInt.DrawPos;
					}
					if (this.thingInt.SpawnedOrAnyParentSpawned)
					{
						return this.thingInt.PositionHeld.ToVector3Shifted();
					}
					return this.thingInt.Position.ToVector3Shifted();
				}
				else
				{
					if (this.cellInt.IsValid)
					{
						return this.cellInt.ToVector3Shifted();
					}
					return default(Vector3);
				}
			}
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000BECD1 File Offset: 0x000BCED1
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000BECE5 File Offset: 0x000BCEE5
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000BECF5 File Offset: 0x000BCEF5
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000BECFD File Offset: 0x000BCEFD
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000BED05 File Offset: 0x000BCF05
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000BED31 File Offset: 0x000BCF31
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000BED67 File Offset: 0x000BCF67
		public TargetInfo ToTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new TargetInfo(this.Thing);
			}
			return new TargetInfo(this.Cell, map, false);
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000BED98 File Offset: 0x000BCF98
		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new GlobalTargetInfo(this.Thing);
			}
			return new GlobalTargetInfo(this.Cell, map, false);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000BEDCC File Offset: 0x000BCFCC
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000BEE2B File Offset: 0x000BD02B
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000BEE37 File Offset: 0x000BD037
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000BEE4F File Offset: 0x000BD04F
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000BEE5D File Offset: 0x000BD05D
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000BEE84 File Offset: 0x000BD084
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString();
			}
			return "null";
		}

		// Token: 0x040012EA RID: 4842
		private Thing thingInt;

		// Token: 0x040012EB RID: 4843
		private IntVec3 cellInt;
	}
}
