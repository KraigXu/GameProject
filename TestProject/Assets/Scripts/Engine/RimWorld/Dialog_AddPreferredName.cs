using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4D RID: 3661
	public class Dialog_AddPreferredName : Window
	{
		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06005879 RID: 22649 RVA: 0x001D599F File Offset: 0x001D3B9F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x001D59B0 File Offset: 0x001D3BB0
		public Dialog_AddPreferredName()
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x001D5A40 File Offset: 0x001D3C40
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label("TypeFirstNickOrLastName".Translate(), -1f, null);
			string text = listing_Standard.TextEntry(this.searchName, 1);
			if (text.Length < 20)
			{
				this.searchName = text;
				this.searchWords = this.searchName.Replace("'", "").Split(new char[]
				{
					' '
				});
			}
			listing_Standard.Gap(4f);
			if (this.searchName.Length > 1)
			{
				foreach (NameTriple nameTriple in this.cachedNames.Where(new Func<NameTriple, bool>(this.FilterMatch)))
				{
					if (listing_Standard.ButtonText(nameTriple.ToString(), null))
					{
						this.TryChooseName(nameTriple);
					}
					if (listing_Standard.CurHeight + 30f > inRect.height - (this.CloseButSize.y + 8f))
					{
						break;
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x001D5B64 File Offset: 0x001D3D64
		private bool FilterMatch(NameTriple n)
		{
			if (n.First == "Tynan" && n.Last == "Sylvester")
			{
				return false;
			}
			if (this.searchWords.Length == 0)
			{
				return false;
			}
			if (this.searchWords.Length == 1)
			{
				return n.Last.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.First.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase);
			}
			return this.searchWords.Length == 2 && n.First.EqualsIgnoreCase(this.searchWords[0]) && (n.Last.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x001D5C3A File Offset: 0x001D3E3A
		private void TryChooseName(NameTriple name)
		{
			if (this.AlreadyPreferred(name))
			{
				Messages.Message("MessageAlreadyPreferredName".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			Prefs.PreferredNames.Add(name.ToString());
			this.Close(true);
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x001D5C77 File Offset: 0x001D3E77
		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}

		// Token: 0x04002FB7 RID: 12215
		private string searchName = "";

		// Token: 0x04002FB8 RID: 12216
		private string[] searchWords;

		// Token: 0x04002FB9 RID: 12217
		private List<NameTriple> cachedNames;
	}
}
