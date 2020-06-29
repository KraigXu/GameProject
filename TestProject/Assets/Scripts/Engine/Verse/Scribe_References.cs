using System;
using System.Xml;
using RimWorld.Planet;

namespace Verse
{
	
	public static class Scribe_References
	{
		
		public static void Look<T>(ref T refee, string label, bool saveDestroyedThings = false) where T : ILoadReferenceable
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (refee == null)
				{
					Scribe.saver.WriteElement(label, "null");
					return;
				}
				Thing thing = refee as Thing;
				if (thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(thing, label, saveDestroyedThings))
				{
					return;
				}
				WorldObject worldObject = refee as WorldObject;
				if (worldObject != null && Scribe_References.CheckSaveReferenceToDestroyedWorldObject(worldObject, label, saveDestroyedThings))
				{
					return;
				}
				string uniqueLoadID = refee.GetUniqueLoadID();
				Scribe.saver.WriteElement(label, uniqueLoadID);
				Scribe.saver.loadIDsErrorsChecker.RegisterReferenced(refee, label);
				return;
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					if (Scribe.loader.curParent != null && Scribe.loader.curParent.GetType().IsValueType)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Trying to load reference of an object of type ",
							typeof(T),
							" with label ",
							label,
							", but our current node is a value type. The reference won't be loaded properly. curParent=",
							Scribe.loader.curParent
						}), false);
					}
					XmlNode xmlNode = Scribe.loader.curXmlParent[label];
					string targetLoadID;
					if (xmlNode != null)
					{
						targetLoadID = xmlNode.InnerText;
					}
					else
					{
						targetLoadID = null;
					}
					Scribe.loader.crossRefs.loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(T), label);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					refee = Scribe.loader.crossRefs.TakeResolvedRef<T>(label);
				}
				return;
			}
		}

		
		public static void Look<T>(ref WeakReference<T> refee, string label, bool saveDestroyedThings = false) where T : class, ILoadReferenceable
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				T t = (refee != null) ? refee.Target : default(T);
				Scribe_References.Look<T>(ref t, label, saveDestroyedThings);
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				T t2 = default(T);
				Scribe_References.Look<T>(ref t2, label, saveDestroyedThings);
				return;
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				T t3 = default(T);
				Scribe_References.Look<T>(ref t3, label, saveDestroyedThings);
				if (t3 != null)
				{
					refee = new WeakReference<T>(t3);
				}
			}
		}

		
		public static bool CheckSaveReferenceToDestroyedThing(Thing th, string label, bool saveDestroyedThings)
		{
			if (!th.Destroyed)
			{
				return false;
			}
			if (!saveDestroyedThings)
			{
				Scribe.saver.WriteElement(label, "null");
				return true;
			}
			if (th.Discarded)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Trying to save reference to a discarded thing ",
					th,
					" with saveDestroyedThings=true. This means that it's not deep-saved anywhere and is no longer managed by anything in the code, so saving its reference will always fail. , label=",
					label
				}), false);
				Scribe.saver.WriteElement(label, "null");
				return true;
			}
			return false;
		}

		
		public static bool CheckSaveReferenceToDestroyedWorldObject(WorldObject w, string label, bool saveDestroyedWorldObjects)
		{
			if (!w.Destroyed)
			{
				return false;
			}
			if (!saveDestroyedWorldObjects)
			{
				Scribe.saver.WriteElement(label, "null");
				return true;
			}
			return false;
		}
	}
}
