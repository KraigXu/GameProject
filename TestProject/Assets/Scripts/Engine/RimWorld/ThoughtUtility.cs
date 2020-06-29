using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class ThoughtUtility
	{
		
		public static void Reset()
		{
			ThoughtUtility.situationalSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && x.IsSocial
			select x).ToList<ThoughtDef>();
			ThoughtUtility.situationalNonSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && !x.IsSocial
			select x).ToList<ThoughtDef>();
		}

		
		public static void GiveThoughtsForPawnExecuted(Pawn victim, PawnExecutionKind kind)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			int forcedStage = 1;
			if (victim.guilt.IsGuilty)
			{
				forcedStage = 0;
			}
			else
			{
				switch (kind)
				{
				case PawnExecutionKind.GenericBrutal:
					forcedStage = 2;
					break;
				case PawnExecutionKind.GenericHumane:
					forcedStage = 1;
					break;
				case PawnExecutionKind.OrganHarvesting:
					forcedStage = 3;
					break;
				}
			}
			ThoughtDef def;
			if (victim.IsColonist)
			{
				def = ThoughtDefOf.KnowColonistExecuted;
			}
			else
			{
				def = ThoughtDefOf.KnowGuestExecuted;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
			{
				if (pawn.IsColonist && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, forcedStage), null);
				}
			}
		}

		
		public static void GiveThoughtsForPawnOrganHarvested(Pawn victim)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			ThoughtDef thoughtDef = null;
			if (victim.IsColonist)
			{
				thoughtDef = ThoughtDefOf.KnowColonistOrganHarvested;
			}
			else if (victim.HostFaction == Faction.OfPlayer)
			{
				thoughtDef = ThoughtDefOf.KnowGuestOrganHarvested;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
			{
				if (pawn.needs.mood != null)
				{
					if (pawn == victim)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null);
					}
					else if (thoughtDef != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, null);
					}
				}
			}
		}

		
		public static Hediff NullifyingHediff(ThoughtDef def, Pawn pawn)
		{
			if (def.IsMemory)
			{
				return null;
			}
			float num = 0f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			Hediff result = null;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.pctConditionalThoughtsNullified > num)
				{
					num = curStage.pctConditionalThoughtsNullified;
					result = hediffs[i];
				}
			}
			if (num == 0f)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 31 + (int)(def.index * 139);
			bool flag = Rand.Value < num;
			Rand.PopState();
			if (!flag)
			{
				return null;
			}
			return result;
		}

		
		public static Trait NullifyingTrait(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingTraits != null)
			{
				for (int i = 0; i < def.nullifyingTraits.Count; i++)
				{
					Trait trait = pawn.story.traits.GetTrait(def.nullifyingTraits[i]);
					if (trait != null)
					{
						return trait;
					}
				}
			}
			return null;
		}

		
		public static TaleDef NullifyingTale(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingOwnTales != null)
			{
				for (int i = 0; i < def.nullifyingOwnTales.Count; i++)
				{
					if (Find.TaleManager.GetLatestTale(def.nullifyingOwnTales[i], pawn) != null)
					{
						return def.nullifyingOwnTales[i];
					}
				}
			}
			return null;
		}

		
		public static void RemovePositiveBedroomThoughts(Pawn pawn)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBedroom, (Thought_Memory thought) => thought.MoodOffset() > 0f);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBarracks, (Thought_Memory thought) => thought.MoodOffset() > 0f);
		}

		
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static bool CanGetThought(Pawn pawn, ThoughtDef def)
		{
			return ThoughtUtility.CanGetThought_NewTemp(pawn, def, false);
		}

		
		public static bool CanGetThought_NewTemp(Pawn pawn, ThoughtDef def, bool checkIfNullified = false)
		{
			try
			{
				if (!def.validWhileDespawned && !pawn.Spawned && !def.IsMemory)
				{
					return false;
				}
				if (!def.requiredTraits.NullOrEmpty<TraitDef>())
				{
					bool flag = false;
					for (int i = 0; i < def.requiredTraits.Count; i++)
					{
						if (pawn.story.traits.HasTrait(def.requiredTraits[i]) && (!def.RequiresSpecificTraitsDegree || def.requiredTraitsDegree == pawn.story.traits.DegreeOfTrait(def.requiredTraits[i])))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (def.nullifiedIfNotColonist && !pawn.IsColonist)
				{
					return false;
				}
				if (checkIfNullified && ThoughtUtility.ThoughtNullified(pawn, def))
				{
					return false;
				}
			}
			finally
			{
			}
			return true;
		}

		
		public static bool ThoughtNullified(Pawn pawn, ThoughtDef def)
		{
			return ThoughtUtility.NullifyingTrait(def, pawn) != null || ThoughtUtility.NullifyingHediff(def, pawn) != null || ThoughtUtility.NullifyingTale(def, pawn) != null;
		}

		
		public static string ThoughtNullifiedMessage(Pawn pawn, ThoughtDef def)
		{
			TaggedString t = "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": ";
			Trait trait = ThoughtUtility.NullifyingTrait(def, pawn);
			if (trait != null)
			{
				return t + trait.LabelCap;
			}
			Hediff hediff = ThoughtUtility.NullifyingHediff(def, pawn);
			if (hediff != null)
			{
				return t + hediff.def.LabelCap;
			}
			TaleDef taleDef = ThoughtUtility.NullifyingTale(def, pawn);
			if (taleDef != null)
			{
				return t + taleDef.LabelCap;
			}
			return "";
		}

		
		public static List<ThoughtDef> situationalSocialThoughtDefs;

		
		public static List<ThoughtDef> situationalNonSocialThoughtDefs;
	}
}
