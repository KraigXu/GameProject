using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Verse.UIFrameWork
{
    public class UIEventTriggerListener :EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);

        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidDelegate onDrag;
        public VoidDelegate onEndDrag;
        public VoidDelegate onBeginDrag;

        static public UIEventTriggerListener Get(GameObject go)
        {
            UIEventTriggerListener listener = go.GetComponent<UIEventTriggerListener>();
            if (listener == null) listener = go.AddComponent<UIEventTriggerListener>();
            return listener;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (onClick != null) onClick(gameObject);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (onDown != null) onDown(gameObject);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (onEnter != null) onEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (onExit != null) onExit(gameObject);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (onUp != null) onUp(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (onSelect != null) onSelect(gameObject);
        }

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            base.OnUpdateSelected(eventData);
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (onDrag != null) onDrag(gameObject);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (onEndDrag != null) onEndDrag(gameObject);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (onBeginDrag != null) onBeginDrag(gameObject);
        }
    }
}