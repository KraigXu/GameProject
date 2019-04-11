using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  GameSystem
{
    public class DistrictRange : MonoBehaviour
    {
        public int DistrictId;
        public Color Color;

        public MeshCollider Collider;
        public MeshFilter Filter;
        public MeshRenderer Renderer;


        void Awake()
        {
            Collider = gameObject.GetComponent<MeshCollider>();
            Filter = gameObject.GetComponent<MeshFilter>();
        }

        

        void Start()
        {


        }
        void Update()
        {


        }

        void OnTriggerEnter(Collider collider)
        {

        }

        void OnTriggerExit(Collider collider)
        {

        }
    }


}
