using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B73 RID: 2931
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600449D RID: 17565 RVA: 0x00172F57 File Offset: 0x00171157
		public PawnBioType BioType
		{
			get
			{
				if (this.pirateKing)
				{
					return PawnBioType.PirateKing;
				}
				if (this.adulthood != null)
				{
					return PawnBioType.BackstoryInGame;
				}
				return PawnBioType.Undefined;
			}
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x00172F6E File Offset: 0x0017116E
		public void PostLoad()
		{
			if (this.childhood != null)
			{
				this.childhood.PostLoad();
			}
			if (this.adulthood != null)
			{
				this.adulthood.PostLoad();
			}
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x00172F98 File Offset: 0x00171198
		public void ResolveReferences()
		{
			if (this.adulthood.spawnCategories.Count == 1 && this.adulthood.spawnCategories[0] == "Trader")
			{
				this.adulthood.spawnCategories.Add("Civil");
			}
			if (this.childhood != null)
			{
				this.childhood.ResolveReferences();
			}
			if (this.adulthood != null)
			{
				this.adulthood.ResolveReferences();
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x00173010 File Offset: 0x00171210
		public IEnumerable<string> ConfigErrors()
		{
			if (this.childhood != null)
			{
				foreach (string text in this.childhood.ConfigErrors(true))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.childhood.title,
						": ",
						text
					});
				}
				IEnumerator<string> enumerator = null;
			}
			if (this.adulthood != null)
			{
				foreach (string text2 in this.adulthood.ConfigErrors(false))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.adulthood.title,
						": ",
						text2
					});
				}
				IEnumerator<string> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x00173020 File Offset: 0x00171220
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}

		// Token: 0x04002731 RID: 10033
		public GenderPossibility gender;

		// Token: 0x04002732 RID: 10034
		public NameTriple name;

		// Token: 0x04002733 RID: 10035
		public Backstory childhood;

		// Token: 0x04002734 RID: 10036
		public Backstory adulthood;

		// Token: 0x04002735 RID: 10037
		public bool pirateKing;
	}
}
