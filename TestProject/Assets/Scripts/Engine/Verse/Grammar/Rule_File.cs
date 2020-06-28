using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x020004C8 RID: 1224
	public class Rule_File : Rule
	{
		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x000D827E File Offset: 0x000D647E
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x000D828C File Offset: 0x000D648C
		public override Rule DeepCopy()
		{
			Rule_File rule_File = (Rule_File)base.DeepCopy();
			rule_File.path = this.path;
			if (this.pathList != null)
			{
				rule_File.pathList = this.pathList.ToList<string>();
			}
			if (this.cachedStrings != null)
			{
				rule_File.cachedStrings = this.cachedStrings.ToList<string>();
			}
			return rule_File;
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000D82E4 File Offset: 0x000D64E4
		public override string Generate()
		{
			if (this.cachedStrings.NullOrEmpty<string>())
			{
				return "Filestring";
			}
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000D8304 File Offset: 0x000D6504
		public override void Init()
		{
			if (!this.path.NullOrEmpty())
			{
				this.LoadStringsFromFile(this.path);
			}
			foreach (string filePath in this.pathList)
			{
				this.LoadStringsFromFile(filePath);
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000D8370 File Offset: 0x000D6570
		private void LoadStringsFromFile(string filePath)
		{
			List<string> list;
			if (Translator.TryGetTranslatedStringsForFile(filePath, out list))
			{
				foreach (string item in list)
				{
					this.cachedStrings.Add(item);
				}
			}
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000D83D0 File Offset: 0x000D65D0
		public override string ToString()
		{
			if (!this.path.NullOrEmpty())
			{
				return string.Concat(new object[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count,
					" strings from file: ",
					this.path,
					")"
				});
			}
			if (this.pathList.Count > 0)
			{
				return string.Concat(new object[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count,
					" strings from ",
					this.pathList.Count,
					" files)"
				});
			}
			return this.keyword + "->(Rule_File with no configuration)";
		}

		// Token: 0x040015CE RID: 5582
		[MayTranslate]
		public string path;

		// Token: 0x040015CF RID: 5583
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x040015D0 RID: 5584
		[Unsaved(false)]
		private List<string> cachedStrings = new List<string>();
	}
}
