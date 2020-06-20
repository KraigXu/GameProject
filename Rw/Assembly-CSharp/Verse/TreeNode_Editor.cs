using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A4 RID: 932
	public class TreeNode_Editor : TreeNode
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x000A7B10 File Offset: 0x000A5D10
		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x000A7B24 File Offset: 0x000A5D24
		public Type ObjectType
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.FieldType;
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetGenericArguments()[0];
				}
				if (this.obj != null)
				{
					return this.obj.GetType();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x000A7B80 File Offset: 0x000A5D80
		// (set) Token: 0x06001B5E RID: 7006 RVA: 0x000A7BF0 File Offset: 0x000A5DF0
		public object Value
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.GetValue(this.ParentObj);
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[]
					{
						this.owningIndex
					});
				}
				throw new InvalidOperationException();
			}
			set
			{
				if (this.owningField != null)
				{
					this.owningField.SetValue(this.ParentObj, value);
				}
				if (this.IsListItem)
				{
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[]
					{
						this.owningIndex
					});
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000A7C5A File Offset: 0x000A5E5A
		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001B60 RID: 7008 RVA: 0x000A7C68 File Offset: 0x000A5E68
		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001B61 RID: 7009 RVA: 0x000A7C70 File Offset: 0x000A5E70
		public override bool Openable
		{
			get
			{
				return this.obj != null && this.nodeType != EditTreeNodeType.TerminalValue && (this.nodeType != EditTreeNodeType.ListRoot || (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) != 0);
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001B62 RID: 7010 RVA: 0x000A7CC6 File Offset: 0x000A5EC6
		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001B63 RID: 7011 RVA: 0x000A7CD4 File Offset: 0x000A5ED4
		public bool HasNewButton
		{
			get
			{
				return (this.nodeType == EditTreeNodeType.ComplexObject && this.obj == null) || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>());
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001B64 RID: 7012 RVA: 0x000A7D0B File Offset: 0x000A5F0B
		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x000A7D3C File Offset: 0x000A5F3C
		public string ExtraInfoText
		{
			get
			{
				if (this.obj == null)
				{
					return "null";
				}
				if (this.obj.GetType().HasAttribute<EditorShowClassNameAttribute>())
				{
					return this.obj.GetType().Name;
				}
				if (this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null);
					return string.Concat(new string[]
					{
						"(",
						num.ToString(),
						" ",
						(num == 1) ? "element" : "elements",
						")"
					});
				}
				return "";
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x000A7E1D File Offset: 0x000A601D
		public string LabelText
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.Name;
				}
				if (this.IsListItem)
				{
					return this.owningIndex.ToString();
				}
				return this.ObjectType.Name;
			}
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000A7E58 File Offset: 0x000A6058
		private TreeNode_Editor()
		{
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000A7E6E File Offset: 0x000A606E
		public static TreeNode_Editor NewRootNode(object rootObj)
		{
			if (rootObj.GetType().IsValueEditable())
			{
				throw new ArgumentException();
			}
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.owningField = null;
			treeNode_Editor.obj = rootObj;
			treeNode_Editor.nestDepth = 0;
			treeNode_Editor.RebuildChildNodes();
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000A7EAC File Offset: 0x000A60AC
		public static TreeNode_Editor NewChildNodeFromField(TreeNode_Editor parent, FieldInfo fieldInfo)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningField = fieldInfo;
			if (!fieldInfo.FieldType.IsValueEditable())
			{
				treeNode_Editor.obj = fieldInfo.GetValue(parent.obj);
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000A7F08 File Offset: 0x000A6108
		private static TreeNode_Editor NewChildNodeFromListItem(TreeNode_Editor parent, int listIndex)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningIndex = listIndex;
			object obj = parent.obj;
			Type type = obj.GetType();
			if (!type.GetGenericArguments()[0].IsValueEditable())
			{
				object value = type.GetProperty("Item").GetValue(obj, new object[]
				{
					listIndex
				});
				treeNode_Editor.obj = value;
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000A7F8C File Offset: 0x000A618C
		private void InitiallyCacheData()
		{
			if (this.obj != null && this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				this.nodeType = EditTreeNodeType.ListRoot;
			}
			else if (this.ObjectType.IsValueEditable())
			{
				this.nodeType = EditTreeNodeType.TerminalValue;
			}
			else
			{
				this.nodeType = EditTreeNodeType.ComplexObject;
			}
			if (this.obj != null)
			{
				this.editWidgetsMethod = this.obj.GetType().GetMethod("DoEditWidgets");
			}
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x000A8020 File Offset: 0x000A6220
		public void RebuildChildNodes()
		{
			if (this.obj == null)
			{
				return;
			}
			this.children = new List<TreeNode>();
			Type objType = this.obj.GetType();
			if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
			{
				int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
				for (int i = 0; i < num; i++)
				{
					TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, i);
					this.children.Add(item);
				}
				return;
			}
			IEnumerable<FieldInfo> fields = this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Func<FieldInfo, int> <>9__0;
			Func<FieldInfo, int> keySelector;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((FieldInfo f) => this.InheritanceDistanceBetween(objType, f.DeclaringType)));
			}
			foreach (FieldInfo fieldInfo in fields.OrderByDescending(keySelector))
			{
				if (fieldInfo.GetCustomAttributes(typeof(UnsavedAttribute), true).Length == 0 && fieldInfo.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length == 0)
				{
					TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, fieldInfo);
					this.children.Add(item2);
				}
			}
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000A8180 File Offset: 0x000A6380
		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			while (!(type == parentType))
			{
				type = type.BaseType;
				num++;
				if (type == null)
				{
					Log.Error(childType + " is not a subclass of " + parentType, false);
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000A81C4 File Offset: 0x000A63C4
		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[]
				{
					this.indexToDelete
				});
				this.RebuildChildNodes();
				this.indexToDelete = -1;
			}
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000A821C File Offset: 0x000A641C
		public void Delete()
		{
			if (this.owningField != null)
			{
				this.owningField.SetValue(this.obj, null);
				return;
			}
			if (this.IsListItem)
			{
				((TreeNode_Editor)this.parentNode).indexToDelete = this.owningIndex;
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000A8270 File Offset: 0x000A6470
		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj == null)
			{
				return;
			}
			if (this.editWidgetsMethod != null)
			{
				WidgetRow widgetRow = listing.StartWidgetsRow(this.nestDepth);
				this.editWidgetsMethod.Invoke(this.obj, new object[]
				{
					widgetRow
				});
			}
			Editable editable = this.obj as Editable;
			if (editable != null)
			{
				GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
				foreach (string text in editable.ConfigErrors())
				{
					listing.InfoText(text, this.nestDepth);
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000A833C File Offset: 0x000A653C
		public override string ToString()
		{
			string text = "EditTreeNode(";
			if (this.ParentObj != null)
			{
				text = text + " owningObj=" + this.ParentObj;
			}
			if (this.owningField != null)
			{
				text = text + " owningField=" + this.owningField;
			}
			if (this.owningIndex >= 0)
			{
				text = text + " owningIndex=" + this.owningIndex;
			}
			return text + ")";
		}

		// Token: 0x04001036 RID: 4150
		public object obj;

		// Token: 0x04001037 RID: 4151
		public FieldInfo owningField;

		// Token: 0x04001038 RID: 4152
		public int owningIndex = -1;

		// Token: 0x04001039 RID: 4153
		private MethodInfo editWidgetsMethod;

		// Token: 0x0400103A RID: 4154
		public EditTreeNodeType nodeType;

		// Token: 0x0400103B RID: 4155
		private int indexToDelete = -1;
	}
}
