using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Final
{
    public class TeamContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static List<TeamContainer> teamContainers = new List<TeamContainer>();
        public bool mouseOver = false;

        public Text teamText;
        public List<Character> tempTeam;
        // Use this for initialization
        void Start()
        {
            teamContainers.Add(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseOver = false;
        }

        public bool AddCharacterToTeam(Character c)
        {
            if (tempTeam.Count < 5)
            {
                tempTeam.Add(c);
            }
            else
            {
                return false;
            }
            string s = "";
            foreach (Character ch in tempTeam)
            {
                s += "•" + " Character " + ch.ID + "\n";
            }
            teamText.text = s;
            return true;
        }
    }
}

