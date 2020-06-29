using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct StatRequest : IEquatable<StatRequest>
	{
		
		
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		
		
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		
		
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		
		
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		
		
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		
		
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		
		
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		
		
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

		
		
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		
		
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		
		
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		
		
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
