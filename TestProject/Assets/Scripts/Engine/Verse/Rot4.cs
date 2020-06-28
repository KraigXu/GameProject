using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000014 RID: 20
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000597D File Offset: 0x00003B7D
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005989 File Offset: 0x00003B89
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00005991 File Offset: 0x00003B91
		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = value % 4;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00005989 File Offset: 0x00003B89
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000599D File Offset: 0x00003B9D
		public int AsInt
		{
			get
			{
				return (int)this.rotInt;
			}
			set
			{
				if (value < 0)
				{
					value += 4000;
				}
				this.rotInt = (byte)(value % 4);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000059B8 File Offset: 0x00003BB8
		public float AsAngle
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return 0f;
				case 1:
					return 90f;
				case 2:
					return 180f;
				case 3:
					return 270f;
				default:
					return 0f;
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00005A04 File Offset: 0x00003C04
		public SpectateRectSide AsSpectateSide
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return SpectateRectSide.Up;
				case 1:
					return SpectateRectSide.Right;
				case 2:
					return SpectateRectSide.Down;
				case 3:
					return SpectateRectSide.Left;
				default:
					return SpectateRectSide.None;
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005A3C File Offset: 0x00003C3C
		public Quaternion AsQuat
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Quaternion.identity;
				case 1:
					return Quaternion.LookRotation(Vector3.right);
				case 2:
					return Quaternion.LookRotation(Vector3.back);
				case 3:
					return Quaternion.LookRotation(Vector3.left);
				default:
					Log.Error("ToQuat with Rot = " + this.AsInt, false);
					return Quaternion.identity;
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005AB0 File Offset: 0x00003CB0
		public Vector2 AsVector2
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Vector2.up;
				case 1:
					return Vector2.right;
				case 2:
					return Vector2.down;
				case 3:
					return Vector2.left;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt);
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005B0E File Offset: 0x00003D0E
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00005B24 File Offset: 0x00003D24
		public static Rot4 North
		{
			get
			{
				return new Rot4(0);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00005B2C File Offset: 0x00003D2C
		public static Rot4 East
		{
			get
			{
				return new Rot4(1);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005B34 File Offset: 0x00003D34
		public static Rot4 South
		{
			get
			{
				return new Rot4(2);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00005B3C File Offset: 0x00003D3C
		public static Rot4 West
		{
			get
			{
				return new Rot4(3);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005B44 File Offset: 0x00003D44
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00005B54 File Offset: 0x00003D54
		public static Rot4 Invalid
		{
			get
			{
				return new Rot4
				{
					rotInt = 200
				};
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005B76 File Offset: 0x00003D76
		public Rot4(byte newRot)
		{
			this.rotInt = newRot;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005991 File Offset: 0x00003B91
		public Rot4(int newRot)
		{
			this.rotInt = (byte)(newRot % 4);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00005B80 File Offset: 0x00003D80
		public IntVec3 FacingCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(0, 0, 1);
				case 1:
					return new IntVec3(1, 0, 0);
				case 2:
					return new IntVec3(0, 0, -1);
				case 3:
					return new IntVec3(-1, 0, 0);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00005BDC File Offset: 0x00003DDC
		public IntVec3 RighthandCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(1, 0, 0);
				case 1:
					return new IntVec3(0, 0, -1);
				case 2:
					return new IntVec3(-1, 0, 0);
				case 3:
					return new IntVec3(0, 0, 1);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00005C38 File Offset: 0x00003E38
		public Rot4 Opposite
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new Rot4(2);
				case 1:
					return new Rot4(3);
				case 2:
					return new Rot4(0);
				case 3:
					return new Rot4(1);
				default:
					return default(Rot4);
				}
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005C8C File Offset: 0x00003E8C
		public void Rotate(RotationDirection RotDir)
		{
			if (RotDir == RotationDirection.Clockwise)
			{
				int asInt = this.AsInt;
				this.AsInt = asInt + 1;
			}
			if (RotDir == RotationDirection.Counterclockwise)
			{
				int asInt = this.AsInt;
				this.AsInt = asInt - 1;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			if (angle < 45f)
			{
				return Rot4.North;
			}
			if (angle < 135f)
			{
				return Rot4.East;
			}
			if (angle < 225f)
			{
				return Rot4.South;
			}
			if (angle < 315f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005D3C File Offset: 0x00003F3C
		public static Rot4 FromIntVec3(IntVec3 offset)
		{
			if (offset.x == 1)
			{
				return Rot4.East;
			}
			if (offset.x == -1)
			{
				return Rot4.West;
			}
			if (offset.z == 1)
			{
				return Rot4.North;
			}
			if (offset.z == -1)
			{
				return Rot4.South;
			}
			Log.Error("FromIntVec3 with bad offset " + offset, false);
			return Rot4.North;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005DA0 File Offset: 0x00003FA0
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005DAE File Offset: 0x00003FAE
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005DD8 File Offset: 0x00003FD8
		public override int GetHashCode()
		{
			switch (this.rotInt)
			{
			case 0:
				return 235515;
			case 1:
				return 5612938;
			case 2:
				return 1215650;
			case 3:
				return 9231792;
			default:
				return (int)this.rotInt;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005E22 File Offset: 0x00004022
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005E30 File Offset: 0x00004030
		public string ToStringHuman()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North".Translate();
			case 1:
				return "East".Translate();
			case 2:
				return "South".Translate();
			case 3:
				return "West".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005EA4 File Offset: 0x000040A4
		public string ToStringWord()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North";
			case 1:
				return "East";
			case 2:
				return "South";
			case 3:
				return "West";
			default:
				return "error";
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005EF0 File Offset: 0x000040F0
		public static Rot4 FromString(string str)
		{
			int num;
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
			}
			else if (!(str == "North"))
			{
				if (!(str == "East"))
				{
					if (!(str == "South"))
					{
						if (!(str == "West"))
						{
							newRot = 0;
							Log.Error("Invalid rotation: " + str, false);
						}
						else
						{
							newRot = 3;
						}
					}
					else
					{
						newRot = 2;
					}
				}
				else
				{
					newRot = 1;
				}
			}
			else
			{
				newRot = 0;
			}
			return new Rot4(newRot);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005F6B File Offset: 0x0000416B
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005F83 File Offset: 0x00004183
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04000035 RID: 53
		private byte rotInt;
	}
}
