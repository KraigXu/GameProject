using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class LoadFolder : IEquatable<LoadFolder>
	{
		
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00052A01 File Offset: 0x00050C01
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredPackageIds.NullOrEmpty<string>() || ModLister.AnyFromListActive(this.requiredPackageIds)) && (this.disallowedPackageIds.NullOrEmpty<string>() || !ModLister.AnyFromListActive(this.disallowedPackageIds));
			}
		}

		
		public LoadFolder(string folderName, List<string> requiredPackageIds, List<string> disallowedPackageIds)
		{
			this.folderName = folderName;
			this.requiredPackageIds = requiredPackageIds;
			this.disallowedPackageIds = disallowedPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredPackageIds != null) ? requiredPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedPackageIds != null) ? disallowedPackageIds.GetHashCode() : 0);
		}

		
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		
		public override bool Equals(object obj)
		{
			LoadFolder other;
			return (other = (obj as LoadFolder)) != null && this.Equals(other);
		}

		
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		
		public string folderName;

		
		public List<string> requiredPackageIds;

		
		public List<string> disallowedPackageIds;

		
		private readonly int hashCodeCached;
	}
}
