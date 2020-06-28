using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003B0 RID: 944
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000A9D7A File Offset: 0x000A7F7A
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000A9D88 File Offset: 0x000A7F88
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000A9D96 File Offset: 0x000A7F96
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x000A9DA8 File Offset: 0x000A7FA8
		public float Alpha
		{
			get
			{
				if (this.TimeLeft < 0.6f)
				{
					return this.TimeLeft / 0.6f;
				}
				return 1f;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x000A9DCC File Offset: 0x000A7FCC
		private static bool ShouldDrawBackground
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return true;
				}
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					if (windowStack[i].CausesMessageBackground())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x00019EA1 File Offset: 0x000180A1
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x00017A00 File Offset: 0x00015C00
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000A9E0B File Offset: 0x000A800B
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x000A9E18 File Offset: 0x000A8018
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000A9E20 File Offset: 0x000A8020
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x000A9E28 File Offset: 0x000A8028
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000A9E33 File Offset: 0x000A8033
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000A9E3B File Offset: 0x000A803B
		public Message()
		{
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000A9E58 File Offset: 0x000A8058
		public Message(string text, MessageTypeDef def)
		{
			this.text = text;
			this.def = def;
			this.startingFrame = RealTime.frameCount;
			this.startingTime = RealTime.LastRealTime;
			this.startingTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextMessageID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000A9ED2 File Offset: 0x000A80D2
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000A9EE3 File Offset: 0x000A80E3
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000A9EF8 File Offset: 0x000A80F8
		public void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<float>(ref this.startingTime, "startingTime", 0f, false);
			Scribe_Values.Look<int>(ref this.startingFrame, "startingFrame", 0, false);
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000A9F9C File Offset: 0x000A819C
		public Rect CalculateRect(float x, float y)
		{
			Text.Font = GameFont.Small;
			if (this.cachedSize.x < 0f)
			{
				this.cachedSize = Text.CalcSize(this.text);
			}
			this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
			this.lastDrawRect = this.lastDrawRect.ContractedBy(-2f);
			return this.lastDrawRect;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000AA014 File Offset: 0x000A8214
		public void Draw(int xOffset, int yOffset)
		{
			Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
			Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect = rect.AtZero();
				float alpha = this.Alpha;
				GUI.color = new Color(1f, 1f, 1f, alpha);
				if (Message.ShouldDrawBackground)
				{
					GUI.color = new Color(0.15f, 0.15f, 0.15f, 0.8f * alpha);
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()) || this.quest != null)
				{
					UIHighlighter.HighlightOpportunity(rect, "Messages");
					Widgets.DrawHighlightIfMouseover(rect);
				}
				Widgets.Label(new Rect(2f, 0f, rect.width - 2f, rect.height), this.text);
				if (Current.ProgramState == ProgramState.Playing && Widgets.ButtonInvisible(rect, true))
				{
					if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(this.lookTargets.TryGetPrimaryTarget());
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
					else if (this.quest != null)
					{
						if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Quests)
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
						else
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						}
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					Messages.Notify_Mouseover(this);
				}
			}, false, false, 0f);
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000AA074 File Offset: 0x000A8274
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000AA0A3 File Offset: 0x000A82A3
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID;
		}

		// Token: 0x04001066 RID: 4198
		public MessageTypeDef def;

		// Token: 0x04001067 RID: 4199
		private int ID;

		// Token: 0x04001068 RID: 4200
		public string text;

		// Token: 0x04001069 RID: 4201
		private float startingTime;

		// Token: 0x0400106A RID: 4202
		public int startingFrame;

		// Token: 0x0400106B RID: 4203
		public int startingTick;

		// Token: 0x0400106C RID: 4204
		public LookTargets lookTargets;

		// Token: 0x0400106D RID: 4205
		public Quest quest;

		// Token: 0x0400106E RID: 4206
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x0400106F RID: 4207
		public Rect lastDrawRect;

		// Token: 0x04001070 RID: 4208
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x04001071 RID: 4209
		private const float FadeoutDuration = 0.6f;
	}
}
