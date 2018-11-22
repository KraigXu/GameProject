// ##############################################################################
//
// ice_editor_io.cs / ICE.World.EditorUtilities.ICEEditorIO
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

using UnityEditor;

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World.EditorUtilities
{
	public class ICEEditorIO : System.Object
	{
		protected static string m_Path = "";

		protected static string Save( string _name, string _suffix ){
			return UnityEditor.EditorUtility.SaveFilePanelInProject( "Save File As", _name.ToLower() + "." + _suffix , _suffix, "" );
		}

		protected static string Load( string _suffix ){
			return UnityEditor.EditorUtility.OpenFilePanel( "Open File", Application.dataPath, _suffix );
		}

		protected static void SaveObjectToFile<T>( T _object ) where T : ICEObject
		{
			XmlSerializer serializer = new XmlSerializer( typeof( T ) );
			FileStream _stream = new FileStream( m_Path, FileMode.Create);
			serializer.Serialize( _stream, _object );
			_stream.Close();
		}

		protected static T LoadObjectFromFile<T>( T _object ) where T : ICEObject
		{
			XmlSerializer serializer = new XmlSerializer(typeof( T ));
			FileStream _stream = new FileStream( m_Path, FileMode.Open);
			_object = serializer.Deserialize(_stream) as T;
			_stream.Close();

			return _object;
		}
	}

	/*
	public class ICEIO : System.Object
	{
		protected static void SaveObjectToFile<T>( T _object, string _path ) where T : ICEObject
		{
			XmlSerializer serializer = new XmlSerializer( typeof( T ) );
			FileStream _stream = new FileStream( _path, FileMode.Create);
			serializer.Serialize( _stream, _object );
			_stream.Close();
		}

		protected static T LoadObjectFromFile<T>( T _object, string _path ) where T : ICEObject
		{
			XmlSerializer serializer = new XmlSerializer(typeof( T ));
			FileStream _stream = new FileStream( _path, FileMode.Open);
			_object = serializer.Deserialize(_stream) as T;
			_stream.Close();

			return _object;
		}
	}
	*/
}