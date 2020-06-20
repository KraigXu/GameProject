using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000C07 RID: 3079
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004923 RID: 18723 RVA: 0x0018D67D File Offset: 0x0018B87D
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				yield return new System.Version(VersionControl.CurrentMajor, VersionControl.CurrentMinor);
				yield break;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004924 RID: 18724 RVA: 0x0018D686 File Offset: 0x0018B886
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004925 RID: 18725 RVA: 0x0018D698 File Offset: 0x0018B898
		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				yield return this.playerFaction;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					yield return this.parts[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004926 RID: 18726 RVA: 0x0018D6A8 File Offset: 0x0018B8A8
		// (set) Token: 0x06004927 RID: 18727 RVA: 0x0018D6C9 File Offset: 0x0018B8C9
		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + this, false);
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x0018D700 File Offset: 0x0018B900
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", Array.Empty<object>());
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.parts.RemoveAll((ScenPart x) => x == null) != 0)
				{
					Log.Warning("Some ScenParts were null after loading.", false);
				}
				if (this.parts.RemoveAll((ScenPart x) => x.HasNullDefs()) != 0)
				{
					Log.Warning("Some ScenParts had null defs.", false);
				}
			}
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x0018D7FA File Offset: 0x0018B9FA
		public IEnumerable<string> ConfigErrors()
		{
			if (this.name.NullOrEmpty())
			{
				yield return "no title";
			}
			if (this.parts.NullOrEmpty<ScenPart>())
			{
				yield return "no parts";
			}
			if (this.playerFaction == null)
			{
				yield return "no playerFaction";
			}
			foreach (ScenPart scenPart in this.AllParts)
			{
				foreach (string text in scenPart.ConfigErrors())
				{
					yield return text;
				}
				IEnumerator<string> enumerator2 = null;
			}
			IEnumerator<ScenPart> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x0018D80C File Offset: 0x0018BA0C
		public string GetFullInformationText()
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart scenPart in this.AllParts)
				{
					scenPart.summarized = false;
				}
				foreach (ScenPart scenPart2 in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = scenPart2.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878, false);
				result = "Cannot read data.";
			}
			return result;
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x0018D990 File Offset: 0x0018BB90
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x0018D998 File Offset: 0x0018BB98
		public Scenario CopyForEditing()
		{
			Scenario scenario = new Scenario();
			scenario.name = this.name;
			scenario.summary = this.summary;
			scenario.description = this.description;
			scenario.playerFaction = (ScenPart_PlayerFaction)this.playerFaction.CopyForEditing();
			scenario.parts.AddRange(from p in this.parts
			select p.CopyForEditing());
			scenario.categoryInt = ScenarioCategory.CustomLocal;
			return scenario;
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x0018DA20 File Offset: 0x0018BC20
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x0018DA6C File Offset: 0x0018BC6C
		public Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			list.Add(new Page_SelectStartingSite());
			foreach (Page item in this.parts.SelectMany((ScenPart p) => p.GetConfigPages()))
			{
				list.Add(item);
			}
			Page page = PageUtility.StitchedPages(list);
			if (page != null)
			{
				Page page2 = page;
				while (page2.next != null)
				{
					page2 = page2.next;
				}
				page2.nextAct = delegate
				{
					PageUtility.InitGameStart();
				};
			}
			return page;
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x0018DB4C File Offset: 0x0018BD4C
		public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x0018DBA4 File Offset: 0x0018BDA4
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x0018DBF0 File Offset: 0x0018BDF0
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x0018DC40 File Offset: 0x0018BE40
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x0018DC78 File Offset: 0x0018BE78
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x0018DCC4 File Offset: 0x0018BEC4
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x0018DD10 File Offset: 0x0018BF10
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x0018DD5C File Offset: 0x0018BF5C
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x0018DDA8 File Offset: 0x0018BFA8
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x0018DDF4 File Offset: 0x0018BFF4
		public float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				ScenPart_StatFactor scenPart_StatFactor = this.parts[i] as ScenPart_StatFactor;
				if (scenPart_StatFactor != null)
				{
					num *= scenPart_StatFactor.GetStatFactor(stat);
				}
			}
			return num;
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x0018DE40 File Offset: 0x0018C040
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x0018DE74 File Offset: 0x0018C074
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + part, false);
			}
			this.parts.Remove(part);
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x0018DEA4 File Offset: 0x0018C0A4
		public bool CanReorder(ScenPart part, ReorderDirection dir)
		{
			if (!part.def.PlayerAddRemovable)
			{
				return false;
			}
			int num = this.parts.IndexOf(part);
			if (dir == ReorderDirection.Up)
			{
				return num != 0 && (num <= 0 || this.parts[num - 1].def.PlayerAddRemovable);
			}
			if (dir == ReorderDirection.Down)
			{
				return num != this.parts.Count - 1;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x0018DF14 File Offset: 0x0018C114
		public void Reorder(ScenPart part, ReorderDirection dir)
		{
			int num = this.parts.IndexOf(part);
			this.parts.RemoveAt(num);
			if (dir == ReorderDirection.Up)
			{
				this.parts.Insert(num - 1, part);
			}
			if (dir == ReorderDirection.Down)
			{
				this.parts.Insert(num + 1, part);
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x0018DF60 File Offset: 0x0018C160
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x0018DF9C File Offset: 0x0018C19C
		public void PrepareForWorkshopUpload()
		{
			string path = this.name + Rand.RangeInclusive(100, 999).ToString();
			this.tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(this.tempUploadDir);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete();
			}
			directoryInfo.Create();
			string text = Path.Combine(this.tempUploadDir, this.name);
			text += ".rsc";
			GameDataSaveLoader.SaveScenario(this, text);
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x0018E020 File Offset: 0x0018C220
		public AcceptanceReport TryUploadReport()
		{
			if (this.name == null || this.name.Length < 3 || this.summary == null || this.summary.Length < 3 || this.description == null || this.description.Length < 3)
			{
				return "TextFieldsMustBeFilled".TranslateSimple();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x0018E084 File Offset: 0x0018C284
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x0018E08C File Offset: 0x0018C28C
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x0018E0BC File Offset: 0x0018C2BC
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x0018E0C4 File Offset: 0x0018C2C4
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x0018E0CC File Offset: 0x0018C2CC
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x0018E0D3 File Offset: 0x0018C2D3
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0018E0E5 File Offset: 0x0018C2E5
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x0018E0F2 File Offset: 0x0018C2F2
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x0018E10E File Offset: 0x0018C30E
		public override string ToString()
		{
			if (this.name.NullOrEmpty())
			{
				return "LabellessScenario";
			}
			return this.name;
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x0018E12C File Offset: 0x0018C32C
		public override int GetHashCode()
		{
			int num = 6126121;
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			if (this.summary != null)
			{
				num ^= this.summary.GetHashCode();
			}
			if (this.description != null)
			{
				num ^= this.description.GetHashCode();
			}
			return num ^ this.publishedFileIdInt.GetHashCode();
		}

		// Token: 0x040029D0 RID: 10704
		[MustTranslate]
		public string name;

		// Token: 0x040029D1 RID: 10705
		[MustTranslate]
		public string summary;

		// Token: 0x040029D2 RID: 10706
		[MustTranslate]
		public string description;

		// Token: 0x040029D3 RID: 10707
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x040029D4 RID: 10708
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x040029D5 RID: 10709
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x040029D6 RID: 10710
		private ScenarioCategory categoryInt;

		// Token: 0x040029D7 RID: 10711
		[NoTranslate]
		public string fileName;

		// Token: 0x040029D8 RID: 10712
		private WorkshopItemHook workshopHookInt;

		// Token: 0x040029D9 RID: 10713
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x040029DA RID: 10714
		public bool enabled = true;

		// Token: 0x040029DB RID: 10715
		public bool showInUI = true;

		// Token: 0x040029DC RID: 10716
		public const int NameMaxLength = 55;

		// Token: 0x040029DD RID: 10717
		public const int SummaryMaxLength = 300;

		// Token: 0x040029DE RID: 10718
		public const int DescriptionMaxLength = 1000;
	}
}
