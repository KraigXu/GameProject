using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EA RID: 2282
	public class PawnRelationDef : Def
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x00127C7C File Offset: 0x00125E7C
		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x00127CAE File Offset: 0x00125EAE
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x00127CD3 File Offset: 0x00125ED3
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x00127CE1 File Offset: 0x00125EE1
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x00127D01 File Offset: 0x00125F01
		public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.lostThoughtFemale;
			}
			return this.lostThought;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x00127D21 File Offset: 0x00125F21
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x00127D41 File Offset: 0x00125F41
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implied && this.reflexive)
			{
				yield return this.defName + ": implied relations can't use the \"reflexive\" option.";
				this.reflexive = false;
			}
			yield break;
			yield break;
		}

		// Token: 0x04001F28 RID: 7976
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x04001F29 RID: 7977
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04001F2A RID: 7978
		public float importance;

		// Token: 0x04001F2B RID: 7979
		public bool implied;

		// Token: 0x04001F2C RID: 7980
		public bool reflexive;

		// Token: 0x04001F2D RID: 7981
		public int opinionOffset;

		// Token: 0x04001F2E RID: 7982
		public float generationChanceFactor;

		// Token: 0x04001F2F RID: 7983
		public float romanceChanceFactor = 1f;

		// Token: 0x04001F30 RID: 7984
		public float incestOpinionOffset;

		// Token: 0x04001F31 RID: 7985
		public bool familyByBloodRelation;

		// Token: 0x04001F32 RID: 7986
		public ThoughtDef diedThought;

		// Token: 0x04001F33 RID: 7987
		public ThoughtDef diedThoughtFemale;

		// Token: 0x04001F34 RID: 7988
		public ThoughtDef lostThought;

		// Token: 0x04001F35 RID: 7989
		public ThoughtDef lostThoughtFemale;

		// Token: 0x04001F36 RID: 7990
		public List<ThoughtDef> soldThoughts;

		// Token: 0x04001F37 RID: 7991
		public ThoughtDef killedThought;

		// Token: 0x04001F38 RID: 7992
		public ThoughtDef killedThoughtFemale;

		// Token: 0x04001F39 RID: 7993
		[Unsaved(false)]
		private PawnRelationWorker workerInt;
	}
}
