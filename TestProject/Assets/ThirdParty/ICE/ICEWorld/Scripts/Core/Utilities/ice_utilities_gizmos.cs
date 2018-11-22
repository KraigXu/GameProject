// ##############################################################################
//
// ice_utilities_gizmos.cs | ICE.World.Utilities.CustomGizmos
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

namespace ICE.World.Utilities
{
	/// <summary>
	/// Custom gizmos.
	/// </summary>
	public static class CustomGizmos 
	{
		public static void Cube( int _id, Vector3 _position, Quaternion _rotation, float _size  )
		{
			#if UNITY_EDITOR && UNITY_5_6_OR_NEWER
					UnityEditor.Handles.CubeHandleCap( _id, _position , _rotation, _size, EventType.Repaint );
			#elif UNITY_EDITOR
				UnityEditor.Handles.CubeCap( _id, _position , _rotation, _size );
			#endif
		}

		public static void Cylinder( int _id, Vector3 _position, Quaternion _rotation, float _size  )
		{
			#if UNITY_EDITOR && UNITY_5_6_OR_NEWER
					UnityEditor.Handles.CylinderHandleCap( _id, _position , _rotation, _size, EventType.Repaint );
			#elif UNITY_EDITOR
				UnityEditor.Handles.CylinderCap( _id, _position , _rotation, _size );
			#endif
		}

		public static void Arrow( int _id, Vector3 _position, Quaternion _rotation, float _size  )
		{
			#if UNITY_EDITOR && UNITY_5_6_OR_NEWER
					UnityEditor.Handles.ArrowHandleCap( _id, _position , _rotation, _size, EventType.Repaint );
			#elif UNITY_EDITOR
				UnityEditor.Handles.ArrowCap( _id, _position , _rotation, _size );
			#endif
		}

		public static void Sphere( int _id, Vector3 _position, Quaternion _rotation, float _size  )
		{
			#if UNITY_EDITOR && UNITY_5_6_OR_NEWER
					UnityEditor.Handles.SphereHandleCap( _id, _position , _rotation, _size, EventType.Repaint );
			#elif UNITY_EDITOR
				UnityEditor.Handles.SphereCap( _id, _position , _rotation, _size );
			#endif
		}

		public static void DrawLine( Vector3 _position_1, Vector3 _position_2 )
		{
			#if UNITY_EDITOR
				UnityEditor.Handles.DrawLine( _position_1, _position_2 );
			#endif
		}

		public static void DrawSolidDisc(  Vector3 _center, Vector3 _normal, float _radius )
		{
			#if UNITY_EDITOR
			UnityEditor.Handles.DrawSolidDisc( _center , _normal, _radius );
			#endif
		}

		public static void DrawSolidArc(  Vector3 _center, Vector3 _normal, Vector3 _from, float _angle, float _radius )
		{
			#if UNITY_EDITOR
			UnityEditor.Handles.DrawSolidArc( _center, _normal, _from, _angle, _radius );
			#endif
		}

		public static void DrawSolidRectangleWithOutline( Vector3[] verts, Color _face, Color _outline )
		{
			#if UNITY_EDITOR
			UnityEditor.Handles.DrawSolidRectangleWithOutline( verts, _face, _outline );
			#endif
		}

		public static void DrawDottedLine( Vector3 _position_1, Vector3 _position_2, float _space )
		{
			#if UNITY_EDITOR
			UnityEditor.Handles.DrawDottedLine( _position_1, _position_2, _space );
			#endif
		}
			
		public static void HandlesColor( Color _color )
		{			
			#if UNITY_EDITOR
			UnityEditor.Handles.color = _color;
			#endif
		}

		public static void GizmosColor( Color _color )
		{			
			#if UNITY_EDITOR
			Gizmos.color = _color;
			#endif
		}

		public static void Color( Color _color )
		{			
			#if UNITY_EDITOR
				Gizmos.color = _color;
				UnityEditor.Handles.color = _color;
			#endif
		}

