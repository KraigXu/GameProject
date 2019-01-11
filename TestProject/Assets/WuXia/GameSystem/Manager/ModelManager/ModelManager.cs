using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class ModelManager : MonoBehaviour
    {

        private static ModelManager _instance;

        public static ModelManager Instance
        {
            get { return _instance; }
        }

        public Dictionary<int,Transform> ModelDic=new Dictionary<int, Transform>();
        public List<ModelKeyValue> ModelKeyValues=new List<ModelKeyValue>();
        public List<Modeler> ModelerArray=new List<Modeler>();

        
        [Serializable]
        public class ModelKeyValue
        {
            public int ModelCode;
            public GameObject Prefab;
        }


        [Serializable]
        public class Modeler
        {

            public GameObject Model;
            public int Id;
        }

        private int idCounter;
        void Awake()
        {
            _instance = this;
        }

        void Update()
        {

        }

        public int AddModel(GameObject prefab,Vector3 point)
        {
            GameObject go= GameObject.Instantiate(prefab, point,Quaternion.identity);
            Modeler modeler=new Modeler();
            modeler.Id = ++idCounter;
            modeler.Model = go;

            ModelerArray.Add(modeler);
            go.SetActive(false);
            return modeler.Id;
        }

        public void ModelStatus(int id,bool flag)
        {
            for (int i = 0; i < ModelerArray.Count; i++)
            {
                if (ModelerArray[i].Id == id)
                {
                    ModelerArray[i].Model.SetActive(flag);
                }
            }
        }
    }



}
