using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000416 RID: 1046
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x000C0A39 File Offset: 0x000BEC39
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x000C0A50 File Offset: 0x000BEC50
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x000C0A5B File Offset: 0x000BEC5B
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x000C0A63 File Offset: 0x000BEC63
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x000C0A7A File Offset: 0x000BEC7A
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x000C0A88 File Offset: 0x000BEC88
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

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x000C0AAD File Offset: 0x000BECAD
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

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x000C0ACC File Offset: 0x000BECCC
		public int Tile
		{
			get
			{
				if (this.thingInt != null && this.thingInt.Tile >= 0)
				{
					return this.thingInt.Tile;
				}
				if (this.cellInt.IsValid && this.mapInt != null)
				{
					return this.mapInt.Tile;
				}
				return -1;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000C0B20 File Offset: 0x000BED20
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x000C0B40 File Offset: 0x000BED40
		public Map Map
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.MapHeld;
				}
				return this.mapInt;
			}
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000C0B5C File Offset: 0x000BED5C
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000C0B78 File Offset: 0x000BED78
		public TargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed TargetInfo with cell=" + cell + " and a null map.", false);
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000C0BC4 File Offset: 0x000BEDC4
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000C0BCC File Offset: 0x000BEDCC
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			if (t.HasThing)
			{
				return new LocalTargetInfo(t.Thing);
			}
			return new LocalTargetInfo(t.Cell);
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000C0BF0 File Offset: 0x000BEDF0
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000C0C1C File Offset: 0x000BEE1C
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000C0C54 File Offset: 0x000BEE54
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000C0CC5 File Offset: 0x000BEEC5
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000C0CD1 File Offset: 0x000BEED1
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000C0CE9 File Offset: 0x000BEEE9
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000C0CF7 File Offset: 0x000BEEF7
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000C0D2C File Offset: 0x000BEF2C
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			return "null";
		}

		// Token: 0x04001313 RID: 4883
		private Thing thingInt;

		// Token: 0x04001314 RID: 4884
		private IntVec3 cellInt;

		// Token: 0x04001315 RID: 4885
		private Map mapInt;
	}
}
