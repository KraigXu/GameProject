using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001F9 RID: 505
	public class LoadFolder : IEquatable<LoadFolder>
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00052A01 File Offset: 0x00050C01
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredPackageIds.NullOrEmpty<string>() || ModLister.AnyFromListActive(this.requiredPackageIds)) && (this.disallowedPackageIds.NullOrEmpty<string>() || !ModLister.AnyFromListActive(this.disallowedPackageIds));
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00052A3C File Offset: 0x00050C3C
		public LoadFolder(string folderName, List<string> requiredPackageIds, List<string> disallowedPackageIds)
		{
			this.folderName = folderName;
			this.requiredPackageIds = requiredPackageIds;
			this.disallowedPackageIds = disallowedPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredPackageIds != null) ? requiredPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedPackageIds != null) ? disallowedPackageIds.GetHashCode() : 0);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00052AB0 File Offset: 0x00050CB0
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00052AC8 File Offset: 0x00050CC8
		public override bool Equals(object obj)
		{
			LoadFolder other;
			return (other = (obj as LoadFolder)) != null && this.Equals(other);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00052AE8 File Offset: 0x00050CE8
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		// Token: 0x04000AD9 RID: 2777
		public string folderName;

		// Token: 0x04000ADA RID: 2778
		public List<string> requiredPackageIds;

		// Token: 0x04000ADB RID: 2779
		public List<string> disallowedPackageIds;

		// Token: 0x04000ADC RID: 2780
		private readonly int hashCodeCached;
	}
}
