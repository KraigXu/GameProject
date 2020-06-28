using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8F RID: 3727
	public class Screen_Credits : Window
	{
		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x000A0AE8 File Offset: 0x0009ECE8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x001EF835 File Offset: 0x001EDA35
		private float ViewWidth
		{
			get
			{
				return 800f;
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x001EF83C File Offset: 0x001EDA3C
		private float ViewHeight
		{
			get
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = this.creds.Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth)) + 20f;
				Text.Font = font;
				return result;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x001EF878 File Offset: 0x001EDA78
		private float MaxScrollPosition
		{
			get
			{
				return Mathf.Max(this.ViewHeight - (float)UI.screenHeight / 2f, 0f);
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x001EF898 File Offset: 0x001EDA98
		private float AutoScrollRate
		{
			get
			{
				if (!this.wonGame)
				{
					return 30f;
				}
				if (this.scrollPosition < this.victoryTextHeight - 200f)
				{
					return 20f;
				}
				float num = SongDefOf.EndCreditsSong.clip.length + 5f - 6f - this.victoryTextHeight / 20f;
				float t = (this.scrollPosition - this.victoryTextHeight) / 200f;
				return Mathf.Lerp(20f, this.MaxScrollPosition / num, t);
			}
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x001EF91D File Offset: 0x001EDB1D
		public Screen_Credits() : this("")
		{
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x001EF92C File Offset: 0x001EDB2C
		public Screen_Credits(string preCreditsMessage)
		{
			this.doWindowBackground = false;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.forcePause = true;
			this.creds = CreditsAssembler.AllCredits().ToList<CreditsEntry>();
			this.creds.Insert(0, new CreditRecord_Space(100f));
			if (!preCreditsMessage.NullOrEmpty())
			{
				this.creds.Insert(1, new CreditRecord_Space(200f));
				this.creds.Insert(2, new CreditRecord_Text(preCreditsMessage, TextAnchor.UpperLeft));
				this.creds.Insert(3, new CreditRecord_Space(50f));
				Text.Font = GameFont.Medium;
				this.victoryTextHeight = this.creds.Take(4).Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth));
			}
			this.creds.Add(new CreditRecord_Space(300f));
			this.creds.Add(new CreditRecord_Text("ThanksForPlaying".Translate(), TextAnchor.UpperCenter));
			string text = string.Empty;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				CreditRecord_Role creditRecord_Role = creditsEntry as CreditRecord_Role;
				if (creditRecord_Role == null)
				{
					text = string.Empty;
				}
				else
				{
					creditRecord_Role.displayKey = (text.NullOrEmpty() || creditRecord_Role.roleKey != text);
					text = creditRecord_Role.roleKey;
				}
			}
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x001EFAAC File Offset: 0x001EDCAC
		public override void PreOpen()
		{
			base.PreOpen();
			this.creationRealtime = Time.realtimeSinceStartup;
			if (this.wonGame)
			{
				this.timeUntilAutoScroll = 6f;
				return;
			}
			this.timeUntilAutoScroll = 1f;
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x001EFAE0 File Offset: 0x001EDCE0
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.timeUntilAutoScroll > 0f)
			{
				this.timeUntilAutoScroll -= Time.deltaTime;
			}
			else
			{
				this.scrollPosition += this.AutoScrollRate * Time.deltaTime;
			}
			if (this.wonGame && !this.playedMusic && Time.realtimeSinceStartup > this.creationRealtime + 5f)
			{
				Find.MusicManagerPlay.ForceStartSong(SongDefOf.EndCreditsSong, true);
				this.playedMusic = true;
			}
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x001EFB68 File Offset: 0x001EDD68
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			GUI.DrawTexture(rect, BaseContent.BlackTex);
			Rect position = new Rect(rect);
			position.yMin += 30f;
			position.yMax -= 30f;
			position.xMin = rect.center.x - 400f;
			position.width = 800f;
			float viewWidth = this.ViewWidth;
			float viewHeight = this.ViewHeight;
			this.scrollPosition = Mathf.Clamp(this.scrollPosition, 0f, this.MaxScrollPosition);
			GUI.BeginGroup(position);
			Rect position2 = new Rect(0f, 0f, viewWidth, viewHeight);
			position2.y -= this.scrollPosition;
			GUI.BeginGroup(position2);
			Text.Font = GameFont.Medium;
			float num = 0f;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				float num2 = creditsEntry.DrawHeight(position2.width);
				Rect rect2 = new Rect(0f, num, position2.width, num2);
				creditsEntry.Draw(rect2);
				num += num2;
			}
			GUI.EndGroup();
			GUI.EndGroup();
			if (Event.current.type == EventType.ScrollWheel)
			{
				this.Scroll(Event.current.delta.y * 25f);
				Event.current.Use();
			}
			if (Event.current.type == EventType.KeyDown)
			{
				if (Event.current.keyCode == KeyCode.DownArrow)
				{
					this.Scroll(250f);
					Event.current.Use();
				}
				if (Event.current.keyCode == KeyCode.UpArrow)
				{
					this.Scroll(-250f);
					Event.current.Use();
				}
			}
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x001EFD64 File Offset: 0x001EDF64
		private void Scroll(float offset)
		{
			this.scrollPosition += offset;
			this.timeUntilAutoScroll = 3f;
		}

		// Token: 0x04003187 RID: 12679
		private List<CreditsEntry> creds;

		// Token: 0x04003188 RID: 12680
		public bool wonGame;

		// Token: 0x04003189 RID: 12681
		private float timeUntilAutoScroll;

		// Token: 0x0400318A RID: 12682
		private float scrollPosition;

		// Token: 0x0400318B RID: 12683
		private bool playedMusic;

		// Token: 0x0400318C RID: 12684
		public float creationRealtime = -1f;

		// Token: 0x0400318D RID: 12685
		private float victoryTextHeight;

		// Token: 0x0400318E RID: 12686
		private const int ColumnWidth = 800;

		// Token: 0x0400318F RID: 12687
		private const float InitialAutoScrollDelay = 1f;

		// Token: 0x04003190 RID: 12688
		private const float InitialAutoScrollDelayWonGame = 6f;

		// Token: 0x04003191 RID: 12689
		private const float AutoScrollDelayAfterManualScroll = 3f;

		// Token: 0x04003192 RID: 12690
		private const float SongStartDelay = 5f;

		// Token: 0x04003193 RID: 12691
		private const float VictoryTextScrollSpeed = 20f;

		// Token: 0x04003194 RID: 12692
		private const float ScrollSpeedLerpHeight = 200f;

		// Token: 0x04003195 RID: 12693
		private const GameFont Font = GameFont.Medium;
	}
}
