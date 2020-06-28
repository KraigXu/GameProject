using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0F RID: 3599
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x060056E5 RID: 22245 RVA: 0x001CD7BD File Offset: 0x001CB9BD
		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x060056E6 RID: 22246 RVA: 0x001CD7CC File Offset: 0x001CB9CC
		private bool ShowGroupFrames
		{
			get
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				for (int i = 0; i < entries.Count; i++)
				{
					num = Mathf.Max(num, entries[i].group);
				}
				return num >= 1;
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x001CD80D File Offset: 0x001CBA0D
		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x001CD815 File Offset: 0x001CBA15
		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060056E9 RID: 22249 RVA: 0x001CD81D File Offset: 0x001CBA1D
		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060056EA RID: 22250 RVA: 0x001CD82F File Offset: 0x001CBA2F
		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060056EB RID: 22251 RVA: 0x001CD83D File Offset: 0x001CBA3D
		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500;
			}
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x001CD85A File Offset: 0x001CBA5A
		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x001CD864 File Offset: 0x001CBA64
		public void ColonistBarOnGUI()
		{
			if (!this.Visible)
			{
				return;
			}
			if (Event.current.type != EventType.Layout)
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				bool showGroupFrames = this.ShowGroupFrames;
				int reorderableGroup = -1;
				for (int i = 0; i < this.cachedDrawLocs.Count; i++)
				{
					Rect rect = new Rect(this.cachedDrawLocs[i].x, this.cachedDrawLocs[i].y, this.Size.x, this.Size.y);
					ColonistBar.Entry entry = entries[i];
					bool flag = num != entry.group;
					num = entry.group;
					if (flag)
					{
						reorderableGroup = ReorderableWidget.NewGroup(entry.reorderAction, ReorderableDirection.Horizontal, this.SpaceBetweenColonistsHorizontal, entry.extraDraggedItemOnGUI);
					}
					bool reordering;
					if (entry.pawn != null)
					{
						this.drawer.HandleClicks(rect, entry.pawn, reorderableGroup, out reordering);
					}
					else
					{
						reordering = false;
					}
					if (Event.current.type == EventType.Repaint)
					{
						if (flag && showGroupFrames)
						{
							this.drawer.DrawGroupFrame(entry.group);
						}
						if (entry.pawn != null)
						{
							this.drawer.DrawColonist(rect, entry.pawn, entry.map, this.colonistsToHighlight.Contains(entry.pawn), reordering);
							if (entry.pawn.HasExtraHomeFaction(null))
							{
								Faction extraHomeFaction = entry.pawn.GetExtraHomeFaction(null);
								GUI.color = extraHomeFaction.Color;
								float num2 = rect.width * 0.5f;
								GUI.DrawTexture(new Rect(rect.xMax - num2 - 2f, rect.yMax - num2 - 2f, num2, num2), extraHomeFaction.def.FactionIcon);
								GUI.color = Color.white;
							}
						}
					}
				}
				num = -1;
				if (showGroupFrames)
				{
					for (int j = 0; j < this.cachedDrawLocs.Count; j++)
					{
						ColonistBar.Entry entry2 = entries[j];
						bool flag2 = num != entry2.group;
						num = entry2.group;
						if (flag2)
						{
							this.drawer.HandleGroupFrameClicks(entry2.group);
						}
					}
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				this.colonistsToHighlight.Clear();
			}
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x001CDAAC File Offset: 0x001CBCAC
		private void CheckRecacheEntries()
		{
			if (!this.entriesDirty)
			{
				return;
			}
			this.entriesDirty = false;
			this.cachedEntries.Clear();
			if (Find.PlaySettings.showColonistBar)
			{
				ColonistBar.tmpMaps.Clear();
				ColonistBar.tmpMaps.AddRange(Find.Maps);
				ColonistBar.tmpMaps.SortBy((Map x) => !x.IsPlayerHome, (Map x) => x.uniqueID);
				int num = 0;
				for (int i = 0; i < ColonistBar.tmpMaps.Count; i++)
				{
					ColonistBar.tmpPawns.Clear();
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.FreeColonists);
					List<Thing> list = ColonistBar.tmpMaps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].IsDessicated())
						{
							Pawn innerPawn = ((Corpse)list[j]).InnerPawn;
							if (innerPawn != null && innerPawn.IsColonist)
							{
								ColonistBar.tmpPawns.Add(innerPawn);
							}
						}
					}
					List<Pawn> allPawnsSpawned = ColonistBar.tmpMaps[i].mapPawns.AllPawnsSpawned;
					for (int k = 0; k < allPawnsSpawned.Count; k++)
					{
						Corpse corpse = allPawnsSpawned[k].carryTracker.CarriedThing as Corpse;
						if (corpse != null && !corpse.IsDessicated() && corpse.InnerPawn.IsColonist)
						{
							ColonistBar.tmpPawns.Add(corpse.InnerPawn);
						}
					}
					PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
					for (int l = 0; l < ColonistBar.tmpPawns.Count; l++)
					{
						this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[l], ColonistBar.tmpMaps[i], num));
					}
					if (!ColonistBar.tmpPawns.Any<Pawn>())
					{
						this.cachedEntries.Add(new ColonistBar.Entry(null, ColonistBar.tmpMaps[i], num));
					}
					num++;
				}
				ColonistBar.tmpCaravans.Clear();
				ColonistBar.tmpCaravans.AddRange(Find.WorldObjects.Caravans);
				ColonistBar.tmpCaravans.SortBy((Caravan x) => x.ID);
				for (int m = 0; m < ColonistBar.tmpCaravans.Count; m++)
				{
					if (ColonistBar.tmpCaravans[m].IsPlayerControlled)
					{
						ColonistBar.tmpPawns.Clear();
						ColonistBar.tmpPawns.AddRange(ColonistBar.tmpCaravans[m].PawnsListForReading);
						PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
						for (int n = 0; n < ColonistBar.tmpPawns.Count; n++)
						{
							if (ColonistBar.tmpPawns[n].IsColonist)
							{
								this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[n], null, num));
							}
						}
						num++;
					}
				}
			}
			this.drawer.Notify_RecachedEntries();
			ColonistBar.tmpPawns.Clear();
			ColonistBar.tmpMaps.Clear();
			ColonistBar.tmpCaravans.Clear();
			this.drawLocsFinder.CalculateDrawLocs(this.cachedDrawLocs, out this.cachedScale);
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x001CDE10 File Offset: 0x001CC010
		public float GetEntryRectAlpha(Rect rect)
		{
			float t;
			if (Messages.CollidesWithAnyMessage(rect, out t))
			{
				return Mathf.Lerp(1f, 0.2f, t);
			}
			return 1f;
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x001CDE3D File Offset: 0x001CC03D
		public void Highlight(Pawn pawn)
		{
			if (!this.Visible)
			{
				return;
			}
			if (!this.colonistsToHighlight.Contains(pawn))
			{
				this.colonistsToHighlight.Add(pawn);
			}
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x001CDE64 File Offset: 0x001CC064
		public void Reorder(int from, int to, int entryGroup)
		{
			int num = 0;
			Pawn pawn = null;
			Pawn pawn2 = null;
			Pawn pawn3 = null;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == from)
					{
						pawn = this.cachedEntries[i].pawn;
					}
					if (num == to)
					{
						pawn2 = this.cachedEntries[i].pawn;
					}
					pawn3 = this.cachedEntries[i].pawn;
					num++;
				}
			}
			if (pawn == null)
			{
				return;
			}
			int num2 = (pawn2 != null) ? pawn2.playerSettings.displayOrder : (pawn3.playerSettings.displayOrder + 1);
			for (int j = 0; j < this.cachedEntries.Count; j++)
			{
				Pawn pawn4 = this.cachedEntries[j].pawn;
				if (pawn4 != null)
				{
					if (pawn4.playerSettings.displayOrder == num2)
					{
						if (pawn2 != null && this.cachedEntries[j].group == entryGroup)
						{
							if (pawn4.thingIDNumber < pawn2.thingIDNumber)
							{
								pawn4.playerSettings.displayOrder--;
							}
							else
							{
								pawn4.playerSettings.displayOrder++;
							}
						}
					}
					else if (pawn4.playerSettings.displayOrder > num2)
					{
						pawn4.playerSettings.displayOrder++;
					}
					else
					{
						pawn4.playerSettings.displayOrder--;
					}
				}
			}
			pawn.playerSettings.displayOrder = num2;
			this.MarkColonistsDirty();
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x001CE01C File Offset: 0x001CC21C
		public void DrawColonistMouseAttachment(int index, Vector2 dragStartPos, int entryGroup)
		{
			Pawn pawn = null;
			Vector2 vector = default(Vector2);
			int num = 0;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == index)
					{
						pawn = this.cachedEntries[i].pawn;
						vector = this.cachedDrawLocs[i];
						break;
					}
					num++;
				}
			}
			if (pawn != null)
			{
				Texture iconTex = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true);
				Rect rect = new Rect(vector.x, vector.y, this.Size.x, this.Size.y);
				Rect pawnTextureRect = this.drawer.GetPawnTextureRect(rect.position);
				pawnTextureRect.position += Event.current.mousePosition - dragStartPos;
				GenUI.DrawMouseAttachment(iconTex, "", 0f, default(Vector2), new Rect?(pawnTextureRect), false, default(Color));
			}
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x001CE144 File Offset: 0x001CC344
		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x001CE168 File Offset: 0x001CC368
		public bool TryGetEntryAt(Vector2 pos, out ColonistBar.Entry entry)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y).Contains(pos))
				{
					entry = entries[i];
					return true;
				}
			}
			entry = default(ColonistBar.Entry);
			return false;
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x001CE1E8 File Offset: 0x001CC3E8
		public List<Pawn> GetColonistsInOrder()
		{
			List<ColonistBar.Entry> entries = this.Entries;
			ColonistBar.tmpColonistsInOrder.Clear();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].pawn != null)
				{
					ColonistBar.tmpColonistsInOrder.Add(entries[i].pawn);
				}
			}
			return ColonistBar.tmpColonistsInOrder;
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x001CE240 File Offset: 0x001CC440
		public List<Thing> ColonistsOrCorpsesInScreenRect(Rect rect)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			ColonistBar.tmpColonistsWithMap.Clear();
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (rect.Overlaps(new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y)))
				{
					Pawn pawn = entries[i].pawn;
					if (pawn != null)
					{
						Thing first;
						if (pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
						{
							first = pawn.Corpse;
						}
						else
						{
							first = pawn;
						}
						ColonistBar.tmpColonistsWithMap.Add(new Pair<Thing, Map>(first, entries[i].map));
					}
				}
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == null))
				{
					ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != null);
					goto IL_179;
				}
			}
			if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == Find.CurrentMap))
			{
				ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != Find.CurrentMap);
			}
			IL_179:
			ColonistBar.tmpColonists.Clear();
			for (int j = 0; j < ColonistBar.tmpColonistsWithMap.Count; j++)
			{
				ColonistBar.tmpColonists.Add(ColonistBar.tmpColonistsWithMap[j].First);
			}
			ColonistBar.tmpColonistsWithMap.Clear();
			return ColonistBar.tmpColonists;
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x001CE418 File Offset: 0x001CC618
		public List<Thing> MapColonistsOrCorpsesInScreenRect(Rect rect)
		{
			ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Spawned)
				{
					ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Add(list[i]);
				}
			}
			return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x001CE47C File Offset: 0x001CC67C
		public List<Pawn> CaravanMembersInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravanPawns.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravanPawns;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.IsCaravanMember())
				{
					ColonistBar.tmpCaravanPawns.Add(pawn);
				}
			}
			return ColonistBar.tmpCaravanPawns;
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x001CE4E4 File Offset: 0x001CC6E4
		public List<Caravan> CaravanMembersCaravansInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravans.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravans;
			}
			List<Pawn> list = this.CaravanMembersInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				ColonistBar.tmpCaravans.Add(list[i].GetCaravan());
			}
			return ColonistBar.tmpCaravans;
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x001CE540 File Offset: 0x001CC740
		public Caravan CaravanMemberCaravanAt(Vector2 at)
		{
			if (!this.Visible)
			{
				return null;
			}
			Pawn pawn = this.ColonistOrCorpseAt(at) as Pawn;
			if (pawn != null && pawn.IsCaravanMember())
			{
				return pawn.GetCaravan();
			}
			return null;
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x001CE578 File Offset: 0x001CC778
		public Thing ColonistOrCorpseAt(Vector2 pos)
		{
			if (!this.Visible)
			{
				return null;
			}
			ColonistBar.Entry entry;
			if (!this.TryGetEntryAt(pos, out entry))
			{
				return null;
			}
			Pawn pawn = entry.pawn;
			Thing result;
			if (pawn != null && pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
			{
				result = pawn.Corpse;
			}
			else
			{
				result = pawn;
			}
			return result;
		}

		// Token: 0x04002F5A RID: 12122
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		// Token: 0x04002F5B RID: 12123
		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		// Token: 0x04002F5C RID: 12124
		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		// Token: 0x04002F5D RID: 12125
		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		// Token: 0x04002F5E RID: 12126
		private float cachedScale = 1f;

		// Token: 0x04002F5F RID: 12127
		private bool entriesDirty = true;

		// Token: 0x04002F60 RID: 12128
		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		// Token: 0x04002F61 RID: 12129
		public static readonly Texture2D BGTex = Command.BGTex;

		// Token: 0x04002F62 RID: 12130
		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		// Token: 0x04002F63 RID: 12131
		public const float BaseSelectedTexJump = 20f;

		// Token: 0x04002F64 RID: 12132
		public const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04002F65 RID: 12133
		public const float EntryInAnotherMapAlpha = 0.4f;

		// Token: 0x04002F66 RID: 12134
		public const float BaseSpaceBetweenGroups = 25f;

		// Token: 0x04002F67 RID: 12135
		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		// Token: 0x04002F68 RID: 12136
		public const float BaseSpaceBetweenColonistsVertical = 32f;

		// Token: 0x04002F69 RID: 12137
		public const float FactionIconSpacing = 2f;

		// Token: 0x04002F6A RID: 12138
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04002F6B RID: 12139
		private static List<Map> tmpMaps = new List<Map>();

		// Token: 0x04002F6C RID: 12140
		private static List<Caravan> tmpCaravans = new List<Caravan>();

		// Token: 0x04002F6D RID: 12141
		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		// Token: 0x04002F6E RID: 12142
		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		// Token: 0x04002F6F RID: 12143
		private static List<Thing> tmpColonists = new List<Thing>();

		// Token: 0x04002F70 RID: 12144
		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		// Token: 0x04002F71 RID: 12145
		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		// Token: 0x02001CED RID: 7405
		public struct Entry
		{
			// Token: 0x0600A3BB RID: 41915 RVA: 0x0030B310 File Offset: 0x00309510
			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
				this.reorderAction = delegate(int from, int to)
				{
					Find.ColonistBar.Reorder(from, to, group);
				};
				this.extraDraggedItemOnGUI = delegate(int index, Vector2 dragStartPos)
				{
					Find.ColonistBar.DrawColonistMouseAttachment(index, dragStartPos, group);
				};
			}

			// Token: 0x04006DAF RID: 28079
			public Pawn pawn;

			// Token: 0x04006DB0 RID: 28080
			public Map map;

			// Token: 0x04006DB1 RID: 28081
			public int group;

			// Token: 0x04006DB2 RID: 28082
			public Action<int, int> reorderAction;

			// Token: 0x04006DB3 RID: 28083
			public Action<int, Vector2> extraDraggedItemOnGUI;
		}
	}
}
