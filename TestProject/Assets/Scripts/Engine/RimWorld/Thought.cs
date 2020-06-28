using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCD RID: 3021
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060047A2 RID: 18338
		public abstract int CurStageIndex { get; }

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060047A3 RID: 18339 RVA: 0x00184EE3 File Offset: 0x001830E3
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060047A4 RID: 18340 RVA: 0x00184EFB File Offset: 0x001830FB
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060047A5 RID: 18341 RVA: 0x00184F08 File Offset: 0x00183108
		public virtual string LabelCap
		{
			get
			{
				if (this.def.Worker == null)
				{
					return this.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN"));
				}
				return this.def.Worker.PostProcessLabel(this.pawn, this.CurStage.LabelCap);
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060047A6 RID: 18342 RVA: 0x00184F69 File Offset: 0x00183169
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060047A7 RID: 18343 RVA: 0x00184F76 File Offset: 0x00183176
		public virtual string LabelCapSocial
		{
			get
			{
				if (this.CurStage.labelSocial != null)
				{
					return this.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"));
				}
				return this.LabelCap;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060047A8 RID: 18344 RVA: 0x00184FB4 File Offset: 0x001831B4
		public virtual string Description
		{
			get
			{
				string text = this.CurStage.description;
				if (text == null)
				{
					text = this.def.description;
				}
				Thought_Memory thought_Memory;
				ISocialThought socialThought;
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessDescription(this.pawn, text);
				}
				else if ((thought_Memory = (this as Thought_Memory)) != null && thought_Memory.otherPawn != null)
				{
					text = text.Formatted(this.pawn.Named("PAWN"), thought_Memory.otherPawn.Named("OTHERPAWN"));
				}
				else if ((socialThought = (this as ISocialThought)) != null && socialThought.OtherPawn() != null)
				{
					text = text.Formatted(this.pawn.Named("PAWN"), socialThought.OtherPawn().Named("OTHERPAWN"));
				}
				else
				{
					text = text.Formatted(this.pawn.Named("PAWN"));
				}
				string text2 = ThoughtUtility.ThoughtNullifiedMessage(this.pawn, this.def);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + "\n\n(" + text2 + ")";
				}
				return text;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060047A9 RID: 18345 RVA: 0x001850CF File Offset: 0x001832CF
		public Texture2D Icon
		{
			get
			{
				if (this.def.Icon != null)
				{
					return this.def.Icon;
				}
				if (this.MoodOffset() > 0f)
				{
					return Thought.DefaultGoodIcon;
				}
				return Thought.DefaultBadIcon;
			}
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x00185108 File Offset: 0x00183308
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0018511C File Offset: 0x0018331C
		public virtual float MoodOffset()
		{
			if (this.CurStage == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"CurStage is null while ShouldDiscard is false on ",
					this.def.defName,
					" for ",
					this.pawn
				}), false);
				return 0f;
			}
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = this.BaseMoodOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true);
			}
			if (this.def.Worker != null)
			{
				num *= this.def.Worker.MoodMultiplier(this.pawn);
			}
			return num;
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x001851DD File Offset: 0x001833DD
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Init()
		{
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x001851ED File Offset: 0x001833ED
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x0400292B RID: 10539
		public Pawn pawn;

		// Token: 0x0400292C RID: 10540
		public ThoughtDef def;

		// Token: 0x0400292D RID: 10541
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x0400292E RID: 10542
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
