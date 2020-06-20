using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200039C RID: 924
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x000A6948 File Offset: 0x000A4B48
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000A695D File Offset: 0x000A4B5D
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x000A6964 File Offset: 0x000A4B64
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x000A6971 File Offset: 0x000A4B71
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x000A697E File Offset: 0x000A4B7E
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000A698B File Offset: 0x000A4B8B
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x000A6993 File Offset: 0x000A4B93
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x000A6948 File Offset: 0x000A4B48
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x000A699B File Offset: 0x000A4B9B
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000A69A4 File Offset: 0x000A4BA4
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000A6A28 File Offset: 0x000A4C28
		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1f)
			{
				rect2.y -= (1f - num2) * 200f;
				color.a = num2 / 1f;
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15f && num2 % 5f < 1f)
			{
				float num3 = (float)UI.screenWidth * 0.06f;
				float num4 = 2f * (num2 % 1f) - 1f;
				float num5 = num3 * (1f - num4 * num4);
				rect2.x -= num5;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (this.def.flashInterval > 0f)
				{
					float num6 = Time.time - (this.arrivalTime + 1f);
					if (num6 > 0f && num6 % this.def.flashInterval < 1f)
					{
						GenUI.DrawFlash(num, topY, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num6) * 0.55f, this.def.flashColor);
					}
				}
				GUI.color = color;
				Widgets.DrawShadowAround(rect2);
				GUI.DrawTexture(rect2, this.def.Icon);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperRight;
				string text = this.PostProcessedLabel();
				Vector2 vector = Text.CalcSize(text);
				float x = vector.x;
				float y = vector.y;
				Vector2 vector2 = new Vector2(rect2.x + rect2.width / 2f, rect2.center.y - y / 2f + 4f);
				float num7 = vector2.x + x / 2f - (float)(UI.screenWidth - 2);
				if (num7 > 0f)
				{
					vector2.x -= num7;
				}
				GUI.DrawTexture(new Rect(vector2.x - x / 2f - 6f - 1f, vector2.y, x + 12f, 16f), TexUI.GrayTextBG);
				GUI.color = new Color(1f, 1f, 1f, 0.75f);
				Widgets.Label(new Rect(vector2.x - x / 2f, vector2.y - 3f, x, 999f), text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (this.CanDismissWithRightClick && Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000A6D54 File Offset: 0x000A4F54
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			if (Mouse.IsOver(new Rect(num, topY, 38f, 30f)))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				TaggedString mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.RawText.NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 += 20f;
					float x = num - 330f - 10f;
					Rect infoRect = new Rect(x, topY - num2 / 2f, 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Rect position = infoRect.AtZero().ContractedBy(10f);
						GUI.BeginGroup(position);
						Widgets.Label(new Rect(0f, 0f, position.width, position.height), mouseoverText.Resolve());
						GUI.EndGroup();
					}, true, false, 1f);
				}
			}
		}

		// Token: 0x06001B30 RID: 6960
		protected abstract string GetMouseoverText();

		// Token: 0x06001B31 RID: 6961
		public abstract void OpenLetter();

		// Token: 0x06001B32 RID: 6962 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Received()
		{
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Removed()
		{
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000A697E File Offset: 0x000A4B7E
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000A6E56 File Offset: 0x000A5056
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000A6E5E File Offset: 0x000A505E
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}

		// Token: 0x0400101E RID: 4126
		public int ID;

		// Token: 0x0400101F RID: 4127
		public LetterDef def;

		// Token: 0x04001020 RID: 4128
		public TaggedString label;

		// Token: 0x04001021 RID: 4129
		public LookTargets lookTargets;

		// Token: 0x04001022 RID: 4130
		public Faction relatedFaction;

		// Token: 0x04001023 RID: 4131
		public int arrivalTick;

		// Token: 0x04001024 RID: 4132
		public float arrivalTime;

		// Token: 0x04001025 RID: 4133
		public string debugInfo;

		// Token: 0x04001026 RID: 4134
		public const float DrawWidth = 38f;

		// Token: 0x04001027 RID: 4135
		public const float DrawHeight = 30f;

		// Token: 0x04001028 RID: 4136
		private const float FallTime = 1f;

		// Token: 0x04001029 RID: 4137
		private const float FallDistance = 200f;
	}
}
