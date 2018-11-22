using UnityEngine;
using System.Collections;

namespace WX.Ui
{
    public class MouseImageControl : MonoBehaviour
    {
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        // private GameObject _root = null;
        public UIEventTriggerListener.VoidDelegate _callback = null;
        void Start()
        {
            //Cursor.visible = false;
            // _root = GameObject.Find("Canvas");
            gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
            UIEventTriggerListener.Get(gameObject).onEnter += OnMouseEnter;
            UIEventTriggerListener.Get(gameObject).onExit += OnMouseExit;
        }

        void OnClick()
        {

            if (_callback != null)
            {
                _callback(gameObject);
            }

        }
        void OnMouseEnter(GameObject go)
        {


            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        void OnMouseExit(GameObject go)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }



    }
}
