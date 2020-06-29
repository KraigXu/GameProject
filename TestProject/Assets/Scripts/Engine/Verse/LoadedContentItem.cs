using System;
using RimWorld.IO;

namespace Verse
{
	
	public class LoadedContentItem<T> where T : class
	{
		
		public LoadedContentItem(VirtualFile internalFile, T contentItem, IDisposable extraDisposable = null)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
			this.extraDisposable = extraDisposable;
		}

		
		public VirtualFile internalFile;

		
		public T contentItem;

		
		public IDisposable extraDisposable;
	}
}
