using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Spirit
{
	// Token: 0x02000070 RID: 112
	public class Def : Editable
	{
		public TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00016D98 File Offset: 0x00014F98
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00016DA1 File Offset: 0x00014FA1
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				yield return base.GetType() + " lacks defName. Label=" + this.label;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
			}
			if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
			}
			if (this.modExtensions != null)
			{
				int num;
				for (int i = 0; i < this.modExtensions.Count; i = num)
				{
					foreach (string text in this.modExtensions[i].ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
					num = i + 1;
				}
			}
			if (this.description != null)
			{
				if (this.description == "")
				{
					yield return "empty description";
				}
				if (char.IsWhiteSpace(this.description[0]))
				{
					yield return "description has leading whitespace";
				}
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					yield return "description has trailing whitespace";
				}
			}
			if (this.descriptionHyperlinks != null && this.descriptionHyperlinks.Count > 0)
			{
				if (this.descriptionHyperlinks.RemoveAll((DefHyperlink x) => x.def == null) != 0)
				{
					Log.Warning("Some descriptionHyperlinks in " + this.defName + " had null def.", false);
				}
				Def.<> c__DisplayClass19_0 <> c__DisplayClass19_ = new Def.<> c__DisplayClass19_0();
				<> c__DisplayClass19_.<> 4__this = this;
				<> c__DisplayClass19_.i = this.descriptionHyperlinks.Count - 1;
				while (<> c__DisplayClass19_.i > 0)
				{
					if (this.descriptionHyperlinks.FirstIndexOf((DefHyperlink h) => h.def == <> c__DisplayClass19_.<> 4__this.descriptionHyperlinks[<> c__DisplayClass19_.i].def) < <> c__DisplayClass19_.i)
					{
						yield return string.Concat(new string[]
						{
							"Hyperlink to ",
							this.descriptionHyperlinks[<>c__DisplayClass19_.i].def.defName,
							" more than once on ",
							this.defName,
							" description"
						});
					}
					int num = <> c__DisplayClass19_.i;
					<> c__DisplayClass19_.i = num - 1;
				}
				<> c__DisplayClass19_ = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00016DB1 File Offset: 0x00014FB1
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00016DBF File Offset: 0x00014FBF
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00016DC7 File Offset: 0x00014FC7
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00016DD4 File Offset: 0x00014FD4
		public T GetModExtension<T>() where T : DefModExtension
		{
			if (this.modExtensions == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.modExtensions.Count; i++)
			{
				if (this.modExtensions[i] is T)
				{
					return this.modExtensions[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00016E3C File Offset: 0x0001503C
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x0400016C RID: 364
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x0400016D RID: 365
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label;

		// Token: 0x0400016E RID: 366
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description;

		// Token: 0x0400016F RID: 367
		[XmlInheritanceAllowDuplicateNodes]
		public List<DefHyperlink> descriptionHyperlinks;

		// Token: 0x04000170 RID: 368
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors;

		// Token: 0x04000171 RID: 369
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x04000172 RID: 370
		[Unsaved(false)]
		public ushort shortHash;

		// Token: 0x04000173 RID: 371
		[Unsaved(false)]
		public ushort index = ushort.MaxValue;

		// Token: 0x04000174 RID: 372
		[Unsaved(false)]
		public ModContentPack modContentPack;

		// Token: 0x04000175 RID: 373
		[Unsaved(false)]
		public string fileName;

		// Token: 0x04000176 RID: 374
		[Unsaved(false)]
		private TaggedString cachedLabelCap = null;

		// Token: 0x04000177 RID: 375
		[Unsaved(false)]
		public bool generated;

		// Token: 0x04000178 RID: 376
		[Unsaved(false)]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x04000179 RID: 377
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x0400017A RID: 378
		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");
	}
}
