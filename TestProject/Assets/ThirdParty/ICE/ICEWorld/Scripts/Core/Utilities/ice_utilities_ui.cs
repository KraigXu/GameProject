// ##############################################################################
//
// ice_utilities_ui.cs 
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
using UnityEngine.UI;
using System.Collections;

using ICE.World.Objects;
using ICE.World.EnumTypes;

namespace ICE.World.Utilities
{
	public class InterfaceTools
	{
		public static RectTransform CreateRectTransformObject( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset )
		{
			GameObject _object = new GameObject( _name );
	
			RectTransform _transform = _object.AddComponent<RectTransform>();
			if( _transform != null )
			{
				_transform.SetParent( _parent, false );

				_transform.localPosition = Vector3.zero; 
				_transform.localScale = Vector3.one;
				_transform.sizeDelta = _size; 	

				_transform.SetAnchor( _anchor );
				_transform.SetPivot( _pivot );
			
				if( _offset.y != 0 )
					_transform.offsetMin = new Vector2( _transform.offsetMin.x, _offset.y );

				if( _offset.height != 0 )
					_transform.offsetMax = new Vector2( _transform.offsetMax.x, _offset.height );

				if( _offset.x != 0 )
					_transform.offsetMin = new Vector2( _offset.x, _transform.offsetMin.y );

				if( _offset.width != 0 )
					_transform.offsetMax = new Vector2( _offset.width, _transform.offsetMax.y );
			}

			return _object.transform as RectTransform;
		}

		public static Canvas AddCanvas( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset, RenderMode _render_mode = RenderMode.ScreenSpaceOverlay )
		{
			if( _parent == null )
				return null;

			Canvas _canvas = null;
			RectTransform _transform = CreateRectTransformObject( _name, _parent, _size, _anchor, _pivot, _offset ); 
			if( _transform != null )
			{
				_canvas = _transform.gameObject.AddComponent<Canvas>();
				_canvas.renderMode = _render_mode;
				_transform.gameObject.AddComponent<CanvasScaler>();
				_transform.gameObject.AddComponent<GraphicRaycaster>();
			}

			return _canvas;
		}

		public static Dropdown AddDropdown( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset, Color _color, string _label )
		{
			if( _parent == null )
				return null;

			Dropdown _dropdown = null;
			RectTransform _transform = CreateRectTransformObject( _name, _parent, _size, _anchor, _pivot, _offset ); 
			if( _transform != null )
			{
				_transform.gameObject.AddComponent<CanvasRenderer>();
				Image _image = _transform.gameObject.AddComponent<Image>();

				#if UNITY_EDITOR
				_image.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
				#endif

				_dropdown = _transform.gameObject.AddComponent<Dropdown>();

				//Text _text = 
				InterfaceTools.AddText( "Label", _dropdown.transform, new Vector2( 0, 0 ), AnchorPresets.StretchAll, PivotPresets.MiddleCenter, new Rect( 30, 0,0,0 ), Color.gray, _label );
				//Image _arrow = 
				InterfaceTools.AddImage( "Arrow", _transform, new Vector2( 20, 20 ), AnchorPresets.MiddleRight, PivotPresets.MiddleRight, new Rect( -15, 0,0,0 ), Color.clear );


			}

			return _dropdown;
		}

		public static Toggle AddToggle( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset, Color _color, string _label )
		{
			if( _parent == null )
				return null;

			Toggle _toggle = null;
			RectTransform _transform = CreateRectTransformObject( _name, _parent, _size, _anchor, _pivot, _offset ); 
			if( _transform != null )
			{
				_transform.gameObject.AddComponent<CanvasRenderer>();
				_toggle = _transform.gameObject.AddComponent<Toggle>();
				//_button.color = _color;

				#if UNITY_EDITOR
				_toggle.graphic = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Graphic>("UI/Skin/Background.psd");
				_toggle.targetGraphic = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Graphic>("UI/Skin/Checkmark.psd");
				#endif
				/*
				Image _background = InterfaceTools.AddImage( "Background", _transform, new Vector2( 20, 20 ), AnchorPresets.TopLeft, PivotPresets.TopLeft, new Rect(0,0,0,0), Color.clear );
				Image _checkmark = InterfaceTools.AddImage( "Checkmark", _background.transform, new Vector2( 20, 20 ), AnchorPresets.MiddleCenter, PivotPresets.MiddleCenter, new Rect(0,0,0,0), Color.clear );

				#if UNITY_EDITOR
				_checkmark.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
				#endif
				*/

				//Text _text = 
				InterfaceTools.AddText( "Text", _transform, new Vector2( 0, 0 ), AnchorPresets.StretchAll, PivotPresets.MiddleCenter, new Rect( 30, 0,0,0 ), Color.gray, _label );
				

			}

			return _toggle;
		}

