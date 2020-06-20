using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C10 RID: 3088
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x0600498C RID: 18828 RVA: 0x0018F3C3 File Offset: 0x0018D5C3
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x0018F3EA File Offset: 0x0018D5EA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x0018F418 File Offset: 0x0018D618
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(this.damage.label, this.radius.ToString());
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x0018F44C File Offset: 0x0018D64C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate
				{
					this.damage = d;
				});
			}
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0018F4F0 File Offset: 0x0018D6F0
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			}
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x0018F54C File Offset: 0x0018D74C
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		// Token: 0x040029EB RID: 10731
		private float radius = 5.9f;

		// Token: 0x040029EC RID: 10732
		private DamageDef damage;

		// Token: 0x040029ED RID: 10733
		private string radiusBuf;
	}
}
