using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct StatRequest : IEquatable<StatRequest>
	{
		
		// (get) Token: 0x06006278 RID: 25208 RVA: 0x00221DDB File Offset: 0x0021FFDB
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		
		// (get) Token: 0x06006279 RID: 25209 RVA: 0x00221DE3 File Offset: 0x0021FFE3
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		
		// (get) Token: 0x0600627A RID: 25210 RVA: 0x00221DEB File Offset: 0x0021FFEB
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		
		// (get) Token: 0x0600627B RID: 25211 RVA: 0x00221DF8 File Offset: 0x0021FFF8
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		
		// (get) Token: 0x0600627C RID: 25212 RVA: 0x00221E05 File Offset: 0x00220005
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		
		// (get) Token: 0x0600627D RID: 25213 RVA: 0x00221E0D File Offset: 0x0022000D
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		
		// (get) Token: 0x0600627E RID: 25214 RVA: 0x00221E15 File Offset: 0x00220015
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		
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

		
		// (get) Token: 0x06006280 RID: 25216 RVA: 0x00221E4B File Offset: 0x0022004B
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		
		// (get) Token: 0x06006281 RID: 25217 RVA: 0x00221E53 File Offset: 0x00220053
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		
		// (get) Token: 0x06006282 RID: 25218 RVA: 0x00221E5B File Offset: 0x0022005B
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		
		// (get) Token: 0x06006283 RID: 25219 RVA: 0x00221E66 File Offset: 0x00220066
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		
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

		
		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
		}

		
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		
		private Thing thingInt;

		
		private Def defInt;

		
		private ThingDef stuffDefInt;

		
		private QualityCategory qualityCategoryInt;

		
		private Faction faction;

		
		private Pawn pawn;
	}
}
