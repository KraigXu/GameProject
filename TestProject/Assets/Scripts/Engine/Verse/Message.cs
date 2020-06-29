using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000A9D7A File Offset: 0x000A7F7A
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000A9D88 File Offset: 0x000A7F88
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000A9D96 File Offset: 0x000A7F96
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		
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

		
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x00019EA1 File Offset: 0x000180A1
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x00017A00 File Offset: 0x00015C00
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000A9E0B File Offset: 0x000A800B
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x000A9E18 File Offset: 0x000A8018
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000A9E20 File Offset: 0x000A8020
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x000A9E28 File Offset: 0x000A8028
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000A9E33 File Offset: 0x000A8033
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		
		public Message()
		{
		}

		
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

		
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		
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

		
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null));
		}

		
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID;
		}

		
		public MessageTypeDef def;

		
		private int ID;

		
		public string text;

		
		private float startingTime;

		
		public int startingFrame;

		
		public int startingTick;

		
		public LookTargets lookTargets;

		
		public Quest quest;

		
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		
		public Rect lastDrawRect;

		
		private const float DefaultMessageLifespan = 13f;

		
		private const float FadeoutDuration = 0.6f;
	}
}