		public static void Circle( Vector3 position, Vector3 up, float radius = 1.0f )
		{
			Vector3 _up = up.normalized * radius;
			Vector3 _forward = Vector3.Slerp(_up, -_up, 0.5f);
			Vector3 _right = Vector3.Cross(_up, _forward).normalized*radius;

			Matrix4x4 _matrix = new Matrix4x4();

			_matrix[0] = _right.x;
			_matrix[1] = _right.y;
			_matrix[2] = _right.z;

			_matrix[4] = _up.x;
			_matrix[5] = _up.y;
			_matrix[6] = _up.z;

			_matrix[8] = _forward.x;
			_matrix[9] = _forward.y;
			_matrix[10] = _forward.z;

			Vector3 _last_point = position + _matrix.MultiplyPoint3x4( new Vector3( Mathf.Cos(0), 0, Mathf.Sin(0) ) );
			Vector3 _next_point = Vector3.zero;

			for(var i = 0; i < 91; i++){
				_next_point.x = Mathf.Cos( (i*4)*Mathf.Deg2Rad );
				_next_point.z = Mathf.Sin( (i*4)*Mathf.Deg2Rad );
				_next_point.y = 0;

				_next_point = position + _matrix.MultiplyPoint3x4(_next_point);

				Gizmos.DrawLine( _last_point, _next_point );
				_last_point = _next_point;
			}
		}

		public static void Capsule( Transform _transform, Vector3  _start, Vector3 _end, float _radius = 1 )
		{
			Gizmos.matrix = Matrix4x4.TRS( _transform.TransformPoint( Vector3.zero ), _transform.rotation, Vector3.one );
		
			Vector3 _up = (_end- _start).normalized*_radius;
			Vector3 _forward = Vector3.Slerp( _up, - _up, 0.5f);
			Vector3  _right = Vector3.Cross( _up, _forward ).normalized * _radius;

			float _height = ( _start - _end ).magnitude;
			float _side_length = Mathf.Max( 0, ( _height * 0.5f )-_radius );
			Vector3 _middle  = ( _end + _start ) * 0.5f;

			_start = _middle + ( ( _start -_middle ).normalized * _side_length );
			_end = _middle + ( ( _end -_middle ).normalized * _side_length );

			//Radial circles
			Circle( _start,  _up, _radius );	
			Circle( _end, - _up, _radius );

			//Side lines
			Gizmos.DrawLine( _start + _right, _end + _right );
			Gizmos.DrawLine( _start - _right, _end - _right );

			Gizmos.DrawLine( _start+_forward, _end+_forward );
			Gizmos.DrawLine( _start-_forward, _end-_forward );

			for(int i = 1; i < 26; i++){

				//Start endcap
				Gizmos.DrawLine(Vector3.Slerp( _right, - _up, i/25.0f)+ _start, Vector3.Slerp( _right, - _up, (i-1)/25.0f)+ _start );
				Gizmos.DrawLine(Vector3.Slerp(- _right, - _up, i/25.0f)+ _start, Vector3.Slerp(- _right, - _up, (i-1)/25.0f)+ _start );
				Gizmos.DrawLine(Vector3.Slerp(_forward, - _up, i/25.0f)+ _start, Vector3.Slerp(_forward, - _up, (i-1)/25.0f)+ _start );
				Gizmos.DrawLine(Vector3.Slerp(-_forward, - _up, i/25.0f)+ _start, Vector3.Slerp(-_forward, - _up, (i-1)/25.0f)+ _start );

				//End endcap
				Gizmos.DrawLine(Vector3.Slerp( _right,  _up, i/25.0f)+_end, Vector3.Slerp( _right,  _up, (i-1)/25.0f)+_end );
				Gizmos.DrawLine(Vector3.Slerp(- _right,  _up, i/25.0f)+_end, Vector3.Slerp(- _right,  _up, (i-1)/25.0f)+_end );
				Gizmos.DrawLine(Vector3.Slerp(_forward,  _up, i/25.0f)+_end, Vector3.Slerp(_forward,  _up, (i-1)/25.0f)+_end );
				Gizmos.DrawLine(Vector3.Slerp(-_forward,  _up, i/25.0f)+_end, Vector3.Slerp(-_forward,  _up, (i-1)/25.0f)+_end );
			}

			Gizmos.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, Vector3.one );
		}

