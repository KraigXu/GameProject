using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Pawn_FilthTracker : IExposable
	{
		
		// (get) Token: 0x06004605 RID: 17925 RVA: 0x0017A1A4 File Offset: 0x001783A4
		public string FilthReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FilthOnFeet".Translate());
				if (this.carriedFilth.Count == 0)
				{
					stringBuilder.Append("(" + "NoneLower".Translate() + ")");
				}
				else
				{
					for (int i = 0; i < this.carriedFilth.Count; i++)
					{
						stringBuilder.AppendLine(this.carriedFilth[i].LabelCap);
					}
				}
				return stringBuilder.ToString();
			}
		}

		
		// (get) Token: 0x06004606 RID: 17926 RVA: 0x0017A23A File Offset: 0x0017843A
		private FilthSourceFlags AdditionalFilthSourceFlags
		{
			get
			{
				if (this.pawn.Faction != null || !this.pawn.RaceProps.Animal)
				{
					return FilthSourceFlags.Unnatural;
				}
				return FilthSourceFlags.Natural;
			}
		}

		
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.lastTerrainFilthDef, "lastTerrainFilthDef");
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.carriedFilth.RemoveAll((Filth x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had null carried filth after loading.", false);
				}
				if (this.carriedFilth.RemoveAll((Filth x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had carried filth with null def after loading.", false);
				}
			}
		}

		
		public void Notify_EnteredNewCell()
		{
			if (Rand.Value < 0.05f)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.1f)
			{
				this.TryPickupFilth();
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (Rand.Value < PawnUtility.AnimalFilthChancePerCell(this.pawn.def, this.pawn.BodySize) && FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1, this.AdditionalFilthSourceFlags))
				{
					FilthMonitor.Notify_FilthAnimalGenerated();
					return;
				}
			}
			else if (Rand.Value < PawnUtility.HumanFilthChancePerCell(this.pawn.def, this.pawn.BodySize))
			{
				ThingDef filth_Trash;
				if (this.lastTerrainFilthDef != null && Rand.Chance(0.66f))
				{
					filth_Trash = this.lastTerrainFilthDef;
				}
				else
				{
					filth_Trash = ThingDefOf.Filth_Trash;
				}
				if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, filth_Trash, 1, this.AdditionalFilthSourceFlags))
				{
					FilthMonitor.Notify_FilthHumanGenerated();
				}
			}
		}

		
		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.generatedFilth != null)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					if (this.carriedFilth[i].def.filth.TerrainSourced && this.carriedFilth[i].def != terrDef.generatedFilth)
					{
						this.ThinCarriedFilth(this.carriedFilth[i]);
					}
				}
				Filth filth = (from f in this.carriedFilth
				where f.def == terrDef.generatedFilth
				select f).FirstOrDefault<Filth>();
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.generatedFilth);
					FilthMonitor.Notify_FilthAccumulated();
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				Filth filth2 = thingList[j] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
		}

		
		private void TryDropFilth()
		{
			if (this.carriedFilth.Count == 0)
			{
				return;
			}
			for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
			{
				if (this.carriedFilth[i].CanDropAt(this.pawn.Position, this.pawn.Map, FilthSourceFlags.None))
				{
					this.DropCarriedFilth(this.carriedFilth[i]);
					FilthMonitor.Notify_FilthDropped();
				}
			}
		}

		
		private void DropCarriedFilth(Filth f)
		{
			if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources, this.AdditionalFilthSourceFlags))
			{
				this.ThinCarriedFilth(f);
			}
		}

		
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		
		public void GainFilth(ThingDef filthDef)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			this.GainFilth(filthDef, null);
		}

		
		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			Filth filth = null;
			for (int i = 0; i < this.carriedFilth.Count; i++)
			{
				if (this.carriedFilth[i].def == filthDef)
				{
					filth = this.carriedFilth[i];
					break;
				}
			}
			if (filth != null)
			{
				if (filth.CanBeThickened)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
					return;
				}
			}
			else
			{
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				this.carriedFilth.Add(filth2);
			}
		}

		
		private Pawn pawn;

		
		private List<Filth> carriedFilth = new List<Filth>();

		
		private ThingDef lastTerrainFilthDef;

		
		private const float FilthPickupChance = 0.1f;

		
		private const float FilthDropChance = 0.05f;

		
		private const int MaxCarriedTerrainFilthThickness = 1;
	}
}
