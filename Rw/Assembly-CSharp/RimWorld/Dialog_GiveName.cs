using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E53 RID: 3667
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x060058BF RID: 22719 RVA: 0x001D841C File Offset: 0x001D661C
		private float Height
		{
			get
			{
				if (!this.useSecondName)
				{
					return 200f;
				}
				return 300f;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x060058C0 RID: 22720 RVA: 0x001D8431 File Offset: 0x001D6631
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, this.Height);
			}
		}

		// Token: 0x060058C1 RID: 22721 RVA: 0x001D8444 File Offset: 0x001D6644
		public Dialog_GiveName()
		{
			if (Find.AnyPlayerHomeMap != null && Find.AnyPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
			{
				if (Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
				}
				else
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement<Pawn>();
				}
			}
			else
			{
				this.suggestingPawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.RandomElement<Pawn>();
			}
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x001D84E4 File Offset: 0x001D66E4
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect2;
			if (!this.useSecondName)
			{
				Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst());
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, 80f, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, 80f, rect.width / 2f + 70f, 35f), this.curName);
				rect2 = new Rect(rect.width / 2f + 90f, rect.height - 35f, rect.width / 2f - 90f, 35f);
			}
			else
			{
				float num = 0f;
				string text = this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst();
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curName);
				num += 60f;
				text = this.secondNameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn);
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.secondNameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true))
				{
					this.curSecondName = this.secondNameGenerator();
				}
				this.curSecondName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curSecondName);
				num += 45f;
				float num2 = rect.width / 2f - 90f;
				rect2 = new Rect(rect.width / 2f - num2 / 2f, rect.height - 35f, num2, 35f);
			}
			if (Widgets.ButtonText(rect2, "OK".Translate(), true, true, true) || flag)
			{
				if (this.IsValidName(this.curName) && (!this.useSecondName || this.IsValidSecondName(this.curSecondName)))
				{
					if (this.useSecondName)
					{
						this.Named(this.curName);
						this.NamedSecond(this.curSecondName);
						Messages.Message(this.gainedNameMessageKey.Translate(this.curName, this.curSecondName), MessageTypeDefOf.TaskCompletion, false);
					}
					else
					{
						this.Named(this.curName);
						Messages.Message(this.gainedNameMessageKey.Translate(this.curName), MessageTypeDefOf.TaskCompletion, false);
					}
					Find.WindowStack.TryRemove(this, true);
				}
				else
				{
					Messages.Message(this.invalidNameMessageKey.Translate(), MessageTypeDefOf.RejectInput, false);
				}
				Event.current.Use();
			}
		}

		// Token: 0x060058C3 RID: 22723
		protected abstract bool IsValidName(string s);

		// Token: 0x060058C4 RID: 22724
		protected abstract void Named(string s);

		// Token: 0x060058C5 RID: 22725 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x04002FF0 RID: 12272
		protected Pawn suggestingPawn;

		// Token: 0x04002FF1 RID: 12273
		protected string curName;

		// Token: 0x04002FF2 RID: 12274
		protected Func<string> nameGenerator;

		// Token: 0x04002FF3 RID: 12275
		protected string nameMessageKey;

		// Token: 0x04002FF4 RID: 12276
		protected string gainedNameMessageKey;

		// Token: 0x04002FF5 RID: 12277
		protected string invalidNameMessageKey;

		// Token: 0x04002FF6 RID: 12278
		protected bool useSecondName;

		// Token: 0x04002FF7 RID: 12279
		protected string curSecondName;

		// Token: 0x04002FF8 RID: 12280
		protected Func<string> secondNameGenerator;

		// Token: 0x04002FF9 RID: 12281
		protected string secondNameMessageKey;

		// Token: 0x04002FFA RID: 12282
		protected string invalidSecondNameMessageKey;
	}
}
