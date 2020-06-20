using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001015 RID: 4117
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x06006278 RID: 25208 RVA: 0x00221DDB File Offset: 0x0021FFDB
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x06006279 RID: 25209 RVA: 0x00221DE3 File Offset: 0x0021FFE3
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x0600627A RID: 25210 RVA: 0x00221DEB File Offset: 0x0021FFEB
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x0600627B RID: 25211 RVA: 0x00221DF8 File Offset: 0x0021FFF8
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x0600627C RID: 25212 RVA: 0x00221E05 File Offset: 0x00220005
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x0600627D RID: 25213 RVA: 0x00221E0D File Offset: 0x0022000D
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x0600627E RID: 25214 RVA: 0x00221E15 File Offset: 0x00220015
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x0600627F RID: 25215 RVA: 0x00221E25 File Offset: 0x00220025
		public List<StatModifier> StatBases
		{
			get
			{
				if (!(this.defInt is BuildableDef))
				{
					return this.AbilityDef.statBases;
				}
				return this.BuildableDef.statBases;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x06006280 RID: 25216 RVA: 0x00221E4B File Offset: 0x0022004B
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x06006281 RID: 25217 RVA: 0x00221E53 File Offset: 0x00220053
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06006282 RID: 25218 RVA: 0x00221E5B File Offset: 0x0022005B
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06006283 RID: 25219 RVA: 0x00221E66 File Offset: 0x00220066
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x00221E74 File Offset: 0x00220074
		public static StatRequest For(Thing thing)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.", false);
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x06006285 RID: 25221 RVA: 0x00221ED0 File Offset: 0x002200D0
		public static StatRequest For(Thing thing, Pawn pawn)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.", false);
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			result.pawn = pawn;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x06006286 RID: 25222 RVA: 0x00221F34 File Offset: 0x00220134
		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				defInt = def,
				stuffDefInt = stuffDef,
				qualityCategoryInt = quality
			};
		}

		// Token: 0x06006287 RID: 25223 RVA: 0x00221F80 File Offset: 0x00220180
		public static StatRequest For(AbilityDef def)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = def,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x06006288 RID: 25224 RVA: 0x00221FCC File Offset: 0x002201CC
		public static StatRequest For(RoyalTitleDef def, Faction faction)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = null,
				faction = faction,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x06006289 RID: 25225 RVA: 0x00222020 File Offset: 0x00220220
		public static StatRequest ForEmpty()
		{
			return new StatRequest
			{
				thingInt = null,
				defInt = null,
				stuffDefInt = null,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x0600628A RID: 25226 RVA: 0x00222058 File Offset: 0x00220258
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return "(" + this.Thing + ")";
			}
			return string.Concat(new object[]
			{
				"(",
				this.Thing,
				", ",
				(this.StuffDef != null) ? this.StuffDef.defName : "null",
				")"
			});
		}

		// Token: 0x0600628B RID: 25227 RVA: 0x002220CC File Offset: 0x002202CC
		public override int GetHashCode()
		{
			int num = 0;
			num = Gen.HashCombineInt(num, (int)this.defInt.shortHash);
			if (this.thingInt != null)
			{
				num = Gen.HashCombineInt(num, this.thingInt.thingIDNumber);
			}
			if (this.stuffDefInt != null)
			{
				num = Gen.HashCombineInt(num, (int)this.stuffDefInt.shortHash);
			}
			return num;
		}

		// Token: 0x0600628C RID: 25228 RVA: 0x00222124 File Offset: 0x00220324
		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x0600628D RID: 25229 RVA: 0x0022216E File Offset: 0x0022036E
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x0022219C File Offset: 0x0022039C
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600628F RID: 25231 RVA: 0x002221A6 File Offset: 0x002203A6
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04003C04 RID: 15364
		private Thing thingInt;

		// Token: 0x04003C05 RID: 15365
		private Def defInt;

		// Token: 0x04003C06 RID: 15366
		private ThingDef stuffDefInt;

		// Token: 0x04003C07 RID: 15367
		private QualityCategory qualityCategoryInt;

		// Token: 0x04003C08 RID: 15368
		private Faction faction;

		// Token: 0x04003C09 RID: 15369
		private Pawn pawn;
	}
}
