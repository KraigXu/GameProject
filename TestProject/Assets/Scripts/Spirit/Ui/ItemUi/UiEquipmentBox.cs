using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    public class UiEquipmentBox : MonoBehaviour
    {

        public Entity Entity;

        public Image Image;
        public Text Text;

        public Slider Slider;


        // Use this for initialization
        void Start()
        {
            Image = gameObject.GetComponent<Image>();
            Text = gameObject.GetComponentInChildren<Text>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}