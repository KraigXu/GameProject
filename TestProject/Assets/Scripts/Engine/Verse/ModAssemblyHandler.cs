using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Verse
{
	
	public class ModAssemblyHandler
	{
		
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		
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

		
		private ModContentPack mod;

		
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		
		private static bool globalResolverIsSet;
	}
}
