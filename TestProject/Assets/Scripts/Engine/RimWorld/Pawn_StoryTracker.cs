using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Pawn_StoryTracker : IExposable
	{
		
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x00172B35 File Offset: 0x00170D35
		// (set) Token: 0x06004490 RID: 17552 RVA: 0x00172B4C File Offset: 0x00170D4C
		public string Title
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				return this.TitleDefault;
			}
			set
			{
				this.title = null;
				if (value != this.Title && !value.NullOrEmpty())
				{
					this.title = value;
				}
			}
		}

		
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x00172B72 File Offset: 0x00170D72
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		
		// (get) Token: 0x06004492 RID: 17554 RVA: 0x00172B80 File Offset: 0x00170D80
		public string TitleDefault
		{
			get
			{
				if (this.adulthood != null)
				{
					return this.adulthood.TitleFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleFor(this.pawn.gender);
				}
				return "";
			}
		}

		
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x00172BD0 File Offset: 0x00170DD0
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x00172BE0 File Offset: 0x00170DE0
		public string TitleShort
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				if (this.adulthood != null)
				{
					return this.adulthood.TitleShortFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleShortFor(this.pawn.gender);
				}
				return "";
			}
		}

		
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x00172C3F File Offset: 0x00170E3F
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x00172C4C File Offset: 0x00170E4C
		public Color SkinColor
		{
			get
			{
				return PawnSkinColors.GetSkinColor(this.melanin);
			}
		}

		
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x00172C59 File Offset: 0x00170E59
		public IEnumerable<Backstory> AllBackstories
		{
			get
			{
				if (this.childhood != null)
				{
					yield return this.childhood;
				}
				if (this.adulthood != null)
				{
					yield return this.adulthood;
				}
				yield break;
			}
		}

		
		// (get) Token: 0x06004498 RID: 17560 RVA: 0x00172C6C File Offset: 0x00170E6C
		public string HeadGraphicPath
		{
			get
			{
				if (this.headGraphicPath == null)
				{
					this.headGraphicPath = GraphicDatabaseHeadRecords.GetHeadRandom(this.pawn.gender, this.pawn.story.SkinColor, this.pawn.story.crownType).GraphicPath;
				}
				return this.headGraphicPath;
			}
		}

		
		// (get) Token: 0x06004499 RID: 17561 RVA: 0x00172CC4 File Offset: 0x00170EC4
		public WorkTags DisabledWorkTagsBackstoryAndTraits
		{
			get
			{
				WorkTags workTags = WorkTags.None;
				if (this.childhood != null)
				{
					workTags |= this.childhood.workDisables;
				}
				if (this.adulthood != null)
				{
					workTags |= this.adulthood.workDisables;
				}
				for (int i = 0; i < this.traits.allTraits.Count; i++)
				{
					workTags |= this.traits.allTraits[i].def.disabledWorkTags;
				}
				return workTags;
			}
		}

		
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		
		public void ExposeData()
		{
			string text = (this.childhood != null) ? this.childhood.identifier : null;
			Scribe_Values.Look<string>(ref text, "childhood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text.NullOrEmpty() && !BackstoryDatabase.TryGetWithIdentifier(text, out this.childhood, true))
			{
				Log.Error("Couldn't load child backstory with identifier " + text + ". Giving random.", false);
				this.childhood = BackstoryDatabase.RandomBackstory(BackstorySlot.Childhood);
			}
			string text2 = (this.adulthood != null) ? this.adulthood.identifier : null;
			Scribe_Values.Look<string>(ref text2, "adulthood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text2.NullOrEmpty() && !BackstoryDatabase.TryGetWithIdentifier(text2, out this.adulthood, true))
			{
				Log.Error("Couldn't load adult backstory with identifier " + text2 + ". Giving random.", false);
				this.adulthood = BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);
			}
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Values.Look<CrownType>(ref this.crownType, "crownType", CrownType.Undefined, false);
			Scribe_Values.Look<string>(ref this.headGraphicPath, "headGraphicPath", null, false);
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<float>(ref this.melanin, "melanin", 0f, false);
			Scribe_Deep.Look<TraitSet>(ref this.traits, "traits", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.birthLastName, "birthLastName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.birthLastName == null && this.pawn.Name is NameTriple)
				{
					this.birthLastName = ((NameTriple)this.pawn.Name).Last;
				}
				if (this.hairDef == null)
				{
					this.hairDef = DefDatabase<HairDef>.AllDefs.RandomElement<HairDef>();
				}
			}
		}

		
		public Backstory GetBackstory(BackstorySlot slot)
		{
			if (slot == BackstorySlot.Childhood)
			{
				return this.childhood;
			}
			return this.adulthood;
		}

		
		private Pawn pawn;

		
		public Backstory childhood;

		
		public Backstory adulthood;

		
		public float melanin;

		
		public Color hairColor = Color.white;

		
		public CrownType crownType;

		
		public BodyTypeDef bodyType;

		
		private string headGraphicPath;

		
		public HairDef hairDef;

		
		public TraitSet traits;

		
		public string title;

		
		public string birthLastName;
	}
}
