using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA2 RID: 3746
	public class ITab_Art : ITab
	{
		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06005B70 RID: 23408 RVA: 0x001F7804 File Offset: 0x001F5A04
		private CompArt SelectedCompArt
		{
			get
			{
				Thing thing = Find.Selector.SingleSelectedThing;
				MinifiedThing minifiedThing = thing as MinifiedThing;
				if (minifiedThing != null)
				{
					thing = minifiedThing.InnerThing;
				}
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompArt>();
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06005B71 RID: 23409 RVA: 0x001F7838 File Offset: 0x001F5A38
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x06005B72 RID: 23410 RVA: 0x001F784F File Offset: 0x001F5A4F
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x001F7878 File Offset: 0x001F5A78
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, ITab_Art.WinSize.x, ITab_Art.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.SelectedCompArt.Title);
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect2 = rect;
			rect2.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, ITab_Art.cachedImageDescription);
		}

		// Token: 0x040031E1 RID: 12769
		private static string cachedImageDescription;

		// Token: 0x040031E2 RID: 12770
		private static CompArt cachedImageSource;

		// Token: 0x040031E3 RID: 12771
		private static TaleReference cachedTaleRef;

		// Token: 0x040031E4 RID: 12772
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
