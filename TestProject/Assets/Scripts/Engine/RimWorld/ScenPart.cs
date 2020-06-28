using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C18 RID: 3096
	public abstract class ScenPart : IExposable
	{
		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x0018FE11 File Offset: 0x0018E011
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x0018FE18 File Offset: 0x0018E018
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x0018FE2A File Offset: 0x0018E02A
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x0018FE3C File Offset: 0x0018E03C
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x0018FE50 File Offset: 0x0018E050
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x0018FE5D File Offset: 0x0018E05D
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x0018FE6C File Offset: 0x0018E06C
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x0018FE79 File Offset: 0x0018E079
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Randomize()
		{
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x0018FE82 File Offset: 0x0018E082
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreConfigure()
		{
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x0018FE8B File Offset: 0x0018E08B
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostGameStart()
		{
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Tick()
		{
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x0018FE94 File Offset: 0x0018E094
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def == null)
			{
				yield return base.GetType().ToString() + " has null def.";
			}
			yield break;
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x0018FEA4 File Offset: 0x0018E0A4
		public virtual bool HasNullDefs()
		{
			return this.def == null;
		}

		// Token: 0x040029FA RID: 10746
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x040029FB RID: 10747
		public bool visible = true;

		// Token: 0x040029FC RID: 10748
		public bool summarized;
	}
}
