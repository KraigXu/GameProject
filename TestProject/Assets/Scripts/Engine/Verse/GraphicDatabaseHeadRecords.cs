using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002FB RID: 763
	public static class GraphicDatabaseHeadRecords
	{
		// Token: 0x0600158A RID: 5514 RVA: 0x0007DCE8 File Offset: 0x0007BEE8
		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0007DD00 File Offset: 0x0007BF00
		private static void BuildDatabaseIfNecessary()
		{
			if (GraphicDatabaseHeadRecords.heads.Count > 0 && GraphicDatabaseHeadRecords.skull != null && GraphicDatabaseHeadRecords.stump != null)
			{
				return;
			}
			GraphicDatabaseHeadRecords.heads.Clear();
			foreach (string text in GraphicDatabaseHeadRecords.HeadsFolderPaths)
			{
				foreach (string str in GraphicDatabaseUtility.GraphicNamesInFolder(text))
				{
					GraphicDatabaseHeadRecords.heads.Add(new GraphicDatabaseHeadRecords.HeadGraphicRecord(text + "/" + str));
				}
			}
			GraphicDatabaseHeadRecords.skull = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.SkullPath);
			GraphicDatabaseHeadRecords.stump = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.StumpPath);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0007DDC0 File Offset: 0x0007BFC0
		public static Graphic_Multi GetHeadNamed(string graphicPath, Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			for (int i = 0; i < GraphicDatabaseHeadRecords.heads.Count; i++)
			{
				GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads[i];
				if (headGraphicRecord.graphicPath == graphicPath)
				{
					return headGraphicRecord.GetGraphic(skinColor, false);
				}
			}
			Log.Message("Tried to get pawn head at path " + graphicPath + " that was not found. Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor, false);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0007DE31 File Offset: 0x0007C031
		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white, true);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0007DE48 File Offset: 0x0007C048
		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor, false);
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0007DE5C File Offset: 0x0007C05C
		public static Graphic_Multi GetHeadRandom(Gender gender, Color skinColor, CrownType crownType)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			Predicate<GraphicDatabaseHeadRecords.HeadGraphicRecord> predicate = (GraphicDatabaseHeadRecords.HeadGraphicRecord head) => head.crownType == crownType && head.gender == gender;
			int num = 0;
			GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord;
			for (;;)
			{
				headGraphicRecord = GraphicDatabaseHeadRecords.heads.RandomElement<GraphicDatabaseHeadRecords.HeadGraphicRecord>();
				if (predicate(headGraphicRecord))
				{
					break;
				}
				num++;
				if (num > 40)
				{
					goto Block_2;
				}
			}
			return headGraphicRecord.GetGraphic(skinColor, false);
			Block_2:
			foreach (GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord2 in GraphicDatabaseHeadRecords.heads.InRandomOrder(null))
			{
				if (predicate(headGraphicRecord2))
				{
					return headGraphicRecord2.GetGraphic(skinColor, false);
				}
			}
			Log.Error("Failed to find head for gender=" + gender + ". Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor, false);
		}

		// Token: 0x04000E1A RID: 3610
		private static List<GraphicDatabaseHeadRecords.HeadGraphicRecord> heads = new List<GraphicDatabaseHeadRecords.HeadGraphicRecord>();

		// Token: 0x04000E1B RID: 3611
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord skull;

		// Token: 0x04000E1C RID: 3612
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord stump;

		// Token: 0x04000E1D RID: 3613
		private static readonly string[] HeadsFolderPaths = new string[]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		// Token: 0x04000E1E RID: 3614
		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		// Token: 0x04000E1F RID: 3615
		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		// Token: 0x020014A1 RID: 5281
		private class HeadGraphicRecord
		{
			// Token: 0x06007B62 RID: 31586 RVA: 0x0029AE24 File Offset: 0x00299024
			public HeadGraphicRecord(string graphicPath)
			{
				this.graphicPath = graphicPath;
				string[] array = Path.GetFileNameWithoutExtension(graphicPath).Split(new char[]
				{
					'_'
				});
				try
				{
					this.crownType = ParseHelper.FromString<CrownType>(array[array.Length - 2]);
					this.gender = ParseHelper.FromString<Gender>(array[array.Length - 3]);
				}
				catch (Exception ex)
				{
					Log.Error("Parse error with head graphic at " + graphicPath + ": " + ex.Message, false);
					this.crownType = CrownType.Undefined;
					this.gender = Gender.None;
				}
			}

			// Token: 0x06007B63 RID: 31587 RVA: 0x0029AEC4 File Offset: 0x002990C4
			public Graphic_Multi GetGraphic(Color color, bool dessicated = false)
			{
				Shader shader = (!dessicated) ? ShaderDatabase.CutoutSkin : ShaderDatabase.Cutout;
				for (int i = 0; i < this.graphics.Count; i++)
				{
					if (color.IndistinguishableFrom(this.graphics[i].Key) && this.graphics[i].Value.Shader == shader)
					{
						return this.graphics[i].Value;
					}
				}
				Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, shader, Vector2.one, color);
				this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
				return graphic_Multi;
			}

			// Token: 0x04004E2B RID: 20011
			public Gender gender;

			// Token: 0x04004E2C RID: 20012
			public CrownType crownType;

			// Token: 0x04004E2D RID: 20013
			public string graphicPath;

			// Token: 0x04004E2E RID: 20014
			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
		}
	}
}
