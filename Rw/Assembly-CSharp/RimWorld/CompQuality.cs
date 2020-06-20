using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7C RID: 3452
	public class CompQuality : ThingComp
	{
		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x0600541B RID: 21531 RVA: 0x001C15D5 File Offset: 0x001BF7D5
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x001C15E0 File Offset: 0x001BF7E0
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x001C160A File Offset: 0x001BF80A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x001C1624 File Offset: 0x001BF824
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x001C1634 File Offset: 0x001BF834
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x001C1656 File Offset: 0x001BF856
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x001C1670 File Offset: 0x001BF870
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(this.Quality.GetLabel().CapitalizeFirst());
		}

		// Token: 0x04002E5F RID: 11871
		private QualityCategory qualityInt = QualityCategory.Normal;
	}
}
