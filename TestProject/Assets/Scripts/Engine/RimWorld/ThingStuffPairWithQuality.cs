using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001081 RID: 4225
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>, IExposable
	{
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06006449 RID: 25673 RVA: 0x0022BB78 File Offset: 0x00229D78
		public QualityCategory Quality
		{
			get
			{
				QualityCategory? qualityCategory = this.quality;
				if (qualityCategory == null)
				{
					return QualityCategory.Normal;
				}
				return qualityCategory.GetValueOrDefault();
			}
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x0022BBA0 File Offset: 0x00229DA0
		public ThingStuffPairWithQuality(ThingDef thing, ThingDef stuff, QualityCategory quality)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.quality = new QualityCategory?(quality);
			if (quality != QualityCategory.Normal && !thing.HasComp(typeof(CompQuality)))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with quality",
					quality,
					" but ",
					thing,
					" doesn't have CompQuality."
				}), false);
				quality = QualityCategory.Normal;
			}
			if (stuff != null && !thing.MadeFromStuff)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with stuff ",
					stuff,
					" but ",
					thing,
					" is not made from stuff."
				}), false);
				stuff = null;
			}
		}

		// Token: 0x0600644B RID: 25675 RVA: 0x0022BC55 File Offset: 0x00229E55
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x0022BC7C File Offset: 0x00229E7C
		public static bool operator ==(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			if (a.thing == b.thing && a.stuff == b.stuff)
			{
				QualityCategory? qualityCategory = a.quality;
				QualityCategory? qualityCategory2 = b.quality;
				return qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null);
			}
			return false;
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x0022BCD6 File Offset: 0x00229ED6
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x0022BCE2 File Offset: 0x00229EE2
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x0022BCFA File Offset: 0x00229EFA
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x0022BD08 File Offset: 0x00229F08
		public override int GetHashCode()
		{
			return Gen.HashCombine<QualityCategory?>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.quality);
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x0022BD2C File Offset: 0x00229F2C
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x06006452 RID: 25682 RVA: 0x0022BD40 File Offset: 0x00229F40
		public Thing MakeThing()
		{
			Thing result = ThingMaker.MakeThing(this.thing, this.stuff);
			CompQuality compQuality = result.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(this.Quality, ArtGenerationContext.Outsider);
			}
			return result;
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x0022BD78 File Offset: 0x00229F78
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thing, "thing");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
		}

		// Token: 0x04003CFC RID: 15612
		public ThingDef thing;

		// Token: 0x04003CFD RID: 15613
		public ThingDef stuff;

		// Token: 0x04003CFE RID: 15614
		public QualityCategory? quality;
	}
}