		public static void Box( Transform _transform, Vector3 _size, Vector3 _center )
		{ 
			Gizmos.matrix = Matrix4x4.TRS( _transform.TransformPoint( _center ), _transform.rotation, Vector3.one );
			Gizmos.DrawWireCube( Vector3.zero, _size );
			Gizmos.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, Vector3.one );
		}

		public static void Text( string _text, Vector3 _world_position, Color _color, float _font_size = 14, FontStyle _font_style = FontStyle.Normal ) {

			#if UNITY_EDITOR

			UnityEditor.Handles.BeginGUI();
	
				UnityEditor.SceneView view = UnityEditor.SceneView.currentDrawingSceneView;
				Vector3 _screen_position = view.camera.WorldToScreenPoint(_world_position);
				Vector2 _size = GUI.skin.label.CalcSize(new GUIContent(_text));

				GUIStyle _style = new GUIStyle();
				_style.normal.textColor = _color;
				float _distance = (int)PositionTools.Distance( view.camera.transform.position, _world_position );
				float _normalized = 1-MathTools.Normalize( _distance, 0, 250 );
				if( _normalized > 0.1f && _normalized < 1 )
				{
				_style.fontSize = (int)( _font_size * _normalized);
				_style.fontStyle = _font_style;
					GUI.Label(new Rect(_screen_position.x - (_size.x / 2), -_screen_position.y + view.position.height + 4, _size.x, _size.y), _text, _style);
				}
			UnityEditor.Handles.EndGUI();

			#endif
		}

		public static void Text(GUISkin gui_skin, string text, Vector3 position, Color? color = null, int font_size = 0, float yOffset = 0, float xOffset = 0)
		{
			#if UNITY_EDITOR
			/*
			GUISkin _prev_skin = GUI.skin;
			if ( gui_skin == null)
				Debug.LogWarning("CAUTION: GUI Skin is null");
			else
				GUI.skin = gui_skin;


			Vector3 _screen_point = Camera.current.WorldToScreenPoint(position);


			int f = 10;
			font_size = (int)(font_size / _screen_point.z * f);
			yOffset = yOffset / _screen_point.z * f;
			xOffset = xOffset / _screen_point.z * f;

			if( font_size > 30 || font_size < 0 )
				return;
			
			GUIContent _text_content = new GUIContent(text + " - \n\n" + font_size);
			
			GUIStyle style = (gui_skin != null) ? new GUIStyle(gui_skin.GetStyle("Label")) : new GUIStyle();
			if (color != null)
				style.normal.textColor = (Color)color;
			if (font_size > 0 )
				style.fontSize = font_size; 


			

			Vector2 _text_size = style.CalcSize(_text_content);

			
			if (_screen_point.z > 0) 
			{
				Vector3 _screen_position = new Vector3(_screen_point.x - _text_size.x * 0.5f + xOffset, _screen_point.y + _text_size.y * 0.5f + yOffset, _screen_point.z);


				Vector3 _world_position = Camera.current.ScreenToWorldPoint( _screen_position );
				UnityEditor.Handles.Label(_world_position, _text_content, style);

				float _offset = 10 / _screen_point.z * f;
				float _handle = 50 / _screen_point.z * f;

				Vector3 _pos_1 = Camera.current.ScreenToWorldPoint( new Vector3( _screen_position.x - _offset,  _screen_position.y + _offset, _screen_point.z ) );
				Vector3 _pos_2 = Camera.current.ScreenToWorldPoint( new Vector3( _screen_position.x + _text_size.x + _offset,  _screen_position.y + _offset, _screen_point.z ) );
				Vector3 _pos_3 = Camera.current.ScreenToWorldPoint( new Vector3( _screen_position.x + _text_size.x + _offset,  _screen_position.y - _text_size.y - _offset, _screen_point.z ) );
				Vector3 _pos_4 = Camera.current.ScreenToWorldPoint( new Vector3( _screen_position.x - _offset,  _screen_position.y - _text_size.y - _offset, _screen_point.z ) );

		
				Vector3 _pos_handle_1 = Camera.current.ScreenToWorldPoint( new Vector3( _screen_position.x - _offset,  _screen_position.y - (_text_size.y/2) , _screen_position.z ) );
				Vector3 _pos_handle_2 = Camera.current.ScreenToWorldPoint( new Vector3( _screen_position.x - _handle,  _screen_position.y - (_text_size.y/2) , _screen_position.z ) );

				Color _org_color = Gizmos.color;
				Gizmos.color = (Color)color;

				Gizmos.DrawLine( _pos_1, _pos_2 );
				Gizmos.DrawLine( _pos_2, _pos_3 );
				Gizmos.DrawLine( _pos_3, _pos_4 );
				Gizmos.DrawLine( _pos_4, _pos_1 );

				Gizmos.DrawLine( _pos_handle_1, _pos_handle_2 );
				Gizmos.DrawLine( _pos_handle_2, position );

				Gizmos.color = _org_color;



			}
			GUI.skin = _prev_skin;

*/
			#endif
		}

		public static void DottedCircle( Vector3 _center, float _radius, float _degrees, float _dotsize  )
		{ 
			#if UNITY_EDITOR
			Vector3 last_position = Vector3.zero;
			
			for( float _angle = 0 ; _angle <= 360 ; _angle += _degrees )
			{
				float a = _angle * Mathf.PI / 180f;
				
				Vector3 new_position = _center + new Vector3(Mathf.Sin(a) * _radius, 0, Mathf.Cos(a) * _radius );
				
				if( last_position != Vector3.zero && new_position != Vector3.zero )
					CustomGizmos.DrawDottedLine( last_position, new_position, _dotsize );

				last_position = new_position;
			}
			
			#endif
		}

		public static void Circle( Vector3 center, float radius, float degrees, bool dotted )
		{
			Circle( center, radius, degrees, dotted, "", false );
		}

		public static float GetBestDegrees( float _radius, float _angle )
		{
			float _degree = 5 + ( 0.5f/_radius*Mathf.Rad2Deg );
			if( _degree > 45 )_degree = 45;
			return _angle * 2 / Mathf.CeilToInt( _angle * 2 /_degree );
		}

		public static void ArrowArc( Transform _target, float _radius, float _degree, float _left, float _right, float _level, bool _invert = false )
		{ 
			if( _target == null || _degree == 0 || _radius == 0 )
				return;
			


			for( float _angle = _left ; _angle <= _right + 0.15f ; _angle += _degree )
			{
				Vector3 _pos_1 = PositionTools.GetDirectionPosition( _target, _angle, _radius );
				Vector3 _pos_2 = PositionTools.GetDirectionPosition( _target, _angle, _radius + 1 );

				_pos_1.y = _level;
				_pos_2.y = _level;

				if( _invert )
					Arrow(_pos_2, _pos_1 - _pos_2 );
				else
					Arrow(_pos_1, _pos_2 - _pos_1 );
							
			}
		}

		public static void Arc( Transform _target, float _radius, float _degress, float _left, float _right, float _level, bool _dotted = true )
		{ 
			if( _target == null )
				return;

			Vector3 _center = _target.position;
			Vector3 _center_pos = PositionTools.GetDirectionPosition( _target, 0, _radius + ( _radius / 10 ) );
			Vector3 _left_pos = PositionTools.GetDirectionPosition( _target, _left, _radius );
			Vector3 _right_pos = PositionTools.GetDirectionPosition( _target, _right, _radius );

			_center.y = _level;
			_center_pos.y = _level;
			_left_pos.y = _level;
			_right_pos.y = _level;

			/*
			Gizmos.DrawLine( _center, _center_pos );
			Gizmos.DrawLine( _center, _left_pos );
			Gizmos.DrawLine( _center, _right_pos );*/

			Vector3 _last_pos = _left_pos;
			for( float _angle = _left ; _angle <= _right ; _angle += _degress )
			{
				Vector3 _pos = PositionTools.GetDirectionPosition( _target, _angle, _radius );
				_pos.y = _level;
				if( ! _dotted || ( (int)_angle % 2 == 0 ) )
					Gizmos.DrawLine( _last_pos, _pos );

				_last_pos = _pos;
			}
		}

		public static void Orbit( Vector3 _creature, Vector3 _center, float _radius, float _degress, float _shift, float _min, float _max, float _level )
		{ 
			float _angle = PositionTools.GetNormalizedVectorAngle( _creature - _center );

			Vector3 _last_position = Vector3.zero;

			while( ( _shift > 0 && _radius < _max ) || ( _shift < 0 && _radius > _min ) )
			{

				_radius += _shift * 0.02f;

				if( _radius < _min )
					_radius = _min;
				else if( _max > 0 && _radius > _max )
					_radius = _max;

				_angle += _degress;
				
				if( _angle > 360 )
					_angle = _angle - 360;
				
				float _a = _angle * Mathf.PI / 180f;
				
				Vector3 _new_position = _center + new Vector3(Mathf.Sin(_a) * _radius, 0, Mathf.Cos(_a) * _radius );
				_new_position.y = _level;
				
				if( _last_position != Vector3.zero )
					Gizmos.DrawLine( _last_position, _new_position );
				
				_last_position = _new_position;



			}

			if( _shift < 0 )
				Circle( _center, _min, 5, true, "", false );
			else if( _shift > 0 )
				Circle( _center, _max, 5, true, "", false );
			else
				Circle( _center, _radius, 5, true, "", false );
			
		}

		public static void Circle( Vector3 center, float radius, float degrees, bool dotted, string _text, bool _show_text )
		{ 
			Vector3 last_position = Vector3.zero;
			
			for( float _angle = 0 ; _angle <= 360 ; _angle += degrees )
			{
				float a = _angle * Mathf.PI / 180f;
				
				Vector3 new_position = center + new Vector3(Mathf.Sin(a) * radius, 0, Mathf.Cos(a) * radius );
				
				if( last_position != Vector3.zero && new_position != Vector3.zero )
				{
					if( ! dotted || ( _angle % 2 == 0 ) )
						Gizmos.DrawLine( last_position, new_position );
				}
				
				last_position = new_position;
			}

			if( _show_text && _text != "" )
			{
				Color _color = Gizmos.color;
				_color.a = 255;
				Text( GUI.skin, _text , last_position, _color, 12, 50, 50 );
				//
			}
		}

		public static float GetCircleDegrees( float _radius )
		{
			//Vector3 _screen_point = Camera.current.WorldToScreenPoint(_center);

			float _max = 90;
			float _min = 1.5f;
			//float r = _screen_point.z % 10;
			float _s = _min;// * _screen_point.z/100;
			float _f = 90/_radius;
			float _p = _s*_f;
			float _pr = _p % _min;
			float _degrees = _p - _pr;

			if( _degrees <= _min )
				_degrees = _min;

			if( _degrees >= _max )
				_degrees = _max;

			return _degrees;
		}

		public static void ZickZackCircle( Vector3 center, float radius, string _text, bool _show_text )
		{ 
			float step = GetCircleDegrees( radius);


			
			Vector3 _text_pos = Vector3.zero;
			
			bool _draw = false;
			for( float _angle = 0 ; _angle <= 360 ; _angle += step )
			{
			
					//	_angle += space;
					float a1 = _angle * Mathf.PI / 180f;
					float a2 = (_angle + step) * Mathf.PI / 180f;
					
					Vector3 _inner_pos_1 = center + new Vector3(Mathf.Sin(a1) * radius, 0, Mathf.Cos(a1) * radius );
					Vector3 _inner_pos_2 = center + new Vector3(Mathf.Sin(a2) * radius, 0, Mathf.Cos(a2) * radius );
					float lenght = PositionTools.Distance( _inner_pos_1, _inner_pos_2 );
					
					Vector3 _outer_pos_1 = center + new Vector3(Mathf.Sin(a1) * (radius - lenght), 0, Mathf.Cos(a1) * (radius - lenght) );
					Vector3 _outer_pos_2 = center + new Vector3(Mathf.Sin(a2) * (radius - lenght), 0, Mathf.Cos(a2) * (radius - lenght) );
					
					Vector3 _outer_pos_3 = Vector3.Lerp( _outer_pos_1, _outer_pos_2, 0.5f );
					
					
					Gizmos.DrawLine( _inner_pos_1, _outer_pos_3 );
					Gizmos.DrawLine( _outer_pos_3, _inner_pos_2 );
					
					if( _text_pos == Vector3.zero )
						_text_pos = _inner_pos_1;

				if( _draw )
				{

					_draw = false;
				}
				else
				{

					_draw = true;
				}
			}

			if( _show_text && _text != "" )
			{
				
				Color _color = Gizmos.color;
				_color.a = 255;
				Text( GUI.skin, _text , _text_pos, _color, 12, -50,200 );
				//Gizmos.color = _org_color;
			}
		}

		public static void Arrow( Vector3 _position, Vector3 _direction, float _head_lenght = 0.25f, float _head_angle = 20.0f )
		{
			Gizmos.DrawRay( _position, _direction );

			ArrowHead( _position, _direction, _head_lenght, _head_angle );

		}

		public static void ArrowHead( Vector3 _position, Vector3 _direction, float _head_lenght = 0.25f, float _head_angle = 20.0f )
		{
			if( _direction == Vector3.zero )
				_direction = Vector3.forward;

			Vector3 _right = Quaternion.LookRotation(_direction) * Quaternion.Euler(0,180+_head_angle,0) * new Vector3(0,0,1);
			Vector3 _left = Quaternion.LookRotation(_direction) * Quaternion.Euler(0,180-_head_angle,0) * new Vector3(0,0,1);
			Gizmos.DrawRay(_position + _direction, _right * _head_lenght);
			Gizmos.DrawRay(_position + _direction, _left * _head_lenght);
		}

		public static void Triangle( Vector3 _position, Vector3 _direction, float _head_lenght = 0.25f, float _head_angle = 20.0f )
		{
			if( _direction == Vector3.zero )
				_direction = Vector3.forward;
			
			Vector3 _right = Quaternion.LookRotation(_direction) * Quaternion.Euler(0,180+_head_angle,0) * new Vector3(0,0,1);
			Vector3 _left = Quaternion.LookRotation(_direction) * Quaternion.Euler(0,180-_head_angle,0) * new Vector3(0,0,1);
			Gizmos.DrawRay(_position + _direction, _right * _head_lenght);
			Gizmos.DrawRay(_position + _direction, _left * _head_lenght);
		}

		public static void TriCircle( Vector3 center, float radius, string _text, bool _show_text, bool _inverse )
		{ 
			float step = GetCircleDegrees( radius);


			Vector3 _text_pos = Vector3.zero;

			bool _draw = false;
			for( float _angle = 0 ; _angle <= 360 ; _angle += step )
			{
				if( _draw )
				{
			//	_angle += space;
				float a1 = _angle * Mathf.PI / 180f;
				float a2 = (_angle + step) * Mathf.PI / 180f;

				float _inner_radius = radius;
				

				Vector3 _inner_pos_1 = center + new Vector3(Mathf.Sin(a1) * _inner_radius, 0, Mathf.Cos(a1) * _inner_radius );
				Vector3 _inner_pos_2 = center + new Vector3(Mathf.Sin(a2) * _inner_radius, 0, Mathf.Cos(a2) * _inner_radius );
				float lenght = PositionTools.Distance( _inner_pos_1, _inner_pos_2 );

				float _outer_radius = radius - lenght;

				if( _inverse )
					_outer_radius = radius + lenght;

				Vector3 _outer_pos_1 = center + new Vector3(Mathf.Sin(a1) * _outer_radius, 0, Mathf.Cos(a1) * _outer_radius );
				Vector3 _outer_pos_2 = center + new Vector3(Mathf.Sin(a2) * _outer_radius, 0, Mathf.Cos(a2) * _outer_radius );

				Vector3 _outer_pos_3 = Vector3.Lerp( _outer_pos_1, _outer_pos_2, 0.5f );

				Gizmos.DrawLine( _inner_pos_1, _outer_pos_3 );
				Gizmos.DrawLine( _outer_pos_3, _inner_pos_2 );
				Gizmos.DrawLine( _inner_pos_2, _inner_pos_1 );

					if( _text_pos == Vector3.zero )
						_text_pos = _inner_pos_1;

					_draw = false;
				}
				else
					_draw = true;


	
				

			}
			
			
			
			if( _show_text && _text != "" )
			{
				
				Color _color = Gizmos.color;
				_color.a = 255;
				Text( GUI.skin, _text , _text_pos, _color, 12, -50,200 );
				//Gizmos.color = _org_color;
			}
		}



		public static void BeamCircle( Vector3 center, float radius, float degrees, bool dotted, float lenght, string _text, bool _show_text, bool _inverse = false )
		{ 
			Vector3 _last_position = Vector3.zero;

			if( _inverse )
				lenght *= -1;
			
			for( float _angle = 0 ; _angle <= 360 ; _angle += degrees )
			{
				float a = _angle * Mathf.PI / 180f;
				
				Vector3 _pos_1 = center + new Vector3(Mathf.Sin(a) * radius, 0, Mathf.Cos(a) * radius );
				Vector3 _pos_2 = center + new Vector3(Mathf.Sin(a) * (radius + lenght), 0, Mathf.Cos(a) * (radius + lenght) );
				
				if( _last_position != Vector3.zero && _pos_1 != Vector3.zero )
				{
					if( ! dotted || ( _angle % 2 == 0 ) )
					{
						Gizmos.DrawLine( _last_position, _pos_1 );
						Gizmos.DrawLine( _pos_1, _pos_2 );
					}
				}
				
				_last_position = _pos_1;
			}
			
			if( _show_text && _text != "" )
			{
				
				Color _color = Gizmos.color;
				_color.a = 255;
				Text( GUI.skin, _text , _last_position, _color, 12, -50,200 );
				//Gizmos.color = _org_color;
			}
		}

		public static void OffsetPath( Vector3 pos_1, float offset_1, Vector3 pos_2, float offset_2, bool _arrow = false, bool _use_level = true )
		{
			Vector3 pos_1a = GetPathSection ( pos_2, pos_1, offset_1, true );
			Vector3 pos_2a = GetPathSection ( pos_1a, pos_2, offset_2, true );

			if( _use_level )
			{
				pos_1a.y = pos_1.y;
				pos_2a.y = pos_2.y;
			}
			
			Gizmos.DrawLine( pos_1a, pos_2a );

			if( _arrow )
			{
				Vector3 _direction = ( pos_2 - pos_1 );

				float _head_lenght = 1f;
				float _head_angle = 25;

				ArrowHead( pos_1, _direction * 0.5f, _head_lenght, _head_angle );
				ArrowHead( pos_1, _direction * 0.55f, _head_lenght, _head_angle );
				ArrowHead( pos_1, _direction * 0.6f, _head_lenght, _head_angle );
			}
		}
		

		public static Vector3 GetPathSection ( Vector3 pos_1, Vector3 pos_2, float lenght, bool inverse ) {
			
			float f = 1;
			float distance = PositionTools.Distance(pos_1, pos_2);
			
			if( inverse )
				lenght = distance - lenght;
			
			if( lenght < distance )
				f = lenght/(distance/100)/100;
			
			return Vector3.Lerp( pos_1, pos_2, f);
		}
	}
}