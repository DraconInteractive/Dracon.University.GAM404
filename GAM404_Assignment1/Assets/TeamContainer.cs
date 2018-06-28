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

        public Text teamStr, teamDex, teamSta, teamAcu;
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

            s += "\n\n";
            int str = 0;
            int dex = 0;
            int acu = 0;
            int sta = 0;
            foreach (Character ch in tempTeam)
            {
                str += ch.attributes.str;
                dex += ch.attributes.dex;
                acu += ch.attributes.acu;
                sta += ch.attributes.sta;
            }

            s += "Totals: " + "\n\n";
            s += "Strength: " + str.ToString() + "\n";
            s += "Dexterity: " + dex.ToString() + "\n";
            s += "Stamina: " + sta.ToString() + "\n";
            s += "Acuity: " + acu.ToString() + "\n";
            teamText.text = s;

            if (tempTeam.Count == 5)
            {
                GetComponent<Image>().color = new Color(0.3f, 0.7f, 0.3f, 0.7f);
            }
            return true;
        }
    }
}