		public static Button AddButton( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset, Color _color )
		{
			if( _parent == null )
				return null;

			Button _button = null;
			RectTransform _transform = CreateRectTransformObject( _name, _parent, _size, _anchor, _pivot, _offset ); 
			if( _transform != null )
			{
				_transform.gameObject.AddComponent<CanvasRenderer>();
				_button = _transform.gameObject.AddComponent<Button>();
				//_button.color = _color;

			}

			return _button;
		}

		public static Image AddImage( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset, Color _color )
		{
			if( _parent == null )
				return null;

			Image _image = null;
			RectTransform _transform = CreateRectTransformObject( _name, _parent, _size, _anchor, _pivot, _offset ); 
			if( _transform != null )
			{
				_transform.gameObject.AddComponent<CanvasRenderer>();
				_image = _transform.gameObject.AddComponent<Image>();
				_image.color = _color;
			}

			return _image;
		}

		public static Text AddText( string _name, Transform _parent, Vector2 _size, AnchorPresets _anchor, PivotPresets _pivot, Rect _offset, Color _color, string _value )
		{
			if( _parent == null )
				return null;

			Text _text = null;
			RectTransform _text_transform = CreateRectTransformObject( _name, _parent, _size, _anchor, _pivot, _offset ); 
			if( _text_transform != null )
			{
				_text_transform.gameObject.AddComponent<CanvasRenderer>();
				_text = _text_transform.gameObject.AddComponent<Text>();
				_text.color = _color;
				_text.resizeTextForBestFit = true;
				_text.text = _value;
				_text.alignment = TextAnchor.MiddleCenter;
			}

			return _text;
		}

		public static Text AddText( string _name, Transform _parent, Vector2 _size, Color _color, string _value, Vector2 _offset, float _inset = 0  )
		{
			if( _parent == null )
				return null;

			Text _text = null;
			RectTransform _text_transform = CreateRectTransformObject( _name, _parent, _size, AnchorPresets.MiddleCenter, PivotPresets.MiddleCenter, new Rect(0,0,0,0) ); 
			if( _text_transform != null )
			{
				float _scale = 0.008f;

				_text_transform.localScale = new Vector3( _scale, _scale, _scale );

				_text_transform.gameObject.AddComponent<CanvasRenderer>();
				_text = _text_transform.gameObject.AddComponent<Text>();
				_text.color = _color;
				_text.resizeTextForBestFit = true;
				_text.text = _value;
				_text.alignment = TextAnchor.MiddleCenter;

			}

			return _text;
		}

		public static Slider AddSlider( string _name, Transform _parent, Vector2 _size, Color _bar_color, Color _bg_color, Vector2 _offset, float _inset = 0  )
		{
			if( _parent == null )
				return null;

			Slider _slider = null;
			RectTransform _slider_transform = CreateRectTransformObject( _name, _parent, _size, AnchorPresets.HorStretchTop, PivotPresets.TopCenter, new Rect(0,0,0,0) ); 
			if( _slider_transform != null )
			{
				if( _offset.y != 0 )
					_slider_transform.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Top, _offset.y, _slider_transform.rect.height );

				if( _offset.x != 0 )
					_slider_transform.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Left, _offset.x, _slider_transform.rect.width );
	
				_slider = _slider_transform.gameObject.AddComponent<Slider>();
				_slider.transition = Selectable.Transition.None;
				_slider.navigation = Navigation.defaultNavigation;

				RectTransform _area_transform = CreateRectTransformObject( "BarArea", _slider_transform, new Vector2( 0, 0 ), AnchorPresets.StretchAll, PivotPresets.TopCenter, new Rect(0,0,0,0) ); 
				if( _area_transform != null )
				{
					_area_transform.gameObject.AddComponent<CanvasRenderer>();
					Image _bg_image = _area_transform.gameObject.AddComponent<Image>();
					if( _bg_image != null )
						_bg_image.color = _bg_color;

					RectTransform _bar_transform = CreateRectTransformObject( "BarFill", _area_transform, new Vector2( 0, 0 ), AnchorPresets.StretchAll, PivotPresets.TopCenter, new Rect(0,0,0,0) );
					if( _bar_transform != null )
					{
						if( _inset > 0 )
						{
							_bar_transform.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Top, _inset, -_inset * 2 );
							_bar_transform.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Left, _inset, -_inset * 2 );
						}


						_slider.fillRect = _bar_transform;

						_slider.value = _slider.maxValue;

						_bar_transform.gameObject.AddComponent<CanvasRenderer>();
						Image _image = _bar_transform.gameObject.AddComponent<Image>();
						if( _image != null )
							_image.color = _bar_color;
					}
				}
			}

			return _slider;
		}
	}
}


