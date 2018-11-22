// ##############################################################################
//
// ice_editor_styles.cs | ICEEditorStyle
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
//
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ICE.World.EditorUtilities
{
	public static class ICEEditorStyle
	{
		static ICEEditorStyle() {}

		private static int m_FixedWidth = 15;
		private static int m_FixedHeight = 15;
		private static int m_FixedHeightLarge = 25;
		private static int m_MarginTop = 2;
		private static int m_MarginLeft = 1;
		private static int m_MarginRight = 1;

		private static GUIStyle[] m_SplitterArray = new GUIStyle[10];

		private static GUIStyle m_SplitterFull;
		private static GUIStyle m_SplitterOffset30;


		private static GUIStyle m_InfoButton;
		private static GUIStyle m_CMDButton;
		private static GUIStyle m_CMDButtonDouble;
		private static GUIStyle m_ButtonLarge;
		private static GUIStyle m_ButtonSmall;
		private static GUIStyle m_ButtonMiddle;
		private static GUIStyle m_Foldout;
		private static GUIStyle m_FoldoutNormal;
		private static GUIStyle m_LabelBold;
		private static GUIStyle m_ToggleBold;
		private static GUIStyle m_ButtonExtraLarge;
		private static GUIStyle m_LinkStyle;
		private static GUIStyle m_SmallTextStyle;
		private static GUIStyle m_Popup;
		private static GUIStyle m_ButtonFlex;
		private static GUIStyle m_GreyMiniLabel;


		private static readonly Color m_DefaultLineColor = EditorGUIUtility.isProSkin ? new Color(0.157f, 0.157f, 0.157f) : new Color(0.5f, 0.5f, 0.5f);
		private static readonly Color m_DefaultTextColor = EditorGUIUtility.isProSkin ? new Color(0.157f, 0.157f, 0.157f) : new Color(0.0f, 0.0f, 0.0f);

		public static Color DefaultLineColor{
			get{return m_DefaultLineColor;}
		}

		public static Color DefaultTextColor{
			get{return m_DefaultTextColor;}
		}

		public static GUIStyle GreyMiniLabel
		{
			get
			{
				if( m_GreyMiniLabel == null )
				{
					m_GreyMiniLabel = new GUIStyle("Label");
					m_GreyMiniLabel.fontSize = 9;
					m_GreyMiniLabel.normal.textColor = Color.gray;
				}
				return m_GreyMiniLabel;
			}
		}


		public static GUIStyle SmallTextStyle
		{
			get
			{
				if( m_SmallTextStyle == null )
				{
					m_SmallTextStyle = new GUIStyle("Label");
					m_SmallTextStyle.fontSize = 9;
				}
				return m_SmallTextStyle;
			}
		}
		
		public static GUIStyle LinkStyle
		{
			get
			{
				if( m_LinkStyle == null )
				{
					m_LinkStyle = new GUIStyle("Label");
					m_LinkStyle.normal.textColor = (EditorGUIUtility.isProSkin ? new Color(0.8f, 0.8f, 1.0f, 1.0f) : Color.blue);
				}
				return m_LinkStyle;
			}
		}

		public static GUIStyle GetLinkStyle( Color _color )
		{
			GUIStyle _style = new GUIStyle("Label");
			_style.normal.textColor = _color;
			return _style;		
		}

		public static GUIStyle SplitterFull
		{
			get
			{
				if (m_SplitterFull == null)
				{
					m_SplitterFull = new GUIStyle();
					m_SplitterFull.normal.background = EditorGUIUtility.whiteTexture;
					m_SplitterFull.stretchWidth = true;
					m_SplitterFull.margin = new RectOffset(0, 0, 7, 7);
					
				}
				return m_SplitterFull;
			}
		}

		public static GUIStyle SplitterOffset30
		{
			get
			{
				if (m_SplitterOffset30 == null)
				{
					m_SplitterOffset30 = new GUIStyle();
					m_SplitterOffset30.normal.background = EditorGUIUtility.whiteTexture;
					m_SplitterOffset30.stretchWidth = true;
					m_SplitterOffset30.margin = new RectOffset( EditorGUI.indentLevel * 30 , 0, 7, 7);
					
				}
				return m_SplitterOffset30;
			}
		}


		public static GUIStyle Popup{
			get
			{
				if (m_Popup == null)
				{
					m_Popup = new GUIStyle("Popup");
					m_Popup.fontSize = 8;
					m_Popup.alignment = TextAnchor.MiddleLeft;
					m_Popup.margin.top = m_MarginTop;
					m_Popup.margin.left = m_MarginLeft;
					m_Popup.margin.right = m_MarginRight;
					m_Popup.padding = new RectOffset(5, 4, 2, 2);
					m_Popup.fixedHeight = m_FixedHeight;
					
				}
				return m_Popup;
			}

		}

		public static GUIStyle Button( int _width )
		{
			GUIStyle _button = new GUIStyle("Button");

			_button.fontSize = 8;
			_button.alignment = TextAnchor.MiddleCenter;
			_button.margin.top = m_MarginTop;
			_button.margin.left = m_MarginLeft;
			_button.margin.right = m_MarginRight;
			_button.padding = new RectOffset(0, 4, 0, 2);
			_button.fixedWidth = _width;
			_button.fixedHeight = m_FixedHeight;

			return _button;
		}


		public static GUIStyle CMDButton
		{
			get
			{
				if (m_CMDButton == null)
				{
					m_CMDButton = new GUIStyle("Button");
					m_CMDButton.fontSize = 8;
					m_CMDButton.alignment = TextAnchor.MiddleCenter;
					m_CMDButton.margin.top = m_MarginTop;
					m_CMDButton.margin.left = m_MarginLeft;
					m_CMDButton.margin.right = m_MarginRight;
					m_CMDButton.padding = new RectOffset(0, 0, 0, 2);
					m_CMDButton.fixedWidth = m_FixedWidth;
					m_CMDButton.fixedHeight = m_FixedHeight;
				}
				return m_CMDButton;
			}
		}

		public static GUIStyle CMDButtonDouble
		{
			get
			{
				if (m_CMDButtonDouble == null)
				{
					m_CMDButtonDouble = new GUIStyle("Button");
					m_CMDButtonDouble.fontSize = 8;
					m_CMDButtonDouble.alignment = TextAnchor.MiddleCenter;
					m_CMDButtonDouble.margin.top = m_MarginTop;
					m_CMDButtonDouble.margin.left = m_MarginLeft;
					m_CMDButtonDouble.margin.right = m_MarginRight;
					m_CMDButtonDouble.padding = new RectOffset(0, 0, 0, 2);
					m_CMDButtonDouble.fixedWidth = m_FixedWidth*2;
					m_CMDButtonDouble.fixedHeight = m_FixedHeight;
				}
				return m_CMDButtonDouble;
			}
		}

		public static GUIStyle ButtonExtraLarge
		{
			get
			{
				if( m_ButtonExtraLarge == null )
				{
					m_ButtonExtraLarge = new GUIStyle("Button");
					//m_ButtonExtraLarge.fontSize = 8;
					m_ButtonExtraLarge.fontStyle = FontStyle.Bold;
					m_ButtonExtraLarge.alignment = TextAnchor.MiddleCenter;
					//m_ButtonExtraLarge.margin.top = 1;
					m_ButtonExtraLarge.margin.left = m_MarginLeft;
					m_ButtonExtraLarge.margin.right = m_MarginRight;
					m_ButtonExtraLarge.padding = new RectOffset(10, 10, 0, 2);
					//m_ButtonExtraLarge.fixedWidth = 15;
					m_ButtonExtraLarge.fixedHeight = m_FixedHeightLarge;
				}
				return m_ButtonExtraLarge;
			}
		}

		public static GUIStyle InfoButton
		{
			get
			{
				if (m_InfoButton == null)
				{
					m_InfoButton = new GUIStyle("Button");
					m_InfoButton.fontSize = 10;
					m_InfoButton.fontStyle = FontStyle.Bold;
					m_InfoButton.alignment = TextAnchor.MiddleCenter;
					m_InfoButton.margin.top = m_MarginTop;
					m_InfoButton.margin.left = m_MarginLeft;
					m_InfoButton.margin.right = m_MarginRight;
					m_InfoButton.padding = new RectOffset(0, 0, 0, 1);
					m_InfoButton.fixedWidth = m_FixedWidth;
					m_InfoButton.fixedHeight = m_FixedHeight;
				}
				return m_InfoButton;
			}
		}

		public static GUIStyle ButtonFlex
		{
			get
			{
				if (m_ButtonFlex == null)
				{
					m_ButtonFlex = new GUIStyle("Button");
					m_ButtonFlex.fontSize = 8;
					m_ButtonFlex.alignment = TextAnchor.MiddleCenter;
					m_ButtonFlex.margin.top = m_MarginTop;
					m_ButtonFlex.margin.left = m_MarginLeft;
					m_ButtonFlex.margin.right = m_MarginRight;
					m_ButtonFlex.padding = new RectOffset(0, 0, 0, 2);
					m_ButtonFlex.fixedHeight = m_FixedHeight;
					m_ButtonFlex.stretchWidth = true;
			
				}
				return m_ButtonFlex;
			}
		}

		public static GUIStyle GetButtonFlex( int _indent )
		{
			GUIStyle _style = new GUIStyle("Button");
			_style.fontSize = 8;
			_style.alignment = TextAnchor.MiddleCenter;
			_style.margin.top = m_MarginTop;
			_style.margin.left = _indent * 15;
			_style.margin.right = m_MarginRight;
			_style.padding = new RectOffset(0, 0, 0, 2);
			_style.fixedHeight = m_FixedHeight;
			_style.stretchWidth = true;

			return _style;
		}

		public static GUIStyle ButtonLarge
		{
			get
			{
				if (m_ButtonLarge == null)
				{
					m_ButtonLarge = new GUIStyle("Button");
					m_ButtonLarge.fontSize = 8;
					m_ButtonLarge.alignment = TextAnchor.MiddleCenter;
					m_ButtonLarge.margin.top = m_MarginTop;
					m_ButtonLarge.margin.left = m_MarginLeft;
					m_ButtonLarge.margin.right = m_MarginRight;
					m_ButtonLarge.padding = new RectOffset(0, 0, 0, 2);
					m_ButtonLarge.fixedWidth = 100;
					m_ButtonLarge.fixedHeight = m_FixedHeight;
				}
				return m_ButtonLarge;
			}
		}

		public static GUIStyle ButtonMiddle
		{
			get
			{
				if (m_ButtonMiddle == null)
				{
					m_ButtonMiddle = new GUIStyle("Button");
					m_ButtonMiddle.fontSize = 8;
					m_ButtonMiddle.alignment = TextAnchor.MiddleCenter;
					m_ButtonMiddle.margin.top = m_MarginTop;
					m_ButtonMiddle.margin.left = m_MarginLeft;
					m_ButtonMiddle.margin.right = m_MarginRight;
					m_ButtonMiddle.padding = new RectOffset(0, 0, 0, 2);
					m_ButtonMiddle.fixedWidth = 50;
					m_ButtonMiddle.fixedHeight = m_FixedHeight;
				}
				return m_ButtonMiddle;
			}
		}

		public static GUIStyle ButtonSmall
		{
			get
			{
				if (m_ButtonSmall == null)
				{
					m_ButtonSmall = new GUIStyle("Button");
					m_ButtonSmall.fontSize = 8;
					m_ButtonSmall.alignment = TextAnchor.MiddleCenter;
					m_ButtonSmall.margin.top = m_MarginTop;
					m_ButtonSmall.margin.left = m_MarginLeft;
					m_ButtonSmall.margin.right = m_MarginRight;
					m_ButtonSmall.padding = new RectOffset(0, 0, 0, 2);
					m_ButtonSmall.fixedWidth = 25;
					m_ButtonSmall.fixedHeight = m_FixedHeight;
				}
				return m_ButtonSmall;
			}
		}

		public static GUIStyle FoldoutNormal
		{
			get
			{
				if (m_FoldoutNormal == null)
				{
					m_FoldoutNormal = new GUIStyle(EditorStyles.foldout);
					
					m_FoldoutNormal.fontStyle = FontStyle.Normal;
					//m_FoldoutNormal.normal.textColor = m_DefaultTextColor;
					//m_FoldoutNormal.onNormal.textColor = m_DefaultTextColor;
					//m_FoldoutNormal.hover.textColor = m_DefaultTextColor;
					//m_FoldoutNormal.onHover.textColor = m_DefaultTextColor;
					////m_FoldoutNormal.focused.textColor = m_DefaultTextColor;
					//m_FoldoutNormal.onFocused.textColor = m_DefaultTextColor;
					//m_FoldoutNormal.active.textColor = m_DefaultTextColor;
					//m_FoldoutNormal.onActive.textColor = m_DefaultTextColor;
				}
				return m_FoldoutNormal;
			}
		}

		public static GUIStyle Foldout
		{
			get
			{
				if (m_Foldout == null)
				{
					m_Foldout = new GUIStyle(EditorStyles.foldout);

					m_Foldout.fontStyle = FontStyle.Bold;
					//m_Foldout.normal.textColor = m_DefaultTextColor;
					//m_Foldout.onNormal.textColor = m_DefaultTextColor;
					//m_Foldout.hover.textColor = m_DefaultTextColor;
					//m_Foldout.onHover.textColor = m_DefaultTextColor;
					//m_Foldout.focused.textColor = m_DefaultTextColor;
					//m_Foldout.onFocused.textColor = m_DefaultTextColor;
					//m_Foldout.active.textColor = m_DefaultTextColor;
					//m_Foldout.onActive.textColor = m_DefaultTextColor;
				}
				return m_Foldout;
			}
		}
		
		public static GUIStyle LabelBold
		{
			get
			{
				if (m_LabelBold == null)
				{
					m_LabelBold = new GUIStyle(EditorStyles.label );
					m_LabelBold.fontStyle = FontStyle.Bold;
				}
				return m_LabelBold;
			}
		}

		public static GUIStyle ToggleBold
		{
			get
			{
				if (m_ToggleBold == null)
				{
					m_ToggleBold = new GUIStyle(EditorStyles.toggle );
					m_ToggleBold.fontStyle = FontStyle.Bold;
					
					m_ToggleBold.active.textColor = Color.cyan;
					m_ToggleBold.focused.textColor = Color.cyan;
					m_ToggleBold.fontSize = 12;// .textColor = Color.cyan;
					
				}

				return m_ToggleBold;
			}
		}

		public static void Splitter( float _thickness = 1 ){
			Splitter( _thickness, SplitterFull );
		}

		public static void SplitterByIndent( int _indent ) 
		{
			if( _indent < 0 || _indent > m_SplitterArray.Length )
				_indent = 0;

			if( m_SplitterArray[ _indent ] == null )
			{
				m_SplitterArray[ _indent ] = new GUIStyle();
				m_SplitterArray[ _indent ].normal.background = EditorGUIUtility.whiteTexture;
				m_SplitterArray[ _indent ].normal.background = EditorGUIUtility.whiteTexture;
				m_SplitterArray[ _indent ].stretchWidth = true;
				m_SplitterArray[ _indent ].margin = new RectOffset( _indent * 15 , 0, 7, 7);
			}

			Splitter( 1.0f, m_SplitterArray[ _indent ] ); 
		}

		public static void Splitter( float _thickness, GUIStyle _style ) 
		{
			Rect _rect = GUILayoutUtility.GetRect( GUIContent.none, _style, GUILayout.Height( _thickness ) );

			if (Event.current.type == EventType.Repaint) 
			{
				Color _color = GUI.color;
				GUI.color = m_DefaultLineColor;
				_style.Draw( _rect, false, false, false, false );
				GUI.color = _color;
			}
		}
	}
}
