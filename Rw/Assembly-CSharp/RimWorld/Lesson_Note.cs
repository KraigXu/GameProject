using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F27 RID: 3879
	public class Lesson_Note : Lesson
	{
		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06005F02 RID: 24322 RVA: 0x0020CB6D File Offset: 0x0020AD6D
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06005F03 RID: 24323 RVA: 0x0020CB7C File Offset: 0x0020AD7C
		public Rect MainRect
		{
			get
			{
				float height = Text.CalcHeight(this.def.HelpTextAdjusted, 432f) + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x06005F04 RID: 24324 RVA: 0x0020CBC0 File Offset: 0x0020ADC0
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x0020CBDB File Offset: 0x0020ADDB
		public Lesson_Note()
		{
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x0020CBF5 File Offset: 0x0020ADF5
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x0020CC16 File Offset: 0x0020AE16
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x0020CC28 File Offset: 0x0020AE28
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x0020CC3C File Offset: 0x0020AE3C
		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01(base.AgeSeconds / 0.4f);
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1f)
				{
					alpha = num / 1.1f;
				}
			}
			Find.WindowStack.ImmediateWindow(134706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.274509817f), true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.expiryTime)
				{
					this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			}, false, false, alpha);
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x0020CCE0 File Offset: 0x0020AEE0
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = this.def.noteTeaches ? KnowledgeAmount.NoteTaught : KnowledgeAmount.NoteClosed;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x06005F0B RID: 24331 RVA: 0x0020CD15 File Offset: 0x0020AF15
		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f && !this.Expiring)
			{
				this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
			}
		}

		// Token: 0x0400337B RID: 13179
		public ConceptDef def;

		// Token: 0x0400337C RID: 13180
		public bool doFadeIn = true;

		// Token: 0x0400337D RID: 13181
		private float expiryTime = float.MaxValue;

		// Token: 0x0400337E RID: 13182
		private const float RectWidth = 500f;

		// Token: 0x0400337F RID: 13183
		private const float TextWidth = 432f;

		// Token: 0x04003380 RID: 13184
		private const float FadeInDuration = 0.4f;

		// Token: 0x04003381 RID: 13185
		private const float DoneButPad = 8f;

		// Token: 0x04003382 RID: 13186
		private const float DoneButSize = 32f;

		// Token: 0x04003383 RID: 13187
		private const float ExpiryDuration = 2.1f;

		// Token: 0x04003384 RID: 13188
		private const float ExpiryFadeTime = 1.1f;
	}
}
