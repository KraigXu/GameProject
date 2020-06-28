using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Verse
{
	// Token: 0x020001F1 RID: 497
	public class ModAssemblyHandler
	{
		// Token: 0x06000E0C RID: 3596 RVA: 0x000505BF File Offset: 0x0004E7BF
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x000505DC File Offset: 0x0004E7DC
		public void ReloadAll()
		{
			if (!ModAssemblyHandler.globalResolverIsSet)
			{
				ResolveEventHandler @object = (object obj, ResolveEventArgs args) => Assembly.GetExecutingAssembly();
				AppDomain.CurrentDomain.AssemblyResolve += @object.Invoke;
				ModAssemblyHandler.globalResolverIsSet = true;
			}
			foreach (FileInfo fileInfo in from f in ModContentPack.GetAllFilesForModPreserveOrder(this.mod, "Assemblies/", (string e) => e.ToLower() == ".dll", null)
			select f.Item2)
			{
				Assembly assembly = null;
				try
				{
					byte[] rawAssembly = File.ReadAllBytes(fileInfo.FullName);
					FileInfo fileInfo2 = new FileInfo(Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.FullName)) + ".pdb");
					if (fileInfo2.Exists)
					{
						byte[] rawSymbolStore = File.ReadAllBytes(fileInfo2.FullName);
						assembly = AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
					}
					else
					{
						assembly = AppDomain.CurrentDomain.Load(rawAssembly);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
					break;
				}
				if (!(assembly == null) && this.AssemblyIsUsable(assembly))
				{
					this.loadedAssemblies.Add(assembly);
				}
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00050778 File Offset: 0x0004E978
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"ReflectionTypeLoadException getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex
				}));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						stringBuilder.AppendLine("   => " + ex2.ToString());
					}
				}
				Log.Error(stringBuilder.ToString(), false);
				return false;
			}
			catch (Exception ex3)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex3
				}), false);
				return false;
			}
			return true;
		}

		// Token: 0x04000A9F RID: 2719
		private ModContentPack mod;

		// Token: 0x04000AA0 RID: 2720
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		// Token: 0x04000AA1 RID: 2721
		private static bool globalResolverIsSet;
	}
}
