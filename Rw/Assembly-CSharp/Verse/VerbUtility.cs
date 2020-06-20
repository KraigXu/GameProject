using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000489 RID: 1161
	public static class VerbUtility
	{
		// Token: 0x06002268 RID: 8808 RVA: 0x000D1D6C File Offset: 0x000CFF6C
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			if (verb_LaunchProjectile == null)
			{
				return null;
			}
			return verb_LaunchProjectile.Projectile;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000D1D8C File Offset: 0x000CFF8C
		public static DamageDef GetDamageDef(this Verb verb)
		{
			if (!verb.verbProps.LaunchesProjectile)
			{
				return verb.verbProps.meleeDamageDef;
			}
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null)
			{
				return projectile.projectile.damageDef;
			}
			return null;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000D1DCC File Offset: 0x000CFFCC
		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000D1DF0 File Offset: 0x000CFFF0
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000D1E14 File Offset: 0x000D0014
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000D1E33 File Offset: 0x000D0033
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000D1E44 File Offset: 0x000D0044
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000D1E70 File Offset: 0x000D0070
		public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = ((Pawn)concreteExample).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				result = hediffDef.ConcreteExample.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000D1EEC File Offset: 0x000D00EC
		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
			if (num < 9f)
			{
				return 0f;
			}
			if (num < 25f)
			{
				return forcedMiss * 0.5f;
			}
			if (num < 49f)
			{
				return forcedMiss * 0.8f;
			}
			return forcedMiss;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000D1F34 File Offset: 0x000D0134
		public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			if (num <= 25f)
			{
				return 0f;
			}
			if (num >= 144f)
			{
				return 1f;
			}
			return Mathf.InverseLerp(25f, 144f, num);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000D1F80 File Offset: 0x000D0180
		public static IEnumerable<VerbUtility.VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
		{
			if (verbProps != null)
			{
				int num;
				for (int i = 0; i < verbProps.Count; i = num + 1)
				{
					yield return new VerbUtility.VerbPropertiesWithSource(verbProps[i]);
					num = i;
				}
			}
			if (tools != null)
			{
				int num;
				for (int i = 0; i < tools.Count; i = num + 1)
				{
					foreach (ManeuverDef maneuverDef in tools[i].Maneuvers)
					{
						yield return new VerbUtility.VerbPropertiesWithSource(maneuverDef.verb, tools[i], maneuverDef);
					}
					IEnumerator<ManeuverDef> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000D1F98 File Offset: 0x000D0198
		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			if (!(caster is Pawn))
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000D1FD0 File Offset: 0x000D01D0
		public static VerbSelectionCategory GetSelectionCategory(this Verb v, Pawn p, float highestWeight)
		{
			float num = VerbUtility.InitialVerbWeight(v, p);
			if (num >= highestWeight * 0.95f)
			{
				return VerbSelectionCategory.Best;
			}
			if (num < highestWeight * 0.25f)
			{
				return VerbSelectionCategory.Worst;
			}
			return VerbSelectionCategory.Mid;
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000D1FFE File Offset: 0x000D01FE
		public static float InitialVerbWeight(Verb v, Pawn p)
		{
			return VerbUtility.DPS(v, p) * VerbUtility.AdditionalSelectionFactor(v);
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000D200E File Offset: 0x000D020E
		public static float DPS(Verb v, Pawn p)
		{
			return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000D204C File Offset: 0x000D024C
		private static float AdditionalSelectionFactor(Verb v)
		{
			float num = (v.tool != null) ? v.tool.chanceFactor : 1f;
			if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
			{
				foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
				{
					num += 0.1f;
				}
			}
			return num;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000D20EC File Offset: 0x000D02EC
		public static float FinalSelectionWeight(Verb verb, Pawn p, List<Verb> allMeleeVerbs, float highestWeight)
		{
			VerbSelectionCategory selectionCategory = verb.GetSelectionCategory(p, highestWeight);
			if (selectionCategory == VerbSelectionCategory.Worst)
			{
				return 0f;
			}
			int num = 0;
			using (List<Verb>.Enumerator enumerator = allMeleeVerbs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetSelectionCategory(p, highestWeight) == selectionCategory)
					{
						num++;
					}
				}
			}
			return 1f / (float)num * ((selectionCategory == VerbSelectionCategory.Mid) ? 0.25f : 0.75f);
		}

		// Token: 0x020016BC RID: 5820
		public struct VerbPropertiesWithSource
		{
			// Token: 0x17001514 RID: 5396
			// (get) Token: 0x060085A1 RID: 34209 RVA: 0x002B2FDF File Offset: 0x002B11DF
			public ToolCapacityDef ToolCapacity
			{
				get
				{
					if (this.maneuver == null)
					{
						return null;
					}
					return this.maneuver.requiredCapacity;
				}
			}

			// Token: 0x060085A2 RID: 34210 RVA: 0x002B2FF6 File Offset: 0x002B11F6
			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			// Token: 0x060085A3 RID: 34211 RVA: 0x002B300D File Offset: 0x002B120D
			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			// Token: 0x04005728 RID: 22312
			public VerbProperties verbProps;

			// Token: 0x04005729 RID: 22313
			public Tool tool;

			// Token: 0x0400572A RID: 22314
			public ManeuverDef maneuver;
		}
	}
}
