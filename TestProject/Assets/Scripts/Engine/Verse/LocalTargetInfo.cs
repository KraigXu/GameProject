using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000BEBAE File Offset: 0x000BCDAE
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000BEBC5 File Offset: 0x000BCDC5
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000BEBD0 File Offset: 0x000BCDD0
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000BEBD8 File Offset: 0x000BCDD8
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x000BEBE5 File Offset: 0x000BCDE5
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000BEBFC File Offset: 0x000BCDFC
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		
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

		
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		
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

		
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		
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

		
		private Thing thingInt;

		
		private IntVec3 cellInt;
	}
}
