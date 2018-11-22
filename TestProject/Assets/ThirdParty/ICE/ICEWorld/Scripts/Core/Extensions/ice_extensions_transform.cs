// ##############################################################################
//
// ice_extensions_transform.cs 
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

using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	public static class RectTransformExtensions
	{
		public static void SetAnchor(this RectTransform _source, AnchorPresets _align, int _offset_x = 0, int _offset_y = 0 )
		{
			_source.anchoredPosition3D = new Vector3( _offset_x, _offset_y, 0 );

			switch( _align )
			{
				case(AnchorPresets.TopLeft):
				{
					_source.anchorMin = new Vector2(0, 1);
					_source.anchorMax = new Vector2(0, 1);
					break;
				}
				case (AnchorPresets.TopCenter):
				{
					_source.anchorMin = new Vector2(0.5f, 1);
					_source.anchorMax = new Vector2(0.5f, 1);
					break;
				}
				case (AnchorPresets.TopRight):
				{
					_source.anchorMin = new Vector2(1, 1);
					_source.anchorMax = new Vector2(1, 1);
					break;
				}

				case (AnchorPresets.MiddleLeft):
				{
					_source.anchorMin = new Vector2(0, 0.5f);
					_source.anchorMax = new Vector2(0, 0.5f);
					break;
				}
				case (AnchorPresets.MiddleCenter):
				{
					_source.anchorMin = new Vector2(0.5f, 0.5f);
					_source.anchorMax = new Vector2(0.5f, 0.5f);
					break;
				}
				case (AnchorPresets.MiddleRight):
				{
					_source.anchorMin = new Vector2(1, 0.5f);
					_source.anchorMax = new Vector2(1, 0.5f);
					break;
				}

				case (AnchorPresets.BottomLeft):
				{
					_source.anchorMin = new Vector2(0, 0);
					_source.anchorMax = new Vector2(0, 0);
					break;
				}
				case (AnchorPresets.BottonCenter):
				{
					_source.anchorMin = new Vector2(0.5f, 0);
					_source.anchorMax = new Vector2(0.5f,0);
					break;
				}
				case (AnchorPresets.BottomRight):
				{
					_source.anchorMin = new Vector2(1, 0);
					_source.anchorMax = new Vector2(1, 0);
					break;
				}

				case (AnchorPresets.HorStretchTop):
				{
					_source.anchorMin = new Vector2(0, 1);
					_source.anchorMax = new Vector2(1, 1);
					break;
				}
				case (AnchorPresets.HorStretchMiddle):
				{
					_source.anchorMin = new Vector2(0, 0.5f);
					_source.anchorMax = new Vector2(1, 0.5f);
					break;
				}
				case (AnchorPresets.HorStretchBottom):
				{
					_source.anchorMin = new Vector2(0, 0);
					_source.anchorMax = new Vector2(1, 0);
					break;
				}

				case (AnchorPresets.VertStretchLeft):
				{
					_source.anchorMin = new Vector2(0, 0);
					_source.anchorMax = new Vector2(0, 1);
					break;
				}
				case (AnchorPresets.VertStretchCenter):
				{
					_source.anchorMin = new Vector2(0.5f, 0);
					_source.anchorMax = new Vector2(0.5f, 1);
					break;
				}
				case (AnchorPresets.VertStretchRight):
				{
					_source.anchorMin = new Vector2(1, 0);
					_source.anchorMax = new Vector2(1, 1);
					break;
				}

				case (AnchorPresets.StretchAll):
				{
					_source.anchorMin = new Vector2(0, 0);
					_source.anchorMax = new Vector2(1, 1);
					break;
				}
			}
		}

		public static void SetPivot( this RectTransform _source, PivotPresets _preset )
		{

			switch( _preset )
			{
				case (PivotPresets.TopLeft):
				{
					_source.pivot = new Vector2(0, 1);
					break;
				}
				case (PivotPresets.TopCenter):
				{
					_source.pivot = new Vector2(0.5f, 1);
					break;
				}
				case (PivotPresets.TopRight):
				{
					_source.pivot = new Vector2(1, 1);
					break;
				}

				case (PivotPresets.MiddleLeft):
				{
					_source.pivot = new Vector2(0, 0.5f);
					break;
				}
				case (PivotPresets.MiddleCenter):
				{
					_source.pivot = new Vector2(0.5f, 0.5f);
					break;
				}
				case (PivotPresets.MiddleRight):
				{
					_source.pivot = new Vector2(1, 0.5f);
					break;
				}

				case (PivotPresets.BottomLeft):
				{
					_source.pivot = new Vector2(0, 0);
					break;
				}
				case (PivotPresets.BottomCenter):
				{
					_source.pivot = new Vector2(0.5f, 0);
					break;
				}
				case (PivotPresets.BottomRight):
				{
					_source.pivot = new Vector2(1, 0);
					break;
				}
			}
		}
	
	
		public static void SetAnchorToParent( this RectTransform _source )
		{
			RectTransform _parent = _source.transform.parent.GetComponent<RectTransform>();

			if( _parent == null )
				return;
			
			var _offsetMin = _source.offsetMin;
			var _offsetMax = _source.offsetMax;
			var _anchorMin = _source.anchorMin;
			var _anchorMax = _source.anchorMax;

			var _parent_width = _parent.rect.width;      
			var _parent_height = _parent.rect.height;  

			_source.anchorMin = new Vector2(
				_anchorMin.x + ( _offsetMin.x / _parent_width ),
				_anchorMin.y + ( _offsetMin.y / _parent_height ) );
			_source.anchorMax = new Vector2(
				_anchorMax.x + ( _offsetMax.x / _parent_width ),
				_anchorMax.y + ( _offsetMax.y / _parent_height ) );

			_source.offsetMin = new Vector2(0, 0);
			_source.offsetMax = new Vector2(0, 0);
			_source.pivot = new Vector2(0.5f, 0.5f);

		}
	}
}
