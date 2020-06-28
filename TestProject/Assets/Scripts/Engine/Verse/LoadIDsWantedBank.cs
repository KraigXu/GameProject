using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020002CA RID: 714
	public class LoadIDsWantedBank
	{
		// Token: 0x06001423 RID: 5155 RVA: 0x000750C0 File Offset: 0x000732C0
		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					for (int i = 0; i < this.idsRead.Count; i++)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  ",
							this.idsRead[i].targetLoadID.ToStringSafe<string>(),
							" of type ",
							this.idsRead[i].targetType,
							". pathRelToParent=",
							this.idsRead[i].pathRelToParent,
							", parent=",
							this.idsRead[i].parent.ToStringSafe<IExposable>()
						}));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					for (int j = 0; j < this.idListsRead.Count; j++)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  List with ",
							(this.idListsRead[j].targetLoadIDs != null) ? this.idListsRead[j].targetLoadIDs.Count : 0,
							" elements. pathRelToParent=",
							this.idListsRead[j].pathRelToParent,
							", parent=",
							this.idListsRead[j].parent.ToStringSafe<IExposable>()
						}));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines(), false);
			}
			this.Clear();
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0007529E File Offset: 0x0007349E
		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x000752B8 File Offset: 0x000734B8
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idsRead.Count; i++)
			{
				if (this.idsRead[i].parent == parent && this.idsRead[i].pathRelToParent == pathRelToParent)
				{
					Log.Error(string.Concat(new string[]
					{
						"Tried to register the same load ID twice: ",
						targetLoadID,
						", pathRelToParent=",
						pathRelToParent,
						", parent=",
						parent.ToStringSafe<IExposable>()
					}), false);
					return;
				}
			}
			this.idsRead.Add(new LoadIDsWantedBank.IdRecord(targetLoadID, targetType, pathRelToParent, parent));
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0007535C File Offset: 0x0007355C
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0007539C File Offset: 0x0007359C
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idListsRead.Count; i++)
			{
				if (this.idListsRead[i].parent == parent && this.idListsRead[i].pathRelToParent == pathRelToParent)
				{
					Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>(), false);
					return;
				}
			}
			this.idListsRead.Add(new LoadIDsWantedBank.IdListRecord(targetLoadIDList, pathRelToParent, parent));
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0007541C File Offset: 0x0007361C
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0007545C File Offset: 0x0007365C
		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idsRead.Count; i++)
			{
				if (this.idsRead[i].parent == parent && this.idsRead[i].pathRelToParent == pathRelToParent)
				{
					string targetLoadID = this.idsRead[i].targetLoadID;
					if (typeof(T) != this.idsRead[i].targetType)
					{
						Log.Error(string.Concat(new object[]
						{
							"Trying to get load ID of object of type ",
							typeof(T),
							", but it was registered as ",
							this.idsRead[i].targetType,
							". pathRelToParent=",
							pathRelToParent,
							", parent=",
							parent.ToStringSafe<IExposable>()
						}), false);
					}
					this.idsRead.RemoveAt(i);
					return targetLoadID;
				}
			}
			Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>(), false);
			return null;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00075578 File Offset: 0x00073778
		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idListsRead.Count; i++)
			{
				if (this.idListsRead[i].parent == parent && this.idListsRead[i].pathRelToParent == pathRelToParent)
				{
					List<string> targetLoadIDs = this.idListsRead[i].targetLoadIDs;
					this.idListsRead.RemoveAt(i);
					return targetLoadIDs;
				}
			}
			Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>(), false);
			return new List<string>();
		}

		// Token: 0x04000D8A RID: 3466
		private List<LoadIDsWantedBank.IdRecord> idsRead = new List<LoadIDsWantedBank.IdRecord>();

		// Token: 0x04000D8B RID: 3467
		private List<LoadIDsWantedBank.IdListRecord> idListsRead = new List<LoadIDsWantedBank.IdListRecord>();

		// Token: 0x02001491 RID: 5265
		private struct IdRecord
		{
			// Token: 0x06007B1A RID: 31514 RVA: 0x0029A241 File Offset: 0x00298441
			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04004DEF RID: 19951
			public string targetLoadID;

			// Token: 0x04004DF0 RID: 19952
			public Type targetType;

			// Token: 0x04004DF1 RID: 19953
			public string pathRelToParent;

			// Token: 0x04004DF2 RID: 19954
			public IExposable parent;
		}

		// Token: 0x02001492 RID: 5266
		private struct IdListRecord
		{
			// Token: 0x06007B1B RID: 31515 RVA: 0x0029A260 File Offset: 0x00298460
			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04004DF3 RID: 19955
			public List<string> targetLoadIDs;

			// Token: 0x04004DF4 RID: 19956
			public string pathRelToParent;

			// Token: 0x04004DF5 RID: 19957
			public IExposable parent;
		}
	}
}
