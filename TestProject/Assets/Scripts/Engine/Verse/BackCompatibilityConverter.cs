using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020003F2 RID: 1010
	public abstract class BackCompatibilityConverter
	{
		// Token: 0x06001E11 RID: 7697
		public abstract bool AppliesToVersion(int majorVer, int minorVer);

		// Token: 0x06001E12 RID: 7698
		public abstract string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null);

		// Token: 0x06001E13 RID: 7699
		public abstract Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node);

		// Token: 0x06001E14 RID: 7700 RVA: 0x000B87B9 File Offset: 0x000B69B9
		public virtual int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			return index;
		}

		// Token: 0x06001E15 RID: 7701
		public abstract void PostExposeData(object obj);

		// Token: 0x06001E16 RID: 7702 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostCouldntLoadDef(string defName)
		{
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000B87BC File Offset: 0x000B69BC
		public bool AppliesToLoadedGameVersion(bool allowInactiveScribe = false)
		{
			return !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() && (allowInactiveScribe || Scribe.mode != LoadSaveMode.Inactive) && this.AppliesToVersion(VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion), VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion));
		}
	}
}
