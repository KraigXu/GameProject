using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompSpawnSubplant : ThingComp
	{
		
		// (get) Token: 0x06005376 RID: 21366 RVA: 0x001BED1E File Offset: 0x001BCF1E
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		
		// (get) Token: 0x06005377 RID: 21367 RVA: 0x001BED2B File Offset: 0x001BCF2B
		public List<Thing> SubplantsForReading
		{
			get
			{
				this.Cleanup();
				return this.subplants;
			}
		}

		
		public void AddProgress(float progress)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Subplant spawners are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 43254, false);
				return;
			}
			this.progressToNextSubplant += progress;
			this.TryGrowSubplants();
		}

		
		public void Cleanup()
		{
			this.subplants.RemoveAll((Thing p) => !p.Spawned);
		}

		
		public override string CompInspectStringExtra()
		{
			return this.Props.subplant.LabelCap + ": " + this.SubplantsForReading.Count + "\n" + "ProgressToNextSubplant".Translate(this.Props.subplant.label, this.progressToNextSubplant.ToStringPercent());
		}

		
		private void TryGrowSubplants()
		{
			while (this.progressToNextSubplant >= 1f)
			{
				this.DoGrowSubplant();
				this.progressToNextSubplant -= 1f;
			}
		}

		
		private void DoGrowSubplant()
		{
			IntVec3 position = this.parent.Position;
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					using (List<Thing>.Enumerator enumerator = thingList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def == this.Props.subplant)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag && this.Props.subplant.CanEverPlantAt_NewTemp(intVec, this.parent.Map, true))
					{
						for (int j = thingList.Count - 1; j >= 0; j--)
						{
							if (thingList[j].def.category == ThingCategory.Plant)
							{
								thingList[j].Destroy(DestroyMode.Vanish);
							}
						}
						this.subplants.Add(GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish));
						if (this.Props.spawnSound != null)
						{
							this.Props.spawnSound.PlayOneShot(new TargetInfo(this.parent));
						}
						Action action = this.onGrassGrown;
						if (action == null)
						{
							return;
						}
						action();
						return;
					}
				}
			}
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Add 100% progress",
				action = delegate
				{
					this.AddProgress(1f);
				}
			};
			yield break;
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.progressToNextSubplant, "progressToNextSubplant", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.subplants, "subplants", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.subplants.RemoveAll((Thing x) => x == null);
			}
		}

		
		private float progressToNextSubplant;

		
		private List<Thing> subplants = new List<Thing>();

		
		public Action onGrassGrown;
	}
}
