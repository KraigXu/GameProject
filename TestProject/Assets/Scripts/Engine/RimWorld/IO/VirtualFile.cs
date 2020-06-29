using System;
using System.IO;

namespace RimWorld.IO
{
	
	public abstract class VirtualFile
	{
		
		// (get) Token: 0x060070D4 RID: 28884
		public abstract string Name { get; }

		
		// (get) Token: 0x060070D5 RID: 28885
		public abstract string FullPath { get; }

		
		// (get) Token: 0x060070D6 RID: 28886
		public abstract bool Exists { get; }

		
		// (get) Token: 0x060070D7 RID: 28887
		public abstract long Length { get; }

		
		public abstract Stream CreateReadStream();

		
		public abstract string ReadAllText();

		
		public abstract string[] ReadAllLines();

		
		public abstract byte[] ReadAllBytes();
	}
}
