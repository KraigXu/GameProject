using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x000A6948 File Offset: 0x000A4B48
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000A695D File Offset: 0x000A4B5D
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x000A6964 File Offset: 0x000A4B64
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x000A6971 File Offset: 0x000A4B71
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x000A697E File Offset: 0x000A4B7E
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000A698B File Offset: 0x000A4B8B
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x000A6993 File Offset: 0x000A4B93
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x000A6948 File Offset: 0x000A4B48
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x000A699B File Offset: 0x000A4B9B
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		
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

		
		protected abstract string GetMouseoverText();

		
		public abstract void OpenLetter();

		
		public virtual void Received()
		{
		}

		
		public virtual void Removed()
		{
		}

		
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}

		
		public int ID;

		
		public LetterDef def;

		
		public TaggedString label;

		
		public LookTargets lookTargets;

		
		public Faction relatedFaction;

		
		public int arrivalTick;

		
		public float arrivalTime;

		
		public string debugInfo;

		
		public const float DrawWidth = 38f;

		
		public const float DrawHeight = 30f;

		
		private const float FallTime = 1f;

		
		private const float FallDistance = 200f;
	}
}
