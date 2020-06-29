using System;
using UnityEngine;

namespace Verse
{
	
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x000C0A39 File Offset: 0x000BEC39
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x000C0A50 File Offset: 0x000BEC50
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x000C0A5B File Offset: 0x000BEC5B
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x000C0A63 File Offset: 0x000BEC63
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x000C0A7A File Offset: 0x000BEC7A
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		
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

		
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000C0B20 File Offset: 0x000BED20
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		
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

		
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		
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

		
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			if (t.HasThing)
			{
				return new LocalTargetInfo(t.Thing);
			}
			return new LocalTargetInfo(t.Cell);
		}

		
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
		}

		
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
		}

		
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

		
		private Thing thingInt;

		
		private IntVec3 cellInt;

		
		private Map mapInt;
	}
}
