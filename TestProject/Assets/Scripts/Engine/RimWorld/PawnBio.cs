using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		
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

		
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}

		
		public GenderPossibility gender;

		
		public NameTriple name;

		
		public Backstory childhood;

		
		public Backstory adulthood;

		
		public bool pirateKing;
	}
}
