// ##############################################################################
//
// ice_editor_templates.cs | ICEWorldTemplateDesigner
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
using System.IO;
using System.Text;
using System.Collections;

namespace ICE.World.EditorUtilities
{
	public enum ICEWorldTemplateType
	{
		WorldBehaviour,
		WorldEntity,
		WorldSingelton
	}

	public class ICEWorldTemplateData : System.Object
	{
		public ICEWorldTemplateType Type = ICEWorldTemplateType.WorldBehaviour;
		public string ProjectName = "MyProject";
		public string Namespace = "MyNamespace";
		public string ClassName = "MYClassName";
	}

	public class ICEWorldTemplateDesigner
	{
		public static void CreateWorldTemplate( ICEWorldTemplateData _data )
		{
			if( string.IsNullOrEmpty( _data.ProjectName ) )
				_data.ProjectName = "MYWORLD_PROJECT";
	
			if( string.IsNullOrEmpty( _data.Namespace ) )
				_data.Namespace = "MYWORLD";

			// PROJECT BEGIN
			string guid = AssetDatabase.CreateFolder("Assets", _data.ProjectName );
			string _template_path = AssetDatabase.GUIDToAssetPath(guid);

				// CONTENT BEGIN
				guid = AssetDatabase.CreateFolder( _template_path, "Content");
				//string _content_path = AssetDatabase.GUIDToAssetPath(guid);
				{
				}
				// CONTENT END

				// SCRIPT BEGIN
				guid = AssetDatabase.CreateFolder( _template_path, "Scripts");
				string _scripts_path = AssetDatabase.GUIDToAssetPath(guid);
				{
					// COMPONENTS BEGIN
					guid = AssetDatabase.CreateFolder( _scripts_path, "Components");
					string _scripts_components_path = AssetDatabase.GUIDToAssetPath(guid);
					{
						// COMPONENTS DEBUG BEGIN
						guid = AssetDatabase.CreateFolder( _scripts_components_path, "Debug");
					//string _scripts_components_debug_path = AssetDatabase.GUIDToAssetPath(guid);
						{
						}
						// COMPONENTS DEBUG END
						//CreateScript( _scripts_components_path, "MyNewComponent" );

						switch( _data.Type )
						{
							case ICEWorldTemplateType.WorldBehaviour:
								CreateWorldBehaviour( _data, _scripts_components_path );
								break;
							case ICEWorldTemplateType.WorldEntity:
								CreateWorldEntity( _data, _scripts_components_path );
								break;
							case ICEWorldTemplateType.WorldSingelton:
								break;
						}
					}
					// COMPONENTS END

		

					// CORE BEGIN
					guid = AssetDatabase.CreateFolder( _scripts_path, "Core");
					string _scripts_core_path = AssetDatabase.GUIDToAssetPath(guid);
					{
						guid = AssetDatabase.CreateFolder( _scripts_core_path, "Base");
						//string _scripts_core_base_path = AssetDatabase.GUIDToAssetPath(guid);

						guid = AssetDatabase.CreateFolder( _scripts_core_path, "Objects");
						//string _scripts_core_objects_path = AssetDatabase.GUIDToAssetPath(guid);

						guid = AssetDatabase.CreateFolder( _scripts_core_path, "Utilities");
						//string _scripts_core_utilities_path = AssetDatabase.GUIDToAssetPath(guid);

						// EDITOR BEGIN
						guid = AssetDatabase.CreateFolder( _scripts_core_path, "Editor");
						string _editor_path = AssetDatabase.GUIDToAssetPath(guid);
						{
							guid = AssetDatabase.CreateFolder( _editor_path, "Components");
							string _editor_components_path = AssetDatabase.GUIDToAssetPath(guid);
							{
								guid = AssetDatabase.CreateFolder( _editor_components_path, "Debug");
								//string _editor_components_debug_path = AssetDatabase.GUIDToAssetPath(guid);
							}

							guid = AssetDatabase.CreateFolder( _editor_path, "Configuration");
							string _editor_configuration_path = AssetDatabase.GUIDToAssetPath(guid);
							{
								CreateInfoFile( _data, _editor_configuration_path );
								CreateInitFile( _data, _editor_configuration_path );
							}

							guid = AssetDatabase.CreateFolder( _editor_path, "EditorUtilities");
							//string _editor_utilities_path = AssetDatabase.GUIDToAssetPath(guid);

							guid = AssetDatabase.CreateFolder( _editor_path, "Menu");
							//string _editor_menu_path = AssetDatabase.GUIDToAssetPath(guid);

							guid = AssetDatabase.CreateFolder( _editor_path, "Windows");
							//string _editor_windows_path = AssetDatabase.GUIDToAssetPath(guid);

							guid = AssetDatabase.CreateFolder( _editor_path, "Recources");
							//string _editor_recources_path = AssetDatabase.GUIDToAssetPath(guid);
						}
						// EDITOR END
					}
					// CORE END
				}
				// SCRIPT END
			// PROJECT END
		}

