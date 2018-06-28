using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Final
{
    public class CharacterSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public Image myImage;
        public Text myText;

        public Canvas canvas;

        GameObject meDrag;
        RectTransform meDragPlane;

        public Sprite dragSprite;
        public bool dragOnSurfaces;
        private void Awake()
        {
            myImage = GetComponent<Image>();
            myText = GetComponentInChildren<Text>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AssignCharacter(Character c)
        {
            myText.text = "Character: " + c.ID.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canvas == null)
                return;

            // We have clicked something that can be dragged.
            // What we want to do is create an icon for this.
            meDrag = Instantiate(new GameObject("DragIcon"));

            meDrag.transform.SetParent(canvas.transform, false);
            meDrag.transform.SetAsLastSibling();
            meDrag.transform.localScale = Vector3.one * 2;
            var image = meDrag.AddComponent<Image>();

            image.sprite = dragSprite;
            
            //image.SetNativeSize();

            if (dragOnSurfaces)
                meDragPlane = transform as RectTransform;
            else
                meDragPlane = canvas.transform as RectTransform;

            SetDraggedPosition(eventData);
        }

        public void OnDrag(PointerEventData data)
        {
            if (meDrag != null)
                SetDraggedPosition(data);
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
                meDragPlane = data.pointerEnter.transform as RectTransform;

            var rt = meDrag.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(meDragPlane, data.position, data.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
                rt.rotation = meDragPlane.rotation;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (meDrag != null)
                Destroy(meDrag);

            
        }
    }
}

