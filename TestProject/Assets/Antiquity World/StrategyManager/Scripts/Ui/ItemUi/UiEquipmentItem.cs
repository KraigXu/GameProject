using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    public class UiEquipmentStyle
    {
        public string Title;
        public Dictionary<string, List<string>> conents=new Dictionary<string, List<string>>();
        public int Level;
        public Sprite Background;
        public int BackgroundId;
        public Dictionary<string,string> Values=new Dictionary<string, string>();
    }

    public class UiEquipmentItem : MonoBehaviour
    {
        private UiEquipmentStyle _style;

        public Text Title;
        void Start()
        {
            if (_style == null) return;

            Title.text = _style.Title;
            
        }
        void Update()
        {

        }
    }
}