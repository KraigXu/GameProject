// ##############################################################################
//
// ice_editor_world_io.cs | ICE.World.ICEWorldIO.cs
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
	public class ICEEditorWorldIO : ICEEditorIO
	{

		/// <summary>
		/// Saves the durability composition to file.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="owner">Owner.</param>
		public static void SaveDurabilityCompositionToFile( DurabilityCompositionObject _object, string owner  )
		{
			m_Path = Save( owner, "durability" );
			if( m_Path.Length == 0 )
				return;

			SaveObjectToFile<DurabilityCompositionObject>( _object );
		}


		/// <summary>
		/// Loads the durability composition from file.
		/// </summary>
		/// <returns>The durability composition from file.</returns>
		/// <param name="_object">Object.</param>
		public static DurabilityCompositionObject LoadDurabilityCompositionFromFile( DurabilityCompositionObject _object )
		{
			m_Path = Load( "durability" );
			if( m_Path.Length == 0 )
				return _object;

			return LoadObjectFromFile<DurabilityCompositionObject>( _object );
		}

	}
}