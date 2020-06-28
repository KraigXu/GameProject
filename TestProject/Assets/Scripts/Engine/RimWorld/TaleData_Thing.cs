using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C34 RID: 3124
	public class TaleData_Thing : TaleData
	{
		// Token: 0x06004A79 RID: 19065 RVA: 0x00192E24 File Offset: 0x00191024
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x00192E87 File Offset: 0x00191087
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			yield return new Rule_String(prefix + "_label", this.thingDef.label);
			yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.thingDef.label, false, false));
			yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.thingDef.label, false, false));
			if (this.stuff != null)
			{
				yield return new Rule_String(prefix + "_stuffLabel", this.stuff.label);
			}
			if (this.title != null)
			{
				yield return new Rule_String(prefix + "_title", this.title);
			}
			yield return new Rule_String(prefix + "_quality", this.quality.GetLabel());
			yield break;
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x00192EA0 File Offset: 0x001910A0
		public static TaleData_Thing GenerateFrom(Thing t)
		{
			TaleData_Thing taleData_Thing = new TaleData_Thing();
			taleData_Thing.thingID = t.thingIDNumber;
			taleData_Thing.thingDef = t.def;
			taleData_Thing.stuff = t.Stuff;
			t.TryGetQuality(out taleData_Thing.quality);
			CompArt compArt = t.TryGetComp<CompArt>();
			if (compArt != null && compArt.Active)
			{
				taleData_Thing.title = compArt.Title;
			}
			return taleData_Thing;
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x00192F04 File Offset: 0x00191104
		public static TaleData_Thing GenerateRandom()
		{
			ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef d)
			{
				if (d.comps != null)
				{
					return d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt));
				}
				return false;
			}).RandomElement<ThingDef>();
			ThingDef thingDef2 = GenStuff.RandomStuffFor(thingDef);
			Thing thing = ThingMaker.MakeThing(thingDef, thingDef2);
			ArtGenerationContext source = (Rand.Value < 0.5f) ? ArtGenerationContext.Colony : ArtGenerationContext.Outsider;
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null && compQuality.Quality < thing.TryGetComp<CompArt>().Props.minQualityForArtistic)
			{
				compQuality.SetQuality(thing.TryGetComp<CompArt>().Props.minQualityForArtistic, source);
			}
			thing.TryGetComp<CompArt>().InitializeArt(source);
			return TaleData_Thing.GenerateFrom(thing);
		}

		// Token: 0x04002A57 RID: 10839
		public int thingID;

		// Token: 0x04002A58 RID: 10840
		public ThingDef thingDef;

		// Token: 0x04002A59 RID: 10841
		public ThingDef stuff;

		// Token: 0x04002A5A RID: 10842
		public string title;

		// Token: 0x04002A5B RID: 10843
		public QualityCategory quality;
	}
}