		public static void CreateWorldBehaviour( ICEWorldTemplateData _data, string _path )
		{
			string _filename = _data.ClassName;
				
			string _copy_path = GetValidPath( _path,_filename );
			if( File.Exists( _copy_path ) == true )
				return;

			using( StreamWriter _outfile = new StreamWriter( _copy_path ) )
			{
				PrintHead( _outfile, _filename, _data.ClassName );
				PrintUsing( _outfile );
				PrintNamespace( _outfile, _data.Namespace );

				_outfile.WriteLine("{");
				_outfile.WriteLine("");
				_outfile.WriteLine("\tpublic class " + _data.ClassName + " : ICEWorldBehaviour {" );
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tprotected override void OnRegisterBehaviourEvents()");
				_outfile.WriteLine("\t\t{");
				_outfile.WriteLine("\t\t\t//base.OnRegisterBehaviourEvents();");
				_outfile.WriteLine("\t\t\t//RegisterBehaviourEvent( \"YOUR_CUSTOM_EVENT\", BehaviourEventParameterType.Float );");
				_outfile.WriteLine("\t\t\t//RegisterBehaviourEvent( \"YOUR_CUSTOM_EVENT\" );");
				_outfile.WriteLine("\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void Awake () {");
				_outfile.WriteLine("\t\t\tbase.Awake();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void Start () {");
				_outfile.WriteLine("\t\t\tbase.Start();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void OnEnable () {");
				_outfile.WriteLine("\t\t\tbase.OnEnable();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void OnDisable () {");
				_outfile.WriteLine("\t\t\tbase.OnDisable();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void OnDestroy() {");
				_outfile.WriteLine("\t\t\tbase.OnDestroy();");
				_outfile.WriteLine("\t\t}");			
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void Update() {");
				_outfile.WriteLine("\t\t\tDoUpdate();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void LateUpdate() {");
				_outfile.WriteLine("\t\t\tDoLateUpdate();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void FixedUpdate() {");
				_outfile.WriteLine("\t\t\tDoFixedUpdate();");
				_outfile.WriteLine("\t\t}");

				PrintFooter( _outfile, _data.ClassName );
			}
		}

		public static void CreateWorldEntity( ICEWorldTemplateData _data, string _path )
		{
			int _count = 1;
			string _filename = _data.ClassName;
			string _copy_path = GetValidPath( _path,_filename );
			while( File.Exists( _copy_path ) == true )
			{
				_filename = _data.ClassName + _count;
				_copy_path = GetValidPath( _path,_filename );
				_count++;
			}

			using( StreamWriter _outfile = new StreamWriter( _copy_path ) )
			{
				PrintHead( _outfile, _filename, _filename );
				PrintUsing( _outfile );
				PrintNamespace( _outfile, _data.Namespace );

				_outfile.WriteLine("{");
				_outfile.WriteLine("");
				_outfile.WriteLine("\tpublic class " + _filename + " : ICEWorldEntity {" );
				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tprotected override void OnRegisterBehaviourEvents()");
				_outfile.WriteLine("\t\t{");
				_outfile.WriteLine("\t\t\tbase.OnRegisterBehaviourEvents(); // the base class already handles ApplyDamage and Reset");
				_outfile.WriteLine("\t\t\t//RegisterBehaviourEvent( \"ApplyDamage\", BehaviourEventParameterType.Float ); // ApplyDamage is member of ICEWorldEntity ");
				_outfile.WriteLine("\t\t\t//RegisterBehaviourEvent( \"Reset\" ); // Reset is member of ICEWorldEntity");
				_outfile.WriteLine("\t\t\t//RegisterBehaviourEvent( \"YOUR_CUSTOM_EVENT\", BehaviourEventParameterType.Float );");
				_outfile.WriteLine("\t\t\t//RegisterBehaviourEvent( \"YOUR_CUSTOM_EVENT\" );");
				_outfile.WriteLine("\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void Awake () {");
				_outfile.WriteLine("\t\t\tbase.Awake();");
				_outfile.WriteLine("\t\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void Start () {");
				_outfile.WriteLine("\t\t\tbase.Start();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");

				_outfile.WriteLine("\t\tpublic override void OnEnable () {");
				_outfile.WriteLine("\t\t\tbase.OnEnable(); // important or implement Lifespan.Init( this );");
				_outfile.WriteLine("\t\t\t//Lifespan.Init( this );");
				_outfile.WriteLine("\t\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void OnDisable () {");
				_outfile.WriteLine("\t\t\tbase.OnDisable();");
				_outfile.WriteLine("\t\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void OnDestroy() {");
				_outfile.WriteLine("\t\t\tbase.OnDestroy();");
				_outfile.WriteLine("\t\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void Update() {");
				_outfile.WriteLine("\t\t\tDoUpdate();");
				_outfile.WriteLine("\t\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\tpublic override void LateUpdate() {");
				_outfile.WriteLine("\t\t\tDoLateUpdate();");
				_outfile.WriteLine("\t\t}");
				_outfile.WriteLine("");

				_outfile.WriteLine("\t\tpublic override void FixedUpdate() {");
				_outfile.WriteLine("\t\t\tDoFixedUpdate();");
				_outfile.WriteLine("\t\t}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\t//protected override void Register(){ // TODO: override Register if required ");
				_outfile.WriteLine("\t\t\t//WorldManager.Register( transform.gameObject );");
				_outfile.WriteLine("\t\t//}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\t//protected override void Deregister(){ // TODO: override Deregister if required ");
				_outfile.WriteLine("\t\t\t//WorldManager.Deregister( transform.gameObject );");
				_outfile.WriteLine("\t\t\t//}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\t//protected override void Remove(){ // TODO: override Remove if required ");
				_outfile.WriteLine("\t\t\t//WorldManager.Remove( transform.gameObject );");
				_outfile.WriteLine("\t\t//}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\t//public override void Reset(){ // TODO: override Reset if required ");
				_outfile.WriteLine("\t\t\t//Status.Reset();");
				_outfile.WriteLine("\t\t//}");

				_outfile.WriteLine("");
				_outfile.WriteLine("\t\t//public override void ApplyDamage( float _damage ){ // TODO: override ApplyDamage if required ");
				_outfile.WriteLine("\t\t\t//ApplyDamage( _damage, Vector3.zero, Vector3.zero, null, 0 );");
				_outfile.WriteLine("\t\t//}");

				PrintFooter( _outfile, _filename );
			}
		}
			
		public static void CreateInitFile( ICEWorldTemplateData _data, string _path )
		{
			string _classname = "Init";
			string _filename = "editor_configuration_init";

			string _copy_path = GetValidPath( _path,_filename );
			if( File.Exists( _copy_path ) == true )
				return;

			using( StreamWriter _outfile = new StreamWriter( _copy_path ) )
			{
				PrintHead( _outfile, _filename, _classname );
				_outfile.WriteLine("");
				_outfile.WriteLine("using UnityEngine;");
				_outfile.WriteLine("");
				_outfile.WriteLine("namespace ICE." + _data.Namespace + ".EditorInfos // TODO: ADAPT THE NAMESPACE AS DESIRED" );
				_outfile.WriteLine("{");
				_outfile.WriteLine("");
				_outfile.WriteLine("\tpublic class " + _classname + " : ICE.World.EditorInfos.Init {" );
				_outfile.WriteLine("");
				_outfile.WriteLine("// example of use : Init.YOUR_DEFAULT_VALUE");
				_outfile.WriteLine("\t\tpublic static readonly float YOUR_DEFAULT_VALUE = 0; // TODO: ADAPT NAME AND VALUE");
				_outfile.WriteLine("");
				PrintFooter( _outfile, _classname );
			}
		}

		public static void CreateInfoFile( ICEWorldTemplateData _data, string _path )
		{
			string _classname = "Info";
			string _filename = "editor_configuration_text";

			string _copy_path = GetValidPath( _path, _filename );
			if( File.Exists( _copy_path ) == true )
				return;

			using( StreamWriter _outfile = new StreamWriter( _copy_path ) )
			{
				PrintHead( _outfile, _filename, _classname );
				_outfile.WriteLine(" ");
				_outfile.WriteLine("using UnityEngine;");
				_outfile.WriteLine("");
				_outfile.WriteLine("namespace ICE." + _data.Namespace + ".EditorInfos // TODO: ADAPT THE NAMESPACE AS DESIRED" );
				_outfile.WriteLine("{");
				_outfile.WriteLine("");
				_outfile.WriteLine("public class " + _classname + " : ICE.World.EditorInfos.Info {" );
				_outfile.WriteLine("");
				_outfile.WriteLine("// example of use : Info.YOUR_TEXT_KEY");
				_outfile.WriteLine("public static readonly string YOUR_TEXT_KEY = \"YOUR TEXT\"; // TODO: DUPLICATE LINE AND ADAPT KEY AND TEXT\"");
				_outfile.WriteLine("");
				PrintFooter( _outfile, _classname );
			}
		}



		// PRINTER

		public static void PrintHead( StreamWriter _outfile, string _filename, string _classname )
		{
			_outfile.WriteLine("// ##############################################################################");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// " + _filename + ".cs | " + _classname );
			_outfile.WriteLine("// Version 0.1");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.");
			_outfile.WriteLine("// http://www.icecreaturecontrol.com");
			_outfile.WriteLine("// mailto:support@icecreaturecontrol.com");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// Unity Asset Store End User License Agreement (EULA)");
			_outfile.WriteLine("// http://unity3d.com/legal/as_terms");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// ##############################################################################");
		}

		public static void PrintUsing( StreamWriter _outfile )
		{
			_outfile.WriteLine(" ");
			_outfile.WriteLine("using UnityEngine;");
			_outfile.WriteLine("");
			_outfile.WriteLine("using ICE;");
			_outfile.WriteLine("using ICE.World;");
			_outfile.WriteLine("using ICE.World.Objects;");
			_outfile.WriteLine("using ICE.World.Utilities;");
			_outfile.WriteLine("");
		}

		public static void PrintNamespace( StreamWriter _outfile, string _namespace )
		{
			if( string.IsNullOrEmpty( _namespace ) )
				_outfile.WriteLine("namespace ICE.CustumWorld // TODO: ADAPT THE NAMESPACE AS DESIRED" );
			else
				_outfile.WriteLine("namespace ICE." + _namespace + " // TODO: ADAPT THE NAMESPACE AS DESIRED" );
		}

		public static void PrintFooter( StreamWriter _outfile, string _name )
		{
			_outfile.WriteLine("");
			_outfile.WriteLine("\t}"); // close class
			_outfile.WriteLine("}"); // close namespace
		}

		public static void PrintBehaviourContent( StreamWriter _outfile, string _name )
		{
			_outfile.WriteLine("/// <summary>");
			_outfile.WriteLine("/// OnRegisterPublicMethods is called within the GetPublicMethods() method to update the");
			_outfile.WriteLine("/// m_PublicMethods list. Override this event to register your own methods by using the");
			_outfile.WriteLine("/// RegisterPublicMethod(); while doing so you can use base.OnRegisterPublicMethods();");
			_outfile.WriteLine("/// to call the event in the base classes too.");
			_outfile.WriteLine("/// </summary>");
			_outfile.WriteLine("protected override void OnRegisterBehaviourEvents()");
			_outfile.WriteLine("{");
			_outfile.WriteLine("// base.OnRegisterPublicMethods();" );
			_outfile.WriteLine("// RegisterBehaviourEvent( \"ApplyDamage\", BehaviourEventParameterType.Float );");
			_outfile.WriteLine("// RegisterBehaviourEvent( \"TriggerBehaviourRuleAudio\" );");
			_outfile.WriteLine("}");
			_outfile.WriteLine("");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void Awake () {");
			_outfile.WriteLine("base.Awake();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void Start () {");
			_outfile.WriteLine("base.Start();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void OnEnable () {");
			_outfile.WriteLine("base.OnEnable();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void OnDisable () {");
			_outfile.WriteLine("base.OnDisable();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void OnDestroy() {");
			_outfile.WriteLine("base.OnDestroy();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void Update()");
			_outfile.WriteLine("{");
			_outfile.WriteLine("");
			_outfile.WriteLine("DoUpdateBegin();");
			_outfile.WriteLine("DoUpdate();");
			_outfile.WriteLine("DoUpdateComplete();");
			_outfile.WriteLine("");
			_outfile.WriteLine("}");
		}

		// UTILITIES

		private static string GetValidPath(  string _path, string _name  )
		{
			if( _path == "" )
				_path = "Assets";
			else if( Path.GetExtension( _path ) != "" )
				_path = _path.Replace( Path.GetFileName( _path ), "" );

			_name = _name.Replace("-","_");
			string _copy_path = _path + "/" + _name + ".cs";
			Debug.Log( "Creating Classfile: " + _copy_path );

			return _copy_path;
		}
	}
}
