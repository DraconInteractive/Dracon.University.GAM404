using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Final
{
    //class for the character slot ui. Include drag interfaces and click detection
    public class CharacterSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        //containers for ui elements, and a character ref for good measure
        [HideInInspector]
        public Character myCharacter = null;
        public Image myImage;
        public Text myText;
        //made this public instead of getting through script because effort. 
        public Canvas canvas;
        //reference to spawned drag prefab
        GameObject meDrag;
        //^
        RectTransform meDragPlane;
        //sprite to put on meDrag
        public Sprite dragSprite;
        //Have no idea what this does, its in the tutorial for dragging XD
        public bool dragOnSurfaces;
        //prefab for draggy thing
        public GameObject descPanelPrefab;
        //Get attached UI elements
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
        //public function to set the character up UI'ishly
        public void AssignCharacter(Character c)
        {
            myText.text = "Character: " + c.ID.ToString();
            myCharacter = c;
        }
        //When the player drags this...
        public void OnBeginDrag(PointerEventData eventData)
        {
            //if there is no canvas, go back
            if (canvas == null)
                return;

            //spawn new gameobject for the draggable
            meDrag = Instantiate(new GameObject("DragIcon"));

            //set the draggable as a child of the canvas
            //Then put it to the bottom of the pile
            //set it a bit bigger, manual but meh
            meDrag.transform.SetParent(canvas.transform, false);
            meDrag.transform.SetAsLastSibling();
            meDrag.transform.localScale = Vector3.one * 2;
            //set the image to the variable up the top
            var image = meDrag.AddComponent<Image>();

            image.sprite = dragSprite;
            
            //image.SetNativeSize();
            //I restate... I have no idea what this does. 
            if (dragOnSurfaces)
                meDragPlane = transform as RectTransform;
            else
                meDragPlane = canvas.transform as RectTransform;
            //See below
            SetDraggedPosition(eventData);
        }
        //If its dragging, move it to the cursor. 
        public void OnDrag(PointerEventData data)
        {
            if (meDrag != null)
                SetDraggedPosition(data);
        }
        //^
        private void SetDraggedPosition(PointerEventData data)
        {
            //Honestly, this is boilerplate code. I needed a quick fix for dragging. 
            if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
                meDragPlane = data.pointerEnter.transform as RectTransform;

            //Unity really needs to learn proper variable names. Storage for the draggables rect transform?
            var rt = meDrag.GetComponent<RectTransform>();
            //Self explanatory, good job Unity
            Vector3 globalMousePos;
            //Right, so set my draggables position to the mouse pos. That makes sense. Rotation is there... but im not sure I want it to be? What if I always want it updown no matter the rotation? 
            //^ interesting but irrelevant for now however. 
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(meDragPlane, data.position, data.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
                rt.rotation = meDragPlane.rotation;
            }
        }
        // Destry the draggable when i stop dragging. 
        public void OnEndDrag(PointerEventData eventData)
        {
            if (meDrag != null)
                Destroy(meDrag);
            //Get ui under mouse using an eventsystem raycast. 
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, result);
            //search for a team container. 
            TeamContainer container = null;
            foreach (RaycastResult r in result)
            {
                container = r.gameObject.transform.GetComponent<TeamContainer>();
                if (container != null)
                {
                    break;
                }
            }
            //If there is a container, add the character to its team, then destroy this slot, disallowing duplicates. 
            if (container != null)
            {
                print("found container");
                if (container.AddCharacterToTeam(myCharacter))
                {
                    Destroy(this.gameObject);
                }
                
            }
            else
            {
                //Do nothing as there is no slot to drop into
                print("no container");
            }
        }
        //If you just click on the slot, it will spawn a window detailing player stats. 
        public void OnPointerClick(PointerEventData eventData)
        {
            CharacterDescUI ui = Instantiate(descPanelPrefab, transform.parent.transform.parent.transform.parent).GetComponent<CharacterDescUI>();
            ui.AssignCharacter(myCharacter);
        }
    }
}

