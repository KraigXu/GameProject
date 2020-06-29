using System;
using Verse;

namespace RimWorld
{
	
	public class CompArt : ThingComp
	{
		
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

		
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x001B09AE File Offset: 0x001AEBAE
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		
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

		
		// (get) Token: 0x06005040 RID: 20544 RVA: 0x001B0A0D File Offset: 0x001AEC0D
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x001B0A18 File Offset: 0x001AEC18
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		
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

		
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.NameFullColored;
			}
		}

		
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

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<TaggedString>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<TaggedString>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", Array.Empty<object>());
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "Author".Translate() + ": " + this.AuthorName + ("\n" + "Title".Translate() + ": " + this.Title);
		}

		
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		
		public override string GetDescriptionPart()
		{
			if (!this.Active)
			{
				return null;
			}
			return "" + this.Title + "\n\n" + this.GenerateImageDescription() + "\n\n" + ("Author".Translate() + ": " + this.AuthorName);
		}

		
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		
		public TaggedString GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		
		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}

		
		private TaggedString authorNameInt = null;

		
		private TaggedString titleInt = null;

		
		private TaleReference taleRef;
	}
}
