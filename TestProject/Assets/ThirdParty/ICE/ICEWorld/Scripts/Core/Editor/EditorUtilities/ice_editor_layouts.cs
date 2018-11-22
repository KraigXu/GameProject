// ##############################################################################
//
// ice_editor_layouts.cs | ICEEditorLayout
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
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	public enum LabelIcon {
		Gray = 0,
		Blue,
		Teal,
		Green,
		Yellow,
		Orange,
		Red,
		Purple
	}


	/// <summary>
	/// Header type.
	/// </summary>
	public enum EditorHeaderType{
		TOGGLE,
		TOGGLE_LEFT,
		TOGGLE_LEFT_BOLD,
		TOGGLE_CUSTOM,
		FOLDOUT,
		FOLDOUT_BOLD,
		FOLDOUT_ENABLED,
		FOLDOUT_ENABLED_BOLD,
		FOLDOUT_CUSTOM,
		LABEL,
		LABEL_BOLD,
		LABEL_ENABLED,
		LABEL_ENABLED_BOLD,
		LABEL_CUSTOM,
		NONE
	}

	public class ICEEditorColors
	{
		static ICEEditorColors() {	
			SetDefaults();
		}

		public static void SetDefaults(){
			DefaultGUIColor = GUI.color;
			DefaultBackgroundColor = GUI.backgroundColor;

			//Debug.Log( HSBColor.FromColor( Color.green ) );
		}

		// yellow : H:0.1533865 S:0.9843137 B:1
		// blue : H:0.6666667 S:1 B:1
		// cyan : H:0.5 S:1 B:1
		// green : H:0.3333333 S:1 B:1

		public static Color DefaultBackgroundColor;
		public static Color DefaultGUIColor;

		protected static float m_default = 0.1f;
		protected static float m_selected = 0.5f;

		public static Color InfoColor = new HSBColor( 0.15f ,0.25f, 1 ).ToColor();

		public static Color SelectionOptionGroup1Color = new HSBColor( 0.45f , m_default, 1 ).ToColor();
		public static Color SelectionOptionGroup2Color = new HSBColor( 0.65f , m_default, 1 ).ToColor();
		public static Color SelectionOptionGroup3Color = new HSBColor( 0.85f , m_default, 1 ).ToColor();
		public static Color SelectionOptionGroup1SelectedColor = new HSBColor( 0.45f , m_selected, 1 ).ToColor();
		public static Color SelectionOptionGroup2SelectedColor = new HSBColor( 0.65f , m_selected, 1 ).ToColor();
		public static Color SelectionOptionGroup3SelectedColor = new HSBColor( 0.85f , m_selected, 1 ).ToColor();

		public static Color DefaultSystemButtonColor = new HSBColor( 0.6f , m_default , 1 ).ToColor();
		public static Color DefaultSystemButtonSelectedColor = new HSBColor( 0.6f , m_selected , 1 ).ToColor();
		public static Color SystemButtonColor = new HSBColor( 0.6f , m_default , 1 ).ToColor();
		public static Color SystemButtonSelectedColor = new HSBColor( 0.6f , m_selected , 1 ).ToColor();

		public static Color DefaultListButtonColor = new HSBColor( 0.8f , m_default , 1 ).ToColor();
		public static Color DefaultListButtonSelectedColor = new HSBColor( 0.8f , m_selected , 1 ).ToColor();
		public static Color ListButtonColor = new HSBColor( 0.8f , m_default , 1 ).ToColor();
		public static Color ListButtonSelectedColor = new HSBColor( 0.8f , m_selected , 1 ).ToColor();

		public static Color DefaultDebugButtonColor = new HSBColor( 0.2f , m_default , 1 ).ToColor();
		public static Color DefaultDebugButtonSelectedColor = new HSBColor( 0.2f , m_selected , 1 ).ToColor();
		public static Color DebugButtonColor = new HSBColor( 0.2f , m_default , 1 ).ToColor();
		public static Color DebugButtonSelectedColor = new HSBColor( 0.2f , m_selected , 1 ).ToColor();


		public static Color AutoButtonColor = new HSBColor( 0.3333333f , m_default , 1 ).ToColor(); 
		public static Color AutoButtonSelectedColor = new HSBColor( 0.3333333f , m_selected , 1 ).ToColor();  

		public static Color DefaultButtonColor = new HSBColor( 0.1533865f ,1f, 1 ).ToColor();  
		public static Color ButtonUpdateRequiredColor = new HSBColor( 0.1533865f ,1f, 1 ).ToColor();  

		public static Color EnabledButtonColor = new HSBColor( 0.1533865f ,1f, 1 ).ToColor(); 
		public static Color EnabledButtonSelectedColor = new HSBColor( 0.1533865f ,1f, 1 ).ToColor(); 

		public static Color CheckButtonColor = new HSBColor( 0.5f , m_default , 1 ).ToColor();
		public static Color CheckButtonSelectedColor = new HSBColor( 0.5f ,1f, 1 ).ToColor();

		public static Color FoldoutButtonColor = new HSBColor( 0.75f , 0.1f , 1 ).ToColor();
		public static Color FoldoutButtonSelectedColor = new HSBColor( 0.75f , 0.25f, 1 ).ToColor();

		public static readonly char openFoldout = '\u25A0';
		public static readonly char closedFoldout = '\u25A1';
		public static readonly char upArrow = '\u25B2';
		public static readonly char downArrow = '\u25BC';
		public static readonly char QuestionMark = '\uFE16';
		public static readonly char EXCLAMATION_MARK = '\uFE15';
		public static readonly char CROSS_MARK = '\u2764';


		public static void SetColors( bool _default )
		{
			if( _default )
			{
				SystemButtonColor = DefaultSystemButtonColor;
				SystemButtonSelectedColor = DefaultSystemButtonSelectedColor;

				ListButtonColor = DefaultListButtonColor;
				ListButtonSelectedColor = DefaultListButtonSelectedColor;

				DebugButtonColor = DefaultDebugButtonColor;
				DebugButtonSelectedColor = DefaultDebugButtonSelectedColor;
			}
			else
			{
				SystemButtonColor = DefaultBackgroundColor;
				SystemButtonSelectedColor = DefaultBackgroundColor;

				ListButtonColor = DefaultBackgroundColor;
				ListButtonSelectedColor = DefaultBackgroundColor;

				DebugButtonColor = DefaultBackgroundColor;
				DebugButtonSelectedColor = DefaultBackgroundColor;
			}
		}

		public static float Round( float _value, float _precision ){
			return ( Application.isPlaying ? _value : Mathf.Round( _value / _precision ) * _precision );
		}

		public static float RoundDisplay( float _value, float _precision ){
			return Mathf.Round( _value / _precision ) * _precision;
		}
	}

	public class ICEEditorBasicLayout : ICEEditorColors
	{
		/// <summary>
		/// Begins the horizontal.
		/// </summary>
		/// <returns>The horizontal.</returns>
		public static Rect BeginHorizontal(){
			return EditorGUILayout.BeginHorizontal();
		}

		/// <summary>
		/// Ends the horizontal.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_help">Help.</param>
		public static void EndHorizontal( ICEInfoDataObject _object, string _help = "" )
		{
			if( _object != null )
				EndHorizontal( ref _object.ShowInfoText , ref _object.InfoText, _help  );
			else
				EndHorizontal( _help  );
		}

		/// <summary>
		/// Ends the horizontal.
		/// </summary>
		/// <param name="_show_info">Show info.</param>
		/// <param name="_info">Info.</param>
		/// <param name="_help">Help.</param>
		public static void EndHorizontal( ref bool _show_info , ref string _info, string _help = "" )
		{
			_show_info = ICEEditorInfo.InfoButton( _show_info, _info  ); 
			EndHorizontal( _help );
			_info = ICEEditorInfo.InfoText( _show_info, _info );
		}

		/// <summary>
		/// Ends the horizontal.
		/// </summary>
		/// <param name="_help">Help.</param>
		public static void EndHorizontal( string _help = "" )
		{
			if( _help == "" )
				EditorGUILayout.EndHorizontal();
			else
			{
				ICEEditorInfo.HelpButton();
				EditorGUILayout.EndHorizontal();
				ICEEditorInfo.ShowHelp( _help );
			}
		}

		/// <summary>
		/// Basic Button specified by _title, _hint, _style and _options.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_style">Style.</param>
		/// <param name="_options">Options.</param>
		public static bool Button( string _title, string _hint, GUIStyle _style, params GUILayoutOption[] _options ){
			return GUILayout.Button( new GUIContent( _title, _hint ), _style, _options );
		}

		/// <summary>
		/// Basic coloured button specified by _title, _hint, _color, _style and _options.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_color">Color.</param>
		/// <param name="_style">Style.</param>
		/// <param name="_options">Options.</param>
		public static bool Button( string _title, string _hint, Color _color, GUIStyle _style, params GUILayoutOption[] _options ){

			GUI.backgroundColor = _color;
			bool _click = Button( _title, _hint, _style, _options );
			GUI.backgroundColor = DefaultBackgroundColor;

			return _click;
		}

		/// <summary>
		/// Basic coloured check button specified by _title, _hint, _default_color, _selected_color, _style and _options.
		/// </summary>
		/// <returns><c>true</c>, if button was checked, <c>false</c> otherwise.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_value">If set to <c>true</c> value.</param>
		/// <param name="_default">Default.</param>
		/// <param name="_selected">Selected.</param>
		/// <param name="_style">Style.</param>
		/// <param name="_options">Options.</param>
		public static bool CheckButton( string _title, string _hint, bool _value, Color _default, Color _selected, GUIStyle _style, params GUILayoutOption[] _options )
		{
			if( Button( _title, _hint, ( _value ? _selected : _default ), _style, _options ) )
				_value = ! _value;

			return _value;
		}

		/// <summary>
		/// Basic system button specified by _title, _hint, _style and _options.
		/// </summary>
		/// <returns><c>true</c>, if button was systemed, <c>false</c> otherwise.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_style">Style.</param>
		public static bool SystemButton( string _title, string _hint, GUIStyle _style, params GUILayoutOption[] _options ){
			return Button( _title, _hint, SystemButtonColor, _style, _options );
		}

		/// <summary>
		/// System check button specified by _title, _hint, _value, _style and _options.
		/// </summary>
		/// <returns><c>true</c>, if check button was systemed, <c>false</c> otherwise.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_value">If set to <c>true</c> value.</param>
		/// <param name="_style">Style.</param>
		/// <param name="_options">Options.</param>
		public static bool SystemCheckButton( string _title, string _hint, bool _value, GUIStyle _style, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, SystemButtonColor, SystemButtonSelectedColor, _style, _options );
		}

		public static bool ListButton( string _title, string _hint, GUIStyle _style, params GUILayoutOption[] _options ){
			return Button( _title, _hint, ListButtonColor, _style, _options );
		}

		public static bool ListCheckButton( string _title, string _hint, bool _value, GUIStyle _style, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ListButtonColor, ListButtonSelectedColor, _style, _options );
		}

		/// <summary>
		/// Basic debug button specified by _title, _hint, _style and _options.
		/// </summary>
		/// <returns><c>true</c>, if button was systemed, <c>false</c> otherwise.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_style">Style.</param>
		public static bool DebugButton( string _title, string _hint, GUIStyle _style, params GUILayoutOption[] _options ){
			return Button( _title, _hint, DebugButtonColor, _style, _options );
		}

		/// <summary>
		/// System debug button specified by _title, _hint, _value, _style and _options.
		/// </summary>
		/// <returns><c>true</c>, if check button was systemed, <c>false</c> otherwise.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_value">If set to <c>true</c> value.</param>
		/// <param name="_style">Style.</param>
		/// <param name="_options">Options.</param>
		public static bool DebugCheckButton( string _title, string _hint, bool _value, GUIStyle _style, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, DebugButtonColor, DebugButtonSelectedColor, _style, _options );
		}
	}

	/// <summary>
	/// ICE editor button layout.
	/// </summary>
	public class ICEEditorButtonLayout : ICEEditorBasicLayout
	{
		public static bool ButtonExtraLarge( string _title, string _hint = "" ){
			return Button( _title, _hint, ICEEditorStyle.ButtonExtraLarge );
		}

		public static bool ButtonLarge( string _title, string _hint = "" ){
			return Button( _title, _hint, ICEEditorStyle.ButtonLarge );
		}

		public static bool ButtonSmall( string _title, string _hint = "" ){
			return Button( _title, _hint, ICEEditorStyle.CMDButtonDouble );
		}

		public static bool ButtonMini( string _title, string _hint = "" ){
			return Button( _title, _hint, ICEEditorStyle.CMDButton );
		}

		public static bool Button( string _title, string _hint = "" ){
			return Button( _title, _hint, ICEEditorStyle.ButtonMiddle );
		}

		// DEBUG BUTTONS

		public static bool DebugButtonSmall( string _title, string _hint = "" ){
			return DebugButton( _title, _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool DebugButton( string _title, string _hint = "" ){
			return DebugButton( _title, _hint, ICEEditorStyle.ButtonMiddle );
		}

		public static bool DebugButtonMini( string _title, string _hint, bool _value ){
			return DebugCheckButton( _title, _hint, _value, ICEEditorStyle.CMDButton );
		}

		public static bool DebugButtonSmall( string _title, string _hint, bool _value ){
			return DebugCheckButton( _title, _hint, _value, ICEEditorStyle.CMDButtonDouble );
		}

		public static bool DebugButtonFlex( string _title, string _hint, bool _value ){
			return DebugCheckButton( _title, _hint, _value, ICEEditorStyle.ButtonFlex );
		}

		public static bool DebugButton( string _title, string _hint, bool _value ){
			return DebugCheckButton( _title, _hint, _value, ICEEditorStyle.ButtonMiddle );
		}

		// SYSTEM BUTTONS

		public static bool SystemButtonSmall( string _title, string _hint = "" ){
			return SystemButton( _title, _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool SystemButton( string _title, string _hint = "" ){
			return SystemButton( _title, _hint, ICEEditorStyle.ButtonMiddle );
		}

		public static bool SystemButtonSmall( string _title, string _hint, bool _value ){
			return SystemCheckButton( _title, _hint, _value, ICEEditorStyle.CMDButtonDouble );
		}

		public static bool SystemButton( string _title, string _hint, bool _value ){
			return SystemCheckButton( _title, _hint, _value, ICEEditorStyle.ButtonMiddle );
		}

		// SAVE

		public static bool SaveButton( string _hint , GUIStyle _style ){
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Stores the current settings to file." : _hint );
			return SystemButton( "SAVE", _hint, _style );
		}

		public static bool SaveButtonSmall( string _hint = "" ){
			return SaveButton( _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool SaveButton( string _hint = "" ){
			return SaveButton( _hint, ICEEditorStyle.ButtonMiddle );
		}

		// LOAD

		public static bool LoadButton( string _hint , GUIStyle _style){
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Loads stored settings from file and overrides the current settings." : _hint );
			return SystemButton( "LOAD", _hint, _style );
		}

		public static bool LoadButtonSmall( string _hint = "" ){
			return LoadButton( _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool LoadButton( string _hint = "" ){
			return LoadButton( _hint, ICEEditorStyle.ButtonMiddle );
		}

		// RESET

		public static bool ResetButton( string _hint , GUIStyle _style, string _title = "" ){
			_title = ( string.IsNullOrEmpty( _title ) ? "RESET" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Resets the current values." : _hint );

			string _warning = "\n\nPlease note, this function will reset parts of your settings. Are you sure you want to do that? " +
				"Press RESET to continue or CANCEL to abort.";

			if( SystemButton( _title, _hint, _style ) )
				return EditorUtility.DisplayDialog( "Reset Message", _hint + _warning, "RESET", "CANCEL" );
			else
				return false;
		}

		public static bool ResetButtonSmall( string _hint = "" ){
			return ResetButton( _hint , ICEEditorStyle.CMDButtonDouble, "RES" );
		}

		public static bool ResetButton( string _hint = "" ){
			return ResetButton( _hint, ICEEditorStyle.ButtonMiddle );
		}

		// ADD

		public static bool AddButton( string _title, string _hint , GUIStyle _style ){
			_title = ( string.IsNullOrEmpty( _title ) ? "ADD" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Creates a new Item" : _hint );
			return ListButton( _title, _hint, _style );
		}

		public static bool AddButtonMini( string _hint = "" ){
			return AddButton( "A", _hint , ICEEditorStyle.CMDButton );
		}

		public static bool AddButtonSmall( string _hint = "" ){
			return AddButton( "ADD", _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool AddButton( string _hint = "" ){
			return AddButton( "ADD", _hint , ICEEditorStyle.ButtonMiddle );
		}

		// NEW

		public static bool NewButton( string _title, string _hint , GUIStyle _style ){
			_title = ( string.IsNullOrEmpty( _title ) ? "NEW" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Creates a new Item" : _hint );
			return ListButton( _title, _hint, _style );
		}

		public static bool NewButtonSmall( string _hint = "" ){
			return NewButton( "NEW", _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool NewButton( string _hint = "" ){
			return NewButton( "NEW", _hint , ICEEditorStyle.ButtonMiddle );
		}

		// BACK

		public static bool BackButton( string _title, string _hint , GUIStyle _style ){
			_title = ( string.IsNullOrEmpty( _title ) ? "BACK" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Aborts the current actions." : _hint );
			return ListButton( _title, _hint, _style );
		}

		public static bool BackButtonSmall( string _hint = "" ){
			return BackButton( "BACK", _hint , ICEEditorStyle.CMDButtonDouble );
		}

		public static bool BackButton( string _hint = "" ){
			return BackButton( "BACK", _hint , ICEEditorStyle.ButtonMiddle );
		}

		// EDIT

		public static bool EditButton( string _title, string _hint, bool _value, GUIStyle _style ){
			_title = ( string.IsNullOrEmpty( _title ) ? "EDIT" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Aborts the current actions." : _hint );
			return ListCheckButton( _title, _hint, _value, _style );
		}

		public static bool EditButtonSmall( bool _value, string _hint = "" ){
			return EditButton( "EDIT", _hint , _value, ICEEditorStyle.CMDButtonDouble );
		}

		public static bool EditButton( bool _value, string _hint = "" ){
			return EditButton( "EDIT", _hint, _value, ICEEditorStyle.ButtonMiddle );
		}


		// DELETE

		public static bool DeleteButton( string _hint , GUIStyle _style, string _title = "" ){
			_title = ( string.IsNullOrEmpty( _title ) ? "DELETE" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Removes this item." : _hint );


			string _warning = "\n\nPlease note, this function will delete values. Are you sure you want to do that? " +
				"Press DELETE to continue or CANCEL to abort.";

			if( ListButton( _title, _hint, _style ) )
				return EditorUtility.DisplayDialog( "Delete Message", _hint + _warning, "DELETE", "CANCEL" );
			else
				return false;
		}

		public static bool DeleteButtonMini( string _hint = "" ){
			return DeleteButton( "Removes the selected item", ICEEditorStyle.CMDButton, "X" );
		}

		public static bool DeleteButtonSmall( string _hint = "" ){
			return DeleteButton( _hint , ICEEditorStyle.CMDButtonDouble, "DEL" );
		}

		public static bool DeleteButton( string _hint = "" ){
			return DeleteButton( _hint, ICEEditorStyle.ButtonMiddle );
		}



		// CLEAR

		public static bool ClearButton( string _hint , GUIStyle _style, string _title = "" ){
			_title = ( string.IsNullOrEmpty( _title ) ? "CLEAR" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Removes all items." : _hint );
			return ListButton( _title, _hint, _style );
		}

		public static bool ClearButtonSmall( string _hint = "" ){
			return ClearButton( _hint , ICEEditorStyle.CMDButtonDouble, "CLR" );
		}

		public static bool ClearButton( string _hint = "" ){
			return ClearButton( _hint, ICEEditorStyle.ButtonMiddle );
		}

		public static bool ClearButtonSmall<T>( ICEDataObject _object, List<T> _list, string _hint = "" ) where T : ICEObject{
			return ClearButton<T>( _object, _list, ICEEditorStyle.CMDButtonDouble, "CLR", _hint );
		}

		public static bool ClearButton<T>( ICEDataObject _object, List<T> _list, string _hint = "" ) where T : ICEObject{
			return ClearButton<T>( _object, _list, ICEEditorStyle.ButtonMiddle, "CLEAR", _hint );
		}

		public static bool ClearButton<T>( ICEDataObject _object, List<T> _list, GUIStyle _style, string _title, string _hint ) where T : ICEObject
		{
			if( _object == null || _list == null )
				return false;

			if( ClearButton( _hint, _style, _title ) )
			{
				_list.Clear();
				_object.Enabled = false;
				_object.Foldout = false;
				return true;
			}
			return false;
		}



		// AUTO BUTTONS

		public static bool AutoButton( string _hint, GUIStyle _style, string _title = "" )
		{
			_title = ( string.IsNullOrEmpty( _title ) ? "AUTO" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Generates automatically suitable values." : _hint );
			return Button( _title, _hint, AutoButtonColor, _style );
		}

		public static bool AutoButton( string _hint, bool _value, GUIStyle _style, string _title = "" )
		{
			_title = ( string.IsNullOrEmpty( _title ) ? "AUTO" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Generates automatically suitable values during the runtime." : _hint );
			return CheckButton( _title, _hint, _value, AutoButtonColor, AutoButtonSelectedColor, _style );
		}

		public static bool AutoButtonSmall( string _hint = "" ){
			return AutoButton( _hint , ICEEditorStyle.CMDButtonDouble, "A" );
		}

		public static bool AutoButton( string _hint = "" ){
			return AutoButton( _hint, ICEEditorStyle.ButtonMiddle );
		}

		public static bool AutoButtonSmall( string _hint, bool _value ){
			return AutoButton( _hint, _value, ICEEditorStyle.CMDButtonDouble, "A" );
		}

		public static bool AutoButton( string _hint, bool _value ){
			return AutoButton( _hint, _value, ICEEditorStyle.ButtonMiddle );
		}












		public static void PriorityButton( int _priority, string _hint = "" )
		{
			if( _hint == "" )
				_hint = "Priority";
			EditorGUI.BeginDisabledGroup( true );
				GUI.backgroundColor = new HSBColor( _priority * 0.0025f ,0.5f,1f ).ToColor();
				ICEEditorLayout.ButtonSmall( _priority.ToString(), _hint );
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();
		}

		public static void StatusButton( string _title, bool _status, Color _color ){
	
			EditorGUI.BeginDisabledGroup( true );
				GUI.backgroundColor = ( _status ? _color : ICEEditorLayout.DefaultBackgroundColor );
				Button( _title );
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();
		}







		public static bool CopyButtonSmall( string _hint = "" ){
			return CopyButton( _hint, ICEEditorStyle.CMDButtonDouble );
		}

		public static bool CopyButtonMiddle( string _hint = "" ){
			return CopyButton( _hint, ICEEditorStyle.ButtonMiddle );
		}
		public static bool CopyButton( string _hint, GUIStyle _style ){

			if( string.IsNullOrEmpty( _hint ) )
				_hint = "Creates a copy of the selected section.";

			GUI.backgroundColor = SystemButtonColor;			
			bool _click = Button( "COPY", _hint, _style );
			GUI.backgroundColor = DefaultBackgroundColor;

			return _click;
		}

		public static bool ButtonOpenMini(){
			return ButtonMini( openFoldout.ToString(), "Unfolds all foldouts in the list" );
		}

		public static bool ButtonClosedMini(){
			return ButtonMini( closedFoldout.ToString(), "Closed all foldouts in the list" );
		}

		public static bool ButtonOpen(){
			return ButtonSmall( openFoldout.ToString(), "Opens all foldouts in the list" );
		}

		public static bool ButtonClosed(){
			return ButtonSmall( closedFoldout.ToString(), "Closed all foldouts in the list" );
		}

		public static bool ButtonUpMini(){
			return ButtonMini( upArrow.ToString(), "Moves the selected item up one position in the list" );
		}

		public static bool ButtonDownMini(){
			return ButtonMini( downArrow.ToString(), "Moves the selected item down one position in the list" );
		}

		public static bool ButtonUp(){
			return ButtonSmall( upArrow.ToString(), "Moves the selected item up one position in the list" );
		}

		public static bool ButtonDown(){
			return ButtonSmall( downArrow.ToString(), "Moves the selected item down one position in the list" );
		}

		public static bool RandomButton( string _hint = "" ){

			_hint = ( string.IsNullOrEmpty( _hint ) ? "Selects the value by chance." : _hint );
			return ButtonSmall( "RND", _hint );
		}
			


		public static void ButtonSelectObject( GameObject _object, GUIStyle _style = null )
		{
			GUI.backgroundColor = InfoColor;

			if( _style == null )
				_style = ICEEditorStyle.ButtonMiddle;

			string _title = "SELECT";
			if( _style == ICEEditorStyle.CMDButtonDouble ) 
				_title = "SEL";

			EditorGUI.BeginDisabledGroup( _object == null );
			if( Button( _title, "Select object", _style ) )
			{
				Selection.activeGameObject = _object;
			}
			EditorGUI.EndDisabledGroup();

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ButtonDisplayObject( GameObject _object, string _title, Color _color, GUIStyle _style = null  )
		{
			GUI.backgroundColor = _color;

			if( _style == null )
				_style = ICEEditorStyle.ButtonMiddle;

			EditorGUI.BeginDisabledGroup( _object == null );
			if( Button( _title, "Show object", _style ) )
			{
				SceneView view = SceneView.currentDrawingSceneView;

				if(view == null)
					view = SceneView.lastActiveSceneView;

				if(view != null && _object != null )
				{
					Vector3 _pos = _object.transform.position; 

					_pos.y += 20;
					view.rotation        = new Quaternion(1,0,0,1);
					view.LookAt( _pos );

				}
				else
				{
					Debug.Log ( "Sorry, currentDrawingSceneView and lastActiveSceneView are not available!");
				}
			}
			EditorGUI.EndDisabledGroup();

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ButtonShowTransform( string _title, string _hint, Transform _transform, GUIStyle _style = null ){

			if( _style == null )
				_style = ICEEditorStyle.ButtonMiddle;

			EditorGUI.BeginDisabledGroup( _transform == null );
			if( Button( _title, _hint, _style ) )
			{
				SceneView view = SceneView.currentDrawingSceneView;

				if(view == null)
					view = SceneView.lastActiveSceneView;

				if(view != null && _transform != null )
				{
					Vector3 _pos = _transform.position; 
					//view.rotation = new Quaternion(0,0,0,1);
					view.LookAt( _pos );

				}
				else
				{
					Debug.Log ( "Sorry, currentDrawingSceneView and lastActiveSceneView are not available!");
				}
			}
			EditorGUI.EndDisabledGroup();
		}

		public static void ButtonDisplayObject( Vector3 _position, GUIStyle _style = null ){

			GUI.backgroundColor = InfoColor;

			if( _style == null )
				_style = ICEEditorStyle.CMDButtonDouble;

			EditorGUI.BeginDisabledGroup( _position == Vector3.zero );
			if( Button( "DIS", "Display object", _style ) )
			{
				SceneView view = SceneView.currentDrawingSceneView;

				if(view == null)
					view = SceneView.lastActiveSceneView;

				if(view != null)
				{
					Vector3 _pos = _position; 

					_pos.y += 20;
					view.rotation        = new Quaternion(1,0,0,1);
					view.LookAt( _pos );

				}
				else
				{
					Debug.Log ( "Sorry, currentDrawingSceneView and lastActiveSceneView are not available!");
				}
			}
			EditorGUI.EndDisabledGroup();

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static bool IgnoreButton( bool _value ){
			return CheckButtonMiddle( "IGNORE", "Ignores the associated feature", _value );
		}

		public static bool EnableButton( string _hint, bool _value ){
			return CheckButtonMiddle( "ENABLED", _hint, _value );
		}

		public static bool EnableButton( bool _value ){
			return CheckButtonMiddle( "ENABLED", "Enables/disables the associated feature", _value );
		}

		public static bool EnableButtonMini( bool _value ){
			return CheckButtonMini( "E", "Enables/disables the associated feature", _value );
		}

		public static bool FoldoutButton( string _title, string _hint, bool _value, Color _color, int _offset, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ( _color == DefaultBackgroundColor ? _color : new HSBColor( _color, m_selected ).ToColor() ), ( _color == DefaultBackgroundColor ? _color : new HSBColor( _color, m_default ).ToColor() ),  ICEEditorStyle.ButtonFlex, _options );
		}

		public static bool CheckButtonFlex( string _title, string _hint, bool _value, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ICEEditorStyle.ButtonFlex, _options );
		}

		public static bool CheckButtonMini( string _title, string _hint, bool _value, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ICEEditorStyle.CMDButton, _options );
		}

		public static bool CheckButtonSmall( string _title, string _hint, bool _value, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ICEEditorStyle.CMDButtonDouble, _options );
		}

		public static bool CheckButtonSmall( string _title, string _hint, bool _value, Color _default, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, _default, CheckButtonColor, ICEEditorStyle.CMDButtonDouble, _options );
		}

		public static bool CheckButtonSmall( string _title, string _hint, bool _value, Color _default, Color _selected, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, _default, _selected, ICEEditorStyle.CMDButtonDouble, _options );
		}

		public static bool CheckButtonMiddle( string _title, string _hint, bool _value, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ICEEditorStyle.ButtonMiddle, _options );
		}

		public static bool CheckButtonLarge( string _title, string _hint, bool _value, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ICEEditorStyle.ButtonLarge, _options );
		}

		public static bool CheckButtonExtraLarge( string _title, string _hint, bool _value, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, ICEEditorStyle.ButtonExtraLarge, _options );
		}

		public static bool CheckButton( string _title, string _hint, bool _value, GUIStyle _style, params GUILayoutOption[] _options ){
			return CheckButton( _title, _hint, _value, CheckButtonColor, CheckButtonSelectedColor,  _style, _options );
		}

		public static bool CheckButton( Rect _rect, string _title, string _hint, bool _value, GUIStyle _style, params GUILayoutOption[] _options )
		{
			GUI.backgroundColor = ( _value ? CheckButtonSelectedColor : CheckButtonColor );
			if( GUI.Button( _rect, new GUIContent( _title, _hint ), _style ) )
				_value = ! _value;

			GUI.backgroundColor = DefaultBackgroundColor;

			return _value;
		}


		/*
		public static bool CheckButton( Rect _rect, string _title, string _hint, bool _value, GUIStyle _style )
		{
			if( _value )
				GUI.backgroundColor = CheckButtonColor;
			else
				GUI.backgroundColor = DefaultBackgroundColor;

			if( GUI.Button( _rect, new GUIContent( _title, _hint ), _style ) )
				_value = ! _value;

			GUI.backgroundColor = DefaultBackgroundColor;

			return _value;
		}*/

		public static void ButtonMinMaxDefault( ref int _min, ref int _max, int _min_default, int _max_default )
		{
			//if( _min != _min_default || _max != _max_default  )
			//	GUI.backgroundColor = Color.yellow;

			Vector2 _value = ICEEditorLayout.ButtonDefault( new Vector2( _min, _max ), new Vector2( _min_default, _max_default ) );

			_min = (int)_value.x;
			_max = (int)_value.y;

			//GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ButtonMinMaxDefault( ref float _min, ref float _max, float _min_default, float _max_default )
		{
			//if( _min != _min_default || _max != _max_default  )
			//	GUI.backgroundColor = Color.yellow;

			Vector2 _value = ICEEditorLayout.ButtonDefault( new Vector2( _min, _max ), new Vector2( _min_default, _max_default ) );

			_min = _value.x;
			_max = _value.y;

			//GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}



		public static float ButtonDefault( float _value, float _default )
		{
			if( ButtonDefault( "Set default value (" + _default + ")" , Round( _value, Init.DECIMAL_PRECISION_MAX ) == Round( _default, Init.DECIMAL_PRECISION_MAX ) ) )
				_value = _default;

			return _value;
		}

		public static int ButtonDefault( int _value, int _default )
		{
			if( ButtonDefault( "Set default value (" + _default + ")" , _value == _default ) )
				_value = _default;

			return _value;
		}

		public static Vector3 ButtonDefault( Vector3 _value, Vector3 _default )
		{
			if( ButtonDefault( "Set default value (" + _default.x + "/" + _default.y + "/"  + _default.z + ")" , _value == _default) )
				_value = _default;

			return _value;
		}

		public static Vector2 ButtonDefault( Vector2 _value, Vector2 _default )
		{
			if( ButtonDefault( "Sets default value (" + _default.x + "/" + _default.y + ")", _value == _default) )
				_value = _default;

			return _value;
		}
	
		public static AnimationCurve ButtonDefault( AnimationCurve _value, AnimationCurve _default )
		{
			if( ButtonDefault( "Sets default curve", AnimationTools.CompareAnimationCurve( _value, _default ) ) )
				_value = _default;

			return _value;
		}

		public static Color ButtonDefault( Color _value, Color _default )
		{
			if( ButtonDefault( "Sets default color", _value == _default ) )
				_value = _default;

			return _value;
		}

		public static string ButtonDefault( string _value, string _default )
		{
			if( ButtonDefault( "Sets default string", _value == _default ) )
				_value = _default;

			return _value;
		}

		public static bool ButtonDefault( string _hint, bool _is_default ){ 
			return ButtonDefault( "D", _hint, _is_default ); 
		}

		public static bool ButtonDefault( string _title, string _hint, bool _is_default )
		{
			_title = ( string.IsNullOrEmpty( _title ) ? "D" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Sets the default value." : _hint );
			GUI.backgroundColor = ( _is_default ? DefaultBackgroundColor : DefaultButtonColor );
			bool _click = Button( _title, _hint, ICEEditorStyle.CMDButtonDouble );
			GUI.backgroundColor = DefaultBackgroundColor;

			return _click;
		}

		public static bool UpdateButton( bool _update_required, string _title = "", string _hint = "" )
		{
			_title = ( string.IsNullOrEmpty( _title ) ? "UPDATE" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Update required." : _hint );
			GUI.backgroundColor = ( _update_required ? ButtonUpdateRequiredColor : DefaultBackgroundColor );
			bool _click = Button( _title, _hint, ICEEditorStyle.ButtonMiddle );
			GUI.backgroundColor = DefaultBackgroundColor;

			return _click;
		}

		public static float ButtonOptionMini( string _title, float _value, float _default, string _hint = ""  )
		{
			_title = ( string.IsNullOrEmpty( _title ) ? "D" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Sets the default value." : _hint );
			GUI.backgroundColor = ( Round( _value, Init.DECIMAL_PRECISION_MAX ) == Round( _default, Init.DECIMAL_PRECISION_MAX ) ? DefaultButtonColor : DefaultBackgroundColor );
			if( Button( _title, _hint, ICEEditorStyle.CMDButton ) )
				_value = _default;
			GUI.backgroundColor = DefaultBackgroundColor;

			return _value;
		}

		public static float ButtonOption( string _title, float _value, float _default, string _hint = ""  )
		{
			_title = ( string.IsNullOrEmpty( _title ) ? "D" : _title );
			_hint = ( string.IsNullOrEmpty( _hint ) ? "Sets the default value." : _hint );
			GUI.backgroundColor = ( Round( _value, Init.DECIMAL_PRECISION_MAX ) == Round( _default, Init.DECIMAL_PRECISION_MAX ) ? DefaultButtonColor : DefaultBackgroundColor );
			if( Button( _title, _hint, ICEEditorStyle.CMDButtonDouble ) )
				_value = _default;
			GUI.backgroundColor = DefaultBackgroundColor;

			return _value;
		}

		public static bool ButtonSmall( string _title, string _hint, params GUILayoutOption[] _options ){
			return Button( _title, _hint, ICEEditorStyle.CMDButtonDouble, _options );
		}

		public static bool ButtonMiddle( string _title, string _hint, params GUILayoutOption[] _options ){
			return Button( _title, _hint, ICEEditorStyle.ButtonMiddle,  _options );
		}
			
		public static bool ButtonLarge( string _title, string _hint, params GUILayoutOption[] _options ){
			return Button( _title, _hint, ICEEditorStyle.ButtonLarge,  _options );
		}

		public static bool ButtonExtraLarge( string _title, string _hint, params GUILayoutOption[] _options ){
			return Button( _title, _hint, ICEEditorStyle.ButtonExtraLarge,  _options );
		}

		public static bool ButtonFlex( string _title, string _hint, params GUILayoutOption[] _options ){
			return Button( _title, _hint, ICEEditorStyle.ButtonFlex,  _options );
		}



		public static bool Button( Rect _rect, string _title, string _hint, GUIStyle _style  )
		{
			bool _value = false;

			if( GUI.Button( _rect, new GUIContent( _title, _hint ), _style ) )
				_value = ! _value;

			return _value;
		}

		public static bool ListClearButton<T>( List<T> _list )
		{
			if( ICEEditorLayout.ClearButton( "Removes all list items." ) )
			{
				_list.Clear();
				return true;
			}

			return false;
		}

		public static bool ListClearButtonSmall<T>( List<T> _list )
		{
			if( ICEEditorLayout.ClearButtonSmall( "Removes all list items." ) )
			{
				_list.Clear();
				return true;
			}

			return false;
		}

		public static bool ListDeleteButtonMini<T>( List<T> _list, T _item, string _hint = "" )
		{
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "Removes this list item.";

			if( ICEEditorLayout.DeleteButtonMini( _hint ) )
			{
				_list.Remove( _item );
				return true;
			}

			return false;
		}

		public static bool ListDeleteButton<T>( List<T> _list, T _item, string _hint = "" )
		{
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "Removes this list item.";

			if( ICEEditorLayout.DeleteButtonSmall( _hint ) )
			{
				_list.Remove( _item );
				return true;
			}

			return false;
		}


		public static int ListFoldoutButtonsMini<T>( List<T> _list ){
			return ListFoldoutButtons<T>( _list, ICEEditorStyle.CMDButton );
		}

		public static int ListFoldoutButtons<T>( List<T> _list ){
			return ListFoldoutButtons<T>( _list, ICEEditorStyle.CMDButtonDouble );
		}

		public static int ListFoldoutButtons<T>( List<T> _list, GUIStyle _style )
		{
			int _result = -1;
			EditorGUI.BeginDisabledGroup( _list.Count == 0 );

			if( ICEEditorLayout.ListButton( "_", "Folds all foldouts in the list" , _style ) )
			{
				foreach( T _item in _list )
				{
					ICEDataObject _obj = _item as ICEDataObject;
					if( _obj != null )_obj.Foldout = false;
				}

				_result = 0;
			}

			if( ICEEditorLayout.ListButton( closedFoldout.ToString(), "Unfolds all foldouts in the list" , _style ) )
			{
				foreach( T _item in _list )
				{
					ICEDataObject _obj = _item as ICEDataObject;
					if( _obj != null )_obj.Foldout = true;
				}

				_result = 1;
			}

			EditorGUI.EndDisabledGroup();

			return _result;
		}

		public static bool ListUpDownButtonsMini<T>( List<T> _list, int _i ){
			return ListUpDownButtons<T>( _list, _i, ICEEditorStyle.CMDButton );
		}

		public static bool ListUpDownButtons<T>( List<T> _list, int _i ){
			return ListUpDownButtons<T>( _list, _i, ICEEditorStyle.CMDButtonDouble );
		}

		public static bool ListUpDownButtons<T>( List<T> _list, int _i, GUIStyle _style )
		{
			EditorGUI.BeginDisabledGroup( _list.Count < 2 );
				EditorGUI.BeginDisabledGroup( _i <= 0 );
					if( ListButton( upArrow.ToString(), "Moves the selected item up one position in the list", _style ) )
					{
						T _tmp_obj = _list[_i]; 
						_list.RemoveAt( _i );

						if( _i - 1 < 0 )
							_list.Add( _tmp_obj );
						else
							_list.Insert( _i - 1, _tmp_obj );

						return true;
					}	
				EditorGUI.EndDisabledGroup();
				EditorGUI.BeginDisabledGroup( _i >= _list.Count - 1 );	
					if( ListButton( downArrow.ToString(), "Moves the selected item down one position in the list", _style ) )
					{
						T _tmp_obj = _list[_i]; 
						_list.RemoveAt( _i );

						if( _i + 1 > _list.Count )
							_list.Insert( 0, _tmp_obj );
						else
							_list.Insert( _i +1, _tmp_obj );

						return true;
					}	
				EditorGUI.EndDisabledGroup();
			EditorGUI.EndDisabledGroup();
			return false;
		}
	}

	/// <summary>
	/// ICEEditorLayout contains a collection of methods to standardize the layout of ICE components and to 
	/// simplify the GUI design. Here you will find methods for drawing standard controls 
	/// </summary>
	public class ICEEditorSliderLayout : ICEEditorButtonLayout
	{
		/// <summary>
		/// Clamp the specified _value, _min and _max.
		/// </summary>
		/// <param name="_value">Value.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static int Clamp( int _value, int _min, int _max  ){
			return (int)Mathf.Clamp( _value, Mathf.Min( _min, _max ), Mathf.Max( _min, _max ) );
		}

		/// <summary>
		/// Clamp the specified _value, _min and _max.
		/// </summary>
		/// <param name="_value">Value.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float Clamp( float _value, float _min, float _max  ){
			return Mathf.Clamp( _value, Mathf.Min( _min, _max ), Mathf.Max( _min, _max ) );
		}

		/// <summary>
		/// Pluses the minus group.
		/// </summary>
		/// <returns>The minus group.</returns>
		/// <param name="_value">Value.</param>
		public static int PlusMinusGroup( int _value ){
			return (int)PlusMinusGroup( _value, 1 );
		}

		/// <summary>
		/// Pluses the minus group.
		/// </summary>
		/// <returns>The minus group.</returns>
		/// <param name="_value">Value.</param>
		/// <param name="_precision">Precision.</param>
		public static float PlusMinusGroup( float _value, float _precision ){
			return PlusMinusGroup( _value, _precision, ICEEditorStyle.CMDButton );
		}

		/// <summary>
		/// Pluses the minus group.
		/// </summary>
		/// <returns>The minus group.</returns>
		/// <param name="_value">Value.</param>
		/// <param name="_precision">Precision.</param>
		/// <param name="_style">Style.</param>
		public static float PlusMinusGroup( float _value, float _precision, GUIStyle _style  )
		{
			if( Button( "<", "minus " + _precision, _style ) )
				_value -= _precision;

			if( Button( ">", "plus " + _precision, _style ) )
				_value += _precision; 

			return _value;
		}

		/// <summary>
		/// Basic Float Slider.
		/// </summary>
		/// <returns>The slider.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_value">Value.</param>
		/// <param name="_precision">Precision.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		/// <param name="_help">Help.</param>
		public static float BasicSlider( string _title, string _hint, float _value, float _precision, float _min = 0, float _max = 0, string _help = ""  )
		{
			_value = Round( _value, _precision );
			_value = Clamp( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _hint ),_value,_min, _max );			
			_value = PlusMinusGroup( _value, _precision  );
			_value = Clamp( _value, _min, _max  );
			_value = Round( _value, _precision );
			return _value;
		}

		/// <summary>
		/// Basics the default slider.
		/// </summary>
		/// <returns>The default slider.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_tooltip">Tooltip.</param>
		/// <param name="_value">Value.</param>
		/// <param name="_precision">Precision.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		/// <param name="_default">Default.</param>
		public static float BasicDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, float _default = 0 )
		{
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ButtonDefault( _value, _default );

			return _value;			
		}

		public static float BasicMaxSlider( string _title, string _tooltip, float _value, float _precision, float _min, ref float _max, string _help = ""  )
		{
			_value = Round( _value, _precision );
			_value = Clamp( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.FloatField( _max, GUILayout.Width( 40 ));
			EditorGUI.indentLevel = _indent;
			_value = PlusMinusGroup( _value, _precision  );
			_value = Clamp( _value, _min, _max  );
			_value = Round( _value, _precision );
			return _value;
		}

		public static float BasicRandomDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, ref bool _random, float _default, string _help = ""  )
		{
			EditorGUI.BeginDisabledGroup( _random == true );
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();

			_random = CheckButtonSmall( "RND", "Use Random Value", _random );

			return _value;
		}

		public static float BasicMaxDefaultSlider( string _title, string _tooltip, float _value, float _default, float _precision, float _min, ref float _max, string _help = ""  )
		{
			_value = Round( _value, _precision );
			_value = Clamp( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.FloatField( _max, GUILayout.Width( 40 ));
			EditorGUI.indentLevel = _indent;
			_value = PlusMinusGroup( _value, _precision  );
			_value = ButtonDefault( _value, _default );
			_value = Clamp( _value, _min, _max  );
			_value = Round( _value, _precision );
			return _value;
		}

		/// <summary>
		/// Slider specified by _title, _tooltip, _value, _precision, _min, _max and _help.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_tooltip">Tooltip.</param>
		/// <param name="_value">Value.</param>
		/// <param name="_precision">Precision.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		/// <param name="_help">Help.</param>
		public static float Slider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, string _help = ""  )
		{
			BeginHorizontal();
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			EndHorizontal( _help );
			return _value;
		}

		/// <summary>
		/// Int Default Slider.
		/// </summary>
		/// <returns>The slider.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_tooltip">Tooltip.</param>
		/// <param name="_value">Value.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		/// <param name="_default">Default.</param>
		/// <param name="_help">Help.</param>
		public static int DefaultSlider( string _title, string _tooltip, int _value, int _min = 0, int _max = 0, int _default = 0 , string _help = "" )
		{
			BeginHorizontal();
				_value = (int)BasicDefaultSlider( _title, _tooltip, _value, 1, _min, _max, _default );
			EndHorizontal( _help );
			return _value;
		}

		/// <summary>
		/// Float Default Slider.
		/// </summary>
		/// <returns>The slider.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_tooltip">Tooltip.</param>
		/// <param name="_value">Value.</param>
		/// <param name="_precision">Precision.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		/// <param name="_default">Default.</param>
		/// <param name="_help">Help.</param>
		public static float DefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, float _default = 0 , string _help = "" )
		{
			_default = Round( _default, _precision );

			BeginHorizontal();
				_value = BasicDefaultSlider( _title, _tooltip, _value, _precision, _min, _max, _default );
			EndHorizontal( _help );
			return _value;
		}

		public static int MaxDefaultSlider( string _title, string _tooltip, int _value, int _min, ref int _max, int _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			float _float_max = _max;
			_value = (int)BasicMaxDefaultSlider( _title, _tooltip, _value, _default, 1, _min, ref _float_max );
			_max = (int)_float_max;
			_value = ButtonDefault( _value, _default );
			EndHorizontal( _help );
			return _value;
		}

		public static float MaxDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min, ref float _max, float _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = BasicMaxDefaultSlider( _title, _tooltip, _value, _default, _precision, _min, ref _max );
			EndHorizontal( _help );
			return _value;
		}

		public static float MaxDefaultSliderDis( string _title, string _tooltip, float _value, float _precision, float _min, ref float _max, bool _disabled, float _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = Round( _value, _precision );
			_value = Clamp( _value, _min, _max  );

			EditorGUI.BeginDisabledGroup( _disabled == true );
				_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );
			EditorGUI.EndDisabledGroup();

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.FloatField( _max, GUILayout.Width( 40 ));
			EditorGUI.indentLevel = _indent;
			_value = PlusMinusGroup( _value, _precision  );
			_value = ButtonDefault( _value, _default );
			_value = Clamp( _value, _min, _max  );
			_value = Round( _value, _precision );
			EndHorizontal( _help );
			return _value;
		}

		public static int InitialMaxDefaultSlider( string _title, string _tooltip, int _value, int _min, ref int _max, ref int _initial, int _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = Clamp( _value, _min, _max  );

			_value = (int)EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.IntField( _max, GUILayout.Width( 40 ));
			_initial = EditorGUILayout.IntField( _initial, GUILayout.Width( 40 ));
			EditorGUI.indentLevel = _indent;
	
			_value = ButtonDefault( _value, _default );
			_value = Clamp( _value, _min, _max  );
			EndHorizontal( _help );
			return _value;
		}

		public static float InitialMaxDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min, ref float _max, ref float _initial, float _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = Round( _value, _precision );
			_value = Clamp( _value, _min, _max  );

			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.FloatField( _max, GUILayout.Width( 40 ));
			_initial = EditorGUILayout.FloatField( _initial, GUILayout.Width( 40 ));
			EditorGUI.indentLevel = _indent;
			_value = PlusMinusGroup( _value, _precision  );
			_value = ButtonDefault( _value, _default );
			_value = Clamp( _value, _min, _max  );
			_value = Round( _value, _precision );
			EndHorizontal( _help );
			return _value;
		}


		public static float RandomDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, ref bool _toggle, float _default, string _help = ""  )
		{
			BeginHorizontal();
				BasicRandomDefaultSlider( _title, _tooltip, _value, _precision, _min, _max, ref _toggle, _default );
			EndHorizontal( _help );
			return _value;
		}

		public static float AutoSlider( string _title, string _hint_slider, float _value, float _precision, float _min, float _max, ref bool _toggle, float _default, string _help = ""  ){
			return AutoSlider( _title, _hint_slider, "", _value, _precision, _min, _max, ref _toggle, _default, _help );
		}

		public static float AutoSlider( string _title, string _hint_slider, string _hint_button, float _value, float _precision, float _min, float _max, ref bool _toggle, float _default, string _help = ""  )
		{
			BeginHorizontal();			
			EditorGUI.BeginDisabledGroup( _toggle == true );
			_value = BasicSlider( _title, _hint_slider, _value, _precision, _min, _max );	
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();

			_hint_button = ( string.IsNullOrEmpty( _hint_button ) ? "Automatic" : _hint_button );
			_toggle = AutoButtonSmall( _hint_button , _toggle );
			EndHorizontal( _help );

			return _value;
		}


		public static float DefaultPlusSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, ref float _plus, float _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = BasicPlusSlider( _title, _tooltip, _value, _precision, _min, _max, ref _plus );
			_value = ButtonDefault( _value, _default );
			EndHorizontal( _help );
			return _value;

		}



		public static float BasicPlusSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, ref float _plus, string _help = ""  )
		{
			_value = Round( _value, _precision );
			_value = Clamp( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_plus = EditorGUILayout.FloatField( _plus, GUILayout.Width( 40 ));
			EditorGUI.indentLevel = _indent;

			_value = PlusMinusGroup( _value, _precision  );
			_value = Clamp( _value, _min, _max  );
			_value = Round( _value, _precision );
			return _value;
		}

	}

	public class ICEEditorMinMaxSliderLayout : ICEEditorSliderLayout
	{
		/// <summary>
		/// don't know it's a good idea but it improved the editor input of MinMaxSlider 
		/// typically, if the control is adjusted to zero you have to  increase first the 
		// right value and than the left one. now it's seems to be more comfortable ...</summary>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		/// <param name="_maximum">Maximum.</param>
		public static void DominantMin( float _min, ref float _max, ref float _maximum  )
		{ 
			_maximum = ( _min > _maximum ? _min : _maximum );
			_max = ( _min > _max ? _min : _max );
		}

		private static readonly int _plus_minus_limit = 30;

		public static void BasicMinMaxSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value, float _precision, float _size = 40 )
		{
			_min = Round( _min, _precision );
			_max = Round( _max, _precision );

			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			if( _size <= _plus_minus_limit )
				_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
				_min = EditorGUILayout.FloatField( _min , GUILayout.Width( _size ) );
				EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 	
				_max = EditorGUILayout.FloatField( _max , GUILayout.Width( _size ) );
			if( _size <= _plus_minus_limit )
				_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
			EditorGUI.indentLevel = _indent;

			DominantMin( _min, ref _max, ref _max_value );
			_min = Clamp( _min, _min_value, _max );
			_max = Clamp( _max, _min, _max_value );
			_min = Round( _min, _precision );
			_max = Round( _max, _precision );
		}

		public static void BasicMinMaxSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value, float _precision, float _size = 40 )
		{
			_min = Round( _min, _precision );
			_max = Round( _max, _precision );
					
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			if( _size <= _plus_minus_limit )
				_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
				_min = EditorGUILayout.FloatField( _min , GUILayout.Width( _size ) );
				EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 	
				_max = EditorGUILayout.FloatField( _max , GUILayout.Width( _size ) );
			if( _size <= _plus_minus_limit )
				_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
				_max_value = EditorGUILayout.FloatField( _max_value, GUILayout.Width( _size ) ); 
			EditorGUI.indentLevel = _indent;

			DominantMin( _min, ref _max, ref _max_value );
			_min = Clamp( _min, _min_value, _max );
			_max = Clamp( _max, _min, _max_value );
			_min = Round( _min, _precision );
			_max = Round( _max, _precision );
		}

		public static void MinMaxSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value, float _precision, float _size = 40, string _help = "" ){
			MinMaxSlider( _title, _tooltip, ref _min, ref _max, _min_value, ref _max_value, _precision, _size, _help );
		}

		public static void MinMaxSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value, float _precision, float _size = 40, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();	
				BasicMinMaxSlider( _title, _tooltip, ref _min, ref _max, _min_value, ref _max_value, _precision, _size );
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static void MinMaxDefaultSlider( string _title, string _tooltip, ref int _min, ref int _max, int _min_value, ref int _max_value, int _min_default, int _max_default, float _size = 40, string _help = "" )
		{
			float _min_float = _min;
			float _max_float = _max;
			float _max_value_float = _max_value;
			MinMaxDefaultSlider( _title, _tooltip, ref _min_float, ref _max_float, _min_value, ref _max_value_float, _min_default, _max_default, 1f, _size, _help );
			_min = (int)_min_float;
			_max = (int)_max_float;
			_max_value = (int)_max_value_float;
		}

		public static void MinMaxDefaultSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value, float _min_default, float _max_default, float _precision, float _size = 40, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();	
			BasicMinMaxSlider( _title, _tooltip, ref _min, ref _max, _min_value, _max_value, _precision, _size );
			ButtonMinMaxDefault( ref _min, ref _max, Round( _min_default, _precision ), Round( _max_default, _precision ) );
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static void MinMaxDefaultSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value, float _min_default, float _max_default, float _precision, float _size = 40, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();	
				BasicMinMaxSlider( _title, _tooltip, ref _min, ref _max, _min_value, ref _max_value, _precision, _size );
				ButtonMinMaxDefault( ref _min, ref _max, Round( _min_default, _precision ), Round( _max_default, _precision ) );
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static void MinMaxRandomDefaultSlider( string _title, string _tooltip, ref int _min, ref int _max, int _min_value, ref int _max_value, int _min_default, int _max_default, float _size = 40, string _help = "" )
		{
			float _min_float = _min;
			float _max_float = _max;
			float _max_value_float = _max_value;
			MinMaxRandomDefaultSlider( _title, _tooltip, ref _min_float, ref _max_float, _min_value, ref _max_value_float, _min_default, _max_default, 1, _size, _help );
			_min = (int)_min_float;
			_max = (int)_max_float;
			_max_value = (int)_max_value_float;
		}

		public static void MinMaxRandomDefaultSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value, float _min_default, float _max_default, float _precision, float _size = 40, string _help = "" ){
			MinMaxRandomDefaultSlider( _title, _tooltip, ref _min, ref _max, _min_value, ref _max_value, _min_default, _max_default, _precision, _size, _help );
		}

		public static void MinMaxRandomDefaultSlider( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value, float _min_default, float _max_default, float _precision, float _size = 40, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();	
			BasicMinMaxSlider( _title, _tooltip, ref _min, ref _max, _min_value, ref _max_value, _precision, _size );

			if( ICEEditorLayout.RandomButton( "" ) )
			{
				_min = UnityEngine.Random.Range( _min_value, _max_value );
				_max = UnityEngine.Random.Range( _min, _max_value );
			}

			ButtonMinMaxDefault( ref _min, ref _max, Round( _min_default, _precision ), Round( _max_default, _precision ) );
			ICEEditorLayout.EndHorizontal( _help );
		}
	}

	/// <summary>
	/// ICEEditorLayout contains a collection of methods to standardize the layout of ICE components and to 
	/// simplify the GUI design. Here you will find methods for drawing standard controls 
	/// </summary>
	public class ICEEditorLayout : ICEEditorMinMaxSliderLayout
	{

		public static bool DrawListAddLine( string _title, ref string _value, string _help = "" )
		{
			bool _click = false;
			_help = ( ! SystemTools.IsValid( _help ) ? _title : _help );
			ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );	
			ICEEditorLayout.BeginHorizontal();			
				EditorGUILayout.PrefixLabel( _title, "Button", EditorStyles.miniLabel );
				_value = EditorGUILayout.TextField( _value );
				_click = ICEEditorLayout.AddButton( _title );
			ICEEditorLayout.EndHorizontal( _help );	
			EditorGUILayout.Separator();

			return _click;
		}

		public static bool DrawListAddLine<T>( List<T> _list, T _object, string _title = "", string _help = "" ){
			return DrawListAddLine<T>( _list, _object, true, _title , _help );
		}

		public static bool DrawListAddLine<T>( List<T> _list, T _object, bool _splitter, string _title = "", string _help = "" )
		{
			bool _click = false;
			_help = ( ! SystemTools.IsValid( _help ) ? _title : _help );
			if( _splitter ) ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );					
			ICEEditorLayout.BeginHorizontal();
				EditorGUILayout.LabelField( _title , EditorStyles.miniLabel );				
				if( ICEEditorLayout.AddButton( _title ) )
				{
					_list.Add( _object );
					_click = true;
				}
			ICEEditorLayout.EndHorizontal( _help );

			return _click;
		}

		public static bool DrawListAddClearLine<T>( List<T> _list, T _object, bool _splitter, string _title = "", string _help = "" )
		{
			bool _click = false;
			_help = ( ! SystemTools.IsValid( _help ) ? _title : _help );
			if( _splitter ) ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );					
			ICEEditorLayout.BeginHorizontal();
				EditorGUILayout.LabelField( _title , EditorStyles.miniLabel );				
				if( ICEEditorLayout.AddButton( "Adds a new item." ) )
				{
					_list.Add( _object );
					_click = true;
				}

				if( ICEEditorLayout.ClearButton( "Removes all items." ) )
				{
					_list.Clear();
					_click = true;
				}
			ICEEditorLayout.EndHorizontal( _help );
			EditorGUILayout.Separator();

			return _click;
		}




		/*
		public static Object ObjectField(Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options);
		public static Object ObjectField(string label, Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options);
		public static Object ObjectField(GUIContent label, Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options); 



		public static System.Object ObjectField(GUIContent label, Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options )
		{
		}
		*/



		public static void MiniLabel( string _text )
		{
			if( string.IsNullOrEmpty( _text ) )
				return;
			
			ICEEditorLayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField( _text , EditorStyles.centeredGreyMiniLabel  );
				GUILayout.FlexibleSpace();
			ICEEditorLayout.EndHorizontal();
		}

		public static void MiniLabelLeft( string _text )
		{
			if( string.IsNullOrEmpty( _text ) )
				return;

			EditorGUILayout.LabelField( _text , EditorStyles.wordWrappedMiniLabel );
		}

		public static void DrawAddRigidbody( GameObject _object ){

			if( _object == null )
				return;

			ICEEditorLayout.BeginHorizontal();
			if( _object.GetComponent<Rigidbody>() == null )
			{
				ICEEditorLayout.Label( "Add Rigidbody" );
				if( ICEEditorLayout.AddButton( "Adds an non-kinematic Rigidbody" ) )
					_object.AddComponent<Rigidbody>();
			}
			else
			{
				EditorGUI.BeginDisabledGroup( true );
				ICEEditorLayout.Label( "Add Rigidbody" );
				ICEEditorLayout.AddButton( "Adds an non-kinematic Rigidbody" );
				EditorGUI.EndDisabledGroup();
			}
			ICEEditorLayout.EndHorizontal( Info.RIGIDBODY );

			//EditorGUILayout.HelpBox( Info.RIGIDBODY_INFO, MessageType.Info );
		}

		public static void DrawToggleRigidbody( GameObject _object ){

			if( _object == null )
				return;

			ICEEditorLayout.BeginHorizontal();
			if( _object.GetComponent<Rigidbody>() == null )
			{
				ICEEditorLayout.Label( "Add Rigidbody" );
				if( ICEEditorLayout.AddButton( "Adds an non-kinematic Rigidbody" ) )
					_object.AddComponent<Rigidbody>();
			}
			else
			{
				ICEEditorLayout.Label( "Remove Rigidbody" );
				if( ICEEditorLayout.DeleteButton( "Removes the Rigidbody" ) )
					GameObject.DestroyImmediate( _object.GetComponent<Rigidbody>() );
			}
			ICEEditorLayout.EndHorizontal( Info.RIGIDBODY );

			//EditorGUILayout.HelpBox( Info.RIGIDBODY_INFO, MessageType.Info );
		}

		private static ColliderType _collider_type;
		public static void DrawAddColliders( GameObject _object ){

			if( _object == null )
				return;

			Collider[] _colliders = _object.GetComponents<Collider>();

			string _title = "Add Collider";
			if( _colliders != null && _colliders.Length > 0 )
				_title = "Add additional Collider (" + _colliders.Length + ")";

			ICEEditorLayout.BeginHorizontal();
			_collider_type = (ColliderType)ICEEditorLayout.EnumPopup( _title, "", _collider_type ); 
			if( ICEEditorLayout.AddButton( "Adds the selected collider" ) )
			{
				if( _collider_type == ColliderType.Sphere )
					_object.AddComponent<SphereCollider>();
				else if( _collider_type == ColliderType.Box )
					_object.AddComponent<BoxCollider>();
				else if( _collider_type == ColliderType.Capsule )
					_object.AddComponent<CapsuleCollider>();
				else if( _collider_type == ColliderType.Mesh )
					_object.AddComponent<MeshCollider>();
			}
			ICEEditorLayout.EndHorizontal( Info.COLLIDER );

			//EditorGUILayout.HelpBox( Info.COLLIDER_INFO, MessageType.Info );
		}

		public static void DrawAddCollider( GameObject _object ){

			if( _object == null )
				return;

			ICEEditorLayout.BeginHorizontal();

			if( _object.GetComponent<Collider>() == null )
			{
				_collider_type = (ColliderType)ICEEditorLayout.EnumPopup( "Add Collider", "", _collider_type ); 
				if( ICEEditorLayout.AddButton( "Adds the selected collider" ) )
				{
					if( _collider_type == ColliderType.Sphere )
						_object.AddComponent<SphereCollider>();
					else if( _collider_type == ColliderType.Box )
						_object.AddComponent<BoxCollider>();
					else if( _collider_type == ColliderType.Capsule )
						_object.AddComponent<CapsuleCollider>();
					else if( _collider_type == ColliderType.Mesh )
						_object.AddComponent<MeshCollider>();
				}
			}
			else
			{
				EditorGUI.BeginDisabledGroup( true );
					_collider_type = (ColliderType)ICEEditorLayout.EnumPopup( "Add Collider", "", _collider_type ); 
					ICEEditorLayout.AddButton( "Adds the selected collider" );
				EditorGUI.EndDisabledGroup();
			}
			ICEEditorLayout.EndHorizontal( Info.COLLIDER );

			//EditorGUILayout.HelpBox( Info.COLLIDER_INFO, MessageType.Info );
		}

		public static void DrawAddTrigger( GameObject _object ){

			if( _object == null )
				return;

			ICEEditorLayout.BeginHorizontal();

			if( _object.GetComponent<Collider>() == null || _object.GetComponent<Collider>().isTrigger == false )
			{
				_collider_type = (ColliderType)ICEEditorLayout.EnumPopup( "Add Trigger", "", _collider_type ); 
				if( ICEEditorLayout.AddButton( "Adds the selected collider" ) )
				{
					Collider _collider = null;
					if( _collider_type == ColliderType.Sphere )
						_collider =_object.AddComponent<SphereCollider>();
					else if( _collider_type == ColliderType.Box )
						_collider =_object.AddComponent<BoxCollider>();
					else if( _collider_type == ColliderType.Capsule )
						_collider =_object.AddComponent<CapsuleCollider>();

					if( _collider != null )
						_collider.isTrigger = true;
				}
			}
			else
			{
				EditorGUI.BeginDisabledGroup( true );
				_collider_type = (ColliderType)ICEEditorLayout.EnumPopup( "Add Trigger", "", _collider_type ); 
				ICEEditorLayout.AddButton( "Adds the selected collider" );
				EditorGUI.EndDisabledGroup();
			}
			ICEEditorLayout.EndHorizontal( Info.COLLIDER );

			//EditorGUILayout.HelpBox( Info.COLLIDER_INFO, MessageType.Info );
		}

		public static Color ColorField( string _title, string _hint, Color _color, string _help = ""  )
		{
			ICEEditorLayout.BeginHorizontal();
				_color = EditorGUILayout.ColorField( new GUIContent( _title, _hint ), _color );
			ICEEditorLayout.EndHorizontal( _help );

			return _color;
		}

		public static AnimationCurve DefaultCurve( string _title, string _tooltip, AnimationCurve _curve, AnimationCurve _default_curve,string _help = "" )
		{
			BeginHorizontal();
			_curve = EditorGUILayout.CurveField( new GUIContent( _title, _tooltip), _curve );

			if( _default_curve == null || _default_curve.length == 0 )
			{
				Keyframe[] _keys = new Keyframe[2];
				_keys[0] = new Keyframe( 3, 0.6f);
				_keys[1] = new Keyframe( 6, 0.3f);

				_default_curve = new AnimationCurve(_keys);
			}

			_curve = ICEEditorLayout.ButtonDefault( _curve, _default_curve );				


			EndHorizontal( _help );
			return _curve;
		}







		public static void AssignLabel(GameObject g , int icon = 0 )
		{
			if( icon < 8 )
			{
				Texture2D tex = EditorGUIUtility.IconContent("sv_label_" + icon ).image as Texture2D;
				Type editorGUIUtilityType  = typeof(EditorGUIUtility);
				BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
				object[] args = new object[] {g, tex};
				editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
			}
		}

		public static void DrawLabelIconBar( string _title, string[] _paths, int _width, int _height, int _left, int _top, int _space )
		{
			BeginHorizontal();

			EditorGUILayout.PrefixLabel( _title );
		//	Label( _title, true);
			
			int _offset = 0;
			foreach( string _path in _paths )
			{
				DrawIcon( _path, _width, _height, _offset, _top );
				_offset += _width + _space;
			}
			
			GUILayout.FlexibleSpace();
			EndHorizontal();
			
			GUILayout.Space(_height + _top);
		}

		public static void DrawIconBar( string[] _paths, int _width, int _height, int _left, int _top, int _space )
		{
			BeginHorizontal();

			int _offset = _left;
			foreach( string _path in _paths )
			{
				DrawIcon( _path, _width, _height, _offset, _top );
				_offset += _width + _space;
			}

			GUILayout.FlexibleSpace();
			EndHorizontal();
			
			GUILayout.Space(_height + _top);
		}

		public static Color DefaultColor( string _title, string _hint, Color _color, Color _default, string _help = "" )
		{
			BeginHorizontal();
				_color = EditorGUILayout.ColorField( new GUIContent( _title, _hint ), _color );			
				_color = ButtonDefault( _color, _default );
			EndHorizontal( _help );
			return _color;
		}


		public static void DrawIcon( string _path, int _width, int _height, int _left, int _top )
		{
			Texture _icon = (Texture)Resources.Load( _path );

			if( _icon == null )
				return;
			
			Rect _rect = EditorGUILayout.BeginVertical();
			EditorGUI.DrawPreviewTexture( new Rect( _rect.x + _left, _rect.y + _top, _width, _height ) , _icon );
			GUILayout.Space(_height + _top);
			EditorGUILayout.EndVertical();

		}

		public static Transform TransformPopup( string _title, string _hint, Transform _selected, Transform _transform, bool _provide_empty = true, string _help = "" )
		{
			if( _selected == null )
				_selected = _transform;

			string _name = TransformPopup( _title, _hint, _selected.name, _transform, _provide_empty, _help );

			return SystemTools.FindChildByName( _name, _transform );
		}

		public static string TransformPopup( string _title, string _hint, string _name, Transform _transform, bool _provide_empty = true, string _help = "" )
		{
			BeginHorizontal();
				List<Transform> _list = new List<Transform>(); 
				SystemTools.GetChildren( _list, _transform );

				_list = _list.OrderBy( Transform => Transform.name ).ToList();

				int _list_count = _list.Count;
				if( _provide_empty )
					_list_count += 1;

				GUIContent[] _options = new GUIContent[ _list_count ];
				int _selected = 0;
				int _index = 0;
				if( _provide_empty )
				{
					_options[0] = new GUIContent( " " );
					_index = 1;
				}

				for( int _i = 0 ; _i < _list.Count ; _i++ )
				{
					_options[ _index ] = new GUIContent( _list[_i].name );

					if( _list[_i].name == _name )
						_selected = _index;

					_index += 1;
				}

				_selected = EditorGUILayout.Popup( new GUIContent( _title, _hint ) , _selected, _options );

				_name = _options[_selected].text;

				Transform _child = SystemTools.FindChildByName( _name, _transform );

				//ButtonShowTransform( "DTL", "Show details", _child, ICEEditorStyle.CMDButtonDouble );
				ButtonSelectObject( (_child != null)?_child.gameObject:null, ICEEditorStyle.CMDButtonDouble  );
			EndHorizontal( _help );
			return _name;
		}



		/// <summary>
		/// Draws the water check.
		/// </summary>
		/// <param name="_type">Type.</param>
		/// <param name="_layers">Layers.</param>
		/// <param name="_help">Help.</param>
		public static void DrawWaterCheck( ref WaterCheckType _type, List<string> _layers, bool _show_default = true, string _help = "" )
		{
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.WATER_CHECK;

			ICEEditorLayout.BeginHorizontal();

				_type = (WaterCheckType)ICEEditorLayout.EnumPopup("Water Handling", "Method to handle water related checks and movements", _type );
				if( _type == WaterCheckType.CUSTOM )
				{
					if( ICEEditorLayout.Button( "Add Layer" ) )
						_layers.Add( (LayerMask.NameToLayer("Water") != -1?"Water":"Default") );
				}				

				if( _show_default )
				{
					EditorGUI.BeginDisabledGroup( WorldManager.GroundLayer == null );
					if( ICEEditorLayout.ButtonSmall( "D" ) )
					{
						_type = WorldManager.WaterCheck;
						if( WorldManager.WaterLayer != null )
						{
							_layers.Clear();
							foreach( string _layer in WorldManager.WaterLayer.Layers )
								_layers.Add( _layer );
						}
					}
					EditorGUI.EndDisabledGroup();
				}

			ICEEditorLayout.EndHorizontal( _help );

			if( _type == WaterCheckType.CUSTOM )
			{
				EditorGUI.indentLevel++;
					DrawLayersList( _layers );
				EditorGUI.indentLevel--;
			}
		}

		public static void DrawCoverCheck( List<string> _layers, bool _show_default = true, string _help = "" )
		{
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.WATER_CHECK;

			ICEEditorLayout.BeginHorizontal();

				ICEEditorLayout.Label( "Cover Layer" );

				if( ICEEditorLayout.Button( "Add Layer" ) )
					_layers.Add( (LayerMask.NameToLayer("Water") != -1?"Water":"Default") );		

				if( _show_default )
				{
					EditorGUI.BeginDisabledGroup( WorldManager.GroundLayer == null );
					if( ICEEditorLayout.ButtonSmall( "D" ) )
					{
						if( WorldManager.ObstacleLayer != null )
						{
							_layers.Clear();
							foreach( string _layer in WorldManager.ObstacleLayer.Layers )
								_layers.Add( _layer );
						}
					}
					EditorGUI.EndDisabledGroup();
				}

			ICEEditorLayout.EndHorizontal( _help );

			EditorGUI.indentLevel++;
				DrawLayersList( _layers );
			EditorGUI.indentLevel--;
		}

		/// <summary>
		/// Draws the obstacle check.
		/// </summary>
		/// <param name="_type">Type.</param>
		/// <param name="_layers">Layers.</param>
		/// <param name="_help">Help.</param>
		public static void DrawObstacleCheck( ref ObstacleCheckType _type, List<string> _layers, bool _show_default = true, string _help = ""  )
		{
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.OBSTACLE_CHECK;

			string _layer_name = "Obstacle";
			
			ICEEditorLayout.BeginHorizontal();
				_type = (ObstacleCheckType)ICEEditorLayout.EnumPopup("Obstacle Avoidance", "Method to handle obstacle checks and avoidances", _type );
				if( _type != ObstacleCheckType.NONE )
				{
					if( ICEEditorLayout.AddButton( "Add Layer" ) )
						_layers.Add( (LayerMask.NameToLayer( _layer_name ) != -1?_layer_name:"Default") );

				}				

				if( _layers.Count == 0 )
				{
					if( _show_default )
					{
						if( ICEEditorLayout.AutoButton( "Assigns the default obstacle layers." ) )
						{
							_type = WorldManager.ObstacleCheck;
							if( WorldManager.ObstacleLayer != null && WorldManager.ObstacleLayer.Layers.Count > 0 )
							{
								_layers.Clear();
								foreach( string _layer in WorldManager.ObstacleLayer.Layers )
								{
									if( LayerMask.NameToLayer( _layer_name ) == -1 )
										EditorTools.AddLayer( _layer_name );
									
									_layers.Add( _layer );
								}
							}
							else
							{
								_type = ObstacleCheckType.BASIC;
								EditorTools.AddLayer( _layer_name );
								_layers.Add( _layer_name );									
							}
						}
					}
					else
					{
						if( ICEEditorLayout.AutoButton( "Assigns the default obstacle layers." ) )
						{
							_type = ObstacleCheckType.BASIC;
							EditorTools.AddLayer( _layer_name );
							_layers.Add( _layer_name );		
						}
					}
				}
				else
				{
					if( ICEEditorLayout.Button( "Reset" ) )
						_layers.Clear();
				}

			ICEEditorLayout.EndHorizontal( _help );

			if( _type != ObstacleCheckType.NONE )
			{
				EditorGUI.indentLevel++;
				DrawLayersList( _layers );
				EditorGUI.indentLevel--;
			}
		}

		/// <summary>
		/// Draws the ground check.
		/// </summary>
		/// <param name="_type">Type.</param>
		/// <param name="_layers">Layers.</param>
		/// <param name="_help">Help.</param>
		public static void DrawGroundCheck( ref GroundCheckType _type, List<string> _layers, bool _show_default = true, string _help = "" )
		{
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.GROUND_CHECK;

			string _layer_name = "WalkableSurface";
				
			ICEEditorLayout.BeginHorizontal();
				_type = (GroundCheckType)ICEEditorLayout.EnumPopup("Ground Handling", "Method to handle ground related checks and movements", _type );
				if( _type == GroundCheckType.RAYCAST )
				{
					if( ICEEditorLayout.AddButton( "Add Layer" ) )
						_layers.Add( ( LayerMask.NameToLayer( _layer_name ) != -1?_layer_name:"Default" ) );

				}			

				if( _layers.Count == 0 )
				{
					if( _show_default )
					{
						if( ICEEditorLayout.AutoButton( "Assigns the default ground layers." ) )
						{
							_type = WorldManager.GroundCheck;
							if( WorldManager.GroundLayer != null && WorldManager.GroundLayer.Layers.Count > 0 )
							{
								_layers.Clear();
								foreach( string _layer in WorldManager.GroundLayer.Layers )
								{
									if( LayerMask.NameToLayer( _layer_name ) == -1 )
										EditorTools.AddLayer( _layer_name );

									_layers.Add( _layer );
								}
							}
							else
							{
								_type = GroundCheckType.RAYCAST;
								EditorTools.AddLayer( _layer_name );
								_layers.Add( _layer_name );									
							}
						}
					}
					else
					{
						if( ICEEditorLayout.AutoButton( "Assigns the default ground layers." ) )
						{
							_type = GroundCheckType.RAYCAST;
							EditorTools.AddLayer( _layer_name );
							_layers.Add( _layer_name );		
						}
					}
				}
				else
				{
					if( ICEEditorLayout.Button( "Reset" ) )
						_layers.Clear();
				}
				
			
			ICEEditorLayout.EndHorizontal( _help );

			if( _type == GroundCheckType.RAYCAST )
			{
				EditorGUI.indentLevel++;
					DrawLayersList( _layers );
				EditorGUI.indentLevel--;
			}
		}

		/// <summary>
		/// Draws the layers list.
		/// </summary>
		/// <param name="_layers">Layers.</param>
		public static void DrawLayersList( List<string> _layers )
		{
			if( _layers.Count == 0 )
			{
				ICEEditorLayout.BeginHorizontal();
				GUILayout.FlexibleSpace();					
				EditorGUILayout.LabelField( new GUIContent( " - No Layer defined -", "" ) );
				GUILayout.FlexibleSpace();
				ICEEditorLayout.EndHorizontal();
			}
			else
			{
				for( int i = 0 ; i < _layers.Count; i++ )
				{
					ICEEditorLayout.BeginHorizontal();
					GUI.backgroundColor = new Vector4( 0.7f, 0.9f, 0.9f, 0.5f);

					int _layer = LayerMask.NameToLayer(_layers[i]);

					string _title = "Layer #" + _layer;

					if( _layer == -1 )
					{
						GUI.backgroundColor = Color.red;
						EditorGUILayout.PrefixLabel( new GUIContent( _title + " (MISSING)", "CREATE MISSING '" + _layers[i] + "' LAYER" ) );
						if( ICEEditorLayout.ButtonFlex( "CREATE MISSING '" + _layers[i] + "' LAYER",  "" ) )
						{
							EditorTools.AddLayer( _layers[i] );
						}
					}
					else
					{
						_layers[i] = LayerMask.LayerToName( EditorGUILayout.LayerField( _title, _layer ) );
					}

					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

					if( ICEEditorLayout.ListDeleteButtonMini<string>( _layers, _layers[i], "Removes this layer." ) )
						return;
					
					ICEEditorLayout.EndHorizontal();
				}
			}
		}

		private static string _temp_string = "";
		public static void DrawStringList( string _title,string _hint, List<string> _strings )
		{
			if( _strings == null )
				return;
			
			for( int i = 0 ; i < _strings.Count; i++ )
			{
				ICEEditorLayout.BeginHorizontal();
				_strings[i] = ICEEditorLayout.Text( _title + " #" + (i+1) , _hint, _strings[i] );

				if( ICEEditorLayout.ListUpDownButtons<string>( _strings, i ) )
					return;
				
				if( ICEEditorLayout.ListDeleteButton<string>( _strings, _strings[i], "Delete" ) )
					return;

				ICEEditorLayout.EndHorizontal();
			}

			ICEEditorLayout.BeginHorizontal();
				_temp_string = ICEEditorLayout.Text( "Add new " + _title , "", _temp_string );
				EditorGUI.BeginDisabledGroup( _temp_string.Trim() == "" );
				if( ICEEditorLayout.AddButton( "Add new " + _title ) )
				{
					_strings.Add( _temp_string );
					_temp_string = "";
				}
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal();

		}

		public static float DrawBaseOffset( Transform _transform, string _title, string _hint, float _offset, ref float _maximum, string _help = "" )
		{
			if( string.IsNullOrEmpty( _title ) )
				_title = "Base Offset";

			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";

			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BASE_OFFSET ;

			return ICEEditorLayout.MaxDefaultSlider( _title, _hint, _offset, 0.01f, -_maximum, ref _maximum, 0, _help );
		}

		/// <summary>
		/// Draws the base offset.
		/// </summary>
		/// <returns>The base offset.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_offset">Offset.</param>
		/// <param name="_maximum">Maximum.</param>
		/// <param name="_grounded">Grounded.</param>
		/// <param name="_help">Help.</param>
		public static float DrawBaseOffsetGround( Transform _transform, string _title, string _hint, float _offset, ref float _maximum, ref bool _grounded, string _help = "" )
		{
			if( string.IsNullOrEmpty( _title ) )
				_title = "Base Offset";

			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";

			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BASE_OFFSET ;

			if(  _transform.parent != null)
				_grounded = false;

			ICEEditorLayout.BeginHorizontal();
			_offset = ICEEditorLayout.BasicMaxDefaultSlider( _title, _hint, _offset, 0, 0.01f, -_maximum, ref _maximum, "" );
			EditorGUI.BeginDisabledGroup( _transform.parent != null );
				_grounded = ICEEditorLayout.CheckButtonSmall( "GND", "Grounded In Editor Mode" , _grounded );
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( _help );

			return _offset;
		}


		public static string CameraPopup( string _title, string _hint, string _camera )
		{
			GUIContent[] _options = new GUIContent[ Camera.allCameras.Length + 1 ];
			int _selected = 0;

			_options[0] = new GUIContent( " " );
			for( int i = 0 ; i < Camera.allCameras.Length ; i++ )
			{
				Camera _cam = Camera.allCameras[i];
				
				int _index = i + 1;
				
				_options[ _index ] = new GUIContent( _cam.name );
				
				if( _cam.name == _camera )
					_selected = _index;
			}

		
			_selected = EditorGUILayout.Popup( new GUIContent( _title, _hint ) , _selected, _options );

			return _options[_selected].text;
		}

		public static Vector3 OffsetGroup( string _title, string _hint, Vector3 _position, Vector3 _offset, GameObject _owner, float _scale, float _range )
		{
			GameObject _target = new GameObject();
			_target.transform.position = _position;

			
			BeginHorizontal();
			_offset = EditorGUILayout.Vector3Field( new GUIContent( _title, _hint ) , _offset );
			
			if( ButtonSmall( "GET", "" ) )
			{
				Vector3 _local_offset = _target.transform.InverseTransformPoint( _owner.transform.position );
				
				_local_offset.x = _local_offset.x*_target.transform.lossyScale.x;
				_local_offset.y = 0;//_local_offset.y*target.transform.lossyScale.y;
				_local_offset.z = _local_offset.z*_target.transform.lossyScale.z;					/**/
				
				_offset = _local_offset;
				//offset = new Vector3(Mathf.Round(offset.x/scale)*scale, Mathf.Round(offset.y/scale)*scale, Mathf.Round(offset.z/scale)*scale) ;
			}
			
			if( ButtonSmall( "SET", "Relocate your creature to the current offset position" ) )
			{
				/*
				owner.transform.position = target.transform.position 
					+ ( target.transform.forward * offset.z ) 
						+ ( target.transform.right * offset.x )
							+ ( target.transform.up * offset.y );*/
				
				Vector3 _local_offset = _offset;
				
				_local_offset.x = _local_offset.x/_target.transform.lossyScale.x;
				_local_offset.y = _local_offset.y/_target.transform.lossyScale.y;
				_local_offset.z = _local_offset.z/_target.transform.lossyScale.z;
				
				Vector3 _pos = _target.transform.TransformPoint( _local_offset );
				
				_pos.y = Terrain.activeTerrain.SampleHeight( _pos );
				
				_owner.transform.position = _pos;
			}
			
			if( ButtonSmall( "RESET", "Reset offset values" ) )
				_offset = Vector3.zero;
	
			EndHorizontal();

			_target = null;
			
			return _offset;
		}

		/*
		public static Vector4 OffsetGroup( string _title, Vector4 _complex_offset, GameObject _owner, GameObject _target, float _scale = 0.5f, float _range = 25, string _help = ""  )
		{

			BeginHorizontal();

				Vector3 _offset = new Vector3( _complex_offset.x, _complex_offset.y, _complex_offset.z );
				_offset = EditorGUILayout.Vector3Field( _title, _offset );
				Vector4 _new_complex_offset = new Vector4( _offset.x, _offset.y, _offset.z, _complex_offset.w ); 

				int _indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
					EditorGUILayout.LabelField( "", "A", GUILayout.MaxWidth( 15 )  );
					_new_complex_offset.w = EditorGUILayout.FloatField( _complex_offset.w, GUILayout.MaxWidth( 40 ) );
				EditorGUI.indentLevel = _indent;

				if( _target != null )
				{
					if( ButtonSmall( "GET", "Gets the offset by using the current positions" ) ){
						//_offset = PositionTools.FixInverseTransformPoint( _target.transform, _owner.transform.position );
					}

					if( ButtonSmall( "SET", "Relocates your creature to the specified offset position" ) ){
						//_owner.transform.position = PositionTools.FixTransformPoint( _target.transform, _offset );
					}
				}

				if( ButtonSmall( "RESET", "Resets the offset" ) ){
					_new_complex_offset = Vector4.zero;
				}
			
			EndHorizontal( _help );


			return _new_complex_offset;
		}*/

		public static Vector3 OffsetGroup( string _title, Vector3 _offset, GameObject _owner, GameObject _target, float _scale = 0.5f, float _range = 25, string _help = ""  )
		{

			BeginHorizontal();

			_offset = EditorGUILayout.Vector3Field( _title, _offset );

			EditorGUI.BeginDisabledGroup( _target == null );
			if( ButtonSmall( "GET", "Gets the offset by using the current positions" ) && _target != null ){
				_offset = PositionTools.FixInverseTransformPoint( _target.transform, _owner.transform.position );
			}

			if( ButtonSmall( "SET", "Relocates your creature to the specified offset position" ) && _target != null ){
				_owner.transform.position = PositionTools.FixTransformPoint( _target.transform, _offset );
			}
			EditorGUI.EndDisabledGroup();

			if( ButtonSmall( "RESET", "Resets the offset" ) ){
				_offset = Vector3.zero;
			}

			EndHorizontal( _help );

			return _offset;
		}

		public static Vector3 OffsetGroup( string _title, Vector3 _offset, string _help = ""  )
		{
			BeginHorizontal();
				_offset = EditorGUILayout.Vector3Field( _title, _offset );				
				if( ButtonSmall( "RESET", "Reset offset values" ))
					_offset = Vector3.zero;
			EndHorizontal( _help );			
			
			return _offset;
		}

		public static Quaternion RotationGroup( string _title, Quaternion _rotation, string _help = ""  )
		{
			Vector4 _vector = Converter.QuaternionToVector4( _rotation );

			BeginHorizontal();
				_vector = EditorGUILayout.Vector4Field( _title, _vector );				
			if( ButtonSmall( "RESET", "Reset rotation values" ))
					_vector = Vector4.zero;
			EndHorizontal( _help );			

			return Converter.Vector4ToQuaternion( _vector );
		}

		public static Vector3 EulerGroup( string _title, Vector3 _rotation, string _help = ""  )
		{
			BeginHorizontal();
			_rotation = EditorGUILayout.Vector3Field( _title, _rotation );				
			if( ButtonSmall( "RESET", "Reset rotation values" ))
				_rotation = Vector3.zero;
			EndHorizontal( _help );			

			return _rotation;
		}


			
		/// <summary>
		/// Simple String Options Popup specified by _selected and _options.
		/// </summary>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		public static int Popup( int _selected, string[] _options ){
			return Popup( "", "", _selected, _options );
		}

		/// <summary>
		/// String Options Popup specified by _title, _hint, _selected, and _options.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		/// <param name="_help">Help.</param>
		public static int Popup( string _title, string _hint, int _selected, string[] _options ){
			return Popup( _title, _hint, _selected, _options, "" );
		}

		/// <summary>
		/// String Options Popup specified by _title, _hint, _selected, _options and _help.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		/// <param name="_help">Help.</param>
		public static int Popup( string _title, string _hint, int _selected, string[] _options, string _help )
		{
			GUIContent[] _content_options = new GUIContent[_options.Length];
			for(int i = 0 ; i < _options.Length ; i++ )
			{
				_content_options[i] = new GUIContent();
				_content_options[i].text = _options[i];

			}

			if( string.IsNullOrEmpty( _help ) )
				return Popup( _title, _hint, _selected, _content_options ); 
			else
				return Popup( _title, _hint, _selected, _content_options, _help ); 
		}

		/// <summary>
		/// Basic Popup specified by _title, _hint, _selected, _options and _help.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		/// <param name="_help">Help.</param>
		public static int Popup( string _title, string _hint, int _selected, GUIContent[] _options, string _help )
		{
			BeginHorizontal();
			int _value = Popup( _title, _hint, _selected, _options ); 
			EndHorizontal( _help );
			return _value;
		}

		/// <summary>
		/// Basic Popup specified by _title, _hint, _selected and _options.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		public static int Popup( string _title, string _hint, int _selected, GUIContent[] _options )
		{
			if( string.IsNullOrEmpty( _title ) && string.IsNullOrEmpty( _hint ) )
				return EditorGUILayout.Popup( _selected, _options ); 
			else
				return EditorGUILayout.Popup( new GUIContent( _title, _hint), _selected, _options ); 
		}

		public static int DayPopup( int _selected, int _year, int _month, params GUILayoutOption[] _options ){
			return DayPopup( "", "", _selected, _year, _month, _options );
		}

		public static int DayPopup( string _title, string _hint, int _selected, int _year, int _month, params GUILayoutOption[] _options ){

			int _m = DateTime.DaysInMonth( _year, _month );

			string[] _labels = new string[_m]; 
			int[] _values = new int[_m]; 
			string _format = "00";

			for( int i = 0 ; i < _m ; i++ )
			{
				int _day = i + 1;
				_labels[i] = _day.ToString( _format );
				_values[i] = _day;
			}

			return IntPopup( _title, _hint, _selected, _labels, _values, _options ); 
		}

		public static int IntPopup( int _selected, string[] _labels, int[] _values, params GUILayoutOption[] _options ){
			return IntPopup( "", "", _selected, _labels, _values, _options ); 
		}

		public static int IntPopup( string _title, string _hint, int _selected, string[] _labels, int[] _values, params GUILayoutOption[] _options ){
			if( string.IsNullOrEmpty( _title ) && string.IsNullOrEmpty( _hint ) )
				return EditorGUILayout.IntPopup( _selected, _labels, _values, _options ); 
			else
				return EditorGUILayout.IntPopup( _title, _selected, _labels, _values, _options ); 
		}

		
		public static Enum EnumPopup( string _title, string _hint, Enum _selected )
		{
			return EditorGUILayout.EnumPopup( new GUIContent( _title, _hint) , _selected );
		}

		public static Enum EnumPopup( string _title, string _hint, Enum _selected, string _help  )
		{
			BeginHorizontal();
				Enum _value = EnumPopup( _title, _hint, _selected );
			EndHorizontal( _help );
			return _value;
		}
			


		public static bool ToggleLeft( string _title, string _hint, bool _toggle, bool _bold )
		{
			if( _bold )
				return EditorGUILayout.ToggleLeft( new GUIContent( _title, _hint ) ,_toggle, EditorStyles.boldLabel );
			else
				return EditorGUILayout.ToggleLeft( new GUIContent( _title, _hint ) ,_toggle );
		}

		public static bool ToggleLeft( string _title, string _hint, bool _toggle, bool _bold, string _help )
		{
			BeginHorizontal();
				bool _value = ToggleLeft( _title, _hint, _toggle , _bold );
				GUILayout.FlexibleSpace();			
			EndHorizontal( _help );
			return _value;
		}

		public static bool Toggle( string _title, string _hint, bool _toggle, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.Toggle( new GUIContent( _title, _hint ) ,_toggle );
			else
			{
				BeginHorizontal();
					bool _value = Toggle( _title, _hint, _toggle );
					GUILayout.FlexibleSpace();			
				EndHorizontal( _help );
				return _value;
			}			
		}

		public static bool Toggle( string _title, string _hint, string _label, bool _toggle, string _help = ""  )
		{
			BeginHorizontal();
			bool _value = Toggle( _title, _hint, _toggle );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUILayout.LabelField( "", _label, ICEEditorStyle.GreyMiniLabel );
			EditorGUI.indentLevel = _indent;

			EndHorizontal( _help );
			return _value;
		}

		public static bool InfoToggle( string _title, string _hint, bool _toggle, string _help = "" )
		{
			BeginHorizontal();
				bool _value = Toggle( _title, _hint, _toggle );
				GUILayout.FlexibleSpace();			
			EndHorizontal( _help );
			return _value;
		}

		public static string Tag( string _title, string _hint, string _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.TagField( new GUIContent( _title, _hint ), _value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.TagField( new GUIContent( _title, _hint ), _value );
				EndHorizontal( _help );
				return _value;
			}			
		}

		public static string ColliderPopup( GameObject _object, string _title, string _hint, string _value, string _help = ""  )
		{

			Collider[] _colliders = _object.GetComponentsInChildren<Collider>();

			GUIContent[] _options = new GUIContent[ _colliders.Length + 1 ];
			int _selected = 0;

			_options[0] = new GUIContent( " " );
			for( int i = 0 ; i < _colliders.Length ; i++ )
			{
				Collider _collider = _colliders[i];

				int _index = i + 1;

				_options[ _index ] = new GUIContent( _collider.name );

				if( _collider.name == _value )
					_selected = _index;
			}


			_selected = Popup( _title, _hint , _selected, _options, _help );

			return _options[_selected].text;
		}

/*
		public static List<string> layers;
		public static List<int> layerNumbers;
		public static string[] layerNames;
		public static long lastUpdateTick;
		public static LayerMask LayerMask( string label, LayerMask selected, bool showSpecial) {
			
			if (layers == null || (System.DateTime.Now.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) {
				lastUpdateTick = System.DateTime.Now.Ticks;
				if (layers == null) {
					layers = new List<string>();
					layerNumbers = new List<int>();
					layerNames = new string[4];
				} else {
					layers.Clear ();
					layerNumbers.Clear ();
				}
				
				int emptyLayers = 0;
				for (int i=0;i<32;i++) {
					string layerName = LayerMask.LayerToName(i);
					
					if (layerName != "") {
						
						for (;emptyLayers>0;emptyLayers--) layers.Add ("Layer "+(i-emptyLayers));
						layerNumbers.Add (i);
						layers.Add (layerName);
					} else {
						emptyLayers++;
					}
				}
				
				if (layerNames.Length != layers.Count) {
					layerNames = new string[layers.Count];
				}
				for (int i=0;i<layerNames.Length;i++) layerNames[i] = layers[i];
			}
			
			selected.value =  EditorGUILayout.MaskField (label,selected.value,layerNames);
			
			return selected;
		}*/

		public static int Layer( string _title, string _hint, int _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.LayerField( new GUIContent( _title, _hint ), _value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.LayerField( new GUIContent( _title, _hint ), _value );		
				EndHorizontal( _help );
				return _value;
			}			
		}


		public static float Float( string _title, string _hint, float _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.FloatField( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.FloatField( new GUIContent( _title, _hint ) ,_value );
				GUILayout.FlexibleSpace();			
				EndHorizontal( _help );
				return _value;
			}			
		}

		public static int Integer( string _title, string _hint, int _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.IntField( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.IntField( new GUIContent( _title, _hint ) ,_value );
				GUILayout.FlexibleSpace();			
				EndHorizontal( _help );
				return _value;
			}			
		}

		public static string Text( string _title, string _hint, string _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.TextField( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.TextField( new GUIContent( _title, _hint ) ,_value );
				EndHorizontal( _help );
				return _value;
			}			
		}

		public static Vector3 DefaultVector3Field( string _title, string _hint, Vector3 _value, Vector3 _default, string _help = ""  )
		{
			BeginHorizontal();
			_value = EditorGUILayout.Vector3Field( new GUIContent( _title, _hint ) , _value );
			_value = ICEEditorLayout.ButtonDefault( _value, _default );
			EndHorizontal( _help );
			return _value;
		}
		
		public static Vector4 Vector4Field( string _title, string _hint, Vector3 _value, string _help = ""  )
		{
#if UNITY_5_4 || UNITY_5_4_OR_NEWER
			if( _help == "" )
				return EditorGUILayout.Vector4Field( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.Vector4Field( new GUIContent( _title, _hint ) , _value );
				EndHorizontal( _help );
				return _value;
			}	
#else
			if( _help == "" )
				return EditorGUILayout.Vector4Field( _title ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.Vector4Field( _title , _value );
				EndHorizontal( _help );
				return _value;
			}	
#endif
		}

		public static Vector3 Vector3Field( string _title, string _hint, Vector3 _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.Vector3Field( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.Vector3Field( new GUIContent( _title, _hint ) , _value );
				EndHorizontal( _help );
				return _value;
			}	
		}

		public static Vector2 Vector2Field( string _title, string _hint, Vector2 _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.Vector2Field( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.Vector2Field( new GUIContent( _title, _hint ) , _value );
				EndHorizontal( _help );
				return _value;
			}	
		}

		public static void PrefixLabel( string _text ){
			PrefixLabel( _text, false );
		}

		public static void PrefixLabel( string _text, bool _bold  ){

			//float _width = EditorGUIUtility.labelWidth;
			//EditorGUIUtility.labelWidth -= 7;

			//Debug.Log( Screen.width + " - " + EditorGUIUtility.labelWidth + " - " + EditorGUIUtility.currentViewWidth );

			if( _bold == true )
				EditorGUILayout.PrefixLabel( _text, "Button", ICEEditorStyle.LabelBold );
			else
				EditorGUILayout.PrefixLabel( _text, "Button" );

			//GUILayout.Space( EditorGUIUtility.labelWidth );
	
			//EditorGUIUtility.labelWidth = _width;
		}




		public static void Label( string _title, string _hint, string _content ){
			EditorGUILayout.LabelField( new GUIContent( _title, _hint ), new GUIContent( _content, _hint ) );
		}
			
		public static void Label( string _title ){
			Label( _title, false );
		}

		public static void RichTextLabel( string _text ){

			GUIStyle style = new GUIStyle ();
			style.richText = true;
			EditorGUILayout.LabelField( _text, style );
		}

		public static void Label( string _text, bool _bold ){
			if( _bold == true )
				EditorGUILayout.LabelField( _text, EditorStyles.boldLabel );
			else
				EditorGUILayout.LabelField( _text );
		}

		public static void Label( string _text, bool _bold, string _help )
		{
			if( _help == "" )
				Label( _text, _bold );
			else
			{
				BeginHorizontal();
					Label( _text, _bold );
					GUILayout.FlexibleSpace();
				EndHorizontal( _help );		
			}
		}

		public static string InfoLabel( string _text, bool _bold, ref bool _show_info, string _info, string _help )
		{
			if( _help == "" )
				Label( _text, _bold );
			else
			{
				BeginHorizontal();
				Label( _text, _bold );
				GUILayout.FlexibleSpace();

				EndHorizontal( ref _show_info, ref _info, _help );		
			}

			return _info;
		}

		public static bool LabelEnableButton( string _title, string _hint, bool _enabled, string _help = "" ){
			return LabelEnableButton( _title, _hint, "", _enabled, _help );
		}
		public static bool LabelEnableButton( string _title, string _hint, string _text, bool _enabled, string _help = "" )
		{
			BeginHorizontal();
			EditorGUI.BeginDisabledGroup( _enabled == false );
			Label( _title, _hint, _text );
			EditorGUI.EndDisabledGroup();
			_enabled = EnableButton( _enabled );
			EndHorizontal( _help );	

			return _enabled;
		}


		public static void EndProperty( Rect _rect, string _help = "" )
		{
			if( _help == "" )
				EditorGUI.EndProperty();
			else
			{
				ICEEditorInfo.HelpButton( _rect );
				EditorGUI.EndProperty();
				ICEEditorInfo.ShowHelp( _help );
			}
		}

		public static int IntField( string _title, string _hint, int _value, string _help = "" )
		{
			if( _help == "" )
				return EditorGUILayout.IntField( new GUIContent( _title, _hint ), _value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.IntField( new GUIContent( _title, _hint ), _value );
				EndHorizontal( _help );
				return _value;
			}	
		}

		/// <summary>
		/// Foldout the specified _foldout, title, _show_info, _info, _help and _bold.
		/// </summary>
		/// <param name="_foldout">If set to <c>true</c> foldout.</param>
		/// <param name="title">Title.</param>
		/// <param name="_show_info">Show info.</param>
		/// <param name="_info">Info.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		public static bool Foldout( bool _foldout, string title, ref bool _show_info , ref string _info , string _help, bool _bold = true )
		{
			BeginHorizontal();
				_foldout = Foldout( _foldout , title, _bold );
			EndHorizontal( ref _show_info, ref _info, _help );
			return _foldout;
		}

		/// <summary>
		/// Foldout the specified _foldout, title, _help and _bold.
		/// </summary>
		/// <param name="_foldout">If set to <c>true</c> foldout.</param>
		/// <param name="title">Title.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		public static bool Foldout( bool _foldout, string title, string _help, bool _bold = true )
		{
			BeginHorizontal();
				_foldout = Foldout( _foldout , title, _bold );
			EndHorizontal( _help );
			return _foldout;
		}

		/// <summary>
		/// Foldout the specified _foldout, title and _bold.
		/// </summary>
		/// <param name="_foldout">If set to <c>true</c> foldout.</param>
		/// <param name="title">Title.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		public static bool Foldout( bool _foldout, string title, bool _bold = true ){
			return EditorGUILayout.Foldout( _foldout , title, (_bold?ICEEditorStyle.Foldout:ICEEditorStyle.FoldoutNormal) );
		}


		public static float DurationSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, float _default, ref bool _transit, string _help = ""  )
		{
			BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _transit == true );				
					_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
					
					if( _default != _value )
						GUI.backgroundColor = Color.yellow;
					
					if( Button( "RESET", "Set value to " + _default ) )
						_value = _default;

				EditorGUI.EndDisabledGroup();
			
				_transit = CheckButtonMiddle( "TRANSIT", "Use target as transit point without stopping and changing behaviours", _transit );

			EndHorizontal( _help );
			
			return _value;
			
			
		}




		public static float ToggleSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, float _default, ref bool _toggle, string _toggle_title, string _toggle_tooltip, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			if( _toggle )
				GUI.backgroundColor = Color.yellow;
			else 
				GUI.backgroundColor = Color.green;
			
			_toggle = CheckButtonSmall( _toggle_title, _toggle_tooltip, _toggle );
			
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			
			EndHorizontal( _help );
			return _value;
		}


		/*
		public static Vector3 Velocity( string _title, string _tooltip, Vector3 _velocity, float _precision, float _min, float _max, Vector3 _default_velocity )
		{
			Label( _title, false );
			EditorGUI.indentLevel++;

			bool _toggle = false;

			_velocity.x = AutoSlider( "Sidewards \t(x)", "x-Velocity", _velocity.x, _precision, _min, _max, ref _toggle, 0 );

			if( _toggle )
				_velocity.x = _default_velocity.x;
			_toggle = false;

			_velocity.y = AutoSlider( "Vertical \t(y)", "y-Velocity", _velocity.y, _precision, _min, _max, ref _toggle,0 );

			if( _toggle )
				_velocity.y = _default_velocity.y;
			_toggle = false;

			_velocity.z = AutoSlider( "Forwards \t(z)", "z-Velocity", _velocity.z, _precision, _min, _max, ref _toggle, 0 );

			if( _toggle )
				_velocity.z = _default_velocity.z;
			_toggle = false;

			EditorGUI.indentLevel--;
			return _velocity;
		}
*/
		public static string AnimatorParametersPopup( Animator _animator, string title, string _hint, string key )
		{
			if( _animator == null || ! _animator.isInitialized || _animator.parameterCount == 0 )
			{
				EditorGUI.BeginDisabledGroup( true );
					Popup( title, _hint, 0, new string[1]{ "-" } );
				EditorGUI.EndDisabledGroup();

				string _msg = "";
				if( _animator == null )
					_msg = "Animator component can not be found! Please add an Animator component to the the GameObject!";
				else if( ! _animator.isInitialized )
					_msg = "Animator is not initialized. Please check and activate the GameObject!";
				else if( _animator.parameterCount == 0 )
					_msg = "Animator does not contain any parameters. Please add the required parameters!";
					
				EditorGUILayout.HelpBox( _msg, MessageType.Warning );

				return key;
			}				
			else
			{
				List<string> _options = new List<string>();
				int _selected = 0;
				for( int _i = 0 ; _i < _animator.parameterCount; _i++ )
				{
					AnimatorControllerParameter _parameter = _animator.parameters[_i];

					if( _parameter !=  null )
					{
						_options.Add( _parameter.name );

						if( _parameter.name == key )
							_selected = _i;
					}
				}

				_selected = Popup( title, _hint, _selected, _options.ToArray() );

				return (string)_options[ _selected ];
			}
		}

		public static AnimatorControllerParameter AnimatorParametersPopupData( Animator _animator, string _key, params GUILayoutOption[] _layouts )
		{
			if( _animator == null )
				return null;


			if( _animator.parameterCount != 0 )
			{
				GUIContent[] _options = new GUIContent[_animator.parameterCount];
				int _selected = 0;
				for( int _i = 0 ; _i < _animator.parameterCount; _i++ )
				{
					AnimatorControllerParameter _parameter = _animator.parameters[_i];
					
					if( _parameter !=  null )
					{
						_options[_i] = new GUIContent();
						_options[_i].text = _parameter.name;
						
						if( _parameter.name == _key )
							_selected = _i;
					}
				}
				_selected = EditorGUILayout.Popup( _selected, _options, _layouts ); 

				return (AnimatorControllerParameter)_animator.parameters[ _selected ];
			}
			else
			{
				ICEEditorLayout.Label( "Invalid Parameter" );	
				return null;
			}
		}

		public static string DrawListPopupExt( string _title, string _key, List<string> _list, params GUILayoutOption[] _gui_options )
		{			
			string[] _options = new string[_list.Count];

			int _index = 0;
			for(int i=0;i < _list.Count ;i++)
			{
				if( _list[i] == " " && i == 0 )
					_options[i] = " ";
				else if( _list[i] == " " )
					_options[i] = "";
				else
					_options[i] = _list[i];

				if( _key == _options[i] )
					_index = i;
			}				

			_index = EditorGUILayout.Popup( _title, _index, _options, _gui_options );

			return (string)_options[ _index ];
		}

		public static string DrawListPopup( string _title, string _key, List<string> _list, params GUILayoutOption[] _gui_options ){
			return DrawListPopup( _title, _key, _list.ToArray(), _gui_options );
		}

		public static string DrawListPopup( string _title, string _key, string[] _options, params GUILayoutOption[] _gui_options )
		{
			int _index = 0;
			for( int _i = 0 ; _i < _options.Length ; _i++)
			{
				if( (string)_options[_i] == _key )
				{
					_index = _i;
					break;
				}
			}

			if( _index < 0 || _index >= _options.Length )
			{
				EditorGUILayout.Popup( _title, 0, new string[]{ _key }, _gui_options );
				return _key;
			}
			else				
			{
				_index = EditorGUILayout.Popup( _title, _index, _options, _gui_options );
				return (string)_options[ _index ];
			}
		}

		public static void DrawProgressBarFull( string _title, float _value, string _help = "" )
		{
			BeginHorizontal();					
			Rect _rect = GUILayoutUtility.GetRect(0,15);
			EditorGUI.ProgressBar( _rect, _value/100, _title + " (" +_value + "%)" );			
			EndHorizontal( _help );
		}

		public static void DrawProgressBar( string _title, float _value, string _help = "" )
		{
			BeginHorizontal();			
				EditorGUILayout.PrefixLabel( _title );			
				Rect _rect = GUILayoutUtility.GetRect(0,16);
				EditorGUI.ProgressBar( _rect, _value/100, _value + "%" );			
			EndHorizontal( _help );
		}

		public static float DrawValueButtons( float _value, float _step, float _min = 0, float _max = 0 )
		{
			
			if( ButtonMini( "<", "minus " + _step ))
				_value -= _step;
			
			if( ButtonMini( ">", "plus " + _step ) )
				_value += _step; 
			
			if( _value < _min )
				_value = _min;
			
			if( _max > _min && _value > _max )
				_value = _max;
			
			return _value;
		}
	}
}
