using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Test
{
    public class TsDefineDataShow : MonoBehaviour
    {
        public static TsDefineDataShow Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject s = new GameObject("TsDefineDataShow");
                    _instance= s.AddComponent<TsDefineDataShow>();
                }
                return _instance;
            }
        }

        private static TsDefineDataShow _instance;

        public List<Material> materials=new List<Material>();

        void Start()
        {
        }

        void Update()
        {

        }
        public void AddMaterial(Material m)
        {
          //  Log.Message("Test Materail Add >>>"+m);
            materials.Add(m);
        }

        public void AddMaterialNew(Material m)
        {
          //  Log.Message("Test Materail Add New >>>" + m);
            materials.Add(m);
        }
        
    }
}

