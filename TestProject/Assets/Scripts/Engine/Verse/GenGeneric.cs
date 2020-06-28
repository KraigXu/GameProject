using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x0200001E RID: 30
	public static class GenGeneric
	{
		// Token: 0x060001FB RID: 507 RVA: 0x000099DE File Offset: 0x00007BDE
		private static MethodInfo MethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			return genericBase.MakeGenericType(new Type[]
			{
				genericParam
			}).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000099F8 File Offset: 0x00007BF8
		public static void InvokeGenericMethod(object objectToInvoke, Type genericParam, string methodName, params object[] args)
		{
			objectToInvoke.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(objectToInvoke, args);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009A1F File Offset: 0x00007C1F
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName, params object[] args)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, args);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009A30 File Offset: 0x00007C30
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, null);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009A41 File Offset: 0x00007C41
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName)
		{
			return baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(null, null);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009A62 File Offset: 0x00007C62
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName, params object[] args)
		{
			return baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(null, args);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009A83 File Offset: 0x00007C83
		private static PropertyInfo PropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			return genericBase.MakeGenericType(new Type[]
			{
				genericParam
			}).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00009A9D File Offset: 0x00007C9D
		public static object GetStaticPropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			return GenGeneric.PropertyOnGenericType(genericBase, genericParam, propertyName).GetGetMethod().Invoke(null, null);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00009AB3 File Offset: 0x00007CB3
		public static bool HasGenericDefinition(this Type type, Type Def)
		{
			return type.GetTypeWithGenericDefinition(Def) != null;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009AC4 File Offset: 0x00007CC4
		public static Type GetTypeWithGenericDefinition(this Type type, Type Def)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (Def == null)
			{
				throw new ArgumentNullException("Def");
			}
			if (!Def.IsGenericTypeDefinition)
			{
				throw new ArgumentException("The Def needs to be a GenericTypeDefinition", "Def");
			}
			if (Def.IsInterface)
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == Def)
					{
						return type2;
					}
				}
			}
			Type type3 = type;
			while (type3 != null)
			{
				if (type3.IsGenericType && type3.GetGenericTypeDefinition() == Def)
				{
					return type3;
				}
				type3 = type3.BaseType;
			}
			return null;
		}

		// Token: 0x0400004B RID: 75
		public const BindingFlags BindingFlagsAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
