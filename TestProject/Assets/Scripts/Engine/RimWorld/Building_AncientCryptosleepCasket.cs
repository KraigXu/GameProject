using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				bool flag = false;
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)this.innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Pawn)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					this.EjectContents();
				}
			}
			absorbed = false;
		}

		
		public override void EjectContents()
		{
			bool contentsKnown = this.contentsKnown;
			List<Thing> list = null;
			if (!contentsKnown)
			{
				list = new List<Thing>();
				list.AddRange(this.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
				list.RemoveDuplicates<Thing>();
			}
			base.EjectContents();
			if (!contentsKnown)
			{
				ThingDef filth_Slime = ThingDefOf.Filth_Slime;
				FilthMaker.TryMakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12), FilthSourceFlags.None);
				this.SetFaction(null, null);
				foreach (Building_AncientCryptosleepCasket building_AncientCryptosleepCasket in this.UnopenedCasketsInGroup())
				{
					building_AncientCryptosleepCasket.contentsKnown = true;
					building_AncientCryptosleepCasket.EjectContents();
				}
				IEnumerable<Pawn> enumerable = from p in list.OfType<Pawn>().ToList<Pawn>()
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
				select p;
				if (enumerable.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, false, true, false, false, false), base.Map, enumerable);
				}
			}
		}

		
		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			yield return this;
			if (this.groupID != -1)
			{
				foreach (Thing thing in base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket))
				{
					Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = thing as Building_AncientCryptosleepCasket;
					if (building_AncientCryptosleepCasket.groupID == this.groupID && !building_AncientCryptosleepCasket.contentsKnown)
					{
						yield return building_AncientCryptosleepCasket;
					}
				}
				List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			}
			yield break;
			yield break;
		}

		
		public int groupID = -1;
	}
}
