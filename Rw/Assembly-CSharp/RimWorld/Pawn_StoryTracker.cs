using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B70 RID: 2928
	public class Pawn_StoryTracker : IExposable
	{
		// Token: 0x17000BF1 RID: 3057
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

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x00172B72 File Offset: 0x00170D72
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		// Token: 0x17000BF3 RID: 3059
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

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x00172BD0 File Offset: 0x00170DD0
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		// Token: 0x17000BF5 RID: 3061
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

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x00172C3F File Offset: 0x00170E3F
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x00172C4C File Offset: 0x00170E4C
		public Color SkinColor
		{
			get
			{
				return PawnSkinColors.GetSkinColor(this.melanin);
			}
		}

		// Token: 0x17000BF8 RID: 3064
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

		// Token: 0x17000BF9 RID: 3065
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

		// Token: 0x17000BFA RID: 3066
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

		// Token: 0x0600449A RID: 17562 RVA: 0x00172D39 File Offset: 0x00170F39
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x00172D60 File Offset: 0x00170F60
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

		// Token: 0x0600449C RID: 17564 RVA: 0x00172F45 File Offset: 0x00171145
		public Backstory GetBackstory(BackstorySlot slot)
		{
			if (slot == BackstorySlot.Childhood)
			{
				return this.childhood;
			}
			return this.adulthood;
		}

		// Token: 0x0400271D RID: 10013
		private Pawn pawn;

		// Token: 0x0400271E RID: 10014
		public Backstory childhood;

		// Token: 0x0400271F RID: 10015
		public Backstory adulthood;

		// Token: 0x04002720 RID: 10016
		public float melanin;

		// Token: 0x04002721 RID: 10017
		public Color hairColor = Color.white;

		// Token: 0x04002722 RID: 10018
		public CrownType crownType;

		// Token: 0x04002723 RID: 10019
		public BodyTypeDef bodyType;

		// Token: 0x04002724 RID: 10020
		private string headGraphicPath;

		// Token: 0x04002725 RID: 10021
		public HairDef hairDef;

		// Token: 0x04002726 RID: 10022
		public TraitSet traits;

		// Token: 0x04002727 RID: 10023
		public string title;

		// Token: 0x04002728 RID: 10024
		public string birthLastName;
	}
}
