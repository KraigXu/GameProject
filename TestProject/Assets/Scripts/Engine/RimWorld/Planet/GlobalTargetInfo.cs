using System;
using Verse;

namespace RimWorld.Planet
{
	
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		
		// (get) Token: 0x0600699E RID: 27038 RVA: 0x0024DA19 File Offset: 0x0024BC19
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid || this.worldObjectInt != null || this.tileInt >= 0;
			}
		}

		
		// (get) Token: 0x0600699F RID: 27039 RVA: 0x0024DA46 File Offset: 0x0024BC46
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		
		// (get) Token: 0x060069A0 RID: 27040 RVA: 0x0024DA5D File Offset: 0x0024BC5D
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt >= 0;
			}
		}

		
		// (get) Token: 0x060069A1 RID: 27041 RVA: 0x0024DA75 File Offset: 0x0024BC75
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		
		// (get) Token: 0x060069A2 RID: 27042 RVA: 0x0024DA80 File Offset: 0x0024BC80
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		
		// (get) Token: 0x060069A3 RID: 27043 RVA: 0x0024DA88 File Offset: 0x0024BC88
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		
		// (get) Token: 0x060069A4 RID: 27044 RVA: 0x0024DA9F File Offset: 0x0024BC9F
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		
		// (get) Token: 0x060069A5 RID: 27045 RVA: 0x0024DAAA File Offset: 0x0024BCAA
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		
		// (get) Token: 0x060069A6 RID: 27046 RVA: 0x0024DAB2 File Offset: 0x0024BCB2
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		
		// (get) Token: 0x060069A7 RID: 27047 RVA: 0x0024DAC0 File Offset: 0x0024BCC0
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		
		// (get) Token: 0x060069A8 RID: 27048 RVA: 0x0024DAF9 File Offset: 0x0024BCF9
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

		
		// (get) Token: 0x060069A9 RID: 27049 RVA: 0x0024DB15 File Offset: 0x0024BD15
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

		
		// (get) Token: 0x060069AA RID: 27050 RVA: 0x0024DB34 File Offset: 0x0024BD34
		public int Tile
		{
			get
			{
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.Tile;
				}
				if (this.tileInt >= 0)
				{
					return this.tileInt;
				}
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

		
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		
		public GlobalTargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed GlobalTargetInfo with cell=" + cell + " and a null map.", false);
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = -1;
		}

		
		public GlobalTargetInfo(int tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		
		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			if (target.HasThing)
			{
				return new GlobalTargetInfo(target.Thing);
			}
			return new GlobalTargetInfo(target.Cell, target.Map, false);
		}

		
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		
		public static explicit operator LocalTargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject " + targ.worldObjectInt, 134566, false);
				return LocalTargetInfo.Invalid;
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt, 7833122, false);
				return LocalTargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return LocalTargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new LocalTargetInfo(targ.thingInt);
			}
			return new LocalTargetInfo(targ.cellInt);
		}

		
		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had WorldObject " + targ.worldObjectInt, 134566, false);
				return TargetInfo.Invalid;
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt, 7833122, false);
				return TargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new TargetInfo(targ.thingInt);
			}
			return new TargetInfo(targ.cellInt, targ.mapInt, false);
		}

		
		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had WorldObject " + targ.worldObjectInt, 134566, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.Cell;
		}

		
		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had WorldObject " + targ.worldObjectInt, 134566, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.thingInt;
		}

		
		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had Thing " + targ.thingInt, 6324165, false);
			}
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had cell " + targ.cellInt, 631672, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.worldObjectInt;
		}

		
		public static bool operator ==(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			if (a.cellInt.IsValid || b.cellInt.IsValid)
			{
				return a.cellInt == b.cellInt && a.mapInt == b.mapInt;
			}
			if (a.WorldObject != null || b.WorldObject != null)
			{
				return a.WorldObject == b.WorldObject;
			}
			return (a.tileInt < 0 && b.tileInt < 0) || a.tileInt == b.tileInt;
		}

		
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		
		public override bool Equals(object obj)
		{
			return obj is GlobalTargetInfo && this.Equals((GlobalTargetInfo)obj);
		}

		
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			if (this.cellInt.IsValid)
			{
				return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
			}
			if (this.worldObjectInt != null)
			{
				return this.worldObjectInt.GetHashCode();
			}
			if (this.tileInt >= 0)
			{
				return this.tileInt;
			}
			return -1;
		}

		
		public override string ToString()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetUniqueLoadID();
			}
			if (this.cellInt.IsValid)
			{
				return this.cellInt.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			if (this.worldObjectInt != null)
			{
				return "@" + this.worldObjectInt.GetUniqueLoadID();
			}
			if (this.tileInt >= 0)
			{
				return this.tileInt.ToString();
			}
			return "null";
		}

		
		private Thing thingInt;

		
		private IntVec3 cellInt;

		
		private Map mapInt;

		
		private WorldObject worldObjectInt;

		
		private int tileInt;

		
		public const char WorldObjectLoadIDMarker = '@';
	}
}
