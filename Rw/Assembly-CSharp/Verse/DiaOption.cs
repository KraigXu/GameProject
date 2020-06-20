using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003E9 RID: 1001
	public class DiaOption
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x000B6469 File Offset: 0x000B4669
		public static DiaOption DefaultOK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					resolveTree = true
				};
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x000B6486 File Offset: 0x000B4686
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000B6494 File Offset: 0x000B4694
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000B64E1 File Offset: 0x000B46E1
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x000B6518 File Offset: 0x000B4718
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x000B6578 File Offset: 0x000B4778
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000B65D2 File Offset: 0x000B47D2
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000B65E2 File Offset: 0x000B47E2
		public void SetText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x000B65EC File Offset: 0x000B47EC
		public float OptOnGUI(Rect rect, bool active = true)
		{
			Color textColor = Widgets.NormalOptionColor;
			string text = this.text;
			if (this.disabled)
			{
				textColor = this.DisabledOptionColor;
				if (this.disabledReason != null)
				{
					text = text + " (" + this.disabledReason + ")";
				}
			}
			rect.height = Text.CalcHeight(text, rect.width);
			if (this.hyperlink.def != null)
			{
				Widgets.HyperlinkWithIcon(rect, this.hyperlink, text, 2f, 6f);
			}
			else if (Widgets.ButtonText(rect, text, false, !this.disabled, textColor, active && !this.disabled))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000B66A0 File Offset: 0x000B48A0
		protected void Activate()
		{
			if (this.clickSound != null && !this.resolveTree)
			{
				this.clickSound.PlayOneShotOnCamera(null);
			}
			if (this.resolveTree)
			{
				this.OwningDialog.Close(true);
			}
			if (this.action != null)
			{
				this.action();
			}
			if (this.linkLateBind != null)
			{
				this.OwningDialog.GotoNode(this.linkLateBind());
				return;
			}
			if (this.link != null)
			{
				this.OwningDialog.GotoNode(this.link);
			}
		}

		// Token: 0x0400121A RID: 4634
		public Window dialog;

		// Token: 0x0400121B RID: 4635
		protected string text;

		// Token: 0x0400121C RID: 4636
		public DiaNode link;

		// Token: 0x0400121D RID: 4637
		public Func<DiaNode> linkLateBind;

		// Token: 0x0400121E RID: 4638
		public bool resolveTree;

		// Token: 0x0400121F RID: 4639
		public Action action;

		// Token: 0x04001220 RID: 4640
		public bool disabled;

		// Token: 0x04001221 RID: 4641
		public string disabledReason;

		// Token: 0x04001222 RID: 4642
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04001223 RID: 4643
		public Dialog_InfoCard.Hyperlink hyperlink;

		// Token: 0x04001224 RID: 4644
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
