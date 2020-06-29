using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public class DiaOption
	{
		
		
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

		
		
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		
		public DiaOption(string text)
		{
			this.text = text;
		}

		
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		
		public void SetText(string newText)
		{
			this.text = newText;
		}

		
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

		
		public Window dialog;

		
		protected string text;

		
		public DiaNode link;

		
		public Func<DiaNode> linkLateBind;

		
		public bool resolveTree;

		
		public Action action;

		
		public bool disabled;

		
		public string disabledReason;

		
		public SoundDef clickSound = SoundDefOf.PageChange;

		
		public Dialog_InfoCard.Hyperlink hyperlink;

		
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
