using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE7 RID: 3303
	public class CompArt : ThingComp
	{
		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x0600503C RID: 20540 RVA: 0x001B093C File Offset: 0x001AEB3C
		public TaggedString AuthorName
		{
			get
			{
				if (this.authorNameInt.NullOrEmpty())
				{
					return "UnknownLower".Translate().CapitalizeFirst();
				}
				return this.authorNameInt.Resolve();
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x0600503D RID: 20541 RVA: 0x001B0979 File Offset: 0x001AEB79
		public string Title
		{
			get
			{
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.", false);
					this.titleInt = "Error";
				}
				return this.titleInt;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x001B09AE File Offset: 0x001AEBAE
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x0600503F RID: 20543 RVA: 0x001B09B8 File Offset: 0x001AEBB8
		public bool CanShowArt
		{
			get
			{
				if (this.Props.mustBeFullGrave)
				{
					Building_Grave building_Grave = this.parent as Building_Grave;
					if (building_Grave == null || !building_Grave.HasCorpse)
					{
						return false;
					}
				}
				QualityCategory qualityCategory;
				return !this.parent.TryGetQuality(out qualityCategory) || qualityCategory >= this.Props.minQualityForArtistic;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06005040 RID: 20544 RVA: 0x001B0A0D File Offset: 0x001AEC0D
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x001B0A18 File Offset: 0x001AEC18
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x001B0A25 File Offset: 0x001AEC25
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x001B0A2F File Offset: 0x001AEC2F
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x001B0A3C File Offset: 0x001AEC3C
		private void InitializeArt(Thing relatedThing, ArtGenerationContext source)
		{
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
			if (this.CanShowArt)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (relatedThing != null)
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(relatedThing);
					}
					else
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArt(source);
					}
				}
				else
				{
					this.taleRef = TaleReference.Taleless;
				}
				this.titleInt = this.GenerateTitle();
				return;
			}
			this.titleInt = null;
			this.taleRef = null;
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x001B0ACC File Offset: 0x001AECCC
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.NameFullColored;
			}
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x001B0AE2 File Offset: 0x001AECE2
		public void Clear()
		{
			this.authorNameInt = null;
			this.titleInt = null;
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x001B0B18 File Offset: 0x001AED18
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<TaggedString>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<TaggedString>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", Array.Empty<object>());
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x001B0B70 File Offset: 0x001AED70
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "Author".Translate() + ": " + this.AuthorName + ("\n" + "Title".Translate() + ": " + this.Title);
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x001B0BDE File Offset: 0x001AEDDE
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x001B0C04 File Offset: 0x001AEE04
		public override string GetDescriptionPart()
		{
			if (!this.Active)
			{
				return null;
			}
			return "" + this.Title + "\n\n" + this.GenerateImageDescription() + "\n\n" + ("Author".Translate() + ": " + this.AuthorName);
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x001B0C78 File Offset: 0x001AEE78
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x001B0C85 File Offset: 0x001AEE85
		public TaggedString GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x001B0CC4 File Offset: 0x001AEEC4
		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}

		// Token: 0x04002CBF RID: 11455
		private TaggedString authorNameInt = null;

		// Token: 0x04002CC0 RID: 11456
		private TaggedString titleInt = null;

		// Token: 0x04002CC1 RID: 11457
		private TaleReference taleRef;
	}
}
